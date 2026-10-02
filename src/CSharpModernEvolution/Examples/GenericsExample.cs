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

        var employees = new Repository<Employee>();
        foreach (var e in SampleData.Employees)
        {
            employees.Add(e);
        }

        var products = Repository<Product>.From(SampleData.Products);

        Demo.SubTitle("Un solo tipo generico, istanziazioni per tre tipi diversi");
        Demo.Line("Repository<Employee>.Count", employees.Count);
        Demo.Line("Repository<Product>.Count", products.Count);
        Demo.Line("Repository<Department>.Count", new Repository<Department>().Count);
        Demo.Note(
            "Repository<T> e lo stesso tipo per tutti: e il compilatore a creare le\n" +
            "specializzazioni. Nessun cast, nessun boxing, nessun object come scappatoia.");

        Demo.SubTitle("Un metodo generico puo pero restringere con 'where'");
        Demo.Line("List<decimal>.MaxByComparable()", new List<decimal> { 1m, 2m, 3m }.MaxByComparable());
        Demo.Line("List<string>.MaxByComparable()", new List<string> { "uno", "tre", "due" }.MaxByComparable());
        Demo.Note(
            "`where T : IComparable<T>` abilita gli operatori di confronto nel corpo del metodo.\n" +
            "Se il tipo non soddisfa il vincolo, il codice che invoca MaxByComparable non compila.\n" +
            "Employee non implementa IComparable<T> e non ne ha bisogno: per i record l'uguaglianza\n" +
            "per valore e automatica e l'ordinamento si fa con OrderBy/MaxBy.");

        Demo.SubTitle("Value equality offerta dai record (C# 9)");
        var marco = SampleData.Employees[0];
        var clonedMarco = marco with { };
        Demo.Line("marco.Equals(marco with { })", marco.Equals(clonedMarco));
        Demo.Line("marco == marco with { }", marco == clonedMarco);
        Demo.Line("ReferenceEquals(marco, copia)", ReferenceEquals(marco, clonedMarco));
        Demo.Line("marco.Equals(marco.Promote(1000))", marco.Equals(marco.Promote(1_000m)));
        Demo.Line("employees.MaxBy(e => e.Salary)", employees.GetAll().MaxBy(e => e.Salary)?.FullName);
    }
}

/// <summary>Tipo di dominio usato solo per mostrare che <c>Repository&lt;T&gt;</c> non ha limiti.</summary>
public sealed record Department(string Name, string Manager);
