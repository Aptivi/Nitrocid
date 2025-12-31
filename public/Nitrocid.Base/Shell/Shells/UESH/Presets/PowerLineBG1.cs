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
using System.Collections.Generic;
using System.Text;
using Terminaux.Colors;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Prompts;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;

namespace Nitrocid.Base.Shell.Shells.UESH.Presets
{
    /// <summary>
    /// PowerLine BG 1 preset
    /// </summary>
    public class PowerLineBG1Preset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "PowerLineBG1";

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
            char TransitionPartChar = Convert.ToChar(0xE0B1);
            char PadlockChar = Convert.ToChar(0xE0A2);

            // PowerLine presets
            List<PowerLineSegment> segments =
            [
                new PowerLineSegment(new Color(85, 255, 255), new Color(25, 25, 25), UserManagement.CurrentUser.Username, default, TransitionPartChar),
                new PowerLineSegment(new Color(85, 255, 255), new Color(25, 25, 25), Config.MainConfig.HostName, PadlockChar, TransitionPartChar),
                new PowerLineSegment(new Color(85, 255, 255), new Color(25, 25, 25), $"{FilesystemTools.CurrentDir}{(Config.MainConfig.ShowShellCount ? $" [{ShellManager.ShellCount}]" : "")}", default, TransitionPartChar),
            ];

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Build the preset
            if (!KernelEntry.Maintenance)
            {
                // Use RenderSegments to render our segments
                PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
                PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground);
            }
            else
            {
                // Maintenance mode
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append(LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_MAINTENANCE") + "> ");
                PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        private string PresetPromptBuilderShowcase()
        {
            // PowerLine glyphs
            char TransitionPartChar = Convert.ToChar(0xE0B1);
            char PadlockChar = Convert.ToChar(0xE0A2);

            // PowerLine presets
            List<PowerLineSegment> segments =
            [
                new PowerLineSegment(new Color(85, 255, 255), new Color(25, 25, 25), "user", default, TransitionPartChar),
                new PowerLineSegment(new Color(85, 255, 255), new Color(25, 25, 25), "host", PadlockChar, TransitionPartChar),
                new PowerLineSegment(new Color(85, 255, 255), new Color(25, 25, 25), $"/home/user{(Config.MainConfig.ShowShellCount ? $" [1]" : "")}", default, TransitionPartChar),
            ];

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Build the preset
            if (!KernelEntry.Maintenance)
            {
                // Use RenderSegments to render our segments
                PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
                PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground);
            }
            else
            {
                // Maintenance mode
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append(LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_MAINTENANCE") + "> ");
                PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        private string PresetPromptCompletionBuilder()
        {
            // Segments
            List<PowerLineSegment> segments =
            [
                new PowerLineSegment(new Color(85, 255, 255), new Color(25, 25, 25), "+"),
            ];

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Use RenderSegments to render our segments
            PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
            PresetStringBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

    }
}
