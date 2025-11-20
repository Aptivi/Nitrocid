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

using System;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Inputs.Interactive;
using Nitrocid.Base.Misc.Interactives;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Configuration.Instances;
using Nitrocid.Base.Security.Permissions;

namespace Nitrocid.Base.Kernel.Configuration.Settings
{
    /// <summary>
    /// Settings application module
    /// </summary>
    public static class SettingsApp
    {

        /// <summary>
        /// Opens the main page for settings, listing all the sections that are configurable
        /// </summary>
        /// <param name="settingsType">Type of settings</param>
        public static void OpenMainPage(string settingsType) =>
            OpenMainPage(Config.GetKernelConfig(settingsType));

        /// <summary>
        /// Opens the main page for settings, listing all the sections that are configurable
        /// </summary>
        /// <param name="settingsType">Type of settings</param>
        public static void OpenMainPage(BaseKernelConfig? settingsType)
        {
            // Verify that we actually have the type
            if (settingsType is null)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_EXCEPTION_TYPENOTFOUND"), true, ThemeColorType.Error);
                return;
            }

            // Verify the user permission
            PermissionsTools.Demand(PermissionTypes.ManipulateSettings);

            // Make a new settings TUI instance
            var tui = new SettingsCli
            {
                config = settingsType,
                lastFirstPaneIdx = -1,
            };
            tui.Bindings.Add(new InteractiveTuiBinding<(string, int), (string, string)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_SET"), ConsoleKey.Enter, (_, entryIdx, _, keyIdx) => tui.Set(entryIdx, keyIdx)));
            tui.Bindings.Add(new InteractiveTuiBinding<(string, int), (string, string)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_SAVE"), ConsoleKey.F1, (_, _, _, _) => tui.Save()));
            tui.Bindings.Add(new InteractiveTuiBinding<(string, int), (string, string)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_SAVEAS"), ConsoleKey.F2, (_, _, _, _) => tui.SaveAs()));
            tui.Bindings.Add(new InteractiveTuiBinding<(string, int), (string, string)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_LOADFROM"), ConsoleKey.F3, (_, _, _, _) => tui.LoadFrom()));
            tui.Bindings.Add(new InteractiveTuiBinding<(string, int), (string, string)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_RELOAD"), ConsoleKey.F4, (_, _, _, _) => tui.Reload()));
            tui.Bindings.Add(new InteractiveTuiBinding<(string, int), (string, string)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_MIGRATE"), ConsoleKey.F5, (_, _, _, _) => tui.Migrate()));
            tui.Bindings.Add(new InteractiveTuiBinding<(string, int), (string, string)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_CHECKUPDATES"), ConsoleKey.F6, (_, _, _, _) => tui.CheckUpdates()));
            tui.Bindings.Add(new InteractiveTuiBinding<(string, int), (string, string)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SYSINFO"), ConsoleKey.F7, (_, _, _, _) => tui.SystemInfo()));
            tui.Bindings.Add(new InteractiveTuiBinding<(string, int), (string, string)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_RESETALL"), ConsoleKey.F8, (_, _, _, _) => tui.ResetAll()));
            tui.Bindings.Add(new InteractiveTuiBinding<(string, int), (string, string)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_RESETENTRY"), ConsoleKey.R, ConsoleModifiers.Shift, (_, entryIdx, _, keyIdx) => tui.ResetEntry(entryIdx, keyIdx)));
            tui.Bindings.Add(new InteractiveTuiBinding<(string, int), (string, string)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SELECTCONFIG"), ConsoleKey.F9, (_, _, _, _) => tui.SelectConfig()));
            // TODO: NKS_KERNEL_CONFIGURATION_SETTINGS_APP_LEGACYMULTIVARPROCESSING -> Legacy Multivar Processing
            tui.Bindings.Add(new InteractiveTuiBinding<(string, int), (string, string)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_LEGACYMULTIVARPROCESSING"), ConsoleKey.F10, (_, _, _, _) => tui.EnableLegacyMultivarProcessing()));
            InteractiveTuiTools.OpenInteractiveTui(tui);
        }
    }
}
