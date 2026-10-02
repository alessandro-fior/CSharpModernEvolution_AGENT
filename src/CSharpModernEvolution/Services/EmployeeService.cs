using CSharpModernEvolution.Models;

namespace CSharpModernEvolution.Services;

/// <summary>
/// Riepilogo per reparto, calcolato con LINQ. Essendo un record, si confronta per valore.
/// </summary>
/// <param name="Department">Reparto.</param>
/// <param name="HeadCount">Numero di dipendenti.</param>
/// <param name="AverageSalary">RAL media.</param>
public sealed record DepartmentSummary(string Department, int HeadCount, decimal AverageSalary);

/// <summary>
/// Servizio di dominio. Mostra insieme:
/// <list type="bullet">
///   <item><description><c>async</c>/<c>await</c> (C# 5) con <see cref="Task{TResult}"/>;</description></item>
///   <item><description>LINQuery con lambda;</description></item>
///   <item><description>Nullable reference types (<c>string?</c>, <c>?.</c>, <c>??</c>);</description></item>
///   <item><description>collezioni immutabili in uscita (<c>IReadOnlyList&lt;T&gt;</c>).</description></item>
/// </list>
/// </summary>
public sealed class EmployeeService
{
    private readonly Repository<Employee> _repository;

    /// <summary>
    /// Costruisce il servizio. Il <c>seed</c> è opzionale: se manca, il servizio parte vuoto.
    /// Questo rende il servizio facile da costruire anche nei test.
    /// </summary>
    public EmployeeService(IEnumerable<Employee>? seed = null) =>
        _repository = seed is null ? new Repository<Employee>() : Repository<Employee>.From(seed);

    /// <summary>
    /// Modello async: <c>Task</c> &rarr; <c>async</c> &rarr; <c>await</c> &rarr; risultato.
    /// <para>
    /// <c>async</c>/<c>await</c> sono stati introdotti in <b>C# 5</b>. Non bloccano un thread:
    /// il metodo si sospende a ogni <c>await</c> e il thread torna libero per altri lavoro.
    /// Qui la sorgente è simulata con <see cref="Task.Delay(int, CancellationToken)"/>:
    /// nessuna chiamata HTTP reale.
    /// </para>
    /// </summary>
    public async Task<IReadOnlyList<Employee>> GetEmployeesAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(300, cancellationToken).ConfigureAwait(false);
        return _repository.GetAll();
    }

    /// <summary>
    /// Restituisce solo i dipendenti attivi. Qui <c>is not null</c> filtra i valori mancanti
    /// in modo type-safe: senza Nullable Reference Types questa verifica sarebbe illegale
    /// su una <c>string</c> non-nullable.
    /// </summary>
    public async Task<IReadOnlyList<Employee>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var employees = await GetEmployeesAsync(cancellationToken).ConfigureAwait(false);
        return [.. employees.Where(e => e is { IsActive: true })];
    }

    /// <summary>
    /// Cerca per reparto. Il parametro è <c>string?</c>: il chiamante può passare <c>null</c>
    /// per ottenere <em>tutti</em> i dipendenti, senza errori a runtime.
    /// </summary>
    public async Task<IReadOnlyList<Employee>> GetByDepartmentAsync(
        string? department,
        CancellationToken cancellationToken = default)
    {
        var employees = await GetEmployeesAsync(cancellationToken).ConfigureAwait(false);

        if (department is null)
        {
            return employees;
        }

        return [.. employees.Where(e => string.Equals(e.Department, department, StringComparison.OrdinalIgnoreCase))];
    }

    /// <summary>Top N per RAL. Usa <c>MaxBy</c> (LINQ, .NET 6) per il primo classificato.</summary>
    public async Task<Employee?> GetTopEarnerAsync(CancellationToken cancellationToken = default)
    {
        var employees = await GetEmployeesAsync(cancellationToken).ConfigureAwait(false);
        return employees.MaxBy(e => e.Salary);
    }

    /// <summary>Riepilogo per reparto con <c>GroupBy</c> + proiezione anonima e <c>record</c>.</summary>
    public async Task<IReadOnlyList<DepartmentSummary>> GetDepartmentSummariesAsync(
        CancellationToken cancellationToken = default)
    {
        var employees = await GetEmployeesAsync(cancellationToken).ConfigureAwait(false);

        return
        [
            .. employees
                .GroupBy(e => e.Department)
                .Select(g => new DepartmentSummary(g.Key, g.Count(), g.Average(e => e.Salary)))
                .OrderByDescending(s => s.AverageSalary)
        ];
    }

    /// <summary>Valuta ogni dipendente producendo record <see cref="Evaluation"/>.</summary>
    public async Task<IReadOnlyList<Evaluation>> GetEvaluationsAsync(
        CancellationToken cancellationToken = default)
    {
        var employees = await GetEmployeesAsync(cancellationToken).ConfigureAwait(false);

        return
        [
            .. employees.Select(e => new Evaluation(
                e.FullName,
                Examples.PatternMatchingExample.GetExperienceLevel(e),
                Examples.PatternMatchingExample.EvaluateSalary(e.Salary)))
        ];
    }

    /// <summary>
    /// Versione "sincrona" equivalente per confronto: niente <c>await</c>, quindi
    /// <em>blocca</em> il thread chiamante. Serve a far notare la differenza.
    /// </summary>
    public IReadOnlyList<Employee> GetEmployeesBlocking() => _repository.GetAll();

    /// <summary>Restituisce una copia difensiva del contenuto del repository.</summary>
    public IReadOnlyList<Employee> Snapshot() => [.. _repository.GetAll()];
}
