namespace CSharpModernEvolution.Models;

/// <summary>
/// Esito di una valutazione: record posizionale, quindi equality basata sul valore.
/// Serve a mostrare come i record rendano naturale un DTO di risultato.
/// </summary>
/// <param name="Subject">Oggetto valutato (nome del dipendente, prodotto, ...).</param>
/// <param name="ExperienceLevel">Livello di seniority.</param>
/// <param name="SalaryBand">Fascia di RAL.</param>
public sealed record Evaluation(string Subject, string ExperienceLevel, string SalaryBand)
{
    /// <summary>
    /// Pattern logico con <c>or</c> su costanti: la valutazione e "positiva" quando il livello
    /// di seniority e almeno Senior.
    /// </summary>
    public bool IsSenior => ExperienceLevel is "Senior" or "Expert";

    public string Summary => $"{Subject} | exp: {ExperienceLevel} | RAL: {SalaryBand}";

    /// <summary>Copia non distruttiva: restituisce una nuova <see cref="Evaluation"/>.</summary>
    public Evaluation WithExperienceLevel(string level) => this with { ExperienceLevel = level };
}
