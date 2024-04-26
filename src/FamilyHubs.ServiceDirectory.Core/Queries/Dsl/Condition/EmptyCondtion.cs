namespace FamilyHubs.ServiceDirectory.Core.Queries.Dsl.Condition;

public class EmptyCondtion : FhQueryCondition
{
    public static readonly EmptyCondtion Instance = new();
    private EmptyCondtion()
    {
    }

    public override FhQueryCondition And(FhQueryCondition other) => other;
    public override FhQueryCondition Or(FhQueryCondition other) => other;

    public override FhParameter[] AllParameters() => Array.Empty<FhParameter>();
    public override string Format() => string.Empty;
}
