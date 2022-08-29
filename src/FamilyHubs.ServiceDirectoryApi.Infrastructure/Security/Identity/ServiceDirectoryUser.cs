using Microsoft.AspNetCore.Identity;

namespace FamilyHubs.ServiceDirectoryApi.Infrastructure.Security.Identity;

// TODO: IdentiyUser doesn't have an interface so can't make ApplicationUser an interface which messes up having the interface in Core and the implementation in Infrastrucure
public class ServiceDirectoryUser : IdentityUser
{
}
