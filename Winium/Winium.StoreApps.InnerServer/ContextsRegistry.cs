using System.Collections.Generic;

namespace Winium.StoreApps.InnerServer
{
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using Windows.UI.Xaml.Controls;

    using Winium.StoreApps.Common;
    using Winium.StoreApps.Common.Exceptions;
    using Winium.StoreApps.InnerServer.Element;
    using Winium.StoreApps.InnerServer.Web;

    internal class ContextsRegistry
    {
        private static int safeInstanceCount;

        private readonly Dictionary<string, WebContext> contexts;

        public const string NativeAppContext = "NATIVE_APP";

        public ContextsRegistry()
        {
            this.contexts = new Dictionary<string, WebContext>();
        }

        public IEnumerable<string> GetAllContexts()
        {
            var webViews = WiniumVirtualRoot.Current.Find(TreeScope.Descendants, x => x is WebView);
            return webViews.Select(this.RegisterContext);
        }

        private string RegisterContext(WiniumElement element)
        {
            var existing = this.contexts.FirstOrDefault(x => Equals(x.Value.Element, element)).Key;

            if (existing != null)
            {
                return existing;
            }

            var id = GenerateId(element);
            this.contexts.Add(id, new WebContext(element));

            return id;
        }

        public WebContext GetContext(string id)
        {
            this.GetAllContexts();
            WebContext context;
            if (this.contexts.TryGetValue(id, out context))
            {
                if (!context.Element.IsStale)
                {
                    return context;
                }
            }
            throw new AutomationException("Stale element reference", ResponseStatus.StaleElementReference);
        }

        private static string GenerateId(WiniumElement element)
        {
            Interlocked.Increment(ref safeInstanceCount);

            var friendlyName = !string.IsNullOrWhiteSpace(element.AutomationId)
                                   ? element.AutomationId
                                   : safeInstanceCount.ToString(CultureInfo.InvariantCulture);

            return string.Format("WEBVIEW_{0}", friendlyName);
        }
    }
}
