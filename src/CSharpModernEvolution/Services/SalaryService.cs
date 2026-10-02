using CSharpModernEvolution.Models;

namespace CSharpModernEvolution.Services;

/// <summary>
/// Servizio con <em>primary constructor</em> (C# 12): le dipendenze diventano parametri
/// del costruttore senza scrgere il costruttore primario a mano.
/// <para>
/// Prima (C# 11 e precedenti) serviva:
/// <code>
/// private readonly decimal _minimumSalary;
/// public SalaryService(decimal minimumSalary) =&gt; _minimumSalary = minimumSalary;
/// </code>
/// </para>
/// <para>
/// Il parametro <paramref name="minimumSalary"/> è capturato dal closure dei metodi che lo usano;
/// se non serve a un campo calcolato non viene materializzato alcun campo di backing.
/// </para>
/// </summary>
/// <param name="minimumSalary">Soglia minima di RAL considerata valida.</param>
/// <param name="maximumSalary">Soglia massima di RAL accettata.</param>
public sealed class SalaryService(decimal minimumSalary, decimal maximumSalary)
{
    public decimal MinimumSalary => minimumSalary;

    public decimal MaximumSalary => maximumSalary;

    public bool IsAboveMinimum(decimal salary) => salary >= minimumSalary;

    public bool IsWithinRange(decimal salary) =>
        salary >= minimumSalary && salary <= maximumSalary;

    /// <summary>Applica un bonus percentuale restituendo un nuovo valore (nessuna mutazione).</summary>
    public decimal ApplyBonus(decimal salary, decimal bonusPercentage) =>
        salary * (1m + (bonusPercentage / 100m));

    /// <summary>Classifica i dipendenti che eccedono la soglia minima, dal più pagato.</summary>
    public IReadOnlyList<Employee> GetAboveMinimum(IEnumerable<Employee> employees, decimal bonusPercentage)
    {
        ArgumentNullException.ThrowIfNull(employees);

        return
        [
            .. employees
                .Where(e => IsAboveMinimum(e.Salary))
                .OrderByDescending(e => e.Salary)
                .Select(e => e with { Salary = ApplyBonus(e.Salary, bonusPercentage) })
        ];
    }
}
