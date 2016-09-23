namespace Winium.StoreApps.Driver.CommandExecutors
{
    using Winium.StoreApps.Driver.Web;

    internal class CommandExecutorWebContextForward : CommandExecutorBase
    {
        #region Methods

        protected override string DoImpl()
        {
            this.ExecutedCommand.Context = this.Automator.CurrentContext;
            var atom = DriverCommandToAtomMapping.GetAtomOrDefault(this.ExecutedCommand.Name);
            this.ExecutedCommand.Atom = atom;

            return this.Automator.CommandForwarder.ForwardCommand(this.ExecutedCommand);
        }

        #endregion
    }
}
