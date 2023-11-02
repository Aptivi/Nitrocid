//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Switches;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lists all configurations
    /// </summary>
    /// <remarks>
    /// This command lists all the configurations.
    /// </remarks>
    class LsConfigsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var configs = Config.GetKernelConfigs();
            bool deep = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-deep");
            foreach (var config in configs)
            {
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Info for") + $" {config.GetType().Name}", true);
                TextWriterColor.Write($"{Translate.DoTranslation("Entries count")}: {config.SettingsEntries.Length}");
                if (deep)
                {
                    foreach (var entry in config.SettingsEntries)
                    {
                        SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Entry name") + $": {entry.Name}", true);
                        TextWriterColor.Write($"{Translate.DoTranslation("Displaying as")}: {entry.DisplayAs}");
                        TextWriterColor.Write($"{Translate.DoTranslation("Description")}: {entry.Desc}");
                        TextWriterColor.Write($"{Translate.DoTranslation("Keys count")}: {entry.Keys.Length}");
                    }
                }
            }
            TextWriterColor.Write(Translate.DoTranslation("Use the {0} command to get the individual keys"), "lsconfigvalues");
            return 0;
        }

    }
}
