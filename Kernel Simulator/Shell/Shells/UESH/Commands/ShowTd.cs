using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows the current time and date
    /// </summary>
    /// <remarks>
    /// If you want to know what time is it without repeatedly going into the clock, you can use this command to show you the current time and date, as well as your time zone.
    /// </remarks>
    class ShowTdCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            TimeDate.TimeDate.ShowCurrentTimes();
        }

    }
}