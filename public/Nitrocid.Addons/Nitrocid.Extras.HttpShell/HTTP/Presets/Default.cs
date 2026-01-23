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

using System.Text;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Languages;
using Terminaux.Shell.Prompts;
using Terminaux.Colors;
using Terminaux.Base.Extensions;

namespace Nitrocid.Extras.HttpShell.HTTP.Presets
{
    /// <summary>
    /// Default preset
    /// </summary>
    public class HTTPDefaultPreset : PromptPresetBase, IPromptPreset
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
        public override string PresetShellType { get; } = "HTTPShell";

        private string PresetPromptBuilder()
        {
            // Build the preset
            var PresetStringBuilder = new StringBuilder();

            // Opening
            PresetStringBuilder.Append(ConsoleColoring.GetGray().VTSequenceForeground());
            PresetStringBuilder.Append('[');

            // HTTP site
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.HostNameShell).VTSequenceForeground());
            PresetStringBuilder.AppendFormat("{0}", HTTPShellCommon.HTTPSite);

            // Closing
            PresetStringBuilder.Append(ConsoleColoring.GetGray().VTSequenceForeground());
            PresetStringBuilder.Append("] > ");
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground());

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

            // HTTP site
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.HostNameShell).VTSequenceForeground());
            PresetStringBuilder.AppendFormat(Translate.DoTranslation("Site"));

            // Closing
            PresetStringBuilder.Append(ConsoleColoring.GetGray().VTSequenceForeground());
            PresetStringBuilder.Append("] > ");
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground());

            // Present final string
            return PresetStringBuilder.ToString();
        }

    }
}
