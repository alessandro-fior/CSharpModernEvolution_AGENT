# C# Modern Evolution Lab

## Obiettivo

Creare un piccolo progetto C#/.NET da pubblicare su GitHub come laboratorio didattico e portfolio tecnico.

Il progetto deve mostrare in modo pratico l'evoluzione di C# dalle funzionalità classiche alle funzionalità moderne, con particolare attenzione a:

- Generics
- Lambda expressions
- LINQ
- async/await
- nullable reference types
- pattern matching
- switch expressions
- relational patterns
- logical patterns (`and`, `or`, `not`)
- records
- `init`
- primary constructors
- collection expressions
- `required`
- raw string literals
- modern collection APIs

Il progetto deve essere semplice da leggere, compilabile e adatto a essere mostrato durante un colloquio.

---

# 1. Tecnologie

Usare:

- C#
- .NET 10 / C# 14
- Console Application
- Nullable reference types abilitati
- Implicit usings abilitati
- Nessun database
- Nessuna dipendenza esterna, salvo quelle strettamente necessarie

Target:

```xml
<TargetFramework>net10.0</TargetFramework>
<Nullable>enable</Nullable>
<ImplicitUsings>enable</ImplicitUsings>
```

---

# 2. Nome progetto

Usare:

```text
CSharpModernEvolution
```

Repository GitHub suggerito:

```text
csharp-modern-evolution
```

---

# 3. Struttura

Creare una struttura simile:

```text
CSharpModernEvolution/
│
├── CSharpModernEvolution.sln
│
├── src/
│   └── CSharpModernEvolution/
│       ├── CSharpModernEvolution.csproj
│       ├── Program.cs
│       │
│       ├── Models/
│       │   ├── Employee.cs
│       │   ├── Product.cs
│       │   └── Evaluation.cs
│       │
│       ├── Services/
│       │   └── EmployeeService.cs
│       │
│       └── Examples/
│           ├── GenericsExample.cs
│           ├── LinqExample.cs
│           ├── AsyncExample.cs
│           ├── PatternMatchingExample.cs
│           └── ModernCSharpExample.cs
│
├── tests/
│   └── CSharpModernEvolution.Tests/
│       ├── CSharpModernEvolution.Tests.csproj
│       └── EmployeeServiceTests.cs
│
├── README.md
├── .gitignore
└── LICENSE
```

Se l'agente ritiene la struttura eccessiva, può semplificarla, ma deve mantenere una separazione ragionevole tra Models, Services ed Examples.

---

# 4. Dominio applicativo

Creare una piccola applicazione che gestisca una lista di dipendenti.

Esempio:

```text
Matricola
Nome
Cognome
Età
Reparto
RAL
Anni di esperienza
Attivo
```

Usare un `record` moderno:

```csharp
public record Employee(
    int Id,
    string FirstName,
    string LastName,
    string Department,
    int Age,
    decimal Salary,
    int YearsOfExperience,
    bool IsActive);
```

Aggiungere eventualmente proprietà calcolate se utili.

---

# 5. Generics

Mostrare un esempio semplice di Generic Repository o Generic Container.

Esempio:

```csharp
public class Repository<T>
{
    private readonly List<T> _items = [];

    public void Add(T item) => _items.Add(item);

    public IReadOnlyList<T> GetAll() => _items;
}
```

Nel README spiegare:

> I Generics sono stati introdotti con C# 2.0 e permettono di scrivere codice riutilizzabile mantenendo la type safety.

---

# 6. Lambda e LINQ

Creare una lista di dipendenti e mostrare:

```csharp
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
```

Mostrare almeno:

- `Where`
- `Select`
- `OrderBy`
- `OrderByDescending`
- `Any`
- `FirstOrDefault`
- `GroupBy`

Nel README spiegare che LINQ e lambda sono una delle trasformazioni fondamentali del C# moderno.

---

# 7. Async / Await

Creare un servizio simulato:

```csharp
public async Task<IReadOnlyList<Employee>> GetEmployeesAsync()
{
    await Task.Delay(300);
    return _employees;
}
```

Non utilizzare chiamate HTTP reali.

Lo scopo è mostrare chiaramente il modello:

