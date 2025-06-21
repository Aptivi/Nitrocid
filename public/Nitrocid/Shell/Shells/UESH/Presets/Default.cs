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

using System.Text;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Files;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Languages;
using Terminaux.Shell.Prompts;
using Terminaux.Shell.Shells;
using Nitrocid.Users;
using Terminaux.Colors;

namespace Nitrocid.Shell.Shells.UESH.Presets
{
    /// <summary>
    /// Default preset
    /// </summary>
    public class DefaultPreset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "Default";

        private string PresetPromptBuilder()
        {
            var PresetStringBuilder = new StringBuilder();
            string UserDollarSign = UserManagement.GetUserDollarSign(UserManagement.CurrentUser.Username);

            // Build the preset
            if (!KernelEntry.Maintenance)
            {
                // Opening
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.AppendFormat($"[{(Config.MainConfig.ShowShellCount ? $"{ShellManager.ShellCount}:" : "")}");

                // Current username
                PresetStringBuilder.Append(ThemeColorsTools.GetColor("UserNameShellColor").VTSequenceForeground);
                PresetStringBuilder.AppendFormat("{0}", UserManagement.CurrentUser.Username);

                // "At" sign
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append('@');

                // Current hostname
                PresetStringBuilder.Append(ThemeColorsTools.GetColor("HostNameShellColor").VTSequenceForeground);
                PresetStringBuilder.AppendFormat("{0}", Config.MainConfig.HostName);

                // Current directory and shell stack
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.AppendFormat("]{0}:", FilesystemTools.CurrentDir);

                // User dollar sign
                PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.UserDollar).VTSequenceForeground);
                PresetStringBuilder.AppendFormat(" {0} ", UserDollarSign);
                PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground);
            }
            else
            {
                // Maintenance mode
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append(LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_MAINTENANCE") + "> ");
                PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        private string PresetPromptBuilderShowcase()
        {
            var PresetStringBuilder = new StringBuilder();

            // Build the preset
            if (!KernelEntry.Maintenance)
            {
                // Opening
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.AppendFormat($"[{(Config.MainConfig.ShowShellCount ? $"1:" : "")}");

                // Current username
                PresetStringBuilder.Append(ThemeColorsTools.GetColor("UserNameShellColor").VTSequenceForeground);
                PresetStringBuilder.AppendFormat("user");

                // "At" sign
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append('@');

                // Current hostname
                PresetStringBuilder.Append(ThemeColorsTools.GetColor("HostNameShellColor").VTSequenceForeground);
                PresetStringBuilder.AppendFormat("host");

                // Current directory and shell stack
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.AppendFormat("]/home/user:");

                // User dollar sign
                PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.UserDollar).VTSequenceForeground);
                PresetStringBuilder.AppendFormat(" $ ");
                PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground);
            }
            else
            {
                // Maintenance mode
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append(LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_MAINTENANCE") + "> ");
                PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
