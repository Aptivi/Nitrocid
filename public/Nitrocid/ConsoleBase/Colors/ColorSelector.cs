
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using Terminaux.Colors;
using Terminaux.Colors.Wheel;

namespace KS.ConsoleBase.Colors
{
    /// <summary>
    /// Color selection application
    /// </summary>
    public static class ColorSelector
    {
        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector() =>
            OpenColorSelector(KernelColorTools.CurrentForegroundColor);

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <param name="initialColor">Initial color to use</param>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector(KernelColorType initialColor) =>
            OpenColorSelector(KernelColorTools.GetColor(initialColor));

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <param name="initialColor">Initial color to use</param>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector(ConsoleColors initialColor) =>
            OpenColorSelector(new Color(initialColor));

        /// <summary>
        /// Opens the color selector
        /// </summary>
        /// <param name="initialColor">Initial color to use</param>
        /// <returns>An instance of Color to get the resulting color</returns>
        public static Color OpenColorSelector(Color initialColor)
        {
            if (!KernelFlags.UseNewColorSelector)
                return ColorWheel.InputForColor(initialColor);

            // Clear screen
            KernelColorTools.LoadBack();

            // Initial color is selected
            Color selectedColor = initialColor;

            // TODO: This is a placeholder code, so things are unfinished here
            // Now, the selector main loop
            bool bail = false;
            while (!bail)
            {
                switch (initialColor.Type)
                {
                    case ColorType.TrueColor:
                        RenderTrueColorSelector(selectedColor);
                        bail = HandleKeypressTrueColor(ref selectedColor);
                        break;
                    case ColorType._255Color:
                        Render255ColorsSelector(selectedColor);
                        bail = HandleKeypress255Colors(ref selectedColor);
                        break;
                    case ColorType._16Color:
                        Render16ColorsSelector(selectedColor);
                        bail = HandleKeypress16Colors(ref selectedColor);
                        break;
                    default:
                        DebugCheck.AssertFail("invalid color type in the color selector");
                        break;
                }
            }

            // Return the selected color
            return selectedColor;
        }

        private static void RenderTrueColorSelector(Color selectedColor)
        {
            RenderPreviewBox(selectedColor);
        }

        private static void Render255ColorsSelector(Color selectedColor)
        {
            RenderPreviewBox(selectedColor);
        }

        private static void Render16ColorsSelector(Color selectedColor)
        {
            RenderPreviewBox(selectedColor);
        }

        private static bool HandleKeypressTrueColor(ref Color selectedColor)
        {
            bool bail = false;
            var color = selectedColor;

            return bail;
        }

        private static bool HandleKeypress255Colors(ref Color selectedColor)
        {
            bool bail = false;
            var color = selectedColor;

            return bail;
        }

        private static bool HandleKeypress16Colors(ref Color selectedColor)
        {
            bool bail = false;
            var color = selectedColor;

            return bail;
        }

        private static void RenderPreviewBox(Color selectedColor)
        {
            // Draw the box that represents the currently selected color
            int boxX = 2;
            int boxY = 1;
            int boxWidth = (ConsoleWrapper.WindowWidth / 2) - 4;
            int boxHeight = ConsoleWrapper.WindowHeight - 4;

            // First, draw the border
            BoxFrameTextColor.WriteBoxFrame($"{selectedColor.PlainSequence} [{selectedColor.PlainSequenceTrueColor}]", boxX, boxY, boxWidth, boxHeight);

            // then, the box
            BoxColor.WriteBox(boxX + 1, boxY, boxWidth, boxHeight, selectedColor);
        }
    }
}
