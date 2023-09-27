using System.Diagnostics;

namespace Observability.ConsoleApp
{
    internal class FirstService
    {
        static HttpClient httpClient = new();
        internal async Task<int> GetGoogleBytes()
        {
            using var activity = ActivitySourceProvider.Source.StartActivity(kind: ActivityKind.Producer, name:"CustomGetGoogleBytes");

            var eventTags = new ActivityTagsCollection();

            activity?.AddEvent(new("Google Request Started"));
            var result = await httpClient.GetAsync("https://www.google.com");
            var responseContent = await result.Content.ReadAsStringAsync();
            eventTags.Add("Google Body Lenght", responseContent.Length);
            activity?.AddEvent(new("Google Request Finished", tags: eventTags));
            return responseContent.Length;
        }
    }
}
