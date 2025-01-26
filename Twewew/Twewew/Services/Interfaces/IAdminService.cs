using Twewew.DTOs;
using Twewew.Requests.Auth;
using Twewew.Requests.User;

namespace Twewew.Services.Interfaces;


public interface IAdminService
{
    Task<List<UserDto>> GetAsync();
    Task<UserDto> GetUserById(UserRequest request);
    Task UpdateAsync(UpdateUserRequest request);
    Task AddRole(string role);
    Task AssignRole(UserRoleRequest request);
}
