using CSharpModernEvolution.Examples;

namespace CSharpModernEvolution.Tests;

public sealed class PatternMatchingExampleTests
{
    // `decimal` non e ammesso come argomento di attributo: si usa MemberData.
    public static TheoryData<decimal, string> SalaryCases => new()
    {
        { 0m, "Junior" },
        { 24_999m, "Junior" },
        { 25_000m, "Intermediate" },
        { 34_999m, "Intermediate" },
        { 35_000m, "Senior" },
        { 49_999m, "Senior" },
        { 50_000m, "Expert" },
        { 999_999m, "Expert" }
    };

    public static TheoryData<int, string> ScoreCases => new()
    {
        { 0, "Insufficient" },
        { 59, "Insufficient" },
        { 60, "Sufficient" },
        { 69, "Sufficient" },
        { 70, "Good" },
        { 89, "Good" },
        { 90, "Excellent" },
        { 100, "Excellent" },
        { 101, "Invalid" }
    };

    // Nota: il ramo `< 60` intercetta anche i valori negativi, quindi -1 resta "Insufficient".
    // Il braccio `_ => "Invalid"` copre solo i valori superiori a 100.
    public static TheoryData<int, string> InvalidScoreCases => new()
    {
        { 101, "Invalid" },
        { 1_000, "Invalid" }
    };

    public static TheoryData<int, string> ExperienceCases => new()
    {
        { 0, "Junior" },
        { 1, "Junior" },
        { 2, "Intermediate" },
        { 4, "Intermediate" },
        { 5, "Senior" },
        { 9, "Senior" },
        { 10, "Expert" },
        { 40, "Expert" }
    };

    [Theory]
    [MemberData(nameof(SalaryCases))]
    public void EvaluateSalary_AppliesRelationalPatterns(decimal salary, string expected)
    {
        Assert.Equal(expected, PatternMatchingExample.EvaluateSalary(salary));
    }

    [Theory]
    [MemberData(nameof(ScoreCases))]
    public void EvaluateScore_AppliesRelationalPatternsAndFallback(int score, string expected)
    {
        Assert.Equal(expected, PatternMatchingExample.EvaluateScore(score));
    }

    [Theory]
    [MemberData(nameof(ExperienceCases))]
    public void GetExperienceLevel_AppliesRelationalPatterns(int years, string expected)
    {
        var employee = TestData.Employee(yearsOfExperience: years);

        Assert.Equal(expected, PatternMatchingExample.GetExperienceLevel(employee));
    }

    [Fact]
    public void EvaluateSalary_IsExhaustiveWithoutDiscardArm()
    {
        // Non esiste il braccio `_`: il compilatore sa che i quattro rami
        // coprono l'intero dominio dei decimal e non segnala nulla.
        Assert.Equal("Junior", PatternMatchingExample.EvaluateSalary(decimal.MinValue));
        Assert.Equal("Expert", PatternMatchingExample.EvaluateSalary(decimal.MaxValue));
    }

    [Fact]
    public void EvaluateScore_TreatsNegativeScoresAsInsufficient()
    {
        Assert.Equal("Insufficient", PatternMatchingExample.EvaluateScore(-1));
    }

    [Theory]
    [MemberData(nameof(InvalidScoreCases))]
    public void EvaluateScore_FallsBackToInvalidAboveTheMaximum(int score, string expected)
    {
        Assert.Equal(expected, PatternMatchingExample.EvaluateScore(score));
    }
}
