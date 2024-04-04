using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Dsl.Condition;

public abstract class FhQueryCondition
{
    public virtual FhQueryCondition And(FhQueryCondition other) => new AndCondition(this, other);
    public virtual FhQueryCondition Or(FhQueryCondition other) => new OrCondition(this, other);

    public object[] AllParameters(bool useSqlite)
    {
        return AllParameters().Select(param => (object) (useSqlite ? (SqliteParameter) param : (SqlParameter) param)).ToArray();
    }

    public abstract FhParameter[] AllParameters();
    public abstract string Format();
}
