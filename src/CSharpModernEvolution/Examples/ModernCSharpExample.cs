using System.Globalization;
using System.Text.Json;

using CSharpModernEvolution.Data;
using CSharpModernEvolution.Models;
using CSharpModernEvolution.Services;

namespace CSharpModernEvolution.Examples;

/// <summary>
/// Sezioni 8 e 12-17 - le funzionalità moderne: nullable reference types, record,
/// <c>init</c>, <c>required</c>, primary constructor, collection expressions,
/// raw string literals e le API di collezione introdotte con .NET 6.
/// </summary>
public static class ModernCSharpExample
{
    public static void Run()
    {
        Records();
        InitAndRequired();
        PrimaryConstructor();
        CollectionExpressions();
        RawStringLiterals();
        NullableReferenceTypes();
        ModernCollectionApis();
    }

    // -----------------------------------------------------------------
    // Sezione 12 - record, value equality, with
    // -----------------------------------------------------------------
    private static void Records()
    {
        Demo.Section("12. record (C# 9) - equality per valore e copia non distruttiva con 'with'");

        var marco = SampleData.Employees[0];
        var giulia = SampleData.Employees[1];

        Demo.Line("marco", marco);
        Demo.Line("ToString() generato", $"{marco.Id} {marco.FullName} - {marco.Department}");

        Demo.SubTitle("Value-based equality");
        Demo.Line("marco.Equals(marco with { })", marco.Equals(marco with { }));
        Demo.Line("marco.Equals(giulia)", marco.Equals(giulia));
        Demo.Line("== (stesso operatore di Equals)", marco == marco with { });

        Demo.SubTitle("Immutabilita orientata al design: 'with' crea una copia");
        var promoted = marco with { Salary = marco.Salary + 3_000m };
        Demo.Line("matricola", promoted.Id);
        Demo.Line("RAL originale", marco.Salary.ToString("C", CultureInfo.CurrentCulture));
        Demo.Line("RAL dopo la copia", promoted.Salary.ToString("C", CultureInfo.CurrentCulture));
        Demo.Line("l'originale e rimasto invariato", marco.Salary.ToString("C", CultureInfo.CurrentCulture));
        Demo.Line("i due record sono lo stesso oggetto", ReferenceEquals(marco, promoted));

        Demo.SubTitle("Metodi che incapsulano il 'with'");
        Demo.Line("marco.Promote(3000)", marco.Promote(3_000m).Salary.ToString("C", CultureInfo.CurrentCulture));
        Demo.Line("marco.Deactivate().IsActive", marco.Deactivate().IsActive);
        Demo.Line("marco.IsActive (originale)", marco.IsActive);

        Demo.SubTitle("Record come DTO di risultato: equality e 'with' utili nei test");
        var evaluation = new Evaluation("Marco Rossi", GetExperienceLevelFor(marco), EvaluateSalaryFor(marco.Salary));
        Demo.Line("evaluation", evaluation.Summary);
        Demo.Line("IsSenior", evaluation.IsSenior);
        Demo.Line("WithExperienceLevel(\"Junior\")", evaluation.WithExperienceLevel("Junior").Summary);
        Demo.Line("l'originale e rimasto invariato", evaluation.Summary);
    }

    // -----------------------------------------------------------------
    // Sezioni 13 e 14 - init e required
    // -----------------------------------------------------------------
    private static void InitAndRequired()
    {
        Demo.Section("13-14. 'init' (C# 9) e 'required' (C# 11)");

        // `Name` è `required`: omittinglo non compila.
        var product = new Product
        {
            Name = "Laptop",
            Price = 1_200m
        };

        Demo.Line("product", product.ToString());
        Demo.Line("Category di default", product.Category);
        Demo.Line("sconto 20%", product.PriceAfterDiscount(20m).ToString("C", CultureInfo.CurrentCulture));

        Demo.Note(
            "Con `set` la proprieta resterebbe scrivibile per tutta la vita dell'oggetto;\n" +
            "con `init` e scrivibile solo durante l'inizializzazione: l'oggetto e 'freeze'.\n" +
            "`required` e una promessa fatta al compilatore: il chiamante DEVE inizializzare\n" +
            "la proprieta, altrimenti il codice non compila.");

        Demo.SubTitle("required impedisce l'omissione, non un valore vuoto");
        Demo.Line("new Product { Name = \"\" }", new Product { Name = string.Empty }.ToString());
        Demo.Line("new Product { } (senza Name)", "errore di compilazione CS9035");

        Demo.SubTitle("record vs class: dove finisce 'with'");
        Demo.Line("Employee con with", "consentito: Employee e un record");
        Demo.Line("Product con with", "non consentito: Product e una class");
    }

