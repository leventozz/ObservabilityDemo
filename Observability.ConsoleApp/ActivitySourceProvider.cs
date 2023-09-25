using System.Diagnostics;

namespace Observability.ConsoleApp
{
    internal static class ActivitySourceProvider
    {
        internal static ActivitySource Source = new ActivitySource(OpenTelemetryConstants.ActivitySourceName);
    }
}
