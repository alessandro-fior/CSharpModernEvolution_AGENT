using CSharpModernEvolution.Data;
using CSharpModernEvolution.Models;

namespace CSharpModernEvolution.Examples;

/// <summary>
/// Sezioni 9, 10 e 11 - pattern matching, relational patterns e switch expressions.
/// <para>
/// Cronologia:
/// <list type="bullet">
///   <item><description><b>C# 7.0</b> - <c>is</c> pattern, type pattern con designazione, <c>when</c>;</description></item>
///   <item><description><b>C# 7.2</b> - <c>switch</c> statement su qualunque espressione;</description></item>
///   <item><description><b>C# 8.0</b> - property pattern, relational patterns, <c>and</c>/<c>or</c>/<c>not</c>, switch expression;</description></item>
///   <item><description><b>C# 9.0</b> - pattern combinatori estesi, <c>not null</c>, relational pattern (<c>&lt;</c>, <c>&gt;</c>, <c>&lt;=</c>, <c>&gt;=</c>);</description></item>
///   <item><description><b>C# 11.0</b> - list pattern, slice pattern.</description></item>
/// </list>
/// </para>
/// </summary>
public static class PatternMatchingExample
{
    public static void Run()
    {
        Demo.Section("9-11. Pattern matching, relational patterns, switch expressions");

        IReadOnlyList<Employee> employees = SampleData.Employees;

        // ---------------------------------------------------------------
        // Sezione 9 - pattern di proprietà con valori costanti e relazionali
        // ---------------------------------------------------------------
        Demo.SubTitle("Property pattern + relational pattern con if");
        foreach (var employee in employees)
        {
            if (employee is { IsActive: true, YearsOfExperience: >= 10 })
            {
                Demo.Item($"{employee.FullName,-16} attivo e con >= 10 anni di esperienza");
            }
        }

        Demo.SubTitle("Null check con 'is not null' (C# 9, prima era '!= null')");
        foreach (var employee in employees)
        {
            if (employee is not null && employee.Department is { Length: > 0 })
            {
                Demo.Item($"{employee.FullName,-16} reparto '{employee.Department}'");
            }
        }

        Demo.SubTitle("Pattern logici: and / or / not");
        foreach (var employee in employees)
        {
            var label = employee switch
            {
                { IsActive: false, Department: "Finance" or "Sales" } => "attivo no / reparto Finance o Sales",
                { IsActive: false } => "non attivo",
                { Department: not ("IT" or "Marketing") } => "attivo in reparto non IT/Marketing",
                _ => "attivo in reparto IT o Marketing"
            };

            Demo.Item($"{employee.FullName,-16} {label}");
        }

        Demo.SubTitle("Type pattern con designazione + switch statement classico");
        foreach (var employee in employees)
        {
            Demo.Item($"{employee.FullName,-16} {DescribeWithSwitchStatement(employee)}");
        }

        Demo.SubTitle("List pattern e slice pattern (C# 11)");
        Employee[] seniors = [.. employees.OrderBy(e => e.YearsOfExperience)];
        Demo.Line("ultimo per anzianita", seniors[^1].FullName);
        Demo.Line("primi due (slice [..2])", string.Join(", ", seniors[..2].Select(e => e.FirstName)));
        Demo.Line("ultimi due (slice [^2..])", string.Join(", ", seniors[^2..].Select(e => e.FirstName)));
        Demo.Line("match [_, var secondo, ..]", seniors is [_, var secondo, ..] ? secondo.FullName : "no match");

        // ---------------------------------------------------------------
        // Sezione 10 - relational patterns (la parte principale)
        // ---------------------------------------------------------------
        Demo.SubTitle("EvaluateSalary: < > <= >= and");
        decimal[] salarySamples = [18_000m, 25_000m, 32_000m, 49_999m, 50_000m, 75_000m];
        foreach (var salary in salarySamples)
        {
            Demo.Line($"RAL {salary,10:C}", EvaluateSalary(salary));
        }

        Demo.SubTitle("EvaluateScore: stesso meccanismo su un voto intero");
        int[] scoreSamples = [45, 60, 70, 89, 90, 100, 130];
        foreach (var score in scoreSamples)
        {
            Demo.Line($"voto {score,3}", EvaluateScore(score));
        }

        // ---------------------------------------------------------------
        // Sezione 11 - switch expressions a confronto
        // ---------------------------------------------------------------
        Demo.SubTitle("GetExperienceLevel: switch expression sull'anni di esperienza");
        foreach (var employee in employees)
        {
            Demo.Line($"{employee.FullName,-16} {employee.YearsOfExperience,3} anni", GetExperienceLevel(employee));
        }

        Demo.SubTitle("switch expression annidata (ternario -> pattern)");
        foreach (var employee in employees)
        {
            Demo.Line(
                $"{employee.FullName,-16}",
                employee switch
                {
                    { IsActive: false } => $"{GetExperienceLevel(employee)} (non in organico)",
                    { YearsOfExperience: >= 10, Salary: >= 50_000m } => $"{GetExperienceLevel(employee)} con compensi primato",
                    { YearsOfExperience: >= 10 } => $"{GetExperienceLevel(employee)} in crescita",
                    { Age: < 30 } => $"{GetExperienceLevel(employee)} da affiancare",
                    _ => $"{GetExperienceLevel(employee)} stabile"
                });
        }
    }

    /// <summary>
    /// Sezione 10 - relational patterns con <c>and</c>.
    /// I relational patterns permettono di usare gli operatori di confronto
    /// direttamente dentro il pattern matching (introdotti in C# 9).
    /// </summary>
    public static string EvaluateSalary(decimal salary) =>
        salary switch
        {
            < 25_000m => "Junior",
            >= 25_000m and < 35_000m => "Intermediate",
            >= 35_000m and < 50_000m => "Senior",
            >= 50_000m => "Expert"
        };

    /// <summary>Secondo esempio di relational patterns, con braccio di fallback esplicito.</summary>
    public static string EvaluateScore(int score) =>
        score switch
        {
            < 60 => "Insufficient",
            >= 60 and < 70 => "Sufficient",
            >= 70 and < 90 => "Good",
            >= 90 and <= 100 => "Excellent",
            _ => "Invalid"
        };

    /// <summary>Terza switch expression: classificazione per anzianita.</summary>
    public static string GetExperienceLevel(Employee employee) =>
        employee.YearsOfExperience switch
        {
            < 2 => "Junior",
            >= 2 and < 5 => "Intermediate",
            >= 5 and < 10 => "Senior",
            _ => "Expert"
        };

    /// <summary>
    /// Stessa logica di <see cref="GetExperienceLevel"/> scritta con il
    /// <c>switch</c> <em>statement</em> tradizionale, per confronto.
    /// </summary>
    private static string DescribeWithSwitchStatement(Employee employee)
    {
        switch (employee.YearsOfExperience)
        {
            case < 2:
                return "Junior";
            case >= 2 and < 5:
                return "Intermediate";
            case >= 5 and < 10:
                return "Senior";
            default:
                return "Expert";
        }
    }
}
