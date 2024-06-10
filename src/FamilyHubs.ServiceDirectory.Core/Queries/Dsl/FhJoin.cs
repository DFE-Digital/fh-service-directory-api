namespace FamilyHubs.ServiceDirectory.Core.Queries.Dsl;

public class FhJoin
{
    private readonly Type _type;
    private readonly string _table;
    private readonly string _condition;

    public FhJoin(Type type, string table, string condition)
    {
        _type = type;
        _table = table;
        _condition = condition;
    }

    public string Format() => $"{_type.ToString().ToUpperInvariant()} JOIN {_table} ON {_condition}";

    public enum Type
    {
        Left, Right, Inner, Outer
    }
}