```text
Task
  ↓
async
  ↓
await
  ↓
risultato
```

Commentare nel codice che `async/await` è stato introdotto in C# 5.

---

# 8. Nullable Reference Types

Abilitare:

```xml
<Nullable>enable</Nullable>
```

Creare almeno un esempio corretto di gestione di un valore nullable.

Esempio:

```csharp
string? department = null;

Console.WriteLine(department?.ToUpper() ?? "NON DEFINITO");
```

Mostrare:

- `string?`
- `?.`
- `??`

Spiegare brevemente perché Nullable Reference Types aiutano a prevenire `NullReferenceException`.

---

# 9. Pattern Matching

Creare una classe:

```text
PatternMatchingExample
```

e mostrare:

```csharp
if (employee is { IsActive: true, YearsOfExperience: >= 10 })
{
    ...
}
```

Mostrare anche:

```csharp
if (employee is not null)
{
    ...
}
```

---

# 10. Relational Pattern Matching

Questa è la parte principale del progetto.

Creare un metodo:

```csharp
public static string EvaluateSalary(decimal salary)
{
    return salary switch
    {
        < 25_000m => "Junior",
        >= 25_000m and < 35_000m => "Intermediate",
        >= 35_000m and < 50_000m => "Senior",
        >= 50_000m => "Expert"
    };
}
```

Mostrare chiaramente:

```text
<
>
<=
>=
and
or
not
```

Spiegare nel README:

> Relational patterns were introduced in C# 9 and allow comparison operators to be used directly inside pattern matching.

Aggiungere anche un esempio con un voto:

```csharp
public static string EvaluateScore(int score) =>
    score switch
    {
        < 60 => "Insufficient",
        >= 60 and < 70 => "Sufficient",
        >= 70 and < 90 => "Good",
        >= 90 and <= 100 => "Excellent",
        _ => "Invalid"
    };
```

---

# 11. Switch Expression

Utilizzare almeno due switch expressions.

Esempio:

```csharp
string GetExperienceLevel(Employee employee) =>
    employee.YearsOfExperience switch
    {
        < 2 => "Junior",
        >= 2 and < 5 => "Intermediate",
        >= 5 and < 10 => "Senior",
        _ => "Expert"
    };
```

Spiegare la differenza concettuale tra:

```csharp
switch statement
```

e:

```csharp
switch expression
```

---

# 12. Records

Usare almeno un `record`.

Esempio:

```csharp
public record Employee(
    int Id,
    string FirstName,
    string LastName,
    string Department,
    int Age,
    decimal Salary,
    int YearsOfExperience,
    bool IsActive);
```

Mostrare eventualmente la non-mutabilità:

```csharp
var promoted = employee with
{
    Salary = employee.Salary + 3000m
};
```

Spiegare:

```text
record
↓
value-based equality
↓
immutability-oriented design
↓
with expression
```

---

# 13. init

Aggiungere un piccolo esempio:

```csharp
public class Product
{
    public required string Name { get; init; }
    public decimal Price { get; init; }
}
```

Mostrare:

```csharp
var product = new Product
{
    Name = "Laptop",
    Price = 1200m
};
```

Spiegare brevemente la differenza tra `set` e `init`.

---

# 14. required

Usare almeno una proprietà `required`.

Esempio:

```csharp
public class Product
{
    public required string Name { get; init; }
}
```

Spiegare che il compilatore obbliga il chiamante a inizializzare la proprietà.

---

# 15. Primary Constructor

Aggiungere un piccolo servizio:

```csharp
public class SalaryService(decimal minimumSalary)
{
    public bool IsAboveMinimum(decimal salary)
        => salary >= minimumSalary;
}
```

Spiegare che i primary constructors sono stati introdotti in C# 12.

---

# 16. Collection Expressions

Mostrare:

```csharp
List<string> departments =
[
    "IT",
    "Finance",
    "HR",
    "Sales"
];
```

Mostrare eventualmente lo spread operator:

```csharp
var allDepartments =
[
    ..departments,
    "Management"
];
```

Spiegare brevemente che le collection expressions appartengono alle funzionalità moderne di C#.

---

# 17. Raw String Literals

Aggiungere un esempio molto semplice:

