using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace KSMod
{
    internal class Command : BaseCommand, ICommand
    {
        public override string Command =>
            "command";

        public override string HelpDefinition =>
            "Definition";

        public override int Execute(IShell shell, CommandParameters parameters, ref string variableValue)
        {
            return 0;
        }
    }
}
