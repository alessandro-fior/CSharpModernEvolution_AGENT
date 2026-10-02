using CSharpModernEvolution.Data;
using CSharpModernEvolution.Models;
using CSharpModernEvolution.Services;

namespace CSharpModernEvolution.Examples;

/// <summary>
/// Sezione 5 - Generics (C# 2.0).
/// </summary>
public static class GenericsExample
{
    public static void Run()
    {
        Demo.Section("5. Generics (C# 2.0) - un solo tipo che lavora con molti tipi");

        Demo.SubTitle("Repository<T> usato con tre tipi diversi");
        var employees = new Repository<Employee>();
        foreach (var e in SampleData.Employees)
        {
            employees.Add(e);
        }

        var products = Repository<Product>.From(SampleData.Products);

        // Un solo tipo generico, tre istanze specializzate dal compilatore.
        Demo.Line("Repository<Employee>.Count", employees.Count);
        Demo.Line("Repository<Product>.Count", products.Count);
        Demo.Line("Repository<Department>.Count", new Repository<Department>().Count);
        Demo.Line("Repository<decimal>.MaxByComparable()", new List<decimal> { 1m, 2m, 3m }.MaxByComparable());
        Demo.Line("Repository<string>.MaxByComparable()", new List<string> { "uno", "tre", "due" }.MaxByComparable());
        Demo.Line("MaxBy(Salary) sui record", employees.GetAll().MaxBy(e => e.Salary)?.FullName);

        Demo.SubTitle("Repository<T> con due vincoli diversi sullo stesso tipo base");
        Demo.Line("Repository<Employee> (record)", "value equality: due Employee con stessi dati sono uguali");
        Demo.Line("Repository<Employee>", employees.GetAll().Count(e => e.Department == "IT"));
        Demo.Line("Repository<string> (per vincolo)", new Repository<string>().Count);

        Demo.SubTitle("Value equality offerta dai record (C# 9)");
        var marco = SampleData.Employees[0];
        var clonedMarco = marco with { };
        Demo.Line("marco == marco with { }", ReferenceEquals(marco, clonedMarco) ? "stessa istanza" : "istanze diverse");
        Demo.Line("marco.Equals(marco with { })", marco.Equals(clonedMarco));
        Demo.Line("marco == marco.Promote(1000)", marco.Equals(marco.Promote(1000m)));
    }
}

/// <summary>Tipo di dominio usato solo per mostrare che <c>Repository&lt;T&gt;</c> non ha limiti.</summary>
public sealed record Department(string Name, string Manager);
