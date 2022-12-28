
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
using KS.Files.Folders;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Text;
using KS.Network.Base;
using KS.Users.Login;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;
using KS.ConsoleBase.Colors;

namespace KS.Shell.Prompts.Presets.UESH
{
    /// <summary>
    /// PowerLine BG 3 preset
    /// </summary>
    public class PowerLineBG3Preset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "PowerLineBG3";

        /// <inheritdoc/>
        public override string PresetPrompt => PresetPromptBuilder();

        internal override string PresetPromptBuilder()
        {
            // PowerLine glyphs
            char TransitionChar = Convert.ToChar(0xE0B0);
            char TransitionPartChar = Convert.ToChar(0xE0B1);
            char PadlockChar = Convert.ToChar(0xE0A2);

            // PowerLine preset colors
            var FirstColorSegmentForeground = new Color(255, 255, 85);
            var FirstColorSegmentBackground = new Color(25, 25, 25);
            var SecondColorSegmentForeground = new Color(255, 255, 85);
            var SecondColorSegmentBackground = new Color(25, 25, 25);
            var ThirdColorSegmentForeground = new Color(255, 255, 85);
            var ThirdColorSegmentBackground = new Color(25, 25, 25);
            var LastTransitionForeground = new Color(25, 25, 25);

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Build the preset
            if (!Flags.Maintenance)
            {
                // Current username
                PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground);
                PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground);
                PresetStringBuilder.AppendFormat(" {0} ", Login.CurrentUser.Username);

                // Transition
                PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground);
                PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground);
                PresetStringBuilder.AppendFormat("{0}", TransitionPartChar);

                // Current hostname
                PresetStringBuilder.Append(SecondColorSegmentForeground.VTSequenceForeground);
                PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground);
                PresetStringBuilder.AppendFormat(" {0} {1} ", PadlockChar, NetworkTools.HostName);

                // Transition
                PresetStringBuilder.Append(SecondColorSegmentForeground.VTSequenceForeground);
                PresetStringBuilder.Append(ThirdColorSegmentBackground.VTSequenceBackground);
                PresetStringBuilder.AppendFormat("{0}", TransitionPartChar);

                // Current directory
                PresetStringBuilder.Append(ThirdColorSegmentForeground.VTSequenceForeground);
                PresetStringBuilder.Append(ThirdColorSegmentBackground.VTSequenceBackground);
                PresetStringBuilder.AppendFormat(" {0} ", CurrentDirectory.CurrentDir);

                // Transition
                PresetStringBuilder.Append(LastTransitionForeground.VTSequenceForeground);
                PresetStringBuilder.Append(Flags.SetBackground ? ColorTools.GetColor(KernelColorType.Background).VTSequenceBackground : Convert.ToString(CharManager.GetEsc()) + $"[49m");
                PresetStringBuilder.AppendFormat("{0} ", TransitionChar);
                PresetStringBuilder.Append(ColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);
            }
            else
            {
                // Maintenance mode
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append(Translate.DoTranslation("Maintenance Mode") + "> ");
                PresetStringBuilder.Append(ColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
