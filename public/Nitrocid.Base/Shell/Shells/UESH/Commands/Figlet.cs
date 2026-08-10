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

using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.Data.Figlet;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Prints text with figlet
    /// </summary>
    class FigletCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "figlet";

        // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_FIGLET_DESC -> Renders text in a nice ASCII figlet
        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_FIGLET_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "text", new()
                    {
                        // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_FIGLET_ARGUMENT_TEXT_DESC -> Text to print in a conversation bubble
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_FIGLET_ARGUMENT_TEXT_DESC"
                    }),
                ],
                [
                    // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_FIGLET_SWITCH_FIGLET_DESC -> Figlet font to render with
                    new SwitchInfo("figlet", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_FIGLET_SWITCH_FIGLET_DESC"),
                ], true)
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

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
