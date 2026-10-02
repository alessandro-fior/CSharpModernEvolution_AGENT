using System.Diagnostics;

using CSharpModernEvolution.Examples;

// Entry point con top-level statements: la forma più breve di un Main.
// Le dichiarazioni di tipo restano ammesse dopo le istruzioni top-level.

Console.OutputEncoding = System.Text.Encoding.UTF8;

var watch = Stopwatch.StartNew();

Console.WriteLine("C# Modern Evolution Lab");
Console.WriteLine($"Runtime: {Environment.Version}   Framework: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");

GenericsExample.Run();
LinqExample.Run();
await AsyncExample.RunAsync();
PatternMatchingExample.Run();
ModernCSharpExample.Run();

watch.Stop();

Console.WriteLine();
Console.WriteLine(new string('=', 78));
Console.WriteLine($"Fine demo in {watch.ElapsedMilliseconds} ms.");
Console.WriteLine(new string('=', 78));
