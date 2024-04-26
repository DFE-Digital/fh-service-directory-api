namespace FamilyHubs.ServiceDirectory.Core.Queries.Dsl;

public class FhQueryOrder
{
    private readonly string _field;
    private readonly Order _order;

    public FhQueryOrder(string field, Order order)
    {
        _field = field;
        _order = order;
    }

    public string Format() => $"{_field} {_order.ToString().ToUpperInvariant()}";

    public static implicit operator FhQueryOrder(string field) => new(field, Order.Asc);

    public enum Order
    {
        Asc, Desc
    }
}
