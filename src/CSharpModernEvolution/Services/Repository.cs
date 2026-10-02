namespace CSharpModernEvolution.Services;

/// <summary>
/// Contenitore generico introdotto con i Generics (C# 2.0).
/// <para>
/// Un tipo generico viene compilato una volta sola: serve un solo <c>Repository&lt;T&gt;</c>
/// per ogni tipo, mantenendo la type safety (niente cast, niente boxing, niente
/// <c>object</c> come comodo passe-partout).
/// </para>
/// </summary>
/// <typeparam name="T">Tipo degli elementi memorizzati.</typeparam>
public sealed class Repository<T>
{
    // `[]` è una collection expression (C# 12): crea una List<T> vuota.
    private readonly List<T> _items = [];

    public int Count => _items.Count;

    public void Add(T item) => _items.Add(item);

    /// <summary>Esposizione in sola lettura: i caller non possono alterare lo stato interno.</summary>
    public IReadOnlyList<T> GetAll() => _items;

    public bool Remove(T item) => _items.Remove(item);

    public void Clear() => _items.Clear();

    /// <summary>Costruttore statico: crea un repository già popolato.</summary>
    public static Repository<T> From(IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var repository = new Repository<T>();
        foreach (var item in source)
        {
            repository.Add(item);
        }

        return repository;
    }

    public IEnumerable<T> Find(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return _items.Where(predicate);
    }
}

/// <summary>
/// Estensioni generiche <em>con vincolo</em>.
/// </summary>
public static class GenericExtensions
{
    /// <summary>
    /// Il vincolo <c>where T : IComparable&lt;T&gt;</c> permette di usare gli operatori di confronto
    /// all'interno del corpo del metodo. Il compilatore garantisce che il corpo sia corretto per
    /// ogni <c>T</c> ammesso: se il tipo non implementa il confronto, il codice proprio non compila.
    /// <para>
    /// Nota: <c>Employee</c> non implementa <c>IComparable</c>, e questo non e un problema -
    /// per i record l'uguaglianza per valore e automatica e l'ordinamento si fa con
    /// <c>OrderBy</c>/<c>MaxBy</c> invece che con codice di confronto scritto a mano.
    /// </para>
    /// </summary>
    public static T? MaxByComparable<T>(this IReadOnlyList<T> items)
        where T : IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(items);

        if (items.Count == 0)
        {
            return default;
        }

        var max = items[0];
        for (var i = 1; i < items.Count; i++)
        {
            if (items[i].CompareTo(max) > 0)
            {
                max = items[i];
            }
        }

        return max;
    }
}
