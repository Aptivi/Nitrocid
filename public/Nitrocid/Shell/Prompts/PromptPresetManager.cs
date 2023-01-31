
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

using System.Collections.Generic;
using System.Data;
using System.Linq;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs.Styles;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Prompts
{
    /// <summary>
    /// Prompt preset management module
    /// </summary>
    public static class PromptPresetManager
    {

        // Current presets
        internal static Dictionary<string, PromptPresetBase> CurrentPresets = new()
        {
            { "Shell", Shell.GetShellInfo(ShellType.Shell).ShellPresets["Default"] },
            { "FTPShell", Shell.GetShellInfo(ShellType.FTPShell).ShellPresets["Default"] },
            { "MailShell", Shell.GetShellInfo(ShellType.MailShell).ShellPresets["Default"] },
            { "SFTPShell", Shell.GetShellInfo(ShellType.SFTPShell).ShellPresets["Default"] },
            { "TextShell", Shell.GetShellInfo(ShellType.TextShell).ShellPresets["Default"] },
            { "RSSShell", Shell.GetShellInfo(ShellType.RSSShell).ShellPresets["Default"] },
            { "JsonShell", Shell.GetShellInfo(ShellType.JsonShell).ShellPresets["Default"] },
            { "HTTPShell", Shell.GetShellInfo(ShellType.HTTPShell).ShellPresets["Default"] },
            { "HexShell", Shell.GetShellInfo(ShellType.HexShell).ShellPresets["Default"] },
            { "ArchiveShell", Shell.GetShellInfo(ShellType.ArchiveShell).ShellPresets["Default"] },
            { "AdminShell", Shell.GetShellInfo(ShellType.AdminShell).ShellPresets["Default"] }
        };

        /// <summary>
        /// Sets the shell preset
        /// </summary>
        /// <param name="PresetName">The preset name</param>
        /// <param name="ShellType">Type of shell</param>
        /// <param name="ThrowOnNotFound">If the preset is not found, throw an exception. Otherwise, use the default preset.</param>
        public static void SetPreset(string PresetName, ShellType ShellType, bool ThrowOnNotFound = true) =>
            SetPreset(PresetName, Shell.GetShellTypeName(ShellType), ThrowOnNotFound);

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
            if (Presets.ContainsKey(PresetName))
            {
                SetPresetInternal(PresetName, ShellType, Presets);
            }
            else if (CustomPresets.ContainsKey(PresetName))
            {
                SetPresetInternal(PresetName, ShellType, CustomPresets);
            }
            else if (ThrowOnNotFound)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Preset {0} for {1} doesn't exist. Throwing...", PresetName, ShellType.ToString());
                throw new KernelException(KernelExceptionType.NoSuchShellPreset, Translate.DoTranslation("The specified preset {0} is not found."), PresetName);
            }
            else
            {
                SetPresetInternal("Default", ShellType, Presets);
            }
        }

        /// <summary>
        /// Sets the preset
        /// </summary>
        /// <param name="PresetName">The preset name</param>
        /// <param name="ShellType">The shell type</param>
        /// <param name="Presets">Dictionary of presets</param>
        internal static void SetPresetInternal(string PresetName, ShellType ShellType, Dictionary<string, PromptPresetBase> Presets) =>
            SetPresetInternal(PresetName, Shell.GetShellTypeName(ShellType), Presets);

        /// <summary>
        /// Sets the preset
        /// </summary>
        /// <param name="PresetName">The preset name</param>
        /// <param name="ShellType">The shell type</param>
        /// <param name="Presets">Dictionary of presets</param>
        internal static void SetPresetInternal(string PresetName, string ShellType, Dictionary<string, PromptPresetBase> Presets)
        {
            CurrentPresets[ShellType] = Presets[PresetName];
            switch (ShellType)
            {
                case "Shell":
                    {
                        ConfigTools.SetConfigValue(ConfigCategory.Shell, "Prompt Preset", PresetName);
                        break;
                    }
                case "TextShell":
                    {
                        ConfigTools.SetConfigValue(ConfigCategory.Shell, "Text Edit Prompt Preset", PresetName);
                        break;
                    }
                case "SFTPShell":
                    {
                        ConfigTools.SetConfigValue(ConfigCategory.Shell, "SFTP Prompt Preset", PresetName);
                        break;
                    }
                case "RSSShell":
                    {
                        ConfigTools.SetConfigValue(ConfigCategory.Shell, "RSS Prompt Preset", PresetName);
                        break;
                    }
                case "MailShell":
                    {
                        ConfigTools.SetConfigValue(ConfigCategory.Shell, "Mail Prompt Preset", PresetName);
                        break;
                    }
                case "JsonShell":
                    {
                        ConfigTools.SetConfigValue(ConfigCategory.Shell, "JSON Shell Prompt Preset", PresetName);
                        break;
                    }
                case "HTTPShell":
                    {
                        ConfigTools.SetConfigValue(ConfigCategory.Shell, "HTTP Shell Prompt Preset", PresetName);
                        break;
                    }
                case "HexShell":
                    {
                        ConfigTools.SetConfigValue(ConfigCategory.Shell, "Hex Edit Prompt Preset", PresetName);
                        break;
                    }
                case "FTPShell":
                    {
                        ConfigTools.SetConfigValue(ConfigCategory.Shell, "FTP Prompt Preset", PresetName);
                        break;
                    }
                case "ArchiveShell":
                    {
                        ConfigTools.SetConfigValue(ConfigCategory.Shell, "Archive Shell Prompt Preset", PresetName);
                        break;
                    }
                case "AdminShell":
                    {
                        ConfigTools.SetConfigValue(ConfigCategory.Shell, "Admin Shell Prompt Preset", PresetName);
                        break;
                    }
            }
        }

        /// <summary>
        /// Gets the current preset base from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static PromptPresetBase GetCurrentPresetBaseFromShell(ShellType ShellType) =>
            GetCurrentPresetBaseFromShell(Shell.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the current preset base from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static PromptPresetBase GetCurrentPresetBaseFromShell(string ShellType) =>
            Shell.GetShellInfo(ShellType).CurrentPreset;

        /// <summary>
        /// Gets the predefined presets from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetPresetsFromShell(ShellType ShellType) =>
            GetPresetsFromShell(Shell.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the predefined presets from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetPresetsFromShell(string ShellType) =>
            Shell.GetShellInfo(ShellType).ShellPresets;

        /// <summary>
        /// Gets the custom presets (defined by mods) from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetCustomPresetsFromShell(ShellType ShellType) =>
            GetCustomPresetsFromShell(Shell.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the custom presets (defined by mods) from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetCustomPresetsFromShell(string ShellType) =>
            Shell.GetShellInfo(ShellType).CustomShellPresets;

        /// <summary>
        /// Writes the shell prompt
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        public static void WriteShellPrompt(ShellType ShellType) =>
            WriteShellPrompt(Shell.GetShellTypeName(ShellType));

        /// <summary>
        /// Writes the shell prompt
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        public static void WriteShellPrompt(string ShellType)
        {
            var CurrentPresetBase = GetCurrentPresetBaseFromShell(ShellType);
            TextWriterColor.Write(CurrentPresetBase.PresetPrompt, false, KernelColorType.Input);
        }

        /// <summary>
        /// Writes the shell completion prompt
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        public static void WriteShellCompletionPrompt(ShellType ShellType) =>
            WriteShellCompletionPrompt(Shell.GetShellTypeName(ShellType));

        /// <summary>
        /// Writes the shell completion prompt
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        public static void WriteShellCompletionPrompt(string ShellType)
        {
            var CurrentPresetBase = GetCurrentPresetBaseFromShell(ShellType);
            TextWriterColor.Write(CurrentPresetBase.PresetPromptCompletion, false, KernelColorType.Input);
        }

        /// <summary>
        /// Prompts a user to select the preset
        /// </summary>
        public static void PromptForPresets() =>
            PromptForPresets(ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].ShellType);

        /// <summary>
        /// Prompts a user to select the preset
        /// </summary>
        /// <param name="shellType">Sets preset in shell type</param>
        public static void PromptForPresets(ShellType shellType) =>
            PromptForPresets(shellType.ToString());

        /// <summary>
        /// Prompts a user to select the preset
        /// </summary>
        /// <param name="shellType">Sets preset in shell type</param>
        public static void PromptForPresets(string shellType)
        {
            var Presets = GetPresetsFromShell(shellType);

            // Add the custom presets to the local dictionary
            foreach (string PresetName in GetCustomPresetsFromShell(shellType).Keys)
                Presets.Add(PresetName, Presets[PresetName]);

            // Now, prompt the user
            var PresetNames = Presets.Keys.ToArray();
            var PresetDisplays = Presets.Values.Select(Preset => Preset.PresetPrompt).ToArray();
            string SelectedPreset = ChoiceStyle.PromptChoice(Translate.DoTranslation("Select preset for {0}:").FormatString(shellType), string.Join("/", PresetNames), PresetDisplays, ChoiceStyle.ChoiceOutputType.Modern, true);
            SetPreset(SelectedPreset, shellType);
        }

    }
}
