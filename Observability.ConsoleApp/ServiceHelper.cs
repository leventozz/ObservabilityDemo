namespace Observability.ConsoleApp
{
    internal class ServiceHelper
    {
        internal async Task Worker1()
        {
            using var activity = ActivitySourceProvider.Source.StartActivity();

            FirstService firstService = new();
            var googleLenght = await firstService.GetGoogleBytes();

            Console.WriteLine("Worker1 is done");
            Console.WriteLine("Google Byte: " + googleLenght);
        }

        internal async Task Worker2()
        {
            using var activity = ActivitySourceProvider.Source.StartActivity();

            FirstService firstService = new();
            var sofLength = await firstService.GetStackOverFlowBytes();

            Console.WriteLine("Worker2 is done");
            Console.WriteLine("SOF Byte: "+ sofLength);
        }
    }
}
