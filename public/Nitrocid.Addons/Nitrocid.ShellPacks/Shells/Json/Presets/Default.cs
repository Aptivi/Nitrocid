//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using System.IO;
using System.Text;
using Terminaux.Themes.Colors;
using Nitrocid.ShellPacks.Tools;
using Terminaux.Shell.Prompts;
using Terminaux.Colors;
using Terminaux.Base.Extensions;

namespace Nitrocid.ShellPacks.Shells.Json.Presets
{
    /// <summary>
    /// Default preset
    /// </summary>
    public class JsonDefaultPreset : PromptPresetBase, IPromptPreset
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
        public override string PresetShellType { get; } = "JsonShell";

        private string PresetPromptBuilder()
        {
            // Build the preset
            var PresetStringBuilder = new StringBuilder();

            // Opening
            PresetStringBuilder.Append(ConsoleColoring.GetGray().VTSequenceForeground());
            PresetStringBuilder.Append('[');

            // File name
            PresetStringBuilder.Append(ThemeColorsTools.GetColor("UserNameShellColor").VTSequenceForeground());
            PresetStringBuilder.AppendFormat(JsonShellCommon.FileStream is not null ? Path.GetFileName(JsonShellCommon.FileStream.Name) : "???");

            // Was file edited?
            PresetStringBuilder.Append(ThemeColorsTools.GetColor("UserNameShellColor").VTSequenceForeground());
            PresetStringBuilder.AppendFormat("{0}", JsonTools.WasJsonEdited() ? "*" : "");

            // Closing
            PresetStringBuilder.Append(ConsoleColoring.GetGray().VTSequenceForeground());
            PresetStringBuilder.Append("] > ");
            PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground());

            // Present final string
            return PresetStringBuilder.ToString();
        }

        private string PresetPromptBuilderShowcase()
        {
            // Build the preset
            var PresetStringBuilder = new StringBuilder();

            // Opening
            PresetStringBuilder.Append(ConsoleColoring.GetGray().VTSequenceForeground());
            PresetStringBuilder.Append('[');

            // File name
            PresetStringBuilder.Append(ThemeColorsTools.GetColor("UserNameShellColor").VTSequenceForeground());
            PresetStringBuilder.AppendFormat("file.json");

            // Was file edited?
            PresetStringBuilder.Append(ThemeColorsTools.GetColor("UserNameShellColor").VTSequenceForeground());
            PresetStringBuilder.AppendFormat("*");

            // Closing
            PresetStringBuilder.Append(ConsoleColoring.GetGray().VTSequenceForeground());
            PresetStringBuilder.Append("] > ");
            PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground());

            // Present final string
            return PresetStringBuilder.ToString();
        }

    }
}
