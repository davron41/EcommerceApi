using System.ComponentModel.DataAnnotations;

namespace Twewew.Settings;
public class EmailSettings
{
    public const string SectionName = nameof(EmailSettings);

    [Required(ErrorMessage = "From Email is required")]
    public required string FromEmail { get; init; }

    [Required(ErrorMessage = "From Name is required")]
    public required string FromName { get; init; }

    [Required(ErrorMessage = "Host is required")]
    public required string Server { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Port is required")]
    public int Port { get; set; }
}
