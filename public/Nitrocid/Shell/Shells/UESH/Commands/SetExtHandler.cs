﻿//
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files.Extensions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Sets the default extension handler
    /// </summary>
    /// <remarks>
    /// This command lets you set the default extension handler to a specified implementer
    /// </remarks>
    class SetExtHandlerCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!ExtensionHandlerTools.IsHandlerRegistered(parameters.ArgumentsList[0]))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No such extension."), KernelColorType.Error);
                return 23;
            }
            if (!ExtensionHandlerTools.IsHandlerRegisteredSpecific(parameters.ArgumentsList[0], parameters.ArgumentsList[1]))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No such implementer."), KernelColorType.Error);
                return 24;
            }
            ExtensionHandlerTools.SetExtensionHandler(parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
            return 0;
        }
    }
}