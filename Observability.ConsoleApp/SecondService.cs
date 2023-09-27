namespace Observability.ConsoleApp
{
    internal class SecondService
    {
        internal async Task<int> WriteToFile(string text)
        {
            using var activity = ActivitySourceProvider.Source.StartActivity();
            await File.WriteAllTextAsync("textSimple.txt", text);
            return (await File.ReadAllTextAsync("textSimple.txt")).Length;
        }
    }
}
