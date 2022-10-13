using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;


namespace fh_service_directory_api.api.Helper;

public static class ObjectSerializer
{
    public static string Serialize(this object obj)
    {
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
        { ContractResolver = new IgnorePropertiesResolver(new[] { "Created", "CreatedBy", "LastModified", "LastModifiedBy" }) });
    }
}

public class IgnorePropertiesResolver : DefaultContractResolver
{
    private readonly HashSet<string> ignoreProps;
    public IgnorePropertiesResolver(IEnumerable<string> propNamesToIgnore)
    {
        this.ignoreProps = new HashSet<string>(propNamesToIgnore);
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);
        if (property != null && property.PropertyName != null && this.ignoreProps.Contains(property.PropertyName))
        {
            property.ShouldSerialize = _ => false;
        }
        return property ?? new JsonProperty();
    }
}