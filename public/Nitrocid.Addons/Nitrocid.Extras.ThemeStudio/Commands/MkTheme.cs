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
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Events;
using Nitrocid.Base.Languages;
using Nitrocid.Extras.ThemeStudio.Studio;
using Terminaux.Inputs.Interactive;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;

namespace Nitrocid.Extras.ThemeStudio.Commands
{
    /// <summary>
    /// Makes a new theme
    /// </summary>
    /// <remarks>
    /// This opens up a theme studio to manage the newly-created theme colors that you can adjust. This will allow you to create your own themes for Nitrocid.
    /// <br></br>
    /// If you want your theme to be included in the default Nitrocid themes, let us know.
    /// </remarks>
    class MkThemeCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "mktheme";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_THEMESTUDIO_COMMAND_MKTHEME_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "themeName", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_THEMESTUDIO_COMMAND_MKTHEME_ARGUMENT_THEMENAME_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Inform user that we're on the studio
            string themeName = parameters.ArgumentsList[0];
            EventsManager.FireEvent(EventType.ThemeStudioStarted);
            DebugWriter.WriteDebug(DebugLevel.I, "Starting theme studio with theme name {0}", vars: [themeName]);

            var tui = new ThemeStudioCli()
            {
                themeName = themeName,
            };
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_KEYBINDING_CHANGE"), ConsoleKey.Enter, (colorType, _, _, _) => tui.Change(colorType ?? ThemeColorType.NeutralText.ToString())));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_KEYBINDING_COPY"), ConsoleKey.F1, (colorType, _, _, _) => tui.Copy(colorType ?? ThemeColorType.NeutralText.ToString())));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_KEYBINDING_SAVE"), ConsoleKey.F2, (_, _, _, _) => tui.SaveThemeToCurrentDirectory()));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_TUI_KEYBINDING_LOAD"), ConsoleKey.F3, (_, _, _, _) => tui.Load()));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_SAVETOOTHER"), ConsoleKey.F4, (_, _, _, _) => tui.SaveThemeToAnotherDirectoryPrompt()));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_SAVETOCURRENTAS"), ConsoleKey.F5, (_, _, _, _) => tui.SaveThemeToCurrentDirectoryAltPrompt()));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_SAVETOOTHERAS"), ConsoleKey.F6, (_, _, _, _) => tui.SaveThemeToAnotherDirectoryAltPrompt()));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_LOADFROM"), ConsoleKey.F7, (_, _, _, _) => tui.LoadThemeFromFilePrompt()));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_LOADFROMBUILTIN"), ConsoleKey.F8, (_, _, _, _) => tui.LoadThemeFromResourcePrompt()));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_THEMESTUDIO_APP_LOADCURRENT"), ConsoleKey.F9, (_, _, _, _) => tui.LoadThemeFromCurrentColors()));
            InteractiveTuiTools.OpenInteractiveTui(tui);

            // Raise event
            EventsManager.FireEvent(EventType.ThemeStudioExit);
            return 0;
        }
    }
}
