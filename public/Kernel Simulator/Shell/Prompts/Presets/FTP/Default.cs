
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Text;
using KS.ConsoleBase.Colors;
using KS.Shell.Shells.FTP;

namespace KS.Shell.Prompts.Presets.FTP
{
    /// <summary>
    /// Default preset
    /// </summary>
    public class FTPDefaultPreset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "Default";

        /// <inheritdoc/>
        public override string PresetPrompt => PresetPromptBuilder();

        /// <inheritdoc/>
        public override string PresetShellType { get; } = "FTPShell";

        internal override string PresetPromptBuilder()
        {
            // Build the preset
            var PresetStringBuilder = new StringBuilder();

            if (FTPShellCommon.FtpConnected)
            {
                // Opening
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append("[");

                // SFTP user
                PresetStringBuilder.Append(ColorTools.GetColor(KernelColorType.UserNameShell).VTSequenceForeground);
                PresetStringBuilder.AppendFormat("{0}", FTPShellCommon.FtpUser);

                // "at" sign
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append("@");

                // SFTP site
                PresetStringBuilder.Append(ColorTools.GetColor(KernelColorType.HostNameShell).VTSequenceForeground);
                PresetStringBuilder.AppendFormat("{0}", FTPShellCommon.FtpSite);

                // Closing
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.AppendFormat("]{0}> ", FTPShellCommon.FtpCurrentRemoteDir);
                PresetStringBuilder.Append(ColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);
            }
            else
            {
                // Current directory
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.AppendFormat("{0}> ", FTPShellCommon.FtpCurrentDirectory);
                PresetStringBuilder.Append(ColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
