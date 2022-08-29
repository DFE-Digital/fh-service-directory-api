﻿using Microsoft.AspNetCore.Identity;

namespace FamilyHubs.ServiceDirectoryApi.Infrastructure.Security.Identity;

// TODO: Move to SharedKernel?
public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }
}
