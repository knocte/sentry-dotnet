using System.Web;
using Sentry.Internal;

namespace Sentry.AspNet.Internal;

internal static class SystemWebVersionLocator
{
    internal static string? Resolve(string? release, HttpContext context)
    {
        if (!string.IsNullOrWhiteSpace(release))
        {
            return release;
        }

        return Resolve(context);
    }

    internal static string? Resolve(HttpContext context)
    {
        if (context.ApplicationInstance?.GetType() is { } type)
        {
            // Usually the type is ASP.global_asax and the BaseType is the Web Application.
            while (type is { Namespace: "ASP" })
            {
                type = type.BaseType;
            }

            if (type?.Assembly is { } assembly)
            {
                return ApplicationVersionLocator.GetCurrent(assembly);
            }
        }

        return null;
    }

    public static string? Resolve(SentryOptions options, HttpContext context)
    {
        var release = options.SettingLocator.GetRelease();
        return Resolve(release, context);
    }
}
