
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Languages;
using KS.Misc.Text;
using System;

namespace KS.ConsoleBase.Presentation
{
    /// <summary>
    /// Presentation tools
    /// </summary>
    public static class PresentationTools
    {
        /// <summary>
        /// The upper left corner of the exterior border (the left position)
        /// </summary>
        public static int PresentationUpperBorderLeft =>
            2;
        /// <summary>
        /// The upper left corner of the exterior border (the top position)
        /// </summary>
        public static int PresentationUpperBorderTop =>
            3;
        /// <summary>
        /// The upper left corner of the inner border (the left position)
        /// </summary>
        public static int PresentationUpperInnerBorderLeft =>
            PresentationUpperBorderLeft + 1;
        /// <summary>
        /// The upper left corner of the inner border (the top position)
        /// </summary>
        public static int PresentationUpperInnerBorderTop =>
            PresentationUpperBorderTop + 1;
        /// <summary>
        /// The lower right corner of the inner border (the left position)
        /// </summary>
        public static int PresentationLowerInnerBorderLeft =>
            ConsoleWrapper.WindowWidth - PresentationUpperInnerBorderLeft * 2;
        /// <summary>
        /// The lower right corner of the inner border (the top position)
        /// </summary>
        public static int PresentationLowerInnerBorderTop =>
            ConsoleWrapper.WindowHeight - PresentationUpperBorderTop * 2 - 2;
        /// <summary>
        /// The title top position
        /// </summary>
        public static int PresentationTitleTop =>
            1;
        /// <summary>
        /// The informational top position
        /// </summary>
        public static int PresentationInformationalTop =>
            ConsoleWrapper.WindowHeight - 2;

        /// <summary>
        /// Present the presentation
        /// </summary>
        /// <param name="presentation">Presentation instance</param>
        public static void Present(Presentation presentation) =>
            Present(presentation, false, false);

        /// <summary>
        /// Present the presentation
        /// </summary>
        /// <param name="presentation">Presentation instance</param>
        /// <param name="kiosk">Prevent any key other than ENTER from being pressed</param>
        /// <param name="required">Prevents exiting the presentation</param>
        public static void Present(Presentation presentation, bool kiosk, bool required)
        {
            // Clear the console
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;

            // Make a border
            BorderColor.WriteBorder(PresentationUpperBorderLeft, PresentationUpperBorderTop, PresentationLowerInnerBorderLeft, PresentationLowerInnerBorderTop, KernelColorType.Separator);

            // Loop for each page
            var pages = presentation.Pages;
            bool presentExit = false;
            for (int i = 0; i < pages.Count; i++)
            {
                // Check to see if we're exiting
                if (presentExit)
                    break;

                // Get the page
                var page = pages[i];

                // Write the name and the page number
                TextWriterWhereColor.WriteWhere(ConsoleExtensions.GetClearLineToRightSequence(), 0, PresentationTitleTop);
                CenteredTextColor.WriteCenteredKernelColor(PresentationTitleTop, $"{(!kiosk ? $"[{i + 1}/{pages.Count}] - " : "")}{page.Name} - {presentation.Name}".Truncate(PresentationLowerInnerBorderLeft + 1) + ConsoleExtensions.GetClearLineToRightSequence(), KernelColorType.NeutralText);

                // Write the bindings
                CenteredTextColor.WriteCenteredKernelColor(PresentationInformationalTop, $"[ENTER] {Translate.DoTranslation("Advance")}{(!kiosk && !required ? $" - [ESC] {Translate.DoTranslation("Exit")}" : "")}".Truncate(PresentationLowerInnerBorderLeft + 1), KernelColorType.NeutralText);

                // Clear the presentation screen
                ClearPresentation();

                // Render all elements
                var pageElements = page.Elements;
                bool checkOutOfBounds = false;
                foreach (var element in pageElements)
                {
                    // Check for possible out-of-bounds
                    if (element.IsPossibleOutOfBounds() && checkOutOfBounds)
                    {
                        Input.DetectKeypress();
                        ClearPresentation();
                    }
                    checkOutOfBounds = true;

                    // Render it to the view
                    element.Render();

                    // Check to see if we need to invoke action
                    if (element.IsInput)
                        if (element.InvokeActionInput is not null)
                            element.InvokeActionInput(new object[] { element.WrittenInput });
                        else
                        if (element.InvokeAction is not null)
                            element.InvokeAction();
                }

                // Wait for the ENTER key to be pressed if in kiosk mode. If not in kiosk mode, handle any key
                bool pageExit = false;
                while (!pageExit)
                {
                    // Get the keypress
                    var key = Input.DetectKeypress();

                    // Now, check for the key
                    switch (key.Key)
                    {
                        case ConsoleKey.Escape:
                            if (required)
                                break;
                            if (kiosk)
                                break;
                            presentExit = true;
                            pageExit = true;
                            break;
                        case ConsoleKey.Enter:
                            pageExit = true;
                            break;
                    }
                }
            }

            // Clean up after ourselves
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = true;
        }

        /// <summary>
        /// Checks to see if the presentation contains input
        /// </summary>
        /// <param name="presentation">Target presentation</param>
        /// <returns>True if one of the elements in a page contains input</returns>
        public static bool PresentationContainsInput(Presentation presentation)
        {
            // Check every page
            foreach (var page in presentation.Pages)
                foreach (var element in page.Elements)
                    if (element.IsInput)
                        // One of the elements contains input
                        return true;

            // If not input, then false
            return false;
        }

        /// <summary>
        /// Clears the presentation
        /// </summary>
        public static void ClearPresentation()
        {
            // Clear the presentation screen
            for (int y = PresentationUpperInnerBorderTop; y <= PresentationLowerInnerBorderTop + 3; y++)
                TextWriterWhereColor.WriteWhere(new string(' ', PresentationLowerInnerBorderLeft), PresentationUpperInnerBorderLeft, y);

            // Seek to the first position inside the border
            ConsoleWrapper.SetCursorPosition(PresentationUpperInnerBorderLeft, PresentationUpperInnerBorderTop);
        }
    }
}
