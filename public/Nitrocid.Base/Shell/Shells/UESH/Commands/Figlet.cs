//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Shells;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using System;
using Textify.Data.Figlet;
using Nitrocid.Base.Kernel.Configuration;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Prints text with figlet
    /// </summary>
    class FigletCommand : BaseCommand, ICommand
    {
        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string figletName = parameters.ContainsSwitch("-figlet") ? parameters.GetSwitchValue("-figlet") : Config.MainConfig.DefaultFigletFontName;
            var figletFont = FigletFonts.TryGetByName(figletName);
            if (figletFont is null)
            {
                // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_FIGLET_INVALIDFIGLET -> Invalid figlet font name.
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_FIGLET_INVALIDFIGLET"), ThemeColorType.Error);
                return 48;
            }

            // Render the figlet text
            variableValue = new FigletText(figletFont)
            {
                Text = parameters.ArgumentsText,
                UseColors = false,
            }.Render();
            TextWriterColor.Write(variableValue);
            return 0;
        }
    }
}
