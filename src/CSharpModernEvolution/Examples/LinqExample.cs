using CSharpModernEvolution.Data;
using CSharpModernEvolution.Models;
using CSharpModernEvolution.Services;

namespace CSharpModernEvolution.Examples;

/// <summary>
/// Sezione 6 - Lambda expressions e LINQ.
/// <para>
/// Le lambda expressions sono state introdotte con <b>C# 3.0</b>; LINQ è la libreria
/// (<c>System.Linq</c>) che le rende utili. Insieme sono una delle trasformazioni
/// fondamentali del C# moderno: permettono di descrivere <em>cosa</em> si vuole ottenere
/// invece di come ottenerlo, con catene di trasformazioni componibili.
/// </para>
/// </summary>
public static class LinqExample
{
    public static void Run()
    {
        Demo.Section("6. Lambda e LINQ (C# 3.0) - descrivere il risultato, non i passaggi");

        IReadOnlyList<Employee> employees = SampleData.Employees;

        // Where + OrderByDescending + Select (proiezione anonima) + ToList
        var experienced = employees
            .Where(e => e.YearsOfExperience >= 10)
            .OrderByDescending(e => e.Salary)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.Salary
            })
            .ToList();

        Demo.SubTitle("Where + OrderByDescending + Select + ToList");
        foreach (var e in experienced)
        {
            Demo.Item($"{e.FirstName} {e.LastName} -> {e.Salary,10:C}");
        }

        Demo.SubTitle("OrderBy (crescente) e OrderByDescending (decrescente)");
        foreach (var e in employees.OrderBy(e => e.Salary).ThenBy(e => e.FirstName))
        {
            Demo.Item($"{e.FullName,-16} {e.Salary,10:C}");
        }

        Demo.SubTitle("Any / All / Count");
        Demo.Line("Any(Salary > 50.000)", employees.Any(e => e.Salary > 50_000m));
        Demo.Line("All(IsActive)", employees.All(e => e.IsActive));
        Demo.Line("Count(YearsOfExperience >= 15)", employees.Count(e => e.YearsOfExperience >= 15));

        Demo.SubTitle("FirstOrDefault e SingleOrDefault");
        var itManager = employees.FirstOrDefault(e => e.Department == "HR");
        Demo.Line("FirstOrDefault(HR)", itManager?.FullName ?? "NON DEFINITO");
        Demo.Line("FirstOrDefault(reparto inesistente)", employees.FirstOrDefault(e => e.Department == "Legal")?.FullName ?? "NON DEFINITO");
        Demo.Line("MaxBy(Salary)", employees.MaxBy(e => e.Salary)?.FullName ?? "NON DEFINITO");

        Demo.SubTitle("GroupBy con proiezione anonima");
        var byDepartment = employees
            .GroupBy(e => e.Department)
            .Select(g => new
            {
                Department = g.Key,
                Count = g.Count(),
                TotalCost = g.Sum(e => e.AnnualCost),
                AverageSalary = g.Average(e => e.Salary),
                Seniors = g.Count(e => e.YearsOfExperience >= 10)
            })
            .OrderByDescending(g => g.TotalCost);

        foreach (var g in byDepartment)
        {
            Demo.Item($"{g.Department,-10} n={g.Count}  media={g.AverageSalary,10:C}  costo={g.TotalCost,13:C}  senior={g.Seniors}");
        }

        Demo.SubTitle("Aggregazioni: Sum / Average / Min / Max");
        Demo.Line("Sum(AnnualCost)", employees.Sum(e => e.AnnualCost).ToString("C"));
        Demo.Line("Average(AnnualCost)", employees.Average(e => e.AnnualCost).ToString("C"));
        Demo.Line("Min(Salary)", employees.Min(e => e.Salary).ToString("C"));
        Demo.Line("Max(Salary)", employees.Max(e => e.Salary).ToString("C"));

        Demo.SubTitle("Query expression: stesso risultato, sintassi alternativa");
        var query = from e in employees
                    where e is { IsActive: true }
                    orderby e.Salary descending
                    select new
                    {
                        e.FullName,
                        e.Department,
                        e.Salary
                    };

        foreach (var row in query.Take(3))
        {
            Demo.Item($"{row.FullName,-16} {row.Department,-10} {row.Salary,10:C}");
        }

        Demo.SubTitle("Deferred execution: la query non parte finche non la enumeri");
        var deferred = employees.Where(e => e.Salary > 30_000m);
        Demo.Line("query creata", "nessun accesso ai dati");
        Demo.Line("prima enumerazione", $"{deferred.Count()} elementi filtrati");
    }
}
