namespace CSharpModernEvolution.Models;

/// <summary>
/// Esito di una valutazione: record posizionale, quindi equality basata sul valore.
/// Serve a mostrare come i record rendono naturale un DTO di risultato.
/// </summary>
/// <param name="Subject">Oggetto valutato (nome del dipendente, prodotto, ...).</param>
/// <param name="Level">Livello risultante dalla valutazione (Junior, Intermediate, ...).</param>
/// <param name="Verdict">Giudizio sintetico.</param>
public sealed record Evaluation(string Subject, string Level, string Verdict)
{
    /// <summary>
    /// Pattern logico con <c>not</c> + <c>or</c> su costanti: il verdetto è positivo
    /// se non rientra nell'insieme dei giudizi negativi.
    /// </summary>
    public bool IsPositive => Verdict is not ("Rejected" or "Insufficient");

    public string Summary => $"{Subject} -> {Level} ({Verdict})";

    /// <summary>Copia non distruttiva: restituisce una nuova <see cref="Evaluation"/>.</summary>
    public Evaluation WithLevel(string level) => this with { Level = level };
}
