using CSharpModernEvolution.Models;
using CSharpModernEvolution.Services;

namespace CSharpModernEvolution.Tests;

public sealed class ProductTests
{
    [Fact]
    public void ObjectInitializer_SetsRequiredAndOptionalMembers()
    {
        var product = new Product { Name = "Laptop", Price = 1_200m };

        Assert.Equal("Laptop", product.Name);
        Assert.Equal(1_200m, product.Price);
        Assert.Equal("Uncategorized", product.Category);
        Assert.True(product.InStock);
    }

    [Fact]
    public void PriceAfterDiscount_AppliesPercentage()
    {
        var product = new Product { Name = "Laptop", Price = 1_200m };

        Assert.Equal(960m, product.PriceAfterDiscount(20m));
        Assert.Equal(1_200m, product.PriceAfterDiscount(0m));
    }

    [Fact]
    public void Product_IsNotARecord_AndAlsoNotAValueObject()
    {
        var a = new Product { Name = "Mouse" };
        var b = new Product { Name = "Mouse" };

        Assert.NotEqual(a, b);
    }
}

public sealed class SalaryServiceTests
{
    private readonly SalaryService _service = new(25_000m, 55_000m);

    // `decimal` non e ammesso come argomento di attributo: si usa MemberData.
    public static TheoryData<decimal, bool> AboveMinimumCases => new()
    {
        { 24_999m, false },
        { 25_000m, true },
        { 60_000m, true }
    };

    public static TheoryData<decimal, bool> WithinRangeCases => new()
    {
        { 25_000m, true },
        { 55_000m, true },
        { 55_001m, false }
    };

    [Theory]
    [MemberData(nameof(AboveMinimumCases))]
    public void IsAboveMinimum_UsesTheCapturedParameter(decimal salary, bool expected)
    {
        Assert.Equal(expected, _service.IsAboveMinimum(salary));
    }

    [Theory]
    [MemberData(nameof(WithinRangeCases))]
    public void IsWithinRange_CheckesBothBounds(decimal salary, bool expected)
    {
        Assert.Equal(expected, _service.IsWithinRange(salary));
    }

    [Fact]
    public void ApplyBonus_ReturnsANewValue()
    {
        Assert.Equal(33_000m, _service.ApplyBonus(30_000m, 10m));
    }

    [Fact]
    public void GetAboveMinimum_FiltersAndAppliesBonus()
    {
        var employees = new List<Employee>
        {
            TestData.Employee(id: 1, salary: 30_000m),
            TestData.Employee(id: 2, salary: 20_000m),
            TestData.Employee(id: 3, salary: 40_000m)
        };

        var result = _service.GetAboveMinimum(employees, 10m);

        Assert.Equal(2, result.Count);
        Assert.Equal(44_000m, result[0].Salary);
        Assert.Equal(33_000m, result[1].Salary);
    }

    [Fact]
    public void GetAboveMinimum_ThrowsOnNull() =>
        Assert.Throws<ArgumentNullException>(
            () => _service.GetAboveMinimum(null!, 10m));
}

public sealed class RepositoryTests
{
    [Fact]
    public void AddAndGetAll_PreserveInsertionOrder()
    {
        var repository = new Repository<int>();

        repository.Add(3);
        repository.Add(1);
        repository.Add(2);

        Assert.Equal(3, repository.Count);
        Assert.Equal([3, 1, 2], repository.GetAll());
    }

    [Fact]
    public void GetAll_ReturnsTheInternalListThroughAReadOnlyInterface()
    {
        var repository = Repository<string>.From(["a", "b"]);

        IReadOnlyList<string> items = repository.GetAll();

        // Il tipo statico e IReadOnlyList<T>: il caller non ha un metodo Add a disposizione.
        // La vista pero resta viva, quindi riflette le aggiunte successive.
        repository.Add("c");

        Assert.Equal(3, items.Count);
        Assert.Equal(["a", "b", "c"], items);
    }

    [Fact]
    public void From_CopiesTheSourceInOrder()
    {
        var source = new List<int> { 1, 2, 3 };

        var repository = Repository<int>.From(source);
        source.Add(4);

        Assert.Equal(3, repository.Count);
    }

    [Fact]
    public void From_ThrowsOnNull() =>
        Assert.Throws<ArgumentNullException>(() => Repository<int>.From(null!));

    [Fact]
    public void MaxByComparable_ReturnsTheLargestElement()
    {
        Assert.Equal(9, new List<int> { 4, 9, 2 }.MaxByComparable());
        Assert.Equal("z", new List<string> { "x", "z", "a" }.MaxByComparable());
    }

    [Fact]
    public void MaxByComparable_OnEmptyCollectionReturnsDefault()
    {
        Assert.Null(new List<string>().MaxByComparable());
        Assert.Equal(0, new List<int>().MaxByComparable());
    }

    [Fact]
    public void RemoveAndClear_UpdateTheInternalState()
    {
        var repository = Repository<int>.From([1, 2, 3]);

        Assert.True(repository.Remove(2));
        Assert.False(repository.Remove(99));
        Assert.Equal(2, repository.Count);

        repository.Clear();

        Assert.Equal(0, repository.Count);
    }

    [Fact]
    public void Find_UsesThePredicate()
    {
        var repository = Repository<int>.From([1, 2, 3, 4]);

        Assert.Equal([2, 4], repository.Find(x => x % 2 == 0));
    }
}
