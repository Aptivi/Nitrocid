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

using System.Collections.Generic;
using System.Data;
using System.Linq;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Inputs.Styles.Selection;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Shells;
using Textify.General;

namespace Nitrocid.Shell.Prompts
{
    /// <summary>
    /// Prompt preset management module
    /// </summary>
    public static class PromptPresetManager
    {

        // Current presets
        internal static Dictionary<string, string> CurrentPresets = new()
        {
            { "Shell", "Default" },
            { "FTPShell", "Default" },
            { "MailShell", "Default" },
            { "SFTPShell", "Default" },
            { "TextShell", "Default" },
            { "RSSShell", "Default" },
            { "JsonShell", "Default" },
            { "HTTPShell", "Default" },
            { "HexShell", "Default" },
            { "AdminShell", "Default" },
            { "SqlShell", "Default" },
            { "DebugShell", "Default" },
        };

        /// <summary>
        /// Sets the shell preset
        /// </summary>
        /// <param name="PresetName">The preset name</param>
        /// <param name="ShellType">Type of shell</param>
        /// <param name="ThrowOnNotFound">If the preset is not found, throw an exception. Otherwise, use the default preset.</param>
        public static void SetPreset(string PresetName, ShellType ShellType, bool ThrowOnNotFound = true) =>
            SetPreset(PresetName, ShellManager.GetShellTypeName(ShellType), ThrowOnNotFound);

        /// <summary>
        /// Sets the shell preset
        /// </summary>
        /// <param name="PresetName">The preset name</param>
        /// <param name="ShellType">Type of shell</param>
        /// <param name="ThrowOnNotFound">If the preset is not found, throw an exception. Otherwise, use the default preset.</param>
        public static void SetPreset(string PresetName, string ShellType, bool ThrowOnNotFound = true)
        {
            var Presets = GetPresetsFromShell(ShellType);
            var CustomPresets = GetCustomPresetsFromShell(ShellType);

            // Check to see if we have the preset
            if (Presets.ContainsKey(PresetName) || CustomPresets.ContainsKey(PresetName))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Preset {0} for {1} exists. Setting dryly...", vars: [PresetName, ShellType.ToString()]);
                CurrentPresets[ShellType] = PresetName;
            }
            else if (ThrowOnNotFound)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Preset {0} for {1} doesn't exist. Throwing...", vars: [PresetName, ShellType.ToString()]);
                throw new KernelException(KernelExceptionType.NoSuchShellPreset, LanguageTools.GetLocalized("NKS_SHELL_PROMPTS_PRESETS_EXCEPTION_PRESETNOTFOUND"), PresetName);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Preset {0} for {1} doesn't exist. Setting dryly to default...", vars: [PresetName, ShellType.ToString()]);
                CurrentPresets[ShellType] = "Default";
            }
        }

        /// <summary>
        /// Gets the current preset base from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static PromptPresetBase GetCurrentPresetBaseFromShell(ShellType ShellType) =>
            GetCurrentPresetBaseFromShell(ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the current preset base from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static PromptPresetBase GetCurrentPresetBaseFromShell(string ShellType) =>
            ShellManager.GetShellInfo(ShellType).CurrentPreset;

        /// <summary>
        /// Gets the predefined presets from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetPresetsFromShell(ShellType ShellType) =>
            GetPresetsFromShell(ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the predefined presets from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetPresetsFromShell(string ShellType) =>
            ShellManager.GetShellInfo(ShellType).ShellPresets;

        /// <summary>
        /// Gets the custom presets (defined by mods) from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetCustomPresetsFromShell(ShellType ShellType) =>
            GetCustomPresetsFromShell(ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the custom presets (defined by mods) from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetCustomPresetsFromShell(string ShellType) =>
            ShellManager.GetShellInfo(ShellType).CustomShellPresets;

        /// <summary>
        /// Gets all presets from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetAllPresetsFromShell(ShellType ShellType) =>
            GetAllPresetsFromShell(ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets all presets from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetAllPresetsFromShell(string ShellType)
        {
            var presets = new Dictionary<string, PromptPresetBase>();
            foreach (var preset in GetPresetsFromShell(ShellType))
                presets.Add(preset.Key, preset.Value);
            foreach (var preset in GetCustomPresetsFromShell(ShellType))
                presets.Add(preset.Key, preset.Value);
            return presets;
        }

        /// <summary>
        /// Writes the shell prompt
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        public static void WriteShellPrompt(ShellType ShellType) =>
            WriteShellPrompt(ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Writes the shell prompt
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        public static void WriteShellPrompt(string ShellType)
        {
            var CurrentPresetBase = GetCurrentPresetBaseFromShell(ShellType);
            TextWriters.Write(CurrentPresetBase.PresetPrompt, false, KernelColorType.Input);
        }

        /// <summary>
        /// Writes the shell completion prompt
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        public static void WriteShellCompletionPrompt(ShellType ShellType) =>
            WriteShellCompletionPrompt(ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Writes the shell completion prompt
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        public static void WriteShellCompletionPrompt(string ShellType)
        {
            var CurrentPresetBase = GetCurrentPresetBaseFromShell(ShellType);
            TextWriters.Write(CurrentPresetBase.PresetPromptCompletion, false, KernelColorType.Input);
        }

        /// <summary>
        /// Prompts a user to select the preset
        /// </summary>
        public static string PromptForPresets() =>
            PromptForPresets(ShellManager.CurrentShellType);

        /// <summary>
        /// Prompts a user to select the preset
        /// </summary>
        /// <param name="shellType">Sets preset in shell type</param>
        public static string PromptForPresets(ShellType shellType) =>
            PromptForPresets(shellType.ToString());

        /// <summary>
        /// Prompts a user to select the preset
        /// </summary>
        /// <param name="shellType">Sets preset in shell type</param>
        public static string PromptForPresets(string shellType)
        {
            var Presets = GetPresetsFromShell(shellType);

            // Add the custom presets to the local dictionary
            foreach (string PresetName in GetCustomPresetsFromShell(shellType).Keys)
                Presets.Add(PresetName, Presets[PresetName]);

            // Now, prompt the user
            var PresetNames = Presets.Select((kvp) => (kvp.Key, kvp.Value.PresetPromptShowcase)).ToArray();
            int SelectedPreset = SelectionStyle.PromptSelection(TextTools.FormatString(LanguageTools.GetLocalized("NKS_SHELL_PROMPTS_PRESETS_SELECT"), shellType), PresetNames);
            if (SelectedPreset == -1)
                return "Default";
            string SelectedPresetName = Presets.Keys.ElementAt(SelectedPreset - 1);
            SetPreset(SelectedPresetName, shellType);
            return SelectedPresetName;
        }

    }
}
