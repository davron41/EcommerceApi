using FluentEmail.MailKitSmtp;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Twewew.Email;
using Twewew.Mappings;
using Twewew.Persistence;
using Twewew.Services;
using Twewew.Services.Interfaces;
using Twewew.Settings;
using Twewew.Validators.Category;

namespace Twewew.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddHttpContextAccessor();
        services.AddAutoMapper(typeof(CategoryMapping).Assembly);
        services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();
        services.AddFluentValidationAutoValidation();

        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderItemService, OrderItemService>();
        services.AddSingleton<ITokenHandler, Services.TokenHandler>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        AddIdentity(services);
        AddSettings(services, configuration);
        AddSwagger(services);
        AddEmail(services, configuration);
        AddRedis(services, configuration);
        AddAuthentication(services, configuration);
        AddAuthorization(services);

        return services;
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>(options =>
        {
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedAccount = true;


            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;

            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);

            options.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services
            .Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(12);
            });


    }

    private static void AddSettings(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<TokenSettings>()
            .Bind(configuration.GetSection(TokenSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddOptions<EmailSettings>()
            .Bind(configuration.GetSection(EmailSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();


    }

    private static void AddControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
            options.ReturnHttpNotAcceptable = true;
            options.RespectBrowserAcceptHeader = true;

        })
            .AddNewtonsoftJson()
            .AddXmlSerializerFormatters()
            .AddXmlDataContractSerializerFormatters();
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Ecommerce API",
                Description = "Ecommerce REST API",
                Contact = new OpenApiContact
                {
                    Name = "Davronbek To'lqinbekov",
                    Email = "davronbek8733@gmail.com",
                    Url = new Uri("https://Ecommerce.uz"),
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            var securityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Enter your JWT token in the text input below.",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                { securityScheme, [] }
            });
        });
    }
    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(TokenSettings.SectionName);
        var jwtOptions = section.Get<TokenSettings>();

        if (jwtOptions is null)
        {
            throw new InvalidOperationException("JWT configuration settings did not load correctly");
        }

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
            });
    }
    private static void AddEmail(IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(EmailSettings.SectionName);
        var emailSettings = section.Get<EmailSettings>();

        if (emailSettings is null)
        {
            throw new InvalidOperationException("Cannot setup email without configuration values.");
        }

        var smtpOptions = new SmtpClientOptions
        {
            Server = emailSettings.Server,
            Port = emailSettings.Port,
            User = emailSettings.FromEmail,
            Password = emailSettings.Password,
            UseSsl = true,
            RequiresAuthentication = true,
        };

        services
            .AddFluentEmail(emailSettings.FromEmail, emailSettings.FromName)
            .AddRazorRenderer()
            .AddMailKitSender(smtpOptions);

    }

    private static void AddRedis(IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "ecommerce_";
        });
    }

    private static void AddAuthorization(IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
            options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
        });
    }
}

