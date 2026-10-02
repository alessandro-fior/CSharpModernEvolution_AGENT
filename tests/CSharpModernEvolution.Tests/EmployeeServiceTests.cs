using CSharpModernEvolution.Data;
using CSharpModernEvolution.Services;

namespace CSharpModernEvolution.Tests;

public sealed class EmployeeServiceTests
{
    private static EmployeeService CreateService() => new(SampleData.Employees);

    [Fact]
    public async Task GetEmployeesAsync_ReturnsSeededEmployees()
    {
        var service = CreateService();

        var result = await service.GetEmployeesAsync();

        Assert.Equal(SampleData.Employees.Count, result.Count);
    }

    [Fact]
    public async Task GetEmployeesAsync_WhenNoSeed_ReturnsEmptyList()
    {
        var service = new EmployeeService();

        var result = await service.GetEmployeesAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetActiveAsync_ReturnsOnlyActiveEmployees()
    {
        var service = CreateService();

        var result = await service.GetActiveAsync();

        Assert.NotEmpty(result);
        Assert.All(result, e => Assert.True(e.IsActive));
    }

    [Fact]
    public async Task GetByDepartmentAsync_WithNull_ReturnsEveryone()
    {
        var service = CreateService();

        var result = await service.GetByDepartmentAsync(department: null);

        Assert.Equal(SampleData.Employees.Count, result.Count);
    }

    [Theory]
    [InlineData("it", 3)]
    [InlineData("IT", 3)]
    [InlineData("Finance", 2)]
    [InlineData("Legal", 0)]
    public async Task GetByDepartmentAsync_IsCaseInsensitiveAndHandlesUnknown(string department, int expected)
    {
        var service = CreateService();

        var result = await service.GetByDepartmentAsync(department);

        Assert.Equal(expected, result.Count);
        Assert.All(result, e => Assert.Equal(department, e.Department, ignoreCase: true));
    }

    [Fact]
    public async Task GetTopEarnerAsync_ReturnsHighestSalary()
    {
        var service = CreateService();

        var result = await service.GetTopEarnerAsync();

        Assert.NotNull(result);
        Assert.Equal(SampleData.Employees.Max(e => e.Salary), result!.Salary);
    }

    [Fact]
    public async Task GetTopEarnerAsync_WhenEmpty_ReturnsNull()
    {
        var service = new EmployeeService();

        var result = await service.GetTopEarnerAsync();

        Assert.Null(result);
    }

    [Fact]
    public async Task GetDepartmentSummariesAsync_GroupsByDepartmentAndOrdersByAverage()
    {
        var service = CreateService();

        var summaries = await service.GetDepartmentSummariesAsync();

        var expectedDepartments = SampleData.Employees.Select(e => e.Department).Distinct().Count();
        Assert.Equal(expectedDepartments, summaries.Count);
        Assert.Equal(SampleData.Employees.Count, summaries.Sum(s => s.HeadCount));
        Assert.Equal(
            summaries.Select(s => s.AverageSalary).OrderByDescending(v => v),
            summaries.Select(s => s.AverageSalary));
    }

    [Fact]
    public async Task GetEvaluationsAsync_ProducesOneEvaluationPerEmployee()
    {
        var service = CreateService();

        var evaluations = await service.GetEvaluationsAsync();

        Assert.Equal(SampleData.Employees.Count, evaluations.Count);
        Assert.Contains(evaluations, e => e.IsSenior);
    }

    [Fact]
    public async Task GetEmployeesAsync_HonoursCancellationToken()
    {
        var service = CreateService();
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => service.GetEmployeesAsync(cts.Token));
    }

    [Fact]
    public void Snapshot_ReturnsACopy()
    {
        var service = CreateService();

        var first = service.Snapshot();
        var second = service.Snapshot();

        Assert.NotSame(first, second);
        Assert.Equal(first, second);
    }

    [Fact]
    public void GetEmployeesBlocking_MatchesAsyncResult()
    {
        var service = CreateService();

        var blocking = service.GetEmployeesBlocking();

        Assert.Equal(SampleData.Employees.Count, blocking.Count);
    }
}
