namespace FamilyHubs.ServiceDirectory.Core.Queries.Dsl.Condition;

public class StringCondition : FhQueryCondition
{
    private readonly string _condition;
    private readonly FhParameter[] _parameters;

    public StringCondition(string condition, params FhParameter[] parameters)
    {
        _condition = condition;
        _parameters = parameters;
    }

    public StringCondition(string dbName, string paramName, string val)
    {
        _condition = $"{dbName} = @{paramName}";
        _parameters = new FhParameter[]
        {
            new($"@{paramName}", val)
        };
    }

    public override FhParameter[] AllParameters() => _parameters;
    public override string Format() => _condition;
}