    // -----------------------------------------------------------------
    // Sezione 15 - primary constructor
    // -----------------------------------------------------------------
    private static void PrimaryConstructor()
    {
        Demo.Section("15. Primary constructor (C# 12)");

        var service = new SalaryService(25_000m, 55_000m);

        Demo.SubTitle("Il vincolo puo essere posizionale (sintassi C# 12)");
        Demo.Line("MinimumSalary", service.MinimumSalary.ToString("C", CultureInfo.CurrentCulture));
        Demo.Line("MaximumSalary", service.MaximumSalary.ToString("C", CultureInfo.CurrentCulture));
        Demo.Line("IsAboveMinimum(30.000)", service.IsAboveMinimum(30_000m));
        Demo.Line("IsAboveMinimum(20.000)", service.IsAboveMinimum(20_000m));
        Demo.Line("IsWithinRange(60.000)", service.IsWithinRange(60_000m));
        Demo.Line("ApplyBonus(30.000, 10%)", service.ApplyBonus(30_000m, 10m).ToString("C", CultureInfo.CurrentCulture));

        Demo.Note(
            "La sintassi `new SalaryService(25_000m, 55_000m)` produce lo stesso risultato\n" +
            "del costruttore classico dichiarato a mano: meno codice, stessa semantica.");
    }

    // -----------------------------------------------------------------
    // Sezione 16 - collection expressions
    // -----------------------------------------------------------------
    private static void CollectionExpressions()
    {
        Demo.Section("16. Collection expressions (C# 12) e spread operator");

        List<string> departments =
        [
            "IT",
            "Finance",
            "HR",
            "Sales"
        ];

        Demo.Line("List<string> departments", $"[{string.Join(", ", departments)}]");

        string[] allDepartments =
        [
            .. departments,
            "Management"
        ];

        Demo.Line("con spread [..departments, ...]", $"[{string.Join(", ", allDepartments)}]");

        Demo.SubTitle("La stessa sintassi per array, HashSet e Dictionary");
        int[] primes = [2, 3, 5, 7, 11];
        HashSet<string> tags = ["console", "csharp", "modern"];
        Dictionary<string, int> headcount = new()
        {
            ["IT"] = 3,
            ["Finance"] = 2
        };

        Demo.Line("int[] primes", $"[{string.Join(", ", primes)}]");
        Demo.Line("HashSet<string> tags", $"[{string.Join(", ", tags)}]");
        Demo.Line("Dictionary<string, int>", $"[{string.Join(", ", headcount.Select(kv => $"{kv.Key}:{kv.Value}"))}]");

        Demo.SubTitle("Espressioni condizionali dentro una collection expression");
        List<int> activeIds = [.. SampleData.Employees.Where(e => e.IsActive).Select(e => e.Id)];
        Demo.Line("id dei dipendenti attivi", $"[{string.Join(", ", activeIds)}]");

        Demo.Note(
            "Prima di C# 12 serviva `new List<string> { ... }`\n" +
            "oppure `new[] { ... }.ToList()` per ottenere lo stesso risultato.");
    }

    // -----------------------------------------------------------------
    // Sezione 17 - raw string literals
    // -----------------------------------------------------------------
    private static void RawStringLiterals()
    {
        Demo.Section("17. Raw string literals (C# 11)");

        // Il delimitatore """ delimita il contenuto: niente \n, niente \" , niente + per andare a capo.
        string query = """
            SELECT e.Id, e.FirstName, e.LastName, e.Department
            FROM   Employee e
            WHERE  e.IsActive = 1
              AND  e.Salary >= 30000
            ORDER BY e.Salary DESC;
            """;

        Demo.Line("SQL su piu righe", $"{query.Split('\n').Length} righe");
        Demo.Line("contenuto", query.Replace("\r\n", " | ").Replace("\n", " | "));

        // Con due `$` le interpolazioni sono delimitate da {{ }}: le graffe singole restano letterali.
        var name = "Marco Rossi";
        var salary = 52_000m;
        var department = "IT";

        string template = $$"""
            Dipendente: {{name}}
            RAL: {{salary:C}}
            Reparto: {{department}}
            """;

        Demo.Line("template interpolato", template.Replace("\r\n", " | ").Replace("\n", " | "));

        Demo.Note(
            "Con le stringhe verbatim (\"\": anteprima) serviva un \\r\\n esplicito per ogni riga.\n" +
            "Con le raw string literals il testo si scrive cosi com'e: l'indentazione comune\n" +
            "del blocco viene rimossa automaticamente in fase di compilazione.");

        Demo.SubTitle("Serializzazione con System.Text.Json");
        var json = JsonSerializer.Serialize(
            SampleData.Employees.Take(2),
            new JsonSerializerOptions { WriteIndented = true });

        foreach (var line in json.Split('\n'))
        {
            Demo.Item(line.TrimEnd('\r'));
        }
    }

