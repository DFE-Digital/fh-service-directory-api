using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Dsl;

public class FhParameter
{
    public readonly string Name;
    private readonly object _obj;

    public FhParameter(string name, object obj)
    {
        Name = name;
        _obj = obj;
    }

    public static explicit operator SqliteParameter(FhParameter parameter) => new(parameter.Name, parameter._obj);
    public static explicit operator SqlParameter(FhParameter parameter) => new(parameter.Name, parameter._obj);
}