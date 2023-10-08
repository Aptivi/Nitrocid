
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
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs.Styles;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
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
            { "Shell", ShellManager.GetShellInfo(ShellType.Shell).ShellPresets["Default"] },
            { "FTPShell", ShellManager.GetShellInfo(ShellType.FTPShell).ShellPresets["Default"] },
            { "MailShell", ShellManager.GetShellInfo(ShellType.MailShell).ShellPresets["Default"] },
            { "SFTPShell", ShellManager.GetShellInfo(ShellType.SFTPShell).ShellPresets["Default"] },
            { "TextShell", ShellManager.GetShellInfo(ShellType.TextShell).ShellPresets["Default"] },
            { "RSSShell", ShellManager.GetShellInfo(ShellType.RSSShell).ShellPresets["Default"] },
            { "JsonShell", ShellManager.GetShellInfo(ShellType.JsonShell).ShellPresets["Default"] },
            { "HTTPShell", ShellManager.GetShellInfo(ShellType.HTTPShell).ShellPresets["Default"] },
            { "HexShell", ShellManager.GetShellInfo(ShellType.HexShell).ShellPresets["Default"] },
            { "AdminShell", ShellManager.GetShellInfo(ShellType.AdminShell).ShellPresets["Default"] },
            { "SqlShell", ShellManager.GetShellInfo(ShellType.SqlShell).ShellPresets["Default"] },
            { "DebugShell", ShellManager.GetShellInfo(ShellType.DebugShell).ShellPresets["Default"] }
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
            if (Presets.ContainsKey(PresetName))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Preset {0} for {1} exists. Setting...", PresetName, ShellType.ToString());
                SetPresetInternal(PresetName, ShellType, Presets);
            }
            else if (CustomPresets.ContainsKey(PresetName))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Preset {0} for {1} from the custom presets exists. Setting...", PresetName, ShellType.ToString());
                SetPresetInternal(PresetName, ShellType, CustomPresets);
            }
            else if (ThrowOnNotFound)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Preset {0} for {1} doesn't exist. Throwing...", PresetName, ShellType.ToString());
                throw new KernelException(KernelExceptionType.NoSuchShellPreset, Translate.DoTranslation("The specified preset {0} is not found."), PresetName);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Preset {0} for {1} doesn't exist. Setting to default...", PresetName, ShellType.ToString());
                SetPresetInternal("Default", ShellType, Presets);
            }
        }

        /// <summary>
        /// Sets the shell preset
        /// </summary>
        /// <param name="PresetName">The preset name</param>
        /// <param name="ShellType">Type of shell</param>
        /// <param name="ThrowOnNotFound">If the preset is not found, throw an exception. Otherwise, use the default preset.</param>
        public static void SetPresetDry(string PresetName, ShellType ShellType, bool ThrowOnNotFound = true) =>
            SetPresetDry(PresetName, ShellManager.GetShellTypeName(ShellType), ThrowOnNotFound);

        /// <summary>
        /// Sets the shell preset
        /// </summary>
        /// <param name="PresetName">The preset name</param>
        /// <param name="ShellType">Type of shell</param>
        /// <param name="ThrowOnNotFound">If the preset is not found, throw an exception. Otherwise, use the default preset.</param>
        public static void SetPresetDry(string PresetName, string ShellType, bool ThrowOnNotFound = true)
        {
            var Presets = GetPresetsFromShell(ShellType);
            var CustomPresets = GetCustomPresetsFromShell(ShellType);

            // Check to see if we have the preset
            if (Presets.ContainsKey(PresetName))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Preset {0} for {1} exists. Setting dryly...", PresetName, ShellType.ToString());
                SetPresetInternal(PresetName, ShellType, Presets, false);
            }
            else if (CustomPresets.ContainsKey(PresetName))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Preset {0} for {1} from the custom presets exists. Setting dryly...", PresetName, ShellType.ToString());
                SetPresetInternal(PresetName, ShellType, CustomPresets, false);
            }
            else if (ThrowOnNotFound)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Preset {0} for {1} doesn't exist. Throwing...", PresetName, ShellType.ToString());
                throw new KernelException(KernelExceptionType.NoSuchShellPreset, Translate.DoTranslation("The specified preset {0} is not found."), PresetName);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Preset {0} for {1} doesn't exist. Setting dryly to default...", PresetName, ShellType.ToString());
                SetPresetInternal("Default", ShellType, Presets, false);
            }
        }

        /// <summary>
        /// Sets the preset
        /// </summary>
        /// <param name="PresetName">The preset name</param>
        /// <param name="ShellType">The shell type</param>
        /// <param name="Presets">Dictionary of presets</param>
        /// <param name="permanent">Saves changes to settings</param>
        internal static void SetPresetInternal(string PresetName, ShellType ShellType, Dictionary<string, PromptPresetBase> Presets, bool permanent = true) =>
            SetPresetInternal(PresetName, ShellManager.GetShellTypeName(ShellType), Presets, permanent);

        /// <summary>
        /// Sets the preset
        /// </summary>
        /// <param name="PresetName">The preset name</param>
        /// <param name="ShellType">The shell type</param>
        /// <param name="Presets">Dictionary of presets</param>
        /// <param name="permanent">Saves changes to settings</param>
        internal static void SetPresetInternal(string PresetName, string ShellType, Dictionary<string, PromptPresetBase> Presets, bool permanent = true)
        {
            CurrentPresets[ShellType] = Presets[PresetName];
            if (permanent)
            {
                switch (ShellType)
                {
                    case "Shell":
                        {
                            Config.MainConfig.PromptPreset = PresetName;
                            break;
                        }
                    case "TextShell":
                        {
                            Config.MainConfig.TextEditPromptPreset = PresetName;
                            break;
                        }
                    case "SFTPShell":
                        {
                            Config.MainConfig.SFTPPromptPreset = PresetName;
                            break;
                        }
                    case "RSSShell":
                        {
                            Config.MainConfig.RSSPromptPreset = PresetName;
                            break;
                        }
                    case "MailShell":
                        {
                            Config.MainConfig.MailPromptPreset = PresetName;
                            break;
                        }
                    case "JsonShell":
                        {
                            Config.MainConfig.JSONShellPromptPreset = PresetName;
                            break;
                        }
                    case "HTTPShell":
                        {
                            Config.MainConfig.HTTPShellPromptPreset = PresetName;
                            break;
                        }
                    case "HexShell":
                        {
                            Config.MainConfig.HexEditPromptPreset = PresetName;
                            break;
                        }
                    case "FTPShell":
                        {
                            Config.MainConfig.FTPPromptPreset = PresetName;
                            break;
                        }
                    case "AdminShell":
                        {
                            Config.MainConfig.AdminShellPromptPreset = PresetName;
                            break;
                        }
                    case "SqlShell":
                        {
                            Config.MainConfig.SqlShellPromptPreset = PresetName;
                            break;
                        }
                    case "DebugShell":
                        {
                            Config.MainConfig.DebugShellPromptPreset = PresetName;
                            break;
                        }
                }
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
            TextWriterColor.WriteKernelColor(CurrentPresetBase.PresetPrompt, false, KernelColorType.Input);
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
            TextWriterColor.WriteKernelColor(CurrentPresetBase.PresetPromptCompletion, false, KernelColorType.Input);
        }

        /// <summary>
        /// Prompts a user to select the preset
        /// </summary>
        public static void PromptForPresets() =>
            PromptForPresets(ShellStart.ShellStack[^1].ShellType);

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
            int SelectedPreset = SelectionStyle.PromptSelection(TextTools.FormatString(Translate.DoTranslation("Select preset for {0}:"), shellType), string.Join("/", PresetNames), PresetDisplays);
            if (SelectedPreset == -1)
                return;
            string SelectedPresetName = Presets.Keys.ElementAt(SelectedPreset - 1);
            SetPreset(SelectedPresetName, shellType);
        }

    }
}
