
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel.Extensions;
using System.Linq;

namespace KS.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can list all the available addons
    /// </summary>
    /// <remarks>
    /// This command lets you list all the available addons that Nitrocid KS registered.
    /// </remarks>
    class LsAddonsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("List of addons"), true);

            // List all the available addons
            var addonNames = AddonTools.ListAddons().Select((addon) => addon.AddonName);
            ListWriterColor.WriteList(addonNames);
            return 0;
        }

        public override int ExecuteDumb(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write(Translate.DoTranslation("List of addons"));

            // List all the available addons
            var addonNames = AddonTools.ListAddons().Select((addon) => addon.AddonName);
            foreach (string addonName in addonNames)
                TextWriterColor.Write($"  - {addonName}");
            return 0;
        }

    }
}
