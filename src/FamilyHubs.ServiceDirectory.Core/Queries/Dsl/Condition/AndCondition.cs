namespace FamilyHubs.ServiceDirectory.Core.Queries.Dsl.Condition;

public class AndCondition : FhQueryCondition
{
    private readonly FhQueryCondition _first;
    private readonly FhQueryCondition _second;

    public AndCondition(FhQueryCondition first, FhQueryCondition second)
    {
        _first = first;
        _second = second;
    }

    public override FhParameter[] AllParameters() => _first.AllParameters().Concat(_second.AllParameters()).ToArray();
    public override string Format() => $"{_first.Format()} AND {_second.Format()}";
}
