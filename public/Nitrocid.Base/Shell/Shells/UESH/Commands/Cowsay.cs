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

using System;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Prints text with cowsay
    /// </summary>
    class CowsayCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "cowsay";

        // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_COWSAY_DESC -> Renders text in a nice ASCII cow
        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COWSAY_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "text", new()
                    {
                        // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_COWSAY_ARGUMENT_TEXT_DESC -> Text to print in a conversation bubble
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COWSAY_ARGUMENT_TEXT_DESC"
                    }),
                ],
                [
                    // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_COWSAY_SWITCH_COW_DESC -> Cow name to render with
                    new SwitchInfo("cow", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COWSAY_SWITCH_COW_DESC"),
                ], true)
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string cowsayName = parameters.ContainsSwitch("-cow") ? parameters.GetSwitchValue("-cow") : nameof(CowName.Default);
            if (!Enum.TryParse<CowName>(cowsayName, out var cow))
            {
                // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_COWSAY_INVALIDCOW -> Invalid cow art name.
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COWSAY_INVALIDCOW"), ThemeColorType.Error);
                return 47;
            }

            // Render the cowsay text
            variableValue = new CowsayText(cow)
            {
                Text = parameters.ArgumentsText,
                UseColors = false,
            }.Render();
            TextWriterColor.Write(variableValue);
            return 0;
        }
    }
}
