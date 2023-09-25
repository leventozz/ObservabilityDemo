namespace Observability.ConsoleApp
{
    internal class ServiceHelper
    {
        internal async Task Worker1()
        {
            using var activity = ActivitySourceProvider.Source.StartActivity();

            Console.WriteLine("Worker1 is done");

            FirstService firstService = new();
            var googleLenght = await firstService.GetGoogleBytes();
            Console.WriteLine("Google Byte: " + googleLenght);
        }
    }
}
