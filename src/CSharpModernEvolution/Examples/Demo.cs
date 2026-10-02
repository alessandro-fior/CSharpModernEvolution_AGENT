namespace CSharpModernEvolution.Examples;

/// <summary>
/// Piccolo helper di output per la console: rende leggibile l'output durante una
/// dimostrazione dal vivo. Nessuna dipendenza esterna.
/// </summary>
internal static class Demo
{
    private const int Width = 78;
    private const int LabelWidth = 38;

    public static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', Width));
        Console.WriteLine($"  {title}");
        Console.WriteLine(new string('=', Width));
    }

    public static void SubTitle(string title) => WriteBlock(ConsoleColor.DarkCyan, $"-- {title}");

    public static void Note(string text) => WriteBlock(ConsoleColor.DarkGray, text);

    public static void Line(string label, object? value)
    {
        Console.Write("  ");
        WriteInline(ConsoleColor.Gray, label.PadRight(LabelWidth));
        WriteInline(ConsoleColor.White, $"{value}");
        Console.WriteLine();
    }

    public static void Item(string text) => WriteInline(ConsoleColor.Gray, $"   * {text}");

    public static void Success(string text) => WriteInline(ConsoleColor.Green, $"   -> {text}");

    private static void WriteBlock(ConsoleColor color, string text)
    {
        Console.WriteLine();

        foreach (var line in text.Split('\n'))
        {
            WriteInline(color, line);
            Console.WriteLine();
        }
    }

    private static void WriteInline(ConsoleColor color, string text)
    {
        var previous = Console.ForegroundColor;
        try
        {
            Console.ForegroundColor = color;
            Console.Write(text);
        }
        finally
        {
            Console.ForegroundColor = previous;
        }
    }
}