    // -----------------------------------------------------------------
    // Sezione 8 - nullable reference types
    // -----------------------------------------------------------------
    private static void NullableReferenceTypes()
    {
        Demo.Section("8. Nullable reference types (C# 8) - prevenire i NullReferenceException");

        string? department = null;

        Demo.Line("department = null", department?.ToUpper() ?? "NON DEFINITO");

        department = "Human Resources";

        Demo.Line("department = \"Human Resources\"", department?.ToUpper() ?? "NON DEFINITO");

        Demo.Line("department?.Length", department?.Length);
        Demo.Line("department!.Length", department!.Length);

        Demo.SubTitle("Operatore null-conditional su catena");
        var manager = FindDepartmentManager(department);
        Demo.Line("FindDepartmentManager(\"IT\")", FindDepartmentManager("IT") ?? "NON DEFINITO");
        Demo.Line("FindDepartmentManager(department)", manager ?? "NON DEFINITO");
        Demo.Line("FindDepartmentManager(\"Legal\")", FindDepartmentManager("Legal") ?? "NON DEFINITO");

        Demo.SubTitle("Assegnazione null-safe con ??=");
        string? notes = null;
        notes ??= "valore di default applicato solo se necessario";
        Demo.Line("notes ??=", notes);

        Demo.Note(
            "Una variabile non-nullable come `string` non puo ricevere null: il compilatore\n" +
            "avvisa con CS8600/CS8602 e il difetto viene trovato prima di eseguire il codice.\n" +
            "Gli operatori usati: `?` sul tipo, `?.` per l'accesso in catena, `??` per il\n" +
            "fallback, `??=` per l'assegnazione condizionale, `!` per l'asserzione esplicita.");
    }

    // -----------------------------------------------------------------
    // API di collezione moderne (.NET 6+)
    // -----------------------------------------------------------------
    private static void ModernCollectionApis()
    {
        Demo.Section("API di collezione moderne (.NET 6+)");

        IReadOnlyList<Employee> employees = SampleData.Employees;

        Demo.SubTitle("Chunk: divide in gruppi di dimensione fissa");
        foreach (var chunk in employees.Chunk(4))
        {
            Demo.Item($"gruppo di {chunk.Length}: {string.Join(", ", chunk.Select(e => e.FirstName))}");
        }

        Demo.SubTitle("CountBy: conta gli elementi raggruppati per chiave");
        foreach (var pair in employees.CountBy(e => e.IsActive))
        {
            Demo.Line($"IsActive = {pair.Key}", pair.Value);
        }

        Demo.SubTitle("DistinctBy / Distinct su chiavi derivate");
        Demo.Line("DistinctBy(Department)", string.Join(", ", employees.DistinctBy(e => e.Department).Select(e => e.Department)));
        Demo.Line("Distinct(Department)", string.Join(", ", employees.Select(e => e.Department).Distinct()));

        Demo.SubTitle("MaxBy / MinBy: estremi con selettore");
        Demo.Line("MaxBy(YearsOfExperience)", employees.MaxBy(e => e.YearsOfExperience)?.FullName);
        Demo.Line("MinBy(Age)", employees.MinBy(e => e.Age)?.FullName);

        Demo.SubTitle("Order e OrderDescending: ordinamento stabile con IComparer");
        var bySalary = Comparer<Employee>.Create(static (a, b) => a.Salary.CompareTo(b.Salary));
        Demo.Line("Order().Take(3)", string.Join(", ", employees.Order(bySalary).Take(3).Select(e => e.FirstName)));
        Demo.Line("OrderDescending().Take(3)", string.Join(", ", employees.OrderDescending(bySalary).Take(3).Select(e => e.FirstName)));
        Demo.Line("OrderBy().Take(3)", string.Join(", ", employees.OrderBy(e => e.Salary).Take(3).Select(e => e.FirstName)));

        Demo.SubTitle("Zip e Aggregate");
        var paired = employees.Zip(SampleData.Products, (e, p) => $"{e.FirstName} <-> {p.Name}");
        Demo.Line("Zip(employees, products)", string.Join(" | ", paired.Take(3)));

        Demo.Line("Aggregate(Salary)", employees.Aggregate(0m, (total, e) => total + e.Salary).ToString("C", CultureInfo.CurrentCulture));

        Demo.SubTitle("ToHashSet e ToLookup");
        Demo.Line("ToHashSet().Count", employees.ToHashSet().Count);
        var lookup = employees.ToLookup(e => e.Department);
        Demo.Line("ToLookup[\"IT\"]", string.Join(", ", lookup["IT"].Select(e => e.FirstName)));
    }

    private static string? FindDepartmentManager(string? department) =>
        department?.ToUpperInvariant() switch
        {
            "IT" => "Marco Rossi",
            "FINANCE" => "Luca Ferrari",
            _ => null
        };

    private static string GetExperienceLevelFor(Employee employee) =>
        employee.YearsOfExperience switch
        {
            < 2 => "Junior",
            >= 2 and < 5 => "Intermediate",
            >= 5 and < 10 => "Senior",
            _ => "Expert"
        };

    private static string EvaluateSalaryFor(decimal salary) =>
        salary switch
        {
            < 25_000m => "Junior",
            >= 25_000m and < 35_000m => "Intermediate",
            >= 35_000m and < 50_000m => "Senior",
            _ => "Expert"
        };
}
