
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
using KS.ConsoleBase.Colors;
using KS.Files.Folders;
using KS.Kernel;
using KS.Languages;
using KS.Network;
using KS.Users.Groups;

namespace KS.Shell.Prompts.Presets.UESH
{
    /// <summary>
    /// Default preset
    /// </summary>
    public class DefaultPreset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "Default";

        /// <inheritdoc/>
        public override string PresetPrompt
        {
            get
            {
                return PresetPromptBuilder();
            }
        }

        internal override string PresetPromptBuilder()
        {
            var PresetStringBuilder = new StringBuilder();
            char UserDollarSign = GroupManagement.HasGroup(Login.Login.CurrentUser.Username, GroupManagement.GroupType.Administrator) ? '#' : '$';

            // Build the preset
            if (!Flags.Maintenance)
            {
                // Opening
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append("[");

                // Current username
                PresetStringBuilder.Append(ColorTools.GetColor(ColorTools.ColTypes.UserName).VTSequenceForeground);
                PresetStringBuilder.AppendFormat("{0}", Login.Login.CurrentUser.Username);

                // "At" sign
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append("@");

                // Current hostname
                PresetStringBuilder.Append(ColorTools.GetColor(ColorTools.ColTypes.HostName).VTSequenceForeground);
                PresetStringBuilder.AppendFormat("{0}", NetworkTools.HostName);

                // Current directory
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.AppendFormat("]{0}", CurrentDirectory.CurrentDir);

                // User dollar sign
                PresetStringBuilder.Append(ColorTools.GetColor(ColorTools.ColTypes.UserDollar).VTSequenceForeground);
                PresetStringBuilder.AppendFormat(" {0} ", UserDollarSign);
                PresetStringBuilder.Append(ColorTools.GetColor(ColorTools.ColTypes.Input).VTSequenceForeground);
            }
            else
            {
                // Maintenance mode
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append(Translate.DoTranslation("Maintenance Mode") + "> ");
                PresetStringBuilder.Append(ColorTools.GetColor(ColorTools.ColTypes.Input).VTSequenceForeground);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
