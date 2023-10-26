//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel.Exceptions;
using KS.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Colors;

namespace KS.ConsoleBase.Themes
{
    /// <summary>
    /// Theme preview tools (simple preview and wheel-based preview)
    /// </summary>
    public static class ThemePreviewTools
    {
        /// <summary>
        /// Prepares the preview of the theme (simple)
        /// </summary>
        /// <param name="theme">Theme name</param>
        public static void PreviewThemeSimple(string theme) =>
            PreviewThemeSimple(ThemeTools.GetThemeInfo(theme));

        /// <summary>
        /// Prepares the preview of the theme (simple)
        /// </summary>
        /// <param name="theme">Theme instance</param>
        public static void PreviewThemeSimple(ThemeInfo theme) =>
            PreviewThemeSimple(ThemeTools.GetColorsFromTheme(theme));

        /// <summary>
        /// Prepares the preview of the theme (simple)
        /// </summary>
        /// <param name="colors">Dictionary of colors</param>
        internal static void PreviewThemeSimple(Dictionary<KernelColorType, Color> colors)
        {
            // Check to see if we're trying to preview theme on non-true color console
            if (ThemeTools.IsTrueColorRequired(colors) && !ConsoleExtensions.ConsoleSupportsTrueColor)
                throw new KernelException(KernelExceptionType.UnsupportedConsole, Translate.DoTranslation("Your console must support true color to use this theme."));

            // Write the prompt
            StringBuilder themeColorPromptText = new();
            ConsoleWrapper.Clear();
            themeColorPromptText.AppendLine(Translate.DoTranslation("Here's how your theme will look like:"));

            // Print every possibility of color types
            for (int key = 0; key < colors.Count; key++)
            {
                var type = colors.Keys.ElementAt(key);
                var color = colors.Values.ElementAt(key);
                themeColorPromptText.Append($"\n{KernelColorTools.GetColor(KernelColorType.Option).VTSequenceForeground}*) {type}: ");
                themeColorPromptText.Append($"[{color.PlainSequence}]{color.VTSequenceForeground} Lorem ipsum dolor sit amet, consectetur adipiscing elit.");
            }
            TextWriterWrappedColor.WriteWrappedKernelColor(themeColorPromptText.ToString(), false, KernelColorType.Option);
        }

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
        /// <param name="theme">Theme instance</param>
        internal static void PreviewTheme(Dictionary<KernelColorType, Color> colors, ThemeInfo theme)
        {
            // Check to see if we're trying to preview theme on non-true color console
            if (ThemeTools.IsTrueColorRequired(colors) && !ConsoleExtensions.ConsoleSupportsTrueColor)
                throw new KernelException(KernelExceptionType.UnsupportedConsole, Translate.DoTranslation("Your console must support true color to use this theme."));

            // Clear the screen
            ConsoleWrapper.CursorVisible = false;
            KernelColorTools.LoadBack();

            // Render the elements
            bool exiting = false;
            int currentTypeNum = (int)KernelColorType.NeutralText;
            while (!exiting)
            {
                // Get info
                var colorType = colors.Keys.ElementAt(currentTypeNum);
                var colorInstance = colors.Values.ElementAt(currentTypeNum);

                // Render the border
                int bindingsY = ConsoleWrapper.WindowHeight - 2;
                TextWriterWhereColor.WriteWhereColor(new string('═', ConsoleWrapper.WindowWidth), 0, bindingsY - 2, true, KernelColorTools.GetGray());

                // Render the bindings
                string bindings = $"[ENTER] {Translate.DoTranslation("Done")} - [<-|->] {Translate.DoTranslation("Switch Types")}";
                CenteredTextColor.WriteCentered(bindingsY, bindings);

                // Render the theme name
                int nameY = 1;
                CenteredTextColor.WriteCentered(nameY, $"{theme.Name} - {(theme.Localizable ? Translate.DoTranslation(theme.Description) : theme.Description)}");

                // Render the type name and some info
                int typeY = ConsoleWrapper.WindowHeight - 6;
                TextWriterWhereColor.WriteWhere(ConsoleExtensions.GetClearLineToRightSequence(), 0, typeY);
                CenteredTextColor.WriteCentered(typeY, $"{colorType} - {colorInstance.PlainSequence} [{colorInstance.PlainSequenceTrueColor}]");

                // Render the color box
                int startExteriorX = 4;
                int endExteriorX = ConsoleWrapper.WindowWidth - 4;
                int startExteriorY = 3;
                int endExteriorY = typeY - 1;
                int diffInteriorX = endExteriorX - startExteriorX - 2;
                int diffInteriorY = endExteriorY - startExteriorY - 2;
                BorderColor.WriteBorder(startExteriorX, startExteriorY, diffInteriorX, diffInteriorY);
                BoxColor.WriteBox(startExteriorX + 1, startExteriorY, diffInteriorX, diffInteriorY, colorInstance);

                // Wait for input
                var input = Input.DetectKeypress();
                switch (input.Key)
                {
                    case ConsoleKey.Enter:
                        exiting = true;
                        break;
                    case ConsoleKey.LeftArrow:
                        currentTypeNum--;
                        if (currentTypeNum < 0)
                            currentTypeNum = colors.Count - 1;
                        break;
                    case ConsoleKey.RightArrow:
                        currentTypeNum++;
                        if (currentTypeNum > colors.Count - 1)
                            currentTypeNum = 0;
                        break;
                }
            }

            // Clean up
            KernelColorTools.LoadBack();
        }
    }
}
