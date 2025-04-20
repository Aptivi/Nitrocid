//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Languages;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Extensions;
using Terminaux.Writer.CyclicWriters;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;

namespace Nitrocid.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can list all the available public fields from an addon
    /// </summary>
    /// <remarks>
    /// This command lets you list all the public fields from an addon.
    /// </remarks>
    class LsAddonFieldsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("List of fields for") + $" {parameters.ArgumentsList[0]}", true);

            // List all the available addons
            var list = InterAddonTools.ListAvailableFields(parameters.ArgumentsList[0]);
            TextWriters.WriteList(list);
            return 0;
        }

        public override int ExecuteDumb(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write(Translate.DoTranslation("List of fields for") + $" {parameters.ArgumentsList[0]}");

            // List all the available addons
            var list = InterAddonTools.ListAvailableFields(parameters.ArgumentsList[0]);
            foreach (string field in list)
                TextWriterColor.Write($"  - {field}");
            return 0;
        }

    }
}
