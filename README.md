# C# Modern Evolution Lab

Laboratorio didattico su **C# 14 / .NET 10**: un'unica console application che mostra, con
codice reale e commenti, come il linguaggio C# si e evoluto dalle funzionalita classiche a
quelle moderne.

Nessun database, nessuna dipendenza esterna, nessuna chiamata di rete: si clona, si lancia e
si legge.

```bash
dotnet run --project src/CSharpModernEvolution
dotnet test
```

---

## Struttura

```text
CSharpModernEvolution/
├── CSharpModernEvolution.sln
├── src/CSharpModernEvolution/
│   ├── Program.cs                       # orchestrazione delle demo
│   ├── Data/SampleData.cs               # dataset in memoria
│   ├── Models/
│   │   ├── Employee.cs                  # record posizionale
│   │   ├── Product.cs                   # class con required + init
│   │   └── Evaluation.cs                # record di risultato
│   ├── Services/
│   │   ├── Repository.cs                # generics
│   │   ├── SalaryService.cs             # primary constructor
│   │   └── EmployeeService.cs           # async/await, LINQ, nullable
│   └── Examples/
│       ├── GenericsExample.cs           # sezione 5
│       ├── LinqExample.cs               # sezione 6
│       ├── AsyncExample.cs              # sezione 7
│       ├── PatternMatchingExample.cs    # sezioni 9, 10, 11
│       ├── ModernCSharpExample.cs       # sezioni 8, 12-17
│       └── Demo.cs                      # helper di output
└── tests/CSharpModernEvolution.Tests/   # xUnit, 77 test
```

---

## Mappa delle funzionalita

| # | Funzionalita | C# | Versione .NET | Dove |
|---|-------------|----|---------------|------|
| 5 | Generics | 2.0 | - | `Services/Repository.cs` |
| 6 | Lambda e LINQ | 3.0 | 3.5 | `Examples/LinqExample.cs` |
| 7 | `async` / `await` | 5.0 | 4.5 | `Examples/AsyncExample.cs` |
| 8 | Nullable reference types | 8.0 | 3.0 | `Examples/ModernCSharpExample.cs` |
| 9 | Pattern matching | 7.0 | 2.0 | `Examples/PatternMatchingExample.cs` |
| 10 | Relational patterns | 9.0 | 5.0 | `Examples/PatternMatchingExample.cs` |
| 11 | Switch expressions | 8.0 | 3.0 | `Examples/PatternMatchingExample.cs` |
| 12 | `record` | 9.0 | 5.0 | `Models/Employee.cs` |
| 13 | `init` | 9.0 | 5.0 | `Models/Product.cs` |
| 14 | `required` | 11.0 | 7.0 | `Models/Product.cs` |
| 15 | Primary constructors | 12.0 | 8.0 | `Services/SalaryService.cs` |
| 16 | Collection expressions | 12.0 | 8.0 | `Examples/ModernCSharpExample.cs` |
| 17 | Raw string literals | 11.0 | 7.0 | `Examples/ModernCSharpExample.cs` |
| - | List/slice patterns | 11.0 | 7.0 | `Examples/PatternMatchingExample.cs` |
| - | API collezione moderne | - | 6.0+ | `Examples/ModernCSharpExample.cs` |

---

## 5. Generics (C# 2.0)

I Generics permettono di scrivere codice riutilizzabile mantenendo la **type safety**:
un solo tipo compilato che serve molti tipi, senza cast, senza boxing e senza usare
`object` come passe-partout.

```csharp
public sealed class Repository<T>
{
    private readonly List<T> _items = [];

    public void Add(T item) => _items.Add(item);

    public IReadOnlyList<T> GetAll() => _items;
}
```

Lo stesso `Repository<T>` e usato per `Employee`, `Product` e `Department`. I vincoli
restringono ulteriormente il tipo ammesso, e il compilatore garantisce che il corpo del
metodo sia corretto:

```csharp
public static T? MaxByComparable<T>(this IReadOnlyList<T> items)
    where T : IComparable<T>
```

`Employee` non implementa `IComparable<T>` e non ne ha bisogno: per i record l'uguaglianza
per valore e automatica.

## 6. Lambda e LINQ (C# 3.0)

LINQ e lambda sono una delle trasformazioni fondamentali del C# moderno: si descrive
**cosa** si vuole ottenere, non **come** ottenerlo.

```csharp
var experienced = employees
    .Where(e => e.YearsOfExperience >= 10)
    .OrderByDescending(e => e.Salary)
    .Select(e => new { e.FirstName, e.LastName, e.Salary })
    .ToList();
```