```csharp
string json = """
    {
      "id": 1,
      "department": "IT"
    }
    """;
```

Spiegare che le raw string literals sono state introdotte con C# 11 e che permettono di
scrivere testi su piu righe senza `\r\n` espliciti e senza escape delle virgolette.
L'indentazione comune del blocco viene rimossa automaticamente.

Mostrare anche le raw string literals interpolate:

```csharp
var name = "Marco Rossi";
var salary = 52_000m;

string template = $$"""
    Dipendente: {{name}}
    RAL: {{salary:C}}
    """;
```

Spiegare che con `$$` le interpolazioni sono delimitate da `{{ }}`, quindi le graffe singole
restano letterali.

---

# 18. Modern Collection APIs

Mostrare le API introdotte con .NET 6 e successivi che sostituiscono codice boilerplate:

```csharp
var perTre = employees.Chunk(3);
var perReparto = employees.CountBy(e => e.IsActive);
var topPaga = employees.MaxBy(e => e.Salary);
var reparti = employees.DistinctBy(e => e.Department);
var perRepartoIndice = employees.ToLookup(e => e.Department);
```

Spiegare che non sono sintassi nuova ma APIs della libreria, e che riducono la quantita di
codice necessario per aggregare, raggruppare e trovare estremi.

Mostrare anche `Order` e `OrderDescending` con un `IComparer<T>`, che eseguono un ordinamento
stabile.

---

# 19. Struttura finale e test

Il progetto deve essere pubblicabile con:

```text
CSharpModernEvolution/
├── CSharpModernEvolution.sln
├── src/CSharpModernEvolution/
│   ├── CSharpModernEvolution.csproj
│   ├── Program.cs
│   ├── Data/SampleData.cs
│   ├── Models/
│   │   ├── Employee.cs
│   │   ├── Product.cs
│   │   └── Evaluation.cs
│   ├── Services/
│   │   ├── Repository.cs
│   │   ├── SalaryService.cs
│   │   └── EmployeeService.cs
│   └── Examples/
│       ├── Demo.cs
│       ├── GenericsExample.cs
│       ├── LinqExample.cs
│       ├── AsyncExample.cs
│       ├── PatternMatchingExample.cs
│       └── ModernCSharpExample.cs
├── tests/CSharpModernEvolution.Tests/
│   ├── CSharpModernEvolution.Tests.csproj
│   ├── TestData.cs
│   ├── EmployeeServiceTests.cs
│   ├── EmployeeTests.cs
│   ├── PatternMatchingExampleTests.cs
│   └── DomainServicesTests.cs
├── README.md
├── .gitignore
└── LICENSE
```

Il progetto di test deve usare xUnit e deve verificare almeno:

- il comportamento asincrono di `EmployeeService`
- l'uguaglianza per valore e l'espressione `with` dei record
- i valori di confine delle switch expression (25.000, 35.000, 50.000, 60, 70, 90, 100)
- la propagazione di `CancellationToken`

Il progetto deve compilare senza warning e senza errori.

Verifica finale:

```bash
dotnet build
dotnet test
dotnet run --project src/CSharpModernEvolution
```

---

# 20. README

Il README deve contenere:

- obiettivo del laboratorio
- istruzioni per eseguire il progetto e i test
- struttura delle cartelle
- tabella che associa ogni funzionalita alla versione di C# che l'ha introdotta
- una sezione per ciascuna funzionalita richiesta, con un esempio di codice e la spiegazione
- la differenza tra `switch` statement e `switch` expression
- la differenza tra `set` e `init`
- la differenza tra `record` e `class`
- licenza

Il README deve essere scritto in italiano e deve essere leggibile da chi non conosce il progetto.

---

# 21. Pubblicazione su GitHub

Repository suggerito:

```text
https://github.com/alessandro-fior/csharp-modern-evolution
```

Procedura:

```bash
git init
git add .
git commit -m "C# Modern Evolution Lab"
git branch -M main
git remote add origin <url-del-repository>
git push -u origin main
```

Verificare prima del push che:

- `.gitignore` esclude `bin/` e `obj/`
- non sono presenti segreti o credenziali
- `LICENSE` e presente
- `dotnet build` e `dotnet test` passano

Non eseguire il push se i test falliscono.
