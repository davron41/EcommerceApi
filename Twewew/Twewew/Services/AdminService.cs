using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twewew.DTOs;
using Twewew.Exceptions;
using Twewew.Persistence;
using Twewew.Requests.Auth;
using Twewew.Requests.User;
using Twewew.Services.Interfaces;

namespace Twewew.Services;

public sealed class AdminService : IAdminService
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;


    public AdminService(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser<Guid>> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
    }

    public async Task<List<UserDto>> GetAsync()
    {
        var users = _context.Users.
             AsNoTracking();

        var dto = await users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        /*var userIds = dto.Select(x => x.UserId);
        var roleIds = _context.UserRoles.Where(x => userIds.Contains(x.UserId)).Select(x => x.RoleId);*/
        return dto;
    }

    public async Task<UserDto> GetUserById(UserRequest request)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        var user = await GetAndValidateUser(request.Id);

        var dto = _mapper.Map<UserDto>(user);

        return dto;
    }

    public async Task UpdateAsync(UpdateUserRequest request)
    {
        var user = await GetAndValidateUser(request.Id);

        _mapper.Map(request, user);
        _context.Update(request);
        await _context.SaveChangesAsync();
    }

    public async Task AddRole([FromBody] string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(role));

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("The role is not created");
            }
        }

    }

    public async Task AssignRole([FromBody] UserRoleRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user is null)
        {
            throw new InvalidOperationException("Invalid username");
        }

        var result = await _userManager.AddToRoleAsync(user, request.Role);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException("The role not asiggned");
        }
    }

    private async Task<IdentityUser<Guid>> GetAndValidateUser(Guid userId)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user is null)
        {
            throw new EntityNotFoundException($"User with id:{userId} is not found");
        }

        return user;
    }
}
