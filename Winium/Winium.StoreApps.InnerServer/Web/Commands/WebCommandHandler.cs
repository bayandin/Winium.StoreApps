namespace Winium.StoreApps.InnerServer.Web.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Threading;

    using Windows.UI.Core;

    using Newtonsoft.Json;

    using Winium.StoreApps.Common;
    using Winium.StoreApps.Common.Exceptions;
    using Winium.StoreApps.InnerServer.Commands;
    using Winium.StoreApps.InnerServer.Web;

    internal abstract class WebCommandHandler : CommandBase
    {
        public WebContext Context { get; private set; }

        public string Atom { get; private set; }

        public WebCommandHandler(WebContext context, string atom)
        {
            this.Context = context;
            this.Atom = atom;
        }

        private TimeSpan atomExecutionTimeout = TimeSpan.FromMilliseconds(-1);

        private static string CreateArgumentString(IEnumerable<object> args)
        {
            var builder = new StringBuilder();
            foreach (var arg in args)
            {
                if (builder.Length > 0)
                {
                    builder.Append(",");
                }

                var argAsString = JsonConvert.SerializeObject(arg);
                builder.Append(argAsString);
            }

            return builder.ToString();
        }

        protected string EvaluateAtom(WebContext environment, string executedAtom, params object[] args)
        {
            var browser = environment.Browser;
            var argumentString = CreateArgumentString(args);

            var scriptSimplified = "(" + executedAtom + ")(" + argumentString + ");";
            var task = browser.InvokeScriptAsync("eval", new[] { scriptSimplified }).AsTask();
            task.Wait(this.atomExecutionTimeout);
            return task.Result;

            // TODO why https://github.com/forcedotcom/windowsphonedriver used to separate invokes?
            //var script = "window.top.__wd_fn_result = (" + executedAtom + ")(" + argumentString + ");";
            //var result = string.Empty;
            //var synchronizer = new ManualResetEvent(false);
            //browser.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, 
            //    () =>
            //    {
            //        try
            //        {
            //            browser.InvokeScriptAsync("eval", new []{script}).AsTask().Wait();
            //            result = browser.InvokeScriptAsync("eval", new []{"window.top.__wd_fn_result"}).AsTask().Result;
            //        }
            //        catch (Exception ex)
            //        {
            //            throw new AutomationException(string.Format(
            //                           CultureInfo.InvariantCulture,
            //                           "Unexpected exception {0}: {1}",
            //                           ex.GetType(),
            //                           ex.Message), ResponseStatus.UnknownError);
            //        }
            //        finally
            //        {
            //            synchronizer.Set();
            //        }
            //    }).AsTask().Wait(this.atomExecutionTimeout);

            //return result;
        }

        /// <summary>
        /// Sets the timeout for execution of an atom.
        /// </summary>
        /// <param name="timeout">The <see cref="TimeSpan"/> representing the timeout for the atom execution.</param>
        protected void SetAtomExecutionTimeout(TimeSpan timeout)
        {
            this.atomExecutionTimeout = timeout;
        }
    }
}
