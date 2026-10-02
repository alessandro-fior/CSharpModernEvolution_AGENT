using CSharpModernEvolution.Data;
using CSharpModernEvolution.Services;

namespace CSharpModernEvolution.Examples;

/// <summary>
/// Sezione 7 - <c>async</c>/<c>await</c> (C# 5).
/// <para>
/// Il modello:
/// <code>
/// Task
///   ↓  dichiari che l'operazione è asincrona e restituisce un risultato
/// async
///   ↓  il metodo può sospendersi senza bloccare il thread
/// await
///   ↓  sospende qui finche la Task non completa, poi riprende con il risultato
/// risultato
/// </code>
/// </para>
/// <para>
/// Nessuna chiamata HTTP reale: la latenza è simulata con <see cref="Task.Delay(int, CancellationToken)"/>.
/// </para>
/// </summary>
public static class AsyncExample
{
    public static async Task RunAsync()
    {
        Demo.Section("7. async / await (C# 5) - operazioni lunghe senza bloccare il thread");

        var service = new EmployeeService(SampleData.Employees);

        Demo.SubTitle("Sequenziale: tre chiamate una dopo l'altra");
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var all = await service.GetEmployeesAsync();
        var active = await service.GetActiveAsync();
        var it = await service.GetByDepartmentAsync("IT");
        watch.Stop();
        Demo.Line("tempo totale", $"{watch.ElapsedMilliseconds} ms (3 x 300 ms attesi)");
        Demo.Line("dipendenti", all.Count);
        Demo.Line("attivi", active.Count);
        Demo.Line("reparto IT", it.Count);

        Demo.SubTitle("Parallelo: le stesse tre chiamate in concorrenza con Task.WhenAll");
        watch.Restart();
        await Task.WhenAll(
            service.GetEmployeesAsync(),
            service.GetActiveAsync(),
            service.GetByDepartmentAsync("IT"));
        watch.Stop();
        Demo.Line("tempo totale", $"{watch.ElapsedMilliseconds} ms (attese sovrapposte)");
        Demo.Success("Stesso risultato, ma le attese non si sommano: il thread resta libero.");

        Demo.SubTitle("Propagazione di CancellationToken");
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
        try
        {
            await service.GetEmployeesAsync(cts.Token);
            Demo.Line("risultato", "il delay di 300 ms ha completato");
        }
        catch (OperationCanceledException)
        {
            Demo.Success("Operazione annullata: il token propagato ha interrotto l'attesa.");
        }

        Demo.SubTitle("Await in parallelo con Task.WhenAll su risultati tipizzati");
        var summariesTask = service.GetDepartmentSummariesAsync();
        var evaluationsTask = service.GetEvaluationsAsync();
        await Task.WhenAll(summariesTask, evaluationsTask);

        foreach (var summary in await summariesTask)
        {
            Demo.Item($"{summary.Department,-10} n={summary.HeadCount}  media={summary.AverageSalary,10:C}");
        }

        Demo.Line("valutazioni prodotte", (await evaluationsTask).Count);

        Demo.SubTitle("Versione sincrona a confronto");
        Demo.Line("GetEmployeesBlocking()", $"{service.GetEmployeesBlocking().Count} elementi, ma blocca il thread");
    }
}
