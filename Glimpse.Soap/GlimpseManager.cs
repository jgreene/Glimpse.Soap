using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Soap
{
    internal static class GlimpseManager
    {
        private const string GlimpseItemKey = "__GlimpseRequestRuntimePermissions";

        public static bool IsGlimpseActive()
        {
            GlimpseRuntime runtime = GlimpseManager.GetRuntime();
            if (runtime == null || runtime.IsInitialized == false)
                return false;

            if (HttpContext.Current.Items.Contains(GlimpseItemKey) == false)
                return false;

            var runtimePolicy = (RuntimePolicy) HttpContext.Current.Items["__GlimpseRequestRuntimePermissions"];

            return runtimePolicy == RuntimePolicy.On;
        }

        private static GlimpseRuntime GetRuntime()
        {
            if (HttpContext.Current == null || HttpContext.Current.ApplicationInstance == null)
                return null;

            return (HttpContext.Current.Application["__GlimpseRuntime"] as GlimpseRuntime);
        }

        public static void LogMessage<T>(T message)
        {
            GlimpseRuntime runtime = GlimpseManager.GetRuntime();
            if (runtime == null || runtime.IsInitialized == false)
                return;

            if (runtime.Configuration == null || runtime.Configuration.MessageBroker == null)
                return;

            var messageBroker = runtime.Configuration.MessageBroker;
            messageBroker.Publish(message);
        }

        public static IExecutionTimer GetExecutionTimer()
        {
            return GetRuntime().Configuration.TimerStrategy();
        }
    }
}