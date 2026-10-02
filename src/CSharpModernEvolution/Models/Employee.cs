namespace CSharpModernEvolution.Models;

/// <summary>
/// Un dipendente, modellato come <c>record</c> posizionale (C# 9).
/// <para>
/// Un record introduce:
/// <list type="bullet">
///   <item><description>equality basata sul valore (due record con gli stessi dati sono uguali);</description></item>
///   <item><description>un <c>ToString()</c> già leggibile;</description></item>
///   <item><description>la copia non distruttiva con l'espressione <c>with</c>;</description></item>
///   <item><description>la tendenza a progettare tipi immutabili.</description></item>
/// </list>
/// </para>
/// </summary>
/// <param name="Id">Matricola.</param>
/// <param name="FirstName">Nome.</param>
/// <param name="LastName">Cognome.</param>
/// <param name="Department">Reparto.</param>
/// <param name="Age">Età.</param>
/// <param name="Salary">RAL (retribuzione annua lorda).</param>
/// <param name="YearsOfExperience">Anni di esperienza.</param>
/// <param name="IsActive">Indica se il dipendente è ancora in organico.</param>
public record Employee(
    int Id,
    string FirstName,
    string LastName,
    string Department,
    int Age,
    decimal Salary,
    int YearsOfExperience,
    bool IsActive)
{
    /// <summary>
    /// Proprietà calcolata: non ha backing field, quindi non partecipa all'equality del record.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>Costo annuo lordo assumendo la tredicesima mensilità.</summary>
    public decimal AnnualCost => Salary * 13m;

    /// <summary>
    /// Restituisce una <em>nuova</em> istanza con la RAL aumentata: l'originale resta immutabile.
    /// </summary>
    public Employee Promote(decimal increase) => this with { Salary = Salary + increase };

    /// <summary>Restituisce una nuova istanza disattivata.</summary>
    public Employee Deactivate() => this with { IsActive = false };
}
