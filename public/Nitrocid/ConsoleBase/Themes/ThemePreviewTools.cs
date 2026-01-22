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

using System.Collections.Generic;
using System.Linq;
using Terminaux.Colors;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Inputs.Styles.Selection;
using Nitrocid.Kernel.Configuration;
using Terminaux.Inputs.Styles;

namespace Nitrocid.ConsoleBase.Themes
{
    /// <summary>
    /// Theme preview tools (simple preview and wheel-based preview)
    /// </summary>
    public static class ThemePreviewTools
    {
        /// <summary>
        /// Prepares the preview of the theme (wheel-based)
        /// </summary>
        /// <param name="theme">Theme name</param>
        public static void PreviewTheme(string theme) =>
            PreviewTheme(ThemeTools.GetThemeInfo(theme));

        /// <summary>
        /// Prepares the preview of the theme (wheel-based)
        /// </summary>
        /// <param name="theme">Theme instance</param>
        public static void PreviewTheme(ThemeInfo theme) =>
            PreviewTheme(ThemeTools.GetColorsFromTheme(theme), theme);

        /// <summary>
        /// Prepares the preview of the theme (wheel-based)
        /// </summary>
        /// <param name="colors">Dictionary of colors</param>
        public static void PreviewTheme(Dictionary<KernelColorType, Color> colors) =>
            PreviewTheme(colors, null);

        /// <summary>
        /// Prepares the preview of the theme (wheel-based)
        /// </summary>
        /// <param name="colors">Dictionary of colors</param>
        /// <param name="theme">Theme instance</param>
        internal static void PreviewTheme(Dictionary<KernelColorType, Color> colors, ThemeInfo? theme)
        {
            // Check to see if we're trying to preview theme on non-true color console
            if (ThemeTools.MinimumTypeRequired(colors, ColorType.TrueColor) && !Config.MainConfig.ConsoleSupportsTrueColor)
                throw new KernelException(KernelExceptionType.UnsupportedConsole, Translate.DoTranslation("Your console must support true color to use this theme."));

            // Render the choices
            var choices = new List<InputChoiceInfo>();
            for (int key = 0; key < colors.Count; key++)
            {
                var type = colors.Keys.ElementAt(key);
                var color = colors.Values.ElementAt(key);
                choices.Add(
                    new(type.ToString(), $"[{color.PlainSequence}]{color.VTSequenceForeground()} Lorem ipsum dolor sit amet, consectetur adipiscing elit.")
                );
            }

            // Alt choices for exiting
            var altChoices = new List<InputChoiceInfo>
            {
                new(Translate.DoTranslation("Exit"), Translate.DoTranslation("Exit the preview"))
            };

            // Give a prompt for theme preview
            while (true)
            {
                int prev = SelectionStyle.PromptSelection((theme is not null ? $"{theme.Name}: {theme.Description}\n\n" : "") + Translate.DoTranslation("Here's how your theme will look like:"), [.. choices], [.. altChoices], true);
                if (prev == choices.Count)
                    break;
                else
                    ColorSelector.OpenColorSelector(colors.Values.ElementAt(prev), readOnly: true);
            }
        }
    }
}
