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

using System;
using System.Text;
using System.Collections.Generic;
using Terminaux.Colors;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Prompts;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Colors.Themes.Colors;

namespace Nitrocid.ShellPacks.Shells.HTTP.Presets
{
    /// <summary>
    /// PowerLine 1 preset
    /// </summary>
    public class HTTPPowerLine1Preset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "PowerLine1";

        /// <inheritdoc/>
        public override string PresetShellType { get; } = "HTTPShell";

        /// <inheritdoc/>
        public override string PresetPrompt =>
            PresetPromptBuilder();

        /// <inheritdoc/>
        public override string PresetPromptCompletion =>
            PresetPromptCompletionBuilder();

        /// <inheritdoc/>
        public override string PresetPromptShowcase =>
            PresetPromptBuilderShowcase();

        /// <inheritdoc/>
        public override string PresetPromptCompletionShowcase =>
            PresetPromptCompletionBuilder();

        private string PresetPromptBuilder()
        {
            // PowerLine glyphs
            char PadlockChar = Convert.ToChar(0xE0A2);

            // Segments
            List<PowerLineSegment> segments =
            [
                new PowerLineSegment(new Color(85, 255, 255), new Color(43, 127, 127), HTTPShellCommon.HTTPSite, PadlockChar)
            ];

            // Builder
            var PresetStringBuilder = new StringBuilder();

            PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
            PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground());

            // Present final string
            return PresetStringBuilder.ToString();
        }

        private string PresetPromptBuilderShowcase()
        {
            // PowerLine glyphs
            char PadlockChar = Convert.ToChar(0xE0A2);

            // Segments
            List<PowerLineSegment> segments =
            [
                new PowerLineSegment(new Color(85, 255, 255), new Color(43, 127, 127), LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_DEFAULTPRESET_SHOWCASE_SITE"), PadlockChar)
            ];

            // Builder
            var PresetStringBuilder = new StringBuilder();

            PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
            PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground());

            // Present final string
            return PresetStringBuilder.ToString();
        }

        private string PresetPromptCompletionBuilder()
        {
            // Segments
            List<PowerLineSegment> segments =
            [
                new PowerLineSegment(new Color(85, 255, 255), new Color(43, 127, 127), "+"),
            ];

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Use RenderSegments to render our segments
            PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
            PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground());

            // Present final string
            return PresetStringBuilder.ToString();
        }

    }
}
