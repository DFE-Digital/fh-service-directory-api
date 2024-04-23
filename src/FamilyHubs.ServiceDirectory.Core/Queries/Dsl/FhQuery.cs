using FamilyHubs.ServiceDirectory.Core.Queries.Dsl.Condition;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Dsl;

public sealed class FhQuery
{
    private readonly string _table;
    private IEnumerable<string> _fields;
    private IEnumerable<FhJoin> _joins = new List<FhJoin>();
    private FhQueryCondition _condition = EmptyCondtion.Instance;
    private IEnumerable<string> _groupBy = new List<string>();
    private IEnumerable<FhQueryOrder> _orderBy = new List<FhQueryOrder>();
    public FhQueryLimit FhQueryLimit { get; private set; } = FhQueryLimit.FromPage(1, 10);

    public FhQuery(string table, params string[] fields)
    {
        _table = table;
        _fields = fields;
    }

    private FhQuery(string table, IEnumerable<string> fields, IEnumerable<FhJoin> joins, FhQueryCondition condition,
        IEnumerable<string> groupBy, IEnumerable<FhQueryOrder> orderBy, FhQueryLimit fhQueryLimit)
    {
        _table = table;
        _fields = fields;
        _joins = joins;
        _condition = condition;
        _groupBy = groupBy;
        _orderBy = orderBy;
        FhQueryLimit = fhQueryLimit;
    }

    public string Format(bool useSqlite, bool includeGroupBy = true, bool includeOrderBy = true, bool includeLimit = true)
    {
        var joins = (_joins.Any() ? " " : "") + string.Join(" ", _joins.Select(j => j.Format()));
        var groupBy = includeGroupBy && _groupBy.Any() ? $" GROUP BY {string.Join(", ", _groupBy)}" : "";
        var orderBy = includeOrderBy && _orderBy.Any() ? $" ORDER BY {string.Join(", ", _orderBy.Select(o => o.Format()))}" : "";
        var limit = includeLimit ? FhQueryLimit.Format(useSqlite) : "";
        var where = _condition != EmptyCondtion.Instance ? $" WHERE {_condition.Format()}" : "";
        return $"SELECT {string.Join(",", _fields)} FROM {_table}{joins}{where}{groupBy}{orderBy} {limit}";
    }

    public object[] AllParameters(bool useSqlite)
    {
        return _condition.AllParameters(useSqlite);
    }

    public FhQuery And(FhQueryCondition condition)
    {
        _condition = _condition.And(condition);
        return this;
    }

    public FhQuery AndWhen(bool when, FhQueryCondition condition)
    {
        if (when)
        {
            And(condition);
        }

        return this;
    }

    public FhQuery AndNotNull(string? str, Func<string, FhQueryCondition> conditionBuilder)
    {
        if (!string.IsNullOrEmpty(str))
        {
            And(conditionBuilder.Invoke(str));
        }

        return this;
    }

    public FhQuery AndNotNull(bool? obj, Func<bool, FhQueryCondition> conditionBuilder)
    {
        if (obj is not null)
        {
            And(conditionBuilder.Invoke(obj.Value));
        }

        return this;
    }

    public FhQuery Or(FhQueryCondition condition)
    {
        _condition = _condition.Or(condition);
        return this;
    }

    public FhQuery SetLimit(FhQueryLimit fhQueryLimit)
    {
        FhQueryLimit = fhQueryLimit;
        return this;
    }

    public FhQuery Join(FhJoin.Type type, string table, string condition)
    {
        _joins = _joins.Append(new FhJoin(type, table, condition));
        return this;
    }

    public FhQuery GroupBy(params string[] fields)
    {
        _groupBy = fields;
        return this;
    }

    public FhQuery OrderBy(params FhQueryOrder[] order)
    {
        _orderBy = order;
        return this;
    }

    public FhQuery Clone() => new(_table, _fields, _joins, _condition, _groupBy, _orderBy, FhQueryLimit);

    public FhQuery AddFields(params string[] fields)
    {
        _fields = _fields.Concat(fields);
        return this;
    }

    public FhQuery AddOrderBy(params FhQueryOrder[] order)
    {
        _orderBy = _orderBy.Concat(order);
        return this;
    }

    private bool Equals(FhQuery other)
    {
        return _table == other._table && _fields.Equals(other._fields) && _joins.Equals(other._joins) && _condition.Equals(other._condition) &&
               _groupBy.Equals(other._groupBy) && _orderBy.Equals(other._orderBy) && FhQueryLimit.Equals(other.FhQueryLimit);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((FhQuery)obj);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}