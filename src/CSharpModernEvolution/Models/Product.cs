namespace CSharpModernEvolution.Models;

/// <summary>
/// Prodotto modellato come <c>class</c> (non record) per mostrare un tipo mutabile-after-init.
/// <para>
/// <c>required</c> (C# 11): il compilatore <em>obbliga</em> il chiamante a inizializzare
/// <see cref="Name"/> nell'object initializer, anche se la classe ha un costruttore.
/// </para>
/// <para>
/// <c>init</c> (C# 9): la proprietà è scrivibile soltanto durante l'inizializzazione.
/// Con <c>set</c> resterebbe scrivibile per tutta la vita dell'oggetto; con <c>init</c>
/// l'oggetto è "freeze" appena costruito, favorendo la concorrenza e il design immutabile.
/// </para>
/// </summary>
public class Product
{
    /// <summary>Obbligatoria: senza questo membro il codice non compila.</summary>
    public required string Name { get; init; }

    public decimal Price { get; init; }

    /// <summary>Ha un valore di default, quindi non è obbligatorio.</summary>
    public string Category { get; init; } = "Uncategorized";

    public bool InStock { get; init; } = true;

    /// <summary>Prezzo dopo l'applicazione di uno sconto espresso in percentuale.</summary>
    public decimal PriceAfterDiscount(decimal discountPercentage) =>
        Price * (1m - (discountPercentage / 100m));

    public override string ToString() =>
        $"{Name} | {Category} | {Price:C} | {(InStock ? "disponibile" : "esaurito")}";
}
