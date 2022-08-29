namespace FamilyHubs.ServiceDirectoryApi.Core.Infrastructure.Security.Authorisation;

/// <summary>
/// Specifies the class this attribute is applied to requires authorization.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthoriseAttribute : Attribute, IAuthoriseAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthoriseAttribute"/> class. 
    /// </summary>
    public AuthoriseAttribute() { }

    /// <summary>
    /// Gets or sets a comma delimited list of roles that are allowed to access the resource.
    /// </summary>
    public string Roles { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the policy name that determines access to the resource.
    /// </summary>
    public string Policy { get; set; } = string.Empty;
}
