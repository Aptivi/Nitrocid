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

using System;
using System.Text;
using System.Collections.Generic;
using Terminaux.Colors;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Users;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Languages;
using Nitrocid.Shell.Prompts;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Files;

namespace Nitrocid.Shell.Shells.UESH.Presets
{
    /// <summary>
    /// PowerLine 2 preset
    /// </summary>
    public class PowerLine2Preset : PromptPresetBase, IPromptPreset
    {

        /// <inheritdoc/>
        public override string PresetName { get; } = "PowerLine2";

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

        internal override string PresetPromptBuilder()
        {
            // PowerLine glyphs
            char PadlockChar = Convert.ToChar(0xE0A2);

            // PowerLine presets
            List<PowerLineSegment> segments =
            [
                new PowerLineSegment(new Color(255, 85, 255), new Color(127, 43, 127), UserManagement.CurrentUser.Username),
                new PowerLineSegment(new Color(0, 0, 0), new Color(255, 85, 255), Config.MainConfig.HostName, PadlockChar),
                new PowerLineSegment(new Color(0, 0, 0), new Color(255, 255, 255), $"{FilesystemTools.CurrentDir}{(Config.MainConfig.ShowShellCount ? $" [{ShellManager.ShellStack.Count}]" : "")}"),
            ];

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Build the preset
            if (!KernelEntry.Maintenance)
            {
                // Use RenderSegments to render our segments
                PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);
            }
            else
            {
                // Maintenance mode
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append(LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_MAINTENANCE") + "> ");
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        internal override string PresetPromptBuilderShowcase()
        {
            // PowerLine glyphs
            char PadlockChar = Convert.ToChar(0xE0A2);

            // PowerLine presets
            List<PowerLineSegment> segments =
            [
                new PowerLineSegment(new Color(255, 85, 255), new Color(127, 43, 127), "user"),
                new PowerLineSegment(new Color(0, 0, 0), new Color(255, 85, 255), "host", PadlockChar),
                new PowerLineSegment(new Color(0, 0, 0), new Color(255, 255, 255), $"/home/user{(Config.MainConfig.ShowShellCount ? $" [1]" : "")}"),
            ];

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Build the preset
            if (!KernelEntry.Maintenance)
            {
                // Use RenderSegments to render our segments
                PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);
            }
            else
            {
                // Maintenance mode
                PresetStringBuilder.Append(ColorTools.GetGray().VTSequenceForeground);
                PresetStringBuilder.Append(LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_MAINTENANCE") + "> ");
                PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);
            }

            // Present final string
            return PresetStringBuilder.ToString();
        }

        internal override string PresetPromptCompletionBuilder()
        {
            // Segments
            List<PowerLineSegment> segments =
            [
                new PowerLineSegment(new Color(255, 85, 255), new Color(127, 43, 127), "+"),
            ];

            // Builder
            var PresetStringBuilder = new StringBuilder();

            // Use RenderSegments to render our segments
            PresetStringBuilder.Append(PowerLineTools.RenderSegments(segments));
            PresetStringBuilder.Append(KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground);

            // Present final string
            return PresetStringBuilder.ToString();
        }

        string IPromptPreset.PresetPromptBuilder() =>
            PresetPromptBuilder();

        string IPromptPreset.PresetPromptCompletionBuilder() =>
            PresetPromptCompletionBuilder();

    }
}
