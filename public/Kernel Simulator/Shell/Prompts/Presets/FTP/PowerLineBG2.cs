
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

using System;
using System.Text;
using ColorSeq;
using KS.Shell.Shells.FTP;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.Shell.Shells.SFTP;
using System.Collections.Generic;

namespace KS.Shell.Prompts.Presets.FTP
{
    /// <summary>
    /// PowerLine BG 2 preset
    /// </summary>
    public class FtpPowerLineBG2Preset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "PowerLineBG2";

        /// <inheritdoc/>
        public override string PresetPrompt => PresetPromptBuilder();

        /// <inheritdoc/>
        public override string PresetShellType { get; } = "FTPShell";

        internal override string PresetPromptBuilder()
        {
            // PowerLine glyphs
            char TransitionPartChar = Convert.ToChar(0xE0B1);
            char PadlockChar = Convert.ToChar(0xE0A2);

            // Segments
            List<PowerLineSegment> segments = new()
            {
                new PowerLineSegment(new Color(255, 85, 255), new Color(25, 25, 25), FTPShellCommon.FtpUser, default, TransitionPartChar),
                new PowerLineSegment(new Color(255, 85, 255), new Color(25, 25, 25), FTPShellCommon.FtpSite, PadlockChar, TransitionPartChar),
                new PowerLineSegment(new Color(255, 85, 255), new Color(25, 25, 25), FTPShellCommon.FtpCurrentRemoteDir, default, TransitionPartChar),
            };
            List<PowerLineSegment> segmentsDisconnected = new()
            {
                new PowerLineSegment(new Color(255, 85, 255), new Color(25, 25, 25), FTPShellCommon.FtpCurrentDirectory, default, TransitionPartChar)
            };

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Build the preset
            if (SFTPShellCommon.SFTPConnected)
                // Use RenderSegments to render our segments
                PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
            else
                // Use RenderSegments to render our segments
                PresetStringBuilder.Append(PowerLineTools.RenderSegments(segmentsDisconnected));

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
