namespace Observability.ConsoleApp
{
    internal class FirstService
    {
        static HttpClient httpClient = new();
        internal async Task<int> GetGoogleBytes()
        {
            using var activity = ActivitySourceProvider.Source.StartActivity();
            var result = await httpClient.GetAsync("https://www.google.com");
            var responseContent = await result.Content.ReadAsStringAsync();
            return responseContent.Length;
        }
    }
}
