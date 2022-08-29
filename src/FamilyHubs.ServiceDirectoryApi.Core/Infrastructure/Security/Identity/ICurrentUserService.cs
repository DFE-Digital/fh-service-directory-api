namespace FamilyHubs.ServiceDirectoryApi.Core.Infrastructure.Security.Identity
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
    }
}