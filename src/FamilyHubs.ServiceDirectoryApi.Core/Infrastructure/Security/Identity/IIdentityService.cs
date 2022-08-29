namespace FamilyHubs.ServiceDirectoryApi.Core.Infrastructure.Security.Identity;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(IResult Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<IResult> DeleteUserAsync(string userId);
}
