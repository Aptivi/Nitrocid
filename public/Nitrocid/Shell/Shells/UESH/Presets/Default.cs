//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Text;
using KS.ConsoleBase.Colors;
using KS.Files.Folders;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.Network.Base;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Shells;
using KS.Users;

namespace KS.Shell.Shells.UESH.Presets
{
    /// <summary>
    /// Default preset
    /// </summary>
    public class DefaultPreset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "Default";

        /// <inheritdoc/>
        public override string PresetPrompt =>
            PresetPromptBuilder();

        /// <inheritdoc/>
        public override string PresetPromptShowcase =>
            PresetPromptBuilderShowcase();

        internal override string PresetPromptBuilder()
        {
            var PresetStringBuilder = new StringBuilder();
            string UserDollarSign = UserManagement.GetUserDollarSign(UserManagement.CurrentUser.Username);

            // Build the preset
            if (!KernelEntry.Maintenance)
            {
                // Opening
                PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.AppendFormat($"[{(Config.MainConfig.ShowShellCount ? $"{ShellManager.ShellStack.Count}:" : "")}");

                // Current username
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.UserNameShell).VTSequenceForeground);
                PresetStringBuilder.AppendFormat("{0}", UserManagement.CurrentUser.Username);

                // "At" sign
                PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append('@');

                // Current hostname
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.HostNameShell).VTSequenceForeground);
                PresetStringBuilder.AppendFormat("{0}", NetworkTools.HostName);

                // Current directory and shell stack
                PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.AppendFormat("]{0}:", CurrentDirectory.CurrentDir);

                // User dollar sign
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.UserDollar).VTSequenceForeground);
                PresetStringBuilder.AppendFormat(" {0} ", UserDollarSign);
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);
            }
            else
            {
                // Maintenance mode
                PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append(Translate.DoTranslation("Maintenance Mode") + "> ");
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        internal override string PresetPromptBuilderShowcase()
        {
            var PresetStringBuilder = new StringBuilder();

            // Build the preset
            if (!KernelEntry.Maintenance)
            {
                // Opening
                PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.AppendFormat("[1:");

                // Current username
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.UserNameShell).VTSequenceForeground);
                PresetStringBuilder.AppendFormat("user");

                // "At" sign
                PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append('@');

                // Current hostname
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.HostNameShell).VTSequenceForeground);
                PresetStringBuilder.AppendFormat("host");

                // Current directory and shell stack
                PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.AppendFormat("]/home/user:");

                // User dollar sign
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.UserDollar).VTSequenceForeground);
                PresetStringBuilder.AppendFormat(" $ ");
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);
            }
            else
            {
                // Maintenance mode
                PresetStringBuilder.Append(KernelColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append(Translate.DoTranslation("Maintenance Mode") + "> ");
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