Nel progetto sono usati `Where`, `Select`, `OrderBy`, `OrderByDescending`, `Any`, `All`,
`Count`, `FirstOrDefault`, `MaxBy`, `MinBy`, `GroupBy`, `Sum`, `Average`, `Min`, `Max`,
`Chunk`, `CountBy`, `DistinctBy`, `ToLookup`, `ToHashSet`, `Zip` e `Aggregate`, sia con
sintassi fluent sia con **query expression**.

Da ricordare: le query LINQ hanno **deferred execution** - creare la query non accede ai
dati, si esegue solo quando la si enumera.

## 7. async / await (C# 5)

```text
Task
  ↓  dichiara che l'operazione e asincrona e restituisce un risultato
async
  ↓  il metodo puo sospendersi senza bloccare il thread
await
  ↓  sospende finche la Task non completa, poi riprende col risultato
risultato
```

```csharp
public async Task<IReadOnlyList<Employee>> GetEmployeesAsync(CancellationToken cancellationToken = default)
{
    await Task.Delay(300, cancellationToken).ConfigureAwait(false);
    return _repository.GetAll();
}
```

La demo mette a confronto tre chiamate sequenziali (circa 900 ms) con le stesse tre in
`Task.WhenAll` (circa 300 ms): stesso risultato, ma le attese non si sommano e il thread
resta libero. E' incluso anche un esempio di propagazione di `CancellationToken`.

## 8. Nullable reference types (C# 8)

Con `<Nullable>enable</Nullable>` il compilatore distingue i tipi che possono essere `null`:

```csharp
string? department = null;

Console.WriteLine(department?.ToUpper() ?? "NON DEFINITO");   // NON DEFINITO
```

| Operatore | Significato |
|-----------|-------------|
| `string?` | il tipo ammette `null` |
| `?.` | accesso condizionale: restituisce `null` invece di lanciare |
| `??` | valore di fallback se l'operando a sinistra e `null` |
| `??=` | assegna il fallback solo se necessario |
| `!` | asserzione esplicita: "qui non e `null`", il compilatore si fida |

Perche aiuta a prevenire i `NullReferenceException`: la distinzione `string` / `string?`
trasforma un errore runtime in un errore di compilazione (CS8600, CS8602, ...). Il difetto
si trova prima di scrivere una sola riga di logica di business.

## 9-11. Pattern matching, relational patterns, switch expressions

