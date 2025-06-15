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
using Nitrocid.Shell.Prompts;
using Terminaux.Colors;

namespace Nitrocid.ShellPacks.Shells.FTP.Presets
{
    /// <summary>
    /// Default preset
    /// </summary>
    public class FTPDefaultPreset : PromptPresetBase, IPromptPreset
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
        public override string PresetShellType { get; } = "FTPShell";

        internal override string PresetPromptBuilder()
        {
            // Build the preset
            var PresetStringBuilder = new StringBuilder();

            // Opening
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append('[');

            // SFTP user
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.UserNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat("{0}", FTPShellCommon.FtpUser);

            // "at" sign
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append('@');

            // SFTP site
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.HostNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat("{0}", FTPShellCommon.FtpSite);

            // Closing
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.AppendFormat("]{0}> ", FTPShellCommon.FtpCurrentRemoteDir);
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        internal override string PresetPromptBuilderShowcase()
        {
            // Build the preset
            var PresetStringBuilder = new StringBuilder();

            // Opening
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append('[');

            // SFTP user
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.UserNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_DEFAULTPRESET_SHOWCASE_USER"));

            // "at" sign
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.Append('@');

            // SFTP site
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.HostNameShell).VTSequenceForeground);
            PresetStringBuilder.AppendFormat(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_DEFAULTPRESET_SHOWCASE_SITE"));

            // Closing
            PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
            PresetStringBuilder.AppendFormat("]/dir> ");
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
