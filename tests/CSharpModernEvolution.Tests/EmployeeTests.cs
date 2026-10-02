using CSharpModernEvolution.Models;

namespace CSharpModernEvolution.Tests;

public sealed class EmployeeTests
{
    [Fact]
    public void Equals_UsesValueEquality()
    {
        var a = TestData.Employee();
        var b = TestData.Employee();

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.NotSame(a, b);
    }

    [Fact]
    public void Equals_ComparesEveryPositionalProperty()
    {
        var original = TestData.Employee();
        var same = TestData.Employee();
        var different = TestData.Employee(salary: original.Salary + 1m);

        Assert.True(original.Equals(same));
        Assert.False(original.Equals(different));
    }

    [Fact]
    public void With_ProducesANewInstanceAndLeavesTheOriginalUntouched()
    {
        var original = TestData.Employee(salary: 40_000m);

        var promoted = original with { Salary = 43_000m };

        Assert.Equal(40_000m, original.Salary);
        Assert.Equal(43_000m, promoted.Salary);
        Assert.NotSame(original, promoted);
        Assert.Equal(original.Id, promoted.Id);
    }

    [Fact]
    public void Promote_UsesWithExpression()
    {
        var original = TestData.Employee(salary: 40_000m);

        var promoted = original.Promote(3_000m);

        Assert.Equal(43_000m, promoted.Salary);
        Assert.Equal(40_000m, original.Salary);
    }

    [Fact]
    public void Deactivate_ReturnsInactiveCopy()
    {
        var original = TestData.Employee();

        Assert.False(original.Deactivate().IsActive);
        Assert.True(original.IsActive);
    }

    [Fact]
    public void ComputedProperties_DeriveFromPositionalProperties()
    {
        var employee = TestData.Employee(firstName: "Marco", lastName: "Rossi", salary: 40_000m);

        Assert.Equal("Marco Rossi", employee.FullName);
        Assert.Equal(520_000m, employee.AnnualCost);
    }

    [Fact]
    public void ToString_ContainsThePositionalValues()
    {
        var text = TestData.Employee(id: 42).ToString();

        Assert.Contains("42", text, StringComparison.Ordinal);
        Assert.Contains("Marco", text, StringComparison.Ordinal);
    }

    [Fact]
    public void EmployeeCanBeUsedAsDictionaryKey_BecauseEqualityIsByValue()
    {
        var dictionary = new Dictionary<Employee, string>
        {
            [TestData.Employee()] = "primo"
        };

        Assert.True(dictionary.TryGetValue(TestData.Employee(), out var value));
        Assert.Equal("primo", value);
    }
}

public sealed class EvaluationTests
{
    [Theory]
    [InlineData("Junior", false)]
    [InlineData("Intermediate", false)]
    [InlineData("Senior", true)]
    [InlineData("Expert", true)]
    public void IsSenior_UsesOrPattern(string level, bool expected)
    {
        Assert.Equal(expected, new Evaluation("Test", level, "Senior").IsSenior);
    }

    [Fact]
    public void WithExperienceLevel_KeepsTheOriginalIntact()
    {
        var evaluation = new Evaluation("Marco Rossi", "Expert", "Expert");

        var junior = evaluation with { ExperienceLevel = "Junior" };

        Assert.Equal("Expert", evaluation.ExperienceLevel);
        Assert.Equal("Junior", junior.ExperienceLevel);
        Assert.Equal(evaluation.SalaryBand, junior.SalaryBand);
    }
}
