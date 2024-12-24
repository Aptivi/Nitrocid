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
using System.Collections.Generic;
using System.Linq;
using Terminaux.Colors;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Base;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Base.Extensions;
using Nitrocid.Kernel.Configuration;
using Terminaux.Inputs.Styles;
using Terminaux.Writer.CyclicWriters;

namespace Nitrocid.ConsoleBase.Themes
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
            if (ThemeTools.MinimumTypeRequired(colors, ColorType.TrueColor) && !Config.MainConfig.ConsoleSupportsTrueColor)
                throw new KernelException(KernelExceptionType.UnsupportedConsole, Translate.DoTranslation("Your console must support true color to use this theme."));

            // Render the choices
            var choices = new List<InputChoiceInfo>();
            for (int key = 0; key < colors.Count; key++)
            {
                var type = colors.Keys.ElementAt(key);
                var color = colors.Values.ElementAt(key);
                choices.Add(
                    new(type.ToString(), $"[{color.PlainSequence}]{color.VTSequenceForeground} Lorem ipsum dolor sit amet, consectetur adipiscing elit.")
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
                int prev = SelectionStyle.PromptSelection(Translate.DoTranslation("Here's how your theme will look like:"), [.. choices], [.. altChoices], true);
                if (prev == choices.Count + 1)
                    break;
            }
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
            if (ThemeTools.MinimumTypeRequired(colors, ColorType.TrueColor) && !Config.MainConfig.ConsoleSupportsTrueColor)
                throw new KernelException(KernelExceptionType.UnsupportedConsole, Translate.DoTranslation("Your console must support true color to use this theme."));

            // Clear the screen
            ConsoleWrapper.CursorVisible = false;
            ColorTools.LoadBack();

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
                TextWriterWhereColor.WriteWhereColor(new string('═', ConsoleWrapper.WindowWidth), 0, bindingsY - 2, true, ColorTools.GetGray());

                // Render the bindings
                string bindings = $"[ENTER] {Translate.DoTranslation("Done")} - [<-|->] {Translate.DoTranslation("Switch Types")}";
                var bindingsText = new AlignedText()
                {
                    Top = bindingsY,
                    Text = bindings,
                };
                TextWriterRaw.WriteRaw(bindingsText.Render());

                // Render the theme name
                int nameY = 1;
                string name = $"{theme.Name} - {(theme.Localizable ? Translate.DoTranslation(theme.Description) : theme.Description)}";
                var themeText = new AlignedText()
                {
                    Top = nameY,
                    Text = name,
                };
                TextWriterRaw.WriteRaw(themeText.Render());

                // Render the type name and some info
                int typeY = ConsoleWrapper.WindowHeight - 6;
                string type = $"{colorType} - {colorInstance.PlainSequence} [{colorInstance.PlainSequenceTrueColor}]";
                TextWriterWhereColor.WriteWhere(ConsoleClearing.GetClearLineToRightSequence(), 0, typeY);
                var typeText = new AlignedText()
                {
                    Top = typeY,
                    Text = type,
                };
                TextWriterRaw.WriteRaw(typeText.Render());

                // Render the color box
                int startExteriorX = 4;
                int endExteriorX = ConsoleWrapper.WindowWidth - 4;
                int startExteriorY = 3;
                int endExteriorY = typeY - 1;
                int diffInteriorX = endExteriorX - startExteriorX - 2;
                int diffInteriorY = endExteriorY - startExteriorY - 2;
                var colorFrame = new Border()
                {
                    Left = startExteriorX,
                    Top = startExteriorY,
                    InteriorWidth = diffInteriorX,
                    InteriorHeight = diffInteriorY,
                };
                var colorBox = new Box()
                {
                    Left = startExteriorX + 1,
                    Top = startExteriorY,
                    InteriorWidth = diffInteriorX,
                    InteriorHeight = diffInteriorY,
                    Color = colorInstance,
                };
                TextWriterRaw.WriteRaw(
                    colorFrame.Render() +
                    colorBox.Render()
                );

                // Wait for input
                var input = Input.ReadKey();
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
            ColorTools.LoadBack();
        }
    }
}
