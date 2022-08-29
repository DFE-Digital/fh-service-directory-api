using FamilyHubs.ServiceDirectoryApi.Core.Infrastructure.Security.Identity;

namespace FamilyHubs.ServiceDirectoryApi.Infrastructure.Security.Identity;

// TODO: Move to SharedKernel?
public class Result : IResult
{
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    public bool Succeeded { get; set; }

    public string[] Errors { get; set; }

    public static Result Success()
    {
        return new Result(true, Array.Empty<string>());
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}
