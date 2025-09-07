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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Languages;
using Terminaux.Shell.Prompts;
using Terminaux.Colors;

namespace Nitrocid.Extras.SftpShell.SFTP.Presets
{
    /// <summary>
    /// Default preset
    /// </summary>
    public class SFTPDefaultPreset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "Default";

        /// <inheritdoc/>
        public override string PresetPrompt =>
            PresetPromptBuilder();

        /// <inheritdoc/>
        public override string PresetPromptShowcase =>
            PresetPromptBuilderShowcase();

        /// <inheritdoc/>
        public override string PresetShellType { get; } = "SFTPShell";

        private string PresetPromptBuilder()
        {
            // Build the preset
            var PresetStringBuilder = new StringBuilder();

            // Opening
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append('[');

            // SFTP user
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.UserNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat("{0}", SFTPShellCommon.SFTPUser);

            // "at" sign
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append('@');

            // SFTP site
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.HostNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat("{0}", SFTPShellCommon.SFTPSite);

            // Closing
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.AppendFormat("]{0}> ", SFTPShellCommon.SFTPCurrentRemoteDir ?? "");
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        private string PresetPromptBuilderShowcase()
        {
            // Build the preset
            var PresetStringBuilder = new StringBuilder();

            // Opening
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append('[');

            // SFTP user
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.UserNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat(Translate.DoTranslation("User"));

            // "at" sign
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append('@');

            // SFTP site
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.HostNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat(Translate.DoTranslation("Site"));

            // Closing
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.AppendFormat("]/path> ");
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

    }
}