Le **relational patterns** (C# 9) permettono di usare gli operatori di confronto
direttamente dentro il pattern matching:

```csharp
public static string EvaluateSalary(decimal salary) =>
    salary switch
    {
        < 25_000m => "Junior",
        >= 25_000m and < 35_000m => "Intermediate",
        >= 35_000m and < 50_000m => "Senior",
        >= 50_000m => "Expert"
    };
```

Gli operatori relazionali usati sono `<`, `>`, `<=`, `>=`; quelli logici sono `and`, `or`,
`not`. Nota che non serve il braccio di fallback `_`: i quattro rami coprono l'intero dominio
dei `decimal` e il compilatore lo sa.

Gli altri pattern mostrati:

```csharp
if (employee is { IsActive: true, YearsOfExperience: >= 10 }) { ... }   // property + relational
if (employee is not null) { ... }                                      // C# 9
employee switch { { Department: "IT" or "HR" } => ..., _ => ... }       // or pattern
employee switch { { Department: not ("IT" or "HR") } => ... }           // not pattern
seniors is [_, var secondo, ..]                                         // list pattern (C# 11)
seniors[^2..]                                                           // slice pattern
```

### `switch` statement vs `switch` expression

| | `switch` statement | `switch` expression |
|---|---|---|
| Forma | costrutto con `case` / `default`, esegue azioni | espressione che produce un **valore** |
| Uscita | `break` o `return` per ogni ramo | il valore dell'espressione e il risultato |
| Uso tipico | side effect, controllo di flusso | classificazioni, mapping, `return` |
| Composizione | non componibile | si usa dentro espressioni, ternary, LINQ |

```csharp
// statement: serve quando il ramo esegue azioni
switch (employee.YearsOfExperience)
{
    case < 2: return "Junior";
    default: return "Expert";
}

// expression: quando il risultato e il valore stesso
var level = employee.YearsOfExperience switch
{
    < 2 => "Junior",
    _ => "Expert"
};
```

Nel progetto sono presenti **tre** switch expression (`EvaluateSalary`, `EvaluateScore`,
`GetExperienceLevel`) piu una versione con `switch` statement a scopo didattico
(`DescribeWithSwitchStatement`) che produce lo stesso risultato.

## 12. record

Un `record` introduce l'**uguaglianza per valore**, un `ToString()` leggibile, la copia non
distruttiva con `with` e orienta verso un design immutabile.

```text
record
  ↓
value-based equality
  ↓
immutability-oriented design
  ↓
with expression
```

```csharp
public record Employee(int Id, string FirstName, string LastName, string Department,
                       int Age, decimal Salary, int YearsOfExperience, bool IsActive)
{
    public string FullName => $"{FirstName} {LastName}";
}

var promoted = employee with { Salary = employee.Salary + 3_000m };
```

`employee` resta immutabile: `with` crea una **nuova** istanza. Le proprietà calcolate come
`FullName` non hanno backing field, quindi non partecipano all'equality del record.

## 13-14. `init` e `required`

```csharp
public class Product
{
    public required string Name { get; init; }
    public decimal Price { get; init; }
    public string Category { get; init; } = "Uncategorized";
}
```

`set` vs `init`:

- **`set`** - la proprieta e scrivibile per tutta la vita dell'oggetto. Ogni modifica e una
  mutazione: il codice diventa difficile da ragionare in modo concorrente.
- **`init`** - la proprieta e scrivibile **solo durante l'inizializzazione**. Dopo, l'oggetto
  e "freeze". Si ottiene lo stesso effetto di una classe immutabile senza rinunciare
  all'object initializer.

`required` (C# 11) e una promessa fatta al compilatore: il chiamante **deve** inizializzare
la proprieta, altrimenti il codice non compila (errore CS9035). Il compilatore da solo la
diagnosi: non serve un controllo a runtime.

```csharp
var product = new Product { Name = "Laptop", Price = 1_200m };   // ok
var broken  = new Product { Price = 1_200m };                    // errore: Name mancante
```

## 15. Primary constructor (C# 12)

```csharp
public sealed class SalaryService(decimal minimumSalary, decimal maximumSalary)
{
    public bool IsAboveMinimum(decimal salary) => salary >= minimumSalary;
}
```

Le dipendenze diventano parametri del costruttore senza scrivere il costruttore primario a
mano, il campo di backing e i relativi `if (x == null) throw` non servono piu. I parametri
restano disponibili nei metodi che li usano e non vengono materializzati come campo se non
serve.

## 16. Collection expressions (C# 12)

```csharp
List<string> departments =
[
    "IT",
    "Finance",
    "HR",
    "Sales"
];

var allDepartments =
[
    .. departments,
    "Management"
];
```

Sintassi unificata per `List<T>`, array, `Span<T>`, `HashSet<T>`, `IEnumerable<T>` e
`Dictionary<K, V>`, con supporto allo **spread** (`..`) e ai `null` condizionali.
Sostituisce `new List<string> { ... }` e `new[] { ... }.ToList()`.

## 17. Raw string literals (C# 11)

```csharp
string query = """
    SELECT e.Id, e.FirstName, e.LastName, e.Department
    FROM   Employee e
    WHERE  e.IsActive = 1
      AND  e.Salary >= 30000
    ORDER BY e.Salary DESC;
    """;
```

Niente piu `\r\n` espliciti, niente escape di virgolette, niente `+` per andare a capo:
l'indentazione comune del blocco viene rimossa in compilazione. Con `$$"""` le interpolazioni
sono delimitate da `{{ }}` e le graffe singole restano letterali.

---

## API di collezione moderne (.NET 6+)

Non sono sintassi ma APIs, e riducono moltissimo codice boilerplate:

| API | Esempio |
|-----|---------|
| `Chunk(n)` | `employees.Chunk(4)` - divide in gruppi di 4 |
| `CountBy(key)` | `employees.CountBy(e => e.IsActive)` |
| `MaxBy/MinBy(key)` | `employees.MaxBy(e => e.Salary)` |
| `DistinctBy(key)` | `employees.DistinctBy(e => e.Department)` |
| `Order/OrderDescending(cmp)` | ordinamento stabile con `IComparer<T>` |
| `ToLookup(key)` | `employees.ToLookup(e => e.Department)["IT"]` |
| `TryGetNonEnumeratedCount` | evita di enumerare per contarne gli elementi |

---

## Test

```bash
dotnet test
```

77 test xUnit coprono il servizio di dominio, i generics, `record`/`with`/equality,
`init`/`required`, il primary constructor e le tre switch expression con i valori di confine
(esattamente 25.000, 35.000, 50.000, 60, 70, 90, 100).

## Requisiti

- .NET SDK 10.0
- Nessun altro software, nessun servizio, nessuna chiave API

## Licenza

MIT - vedi [LICENSE](LICENSE).
