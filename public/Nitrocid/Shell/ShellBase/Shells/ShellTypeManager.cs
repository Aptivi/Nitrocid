
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

using KS.Kernel.Debugging;
using KS.Shell.Prompts;
using System;

namespace KS.Shell.ShellBase.Shells
{
    /// <summary>
    /// Shell type management module
    /// </summary>
    public static class ShellTypeManager
    {
        /// <summary>
        /// Registers the custom shell. Should be called when the mods start up.
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellTypeInfo">The shell type information</param>
        public static void RegisterShell(string ShellType, BaseShellInfo ShellTypeInfo)
        {
            if (!ShellTypeExists(ShellType))
            {
                // First, add the shell
                ShellManager.availableShells.Add(ShellType, ShellTypeInfo);

                // Then, add the default preset if the current preset is not found
                if (PromptPresetManager.CurrentPresets.ContainsKey(ShellType))
                    return;

                // Rare state.
                DebugWriter.WriteDebug(DebugLevel.I, "Reached rare state or unconfigurable shell.");
                var presets = ShellTypeInfo.ShellPresets;
                var basePreset = new PromptPresetBase();
                if (presets is not null)
                {
                    // Add a default preset
                    if (presets.ContainsKey("Default"))
                        PromptPresetManager.CurrentPresets.Add(ShellType, "Default");
                    else
                        PromptPresetManager.CurrentPresets.Add(ShellType, basePreset.PresetName);
                }
                else
                {
                    // Make a base shell preset and set it as default.
                    PromptPresetManager.CurrentPresets.Add(ShellType, basePreset.PresetName);
                }
            }
        }

        /// <summary>
        /// Unregisters the custom shell. Should be called when the mods shut down.
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static void UnregisterShell(string ShellType)
        {
            if (!IsShellBuiltin(ShellType))
            {
                // First, remove the shell
                ShellManager.availableShells.Remove(ShellType);

                // Then, remove the preset
                PromptPresetManager.CurrentPresets.Remove(ShellType);
            }
        }

        /// <summary>
        /// Is the shell pre-defined in Nitrocid KS?
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        /// <returns>If available in ShellType, then it's a built-in shell, thus returning true. Otherwise, false for custom shells.</returns>
        public static bool IsShellBuiltin(string ShellType) =>
            Enum.IsDefined(typeof(ShellType), ShellType);

        /// <summary>
        /// Does the shell exist?
        /// </summary>
        /// <param name="ShellType">Shell type to check</param>
        /// <returns>True if it exists; false otherwise.</returns>
        public static bool ShellTypeExists(string ShellType) =>
            ShellManager.AvailableShells.ContainsKey(ShellType);
    }
}
