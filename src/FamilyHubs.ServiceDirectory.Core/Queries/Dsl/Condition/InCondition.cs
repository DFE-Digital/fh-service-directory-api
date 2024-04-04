namespace FamilyHubs.ServiceDirectory.Core.Queries.Dsl.Condition;

public class InCondition : FhQueryCondition
{
    private readonly string _condition;
    private readonly FhParameter[] _parameters;

    public InCondition(string field, string friendlyName, string ids) : this(field, friendlyName, ids.Split(","))
    {
    }

    public InCondition(string field, string friendlyName, IEnumerable<object> id)
    {
        _parameters = id.Select((obj, idx) => new FhParameter($"@{friendlyName}{idx}", obj)).ToArray();
        _condition = $"{field} IN ({string.Join(", ", _parameters.Select(p => p.Name))})";
    }

    public override FhParameter[] AllParameters() => _parameters;
    public override string Format() => _condition;
}