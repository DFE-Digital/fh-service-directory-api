using FamilyHubs.ServiceDirectory.Core.Queries.Dsl;
using FamilyHubs.ServiceDirectory.Core.Queries.Dsl.Condition;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.QueryDsl;

public class Simple
{
    [Theory]
    [InlineData(false, "SELECT e.Id FROM [Example] e OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY")]
    [InlineData(true, "SELECT e.Id FROM [Example] e LIMIT 0, 10")]
    public void BasicQuery(bool useSqlite, string expected)
    {
        // Arrange
        var query = new FhQuery("[Example] e", "e.Id");

        // Act
        var sql = query.Format(useSqlite);
        var pArr = query.AllParameters(useSqlite);

        // Assert
        sql.Should().Be(expected);
        pArr.Should().BeEmpty();
    }

    [Theory]
    [InlineData(false, "SELECT e.Id,e.testId,e.additionalField FROM [Example] e INNER JOIN [Test] t ON e.testId = t.Id WHERE (e.Example = @Example OR t.Codes IN (@Codes0, @Codes1)) GROUP BY e.Group ORDER BY e.Order ASC, e.ThenOrder DESC OFFSET 5 ROWS FETCH NEXT 5 ROWS ONLY", 3)]
    [InlineData(true, "SELECT e.Id,e.testId,e.additionalField FROM [Example] e INNER JOIN [Test] t ON e.testId = t.Id WHERE (e.Example = @Example OR t.Codes IN (@Codes0, @Codes1)) GROUP BY e.Group ORDER BY e.Order ASC, e.ThenOrder DESC LIMIT 5, 5", 3)]
    public void ComplexQuery(bool useSqlite, string expected, int paramLength)
    {
        // Arrange
        var query = new FhQuery("[Example] e", "e.Id", "e.testId")
            .AddFields("e.additionalField")
            .Join(FhJoin.Type.Inner, "[Test] t", "e.testId = t.Id")
            .And(new StringCondition("e.Example", "Example", "Test"))
            .Or(new InCondition("t.Codes", "Codes", "EN,GB"))
            .GroupBy("e.Group").OrderBy("e.Order").AddOrderBy(new FhQueryOrder("e.ThenOrder", FhQueryOrder.Order.Desc))
            .SetLimit(FhQueryLimit.FromPage(2, 10).WithSize(5).Minus(5));

        // Act
        var cloned = query.Clone();
        var sql = query.Format(useSqlite);
        var pArr = query.AllParameters(useSqlite);

        // Assert
        query.Should().NotBeSameAs(cloned);
        query.Should().Be(cloned);

        sql.Should().Be(expected);
        pArr.Should().HaveCount(paramLength);
    }

    [Theory]
    [InlineData(false, "SELECT e.Id,e.testId FROM [Example] e WHERE (e.Test IS NULL AND Not Used AND e.TestBool = @ParamBool OR t.Codes IN (@Codes0, @Codes1)) OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY", 3)]
    [InlineData(true, "SELECT e.Id,e.testId FROM [Example] e WHERE (e.Test IS NULL AND Not Used AND e.TestBool = @ParamBool OR t.Codes IN (@Codes0, @Codes1)) LIMIT 0, 10", 3)]
    public void Conditionals(bool useSqlite, string expected, int paramLength)
    {
        // Arrange
        string? nullableNull = null;
        var nullable = "Example";
        bool? nullableBool = false;

        var query = new FhQuery("[Example] e", "e.Id", "e.testId")
            .AndWhen(false, new StringCondition("Not Used"))
            .AndWhen(true, new StringCondition("e.Test IS NULL"))
            .AndNotNull(nullable, _ => new StringCondition("Not Used"))
            .AndNotNull(nullableNull, s => new StringCondition("e.Test", "Param", s))
            .AndNotNull(nullableBool, b => new StringCondition("e.TestBool = @ParamBool", new FhParameter("@ParamBool", b)))
            .Or(new InCondition("t.Codes", "Codes", "EN,GB"));

        // Act
        var sql = query.Format(useSqlite);
        var pArr = query.AllParameters(useSqlite);

        // Assert
        sql.Should().Be(expected);
        pArr.Should().HaveCount(paramLength);
    }
}