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
    /// You can list all the available function parameters from a function
    /// </summary>
    /// <remarks>
    /// This command lets you list all the function parameters from a function.
    /// </remarks>
    class LsAddonFuncParamsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("List of function parameters for") + $" {parameters.ArgumentsList[0]} -> {parameters.ArgumentsList[1]}", true);

            // List all the available addons
            var list = InterAddonTools.GetFunctionParameters(parameters.ArgumentsList[0], parameters.ArgumentsList[1]) ?? [];
            TextWriters.WriteList(list);
            return 0;
        }

    }
}
