namespace FamilyHubs.ServiceDirectoryApi.Core.Infrastructure.Security.Identity
{
    public interface IResult
    {
        string[] Errors { get; set; }
        bool Succeeded { get; set; }
    }
}