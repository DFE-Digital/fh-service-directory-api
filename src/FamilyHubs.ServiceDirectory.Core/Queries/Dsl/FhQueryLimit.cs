namespace FamilyHubs.ServiceDirectory.Core.Queries.Dsl;

public class FhQueryLimit
{
    public readonly int Limit;
    public readonly int Offset;

    public FhQueryLimit(int limit, int offset)
    {
        Limit = limit;
        Offset = offset;
    }

    public string Format(bool useSqlite)
    {
        return useSqlite ? $"LIMIT {Offset}, {Limit}" : $"OFFSET {Offset} ROWS FETCH NEXT {Limit} ROWS ONLY";
    }

    public static FhQueryLimit FromPage(int page, int pageSize)
    {
        return new FhQueryLimit(pageSize, (page - 1) * pageSize);
    }

    public FhQueryLimit Minus(int minus) => new(Limit, Offset - minus);
    public FhQueryLimit WithSize(int size) => new(size, Offset);
}