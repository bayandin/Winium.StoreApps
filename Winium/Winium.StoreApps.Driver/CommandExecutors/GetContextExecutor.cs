namespace Winium.StoreApps.Driver.CommandExecutors
{
    #region

    using Winium.StoreApps.Common;

    #endregion

    internal class GetContextExecutor : CommandExecutorBase
    {
        protected override string DoImpl()
        {
            return this.JsonResponse(ResponseStatus.Success, this.Automator.CurrentContext);
        }
    }
}
