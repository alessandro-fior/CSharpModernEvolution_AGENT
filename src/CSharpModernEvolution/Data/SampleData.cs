namespace CSharpModernEvolution.Data;

/// <summary>
/// Dati di esempio. Nessun database, nessuna dipendenza esterna: il progetto resta
/// eseguibile e leggibile in qualsiasi ambiente.
/// </summary>
public static class SampleData
{
    /// <summary>Collection expression: sintassi compatta per costruire liste e array.</summary>
    public static IReadOnlyList<Models.Employee> Employees { get; } =
    [
        new(1, "Marco", "Rossi", "IT", 34, 52_000m, 12, true),
        new(2, "Giulia", "Bianchi", "IT", 29, 41_000m, 7, true),
        new(3, "Luca", "Ferrari", "Finance", 45, 33_500m, 20, true),
        new(4, "Anna", "Russo", "Finance", 31, 28_000m, 5, false),
        new(5, "Paolo", "Esposito", "HR", 38, 24_500m, 11, true),
        new(6, "Elena", "Romano", "HR", 26, 21_000m, 3, true),
        new(7, "Davide", "Gallo", "Sales", 41, 47_000m, 15, false),
        new(8, "Chiara", "Costa", "Sales", 33, 36_000m, 9, true),
        new(9, "Matteo", "Fontana", "IT", 52, 58_000m, 25, true),
        new(10, "Sara", "Conti", "Marketing", 27, 26_000m, 4, true)
    ];

    public static IReadOnlyList<Models.Product> Products { get; } =
    [
        new() { Name = "Laptop", Price = 1_200m, Category = "Hardware" },
        new() { Name = "Monitor 27\"", Price = 380m, Category = "Hardware" },
        new() { Name = "Tastiera", Price = 95m, Category = "Accessori", InStock = false },
        new() { Name = "Mouse", Price = 45m, Category = "Accessori" },
        new() { Name = "Docking station", Price = 210m, Category = "Hardware" },
        new() { Name = "Cuffie", Price = 150m, Category = "Accessori" }
    ];
}
