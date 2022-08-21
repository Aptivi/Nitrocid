using System;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Misc.Text;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.SFTP;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Shell.Prompts.Presets.SFTP
{
    public class SftpPowerLine3Preset : PromptPresetBase, IPromptPreset
    {

        public override string PresetName { get; } = "PowerLine3";

        public override string PresetPrompt
        {
            get
            {
                return PresetPromptBuilder();
            }
        }

        public override ShellType PresetShellType { get; } = ShellType.SFTPShell;

        internal override string PresetPromptBuilder()
        {
            // PowerLine glyphs
            char TransitionChar = Convert.ToChar(0xE0B0);
            char PadlockChar = Convert.ToChar(0xE0A2);

            // PowerLine preset colors
            var FirstColorSegmentForeground = new Color(255, 255, 85);
            var FirstColorSegmentBackground = new Color(127, 127, 43);
            var SecondColorSegmentForeground = new Color(0, 0, 0);
            var SecondColorSegmentBackground = new Color(255, 255, 85);
            var ThirdColorSegmentForeground = new Color(0, 0, 0);
            var ThirdColorSegmentBackground = new Color(255, 255, 255);
            var LastTransitionForeground = new Color(255, 255, 255);

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Build the preset
            if (SFTPShellCommon.SFTPConnected)
            {
                // Current username
                PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground);
                PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground);
                PresetStringBuilder.AppendFormat(" {0} ", SFTPShellCommon.SFTPUser);

                // Transition
                PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceForeground);
                PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground);
                PresetStringBuilder.AppendFormat("{0}", TransitionChar);

                // Current hostname
                PresetStringBuilder.Append(SecondColorSegmentForeground.VTSequenceForeground);
                PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground);
                PresetStringBuilder.AppendFormat(" {0} {1} ", PadlockChar, SFTPShellCommon.SFTPSite);

                // Transition
                PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceForeground);
                PresetStringBuilder.Append(ThirdColorSegmentBackground.VTSequenceBackground);
                PresetStringBuilder.AppendFormat("{0}", TransitionChar);

                // Current directory
                PresetStringBuilder.Append(ThirdColorSegmentForeground.VTSequenceForeground);
                PresetStringBuilder.Append(ThirdColorSegmentBackground.VTSequenceBackground);
                PresetStringBuilder.AppendFormat(" {0} ", SFTPShellCommon.SFTPCurrentRemoteDir);

                // Transition
                PresetStringBuilder.Append(LastTransitionForeground.VTSequenceForeground);
                PresetStringBuilder.Append(Flags.SetBackground ? ColorTools.BackgroundColor.VTSequenceBackground : Conversions.ToString(CharManager.GetEsc()) + $"[49m");
                PresetStringBuilder.AppendFormat("{0} ", TransitionChar);
            }
            else
            {
                // SFTP current directory
                PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground);
                PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground);
                PresetStringBuilder.AppendFormat(" {0} ", SFTPShellCommon.SFTPCurrDirect);

                // Transition
                PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceForeground);
                PresetStringBuilder.Append(Flags.SetBackground ? ColorTools.BackgroundColor.VTSequenceBackground : Conversions.ToString(CharManager.GetEsc()) + $"[49m");
                PresetStringBuilder.AppendFormat("{0} ", TransitionChar);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}