
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
using System.IO;
using System.Text;
using ColorSeq;
using KS.Kernel;
using KS.Misc.Text;
using KS.Shell.Shells.Archive;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;
using KS.ConsoleBase.Colors;

namespace KS.Shell.Prompts.Presets.Archive
{
    /// <summary>
    /// PowerLine 3 preset
    /// </summary>
    public class ArchivePowerLine3Preset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "PowerLine3";

        /// <inheritdoc/>
        public override string PresetPrompt => PresetPromptBuilder();

        /// <inheritdoc/>
        public override string PresetShellType { get; } = "ArchiveShell";

        internal override string PresetPromptBuilder()
        {
            // PowerLine glyphs
            char TransitionChar = Convert.ToChar(0xE0B0);

            // PowerLine preset colors
            var FirstColorSegmentForeground = new Color(255, 255, 85);
            var FirstColorSegmentBackground = new Color(127, 127, 43);
            var SecondColorSegmentForeground = new Color(0, 0, 0);
            var SecondColorSegmentBackground = new Color(255, 255, 85);
            var LastTransitionForeground = new Color(255, 255, 255);

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // File name
            PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground);
            PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground);
            PresetStringBuilder.AppendFormat(" {0} ", Path.GetFileName(ArchiveShellCommon.ArchiveShell_FileStream.Name));

            // Transition
            PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceForeground);
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground);
            PresetStringBuilder.AppendFormat("{0}", TransitionChar);

            // Current archive directory
            PresetStringBuilder.Append(SecondColorSegmentForeground.VTSequenceForeground);
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground);
            PresetStringBuilder.AppendFormat(" {0} ", ArchiveShellCommon.ArchiveShell_CurrentArchiveDirectory);

            // Transition
            PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceForeground);
            PresetStringBuilder.Append(Flags.SetBackground ? ColorTools.GetColor(KernelColorType.Background).VTSequenceBackground : Convert.ToString(CharManager.GetEsc()) + $"[49m");
            PresetStringBuilder.AppendFormat("{0} ", TransitionChar);
            PresetStringBuilder.Append(ColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
