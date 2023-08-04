
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

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KS.Misc.Interactive
{
    /// <summary>
    /// Tools for the interactive TUI implementation
    /// </summary>
    public static class InteractiveTuiTools
    {

        private static string _finalInfoRendered = "";
        private static readonly object _interactiveTuiLock = new();

        /// <summary>
        /// Opens the interactive TUI
        /// </summary>
        /// <param name="interactiveTui">The inherited class instance of the interactive TUI</param>
        /// <exception cref="KernelException"></exception>
        public static void OpenInteractiveTui(BaseInteractiveTui interactiveTui)
        {
            lock (_interactiveTuiLock)
            {
                if (interactiveTui is null)
                    throw new KernelException(KernelExceptionType.InteractiveTui, Translate.DoTranslation("Please provide a base Interactive TUI class and try again."));

                bool notifyCrash = false;
                string crashReason = "";
                try
                {
                    // Loop until the user requests to exit
                    while (!interactiveTui.isExiting)
                    {
                        // Draw the boxes
                        DrawInteractiveTui(interactiveTui);
                        DebugWriter.WriteDebug(DebugLevel.I, "Interactive TUI drawn.");

                        // Draw the first pane
                        DrawInteractiveTuiItems(interactiveTui, 1);
                        DebugWriter.WriteDebug(DebugLevel.I, "Interactive TUI items (first pane) drawn.");

                        // Draw the second pane
                        if (interactiveTui.SecondPaneInteractable)
                        {
                            DrawInteractiveTuiItems(interactiveTui, 2);
                            DebugWriter.WriteDebug(DebugLevel.I, "Interactive TUI items (second pane) drawn.");
                        }
                        else
                        {
                            DrawInformationOnSecondPane(interactiveTui);
                            DebugWriter.WriteDebug(DebugLevel.I, "Info drawn.");
                        }

                        DrawStatus(interactiveTui);
                        DebugWriter.WriteDebug(DebugLevel.I, "Status drawn.");

                        // Wait for user input
                        RespondToUserInput(interactiveTui);
                        DebugWriter.WriteDebug(DebugLevel.I, "Responded to user input.");
                    }
                }
                catch (Exception ex)
                {
                    notifyCrash = true;
                    crashReason = TextTools.FormatString(Translate.DoTranslation("The interactive TUI, {0}, has crashed for the following reason:"), interactiveTui.GetType().Name) + $" {ex.Message}";
                    DebugWriter.WriteDebug(DebugLevel.E, "Interactive TUI {0} crashed! {1}", interactiveTui.GetType().Name, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }

                // Clear the console to clean up
                KernelColorTools.LoadBack();
                
                // If there is a crash, notify the user about it
                if (notifyCrash)
                {
                    notifyCrash = false;
                    TextWriterColor.Write(crashReason, true, KernelColorType.Error);
                    TextWriterColor.Write();
                    TextWriterColor.Write(Translate.DoTranslation("Press any key to exit this program..."));
                    Input.DetectKeypress();
                }

                // Reset some static variables
                BaseInteractiveTui.RedrawRequired = true;
                BaseInteractiveTui.CurrentPane = 1;
                BaseInteractiveTui.FirstPaneCurrentSelection = 1;
                BaseInteractiveTui.SecondPaneCurrentSelection = 1;
                BaseInteractiveTui.Status = "";
            }
        }

        internal static int CountElements(IEnumerable enumerable)
        {
            int dataCount = 0;
            foreach (var item in enumerable)
                // IEnumerable from System.Collections doesn't implement Count() or Length, hence the array, List<>, Dictionary<>,
                // and other collections have either Count or Length. This is an ugly hack that we should live with.
                dataCount++;
            DebugWriter.WriteDebug(DebugLevel.I, "{0} elements.", dataCount);
            return dataCount;
        }

        internal static object GetElementFromIndex(IEnumerable enumerable, int index)
        {
            // Here, it's getting uglier as we don't have ElementAt() in IEnumerable, too!
            object dataObject = null;
            int steppedItems = 0;
            foreach (var item in enumerable)
            {
                steppedItems++;
                if (steppedItems == index + 1)
                {
                    // We found the item that we need! Assign it to dataObject so GetEntryFromItem() can formulate a string.
                    DebugWriter.WriteDebug(DebugLevel.I, "Found required item index {0}.", index);
                    dataObject = item;
                    break;
                }
            }
            return dataObject;
        }

        private static void DrawInteractiveTui(BaseInteractiveTui interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            DebugCheck.AssertNull(interactiveTui,
                "attempted to render TUI items on null");

            // Prepare the console
            ConsoleWrapper.CursorVisible = false;
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = (ConsoleWrapper.WindowWidth / 2) - 2;
            int SeparatorMinimumHeight = 1;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

            // Redraw the entire TUI screen
            if (BaseInteractiveTui.RedrawRequired)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "We're redrawing.");
                KernelColorTools.LoadBack(BaseInteractiveTui.BackgroundColor, true);

                // Make a separator that separates the two panes to make it look like Total Commander or Midnight Commander. We need information in the upper and the
                // lower part of the console, so we need to render the entire program to look like this: (just a concept mockup)
                //
                //       | vvvvvvvvvvvvvvvvvvvvvv (SeparatorHalfConsoleWidth)
                //       |  vvvvvvvvvvvvvvvvvvvv  (SeparatorHalfConsoleWidthInterior)
                // H: 0  |
                // H: 1  | a--------------------|c---------------------| < ----> (SeparatorMinimumHeight)
                // H: 2  | |b                   ||d                    | << ----> (SeparatorMinimumHeightInterior)
                // H: 3  | |                    ||                     | <<
                // H: 4  | |                    ||                     | <<
                // H: 5  | |                    ||                     | <<
                // H: 6  | |                    ||                     | <<
                // H: 7  | |                    ||                     | <<
                // H: 8  | |                    ||                     | << ----> (SeparatorMaximumHeightInterior)
                // H: 9  | |--------------------||---------------------| < ----> (SeparatorMaximumHeight)
                // H: 10 |
                //       | where a is the dimension for the first pane upper left corner           (0, SeparatorMinimumHeight                                     (usually 1))
                //       |   and b is the dimension for the first pane interior upper left corner  (1, SeparatorMinimumHeightInterior                             (usually 2))
                //       |   and c is the dimension for the second pane upper left corner          (SeparatorHalfConsoleWidth, SeparatorMinimumHeight             (usually 1))
                //       |   and d is the dimension for the second pane interior upper left corner (SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior (usually 2))

                // First, the horizontal and vertical separators
                var finalForeColorFirstPane  = BaseInteractiveTui.CurrentPane == 1 ? BaseInteractiveTui.PaneSelectedSeparatorColor : BaseInteractiveTui.PaneSeparatorColor;
                var finalForeColorSecondPane = BaseInteractiveTui.CurrentPane == 2 ? BaseInteractiveTui.PaneSelectedSeparatorColor : BaseInteractiveTui.PaneSeparatorColor;
                DebugWriter.WriteDebug(DebugLevel.I, "0, {0}, {1}, {2}, {3}, {4}", SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, finalForeColorFirstPane.PlainSequence, BaseInteractiveTui.PaneBackgroundColor.PlainSequence);
                BorderColor.WriteBorder(0, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, finalForeColorFirstPane, BaseInteractiveTui.PaneBackgroundColor);
                DebugWriter.WriteDebug(DebugLevel.I, "{0}, {1}, {2}, {3}, {4}, {5}", SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, finalForeColorSecondPane.PlainSequence, BaseInteractiveTui.PaneBackgroundColor.PlainSequence);
                BorderColor.WriteBorder(SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, finalForeColorSecondPane, BaseInteractiveTui.PaneBackgroundColor);

                // Render the key bindings
                ConsoleWrapper.CursorLeft = 0;
                var finalBindings = new List<InteractiveTuiBinding>(interactiveTui.Bindings)
                {
                    new InteractiveTuiBinding(/* Localizable */ "Exit", ConsoleKey.Escape, null)
                };
                foreach (InteractiveTuiBinding binding in finalBindings)
                {
                    // First, check to see if the rendered binding info is going to exceed the console window width
                    if (!($" {binding.BindingKeyName} {binding.BindingName}  ".Length + ConsoleWrapper.CursorLeft >= ConsoleWrapper.WindowWidth))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Drawing binding {0} with description {1}...", binding.BindingKeyName.ToString(), binding.BindingName);
                        TextWriterWhereColor.WriteWhere($" {binding.BindingKeyName} ", ConsoleWrapper.CursorLeft + 0, ConsoleWrapper.WindowHeight - 1, BaseInteractiveTui.KeyBindingOptionColor, BaseInteractiveTui.OptionBackgroundColor);
                        TextWriterWhereColor.WriteWhere($"{(binding._localizable ? Translate.DoTranslation(binding.BindingName) : binding.BindingName)}  ", ConsoleWrapper.CursorLeft + 1, ConsoleWrapper.WindowHeight - 1, BaseInteractiveTui.OptionForegroundColor, BaseInteractiveTui.BackgroundColor);
                    }
                }

                // Don't require redraw
                BaseInteractiveTui.RedrawRequired = false;
            }
        }

        private static void DrawInteractiveTuiItems(BaseInteractiveTui interactiveTui, int paneNum)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            DebugCheck.AssertNull(interactiveTui,
                "attempted to render TUI items on null");

            // Check to make sure that we're not rendering the second pane on the first-pane-only interactive TUI
            DebugCheck.Assert((!interactiveTui.SecondPaneInteractable && paneNum == 1) || interactiveTui.SecondPaneInteractable,
                "tried to render interactive TUI items for the secondary pane on an interactive TUI that only allows interaction from one pane.");

            // Get some positions
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = (ConsoleWrapper.WindowWidth / 2) - 2;
            int SeparatorMinimumHeightInterior = 2;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

            // Check the pane number
            if (paneNum < 1)
                paneNum = 1;
            if (paneNum > 2)
                paneNum = 2;

            // Get how many data are there in the chosen data source
            var data = paneNum == 2 ? interactiveTui.SecondaryDataSource : interactiveTui.PrimaryDataSource;
            int dataCount = CountElements(data);

            // Render the pane right away
            int answersPerPage = SeparatorMaximumHeightInterior - 1;
            int paneCurrentSelection = paneNum == 2 ? BaseInteractiveTui.SecondPaneCurrentSelection : BaseInteractiveTui.FirstPaneCurrentSelection;
            int currentPage = (paneCurrentSelection - 1) / answersPerPage;
            int startIndex = answersPerPage * currentPage;
            for (int i = 0; i <= answersPerPage; i++)
            {
                // Populate the first pane
                string finalEntry = "";
                int finalIndex = i + startIndex;
                if (finalIndex <= dataCount - 1)
                {
                    // Here, it's getting uglier as we don't have ElementAt() in IEnumerable, too!
                    object dataObject = null;
                    int steppedItems = 0;
                    foreach (var item in data)
                    {
                        steppedItems++;
                        if (steppedItems == startIndex + i + 1)
                        {
                            // We found the item that we need! Assign it to dataObject so GetEntryFromItem() can formulate a string.
                            DebugWriter.WriteDebug(DebugLevel.I, "Found required item index {0} [{1}, offset {2}, final {3}].", steppedItems, i, startIndex, finalIndex);
                            dataObject = item;
                            break;
                        }
                    }

                    // Here, we're now doing our job
                    finalEntry = interactiveTui.GetEntryFromItem(dataObject).Truncate(SeparatorHalfConsoleWidthInterior - 4);
                }

                // Render an entry
                var finalForeColor = finalIndex == paneCurrentSelection - 1 ? BaseInteractiveTui.PaneSelectedItemForeColor : BaseInteractiveTui.PaneItemForeColor;
                var finalBackColor = finalIndex == paneCurrentSelection - 1 ? BaseInteractiveTui.PaneSelectedItemBackColor : BaseInteractiveTui.PaneItemBackColor;
                int left = paneNum == 2 ? SeparatorHalfConsoleWidth + 1 : 1;
                int top = SeparatorMinimumHeightInterior + finalIndex - startIndex;
                TextWriterWhereColor.WriteWhere(finalEntry + new string(' ', SeparatorHalfConsoleWidthInterior - finalEntry.Length), left, top, finalForeColor, finalBackColor);
            }

            // Render the vertical bar
            if (Config.MainConfig.EnableScrollBarInSelection)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing scroll bar.");
                int left = paneNum == 2 ? ConsoleWrapper.WindowWidth - 3 : SeparatorHalfConsoleWidthInterior - 1;
                ProgressBarVerticalColor.WriteVerticalProgress(100 * ((double)paneCurrentSelection / dataCount), left, 1, 2, 2, false);
            }
        }

        private static void DrawInformationOnSecondPane(BaseInteractiveTui interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            DebugCheck.AssertNull(interactiveTui,
                "attempted to render TUI items on null");

            // Check to make sure that we're not rendering the information pane on the both-panes interactive TUI
            DebugCheck.Assert(!interactiveTui.SecondPaneInteractable,
                "tried to render information the secondary pane on an interactive TUI that allows interaction from two panes, messing the selection rendering up there.");

            // Populate some positions
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = (ConsoleWrapper.WindowWidth / 2) - 2;
            int SeparatorMinimumHeight = 1;
            int SeparatorMinimumHeightInterior = 2;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

            // Populate some colors
            var ForegroundColor = BaseInteractiveTui.ForegroundColor;
            var PaneItemBackColor = BaseInteractiveTui.PaneItemBackColor;

            // Now, do the job!
            string finalInfoRendered;
            try
            {
                // Populate data source and its count
                int paneCurrentSelection = BaseInteractiveTui.CurrentPane == 2 ?
                                           BaseInteractiveTui.SecondPaneCurrentSelection :
                                           BaseInteractiveTui.FirstPaneCurrentSelection;
                var data = BaseInteractiveTui.CurrentPane == 2 ?
                           interactiveTui.SecondaryDataSource :
                           interactiveTui.PrimaryDataSource;
                int dataCount = CountElements(data);

                // Populate selected data
                DebugWriter.WriteDebug(DebugLevel.I, "{0} elements.", dataCount);
                if (dataCount > 0)
                {
                    object selectedData = GetElementFromIndex(data, paneCurrentSelection - 1);
                    DebugCheck.AssertNull(selectedData,
                        "attempted to render info about null data");
                    finalInfoRendered = interactiveTui.GetInfoFromItem(selectedData);
                    DebugWriter.WriteDebug(DebugLevel.I, "Rendered info: {0}", finalInfoRendered);
                }
                else
                {
                    finalInfoRendered = Translate.DoTranslation("No info.");
                    DebugWriter.WriteDebug(DebugLevel.W, "There is no info!");
                }
            }
            catch (Exception ex)
            {
                finalInfoRendered = Translate.DoTranslation("Failed to get information.");
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to get information in interactive TUI: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }

            // Now, write info
            var finalForeColorSecondPane = BaseInteractiveTui.CurrentPane == 2 ? BaseInteractiveTui.PaneSelectedSeparatorColor : BaseInteractiveTui.PaneSeparatorColor;
            BorderColor.WriteBorder(SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, finalForeColorSecondPane, BaseInteractiveTui.PaneBackgroundColor);
            _finalInfoRendered = finalInfoRendered;
            string[] finalInfoStrings = TextTools.GetWrappedSentences(finalInfoRendered, SeparatorHalfConsoleWidthInterior);
            for (int infoIndex = 0; infoIndex < finalInfoStrings.Length; infoIndex++)
            {
                // Check to see if the info is overpopulated
                if (infoIndex >= SeparatorMaximumHeightInterior - 1)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Info is overflowing! {0} >= {1}", infoIndex, SeparatorMaximumHeightInterior - 1);
                    string truncated = Translate.DoTranslation("Shift+I = more info");
                    TextWriterWhereColor.WriteWhere(truncated + new string(' ', SeparatorHalfConsoleWidthInterior - truncated.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + infoIndex, ForegroundColor, PaneItemBackColor);
                    break;
                }

                // Now, render the info
                string finalInfo = finalInfoStrings[infoIndex];
                DebugWriter.WriteDebug(DebugLevel.I, "Rendering final info {0}...", finalInfo);
                TextWriterWhereColor.WriteWhere(finalInfo + new string(' ', SeparatorHalfConsoleWidthInterior - finalInfo.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + infoIndex, ForegroundColor, PaneItemBackColor);
            }
        }

        private static void DrawStatus(BaseInteractiveTui interactiveTui)
        {
            // Populate some necessary variables
            int paneCurrentSelection = BaseInteractiveTui.CurrentPane == 2 ?
                                       BaseInteractiveTui.SecondPaneCurrentSelection :
                                       BaseInteractiveTui.FirstPaneCurrentSelection;
            var data = BaseInteractiveTui.CurrentPane == 2 ?
                       interactiveTui.SecondaryDataSource :
                       interactiveTui.PrimaryDataSource;
            object selectedData = GetElementFromIndex(data, paneCurrentSelection - 1);
            interactiveTui.RenderStatus(selectedData);
            DebugWriter.WriteDebug(DebugLevel.I, "Status rendered. {0}", BaseInteractiveTui.Status);

            // Now, write info
            TextWriterWhereColor.WriteWhere(BaseInteractiveTui.Status.Truncate(ConsoleWrapper.WindowWidth - 3), 0, 0, BaseInteractiveTui.ForegroundColor, BaseInteractiveTui.BackgroundColor);
            ConsoleExtensions.ClearLineToRight();
        }

        private static void RespondToUserInput(BaseInteractiveTui interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            DebugCheck.AssertNull(interactiveTui,
                "attempted to respond to user input on null");

            // Populate some necessary variables
            int paneCurrentSelection = BaseInteractiveTui.CurrentPane == 2 ?
                                       BaseInteractiveTui.SecondPaneCurrentSelection :
                                       BaseInteractiveTui.FirstPaneCurrentSelection;
            var data = BaseInteractiveTui.CurrentPane == 2 ?
                       interactiveTui.SecondaryDataSource :
                       interactiveTui.PrimaryDataSource;
            int dataCount = CountElements(data);

            // Populate selected data
            object selectedData = GetElementFromIndex(data, paneCurrentSelection - 1);

            // Wait for key
            try
            {
                ConsoleKeyInfo pressedKey;
                if (interactiveTui.RefreshInterval == 0 || interactiveTui.SecondPaneInteractable)
                    pressedKey = Input.DetectKeypress();
                else
                    pressedKey = Input.ReadKeyTimeout(true, TimeSpan.FromMilliseconds(interactiveTui.RefreshInterval));

                // Handle the key
                DebugWriter.WriteDebug(DebugLevel.I, "Pressed key. Handling...");
                switch (pressedKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (BaseInteractiveTui.CurrentPane == 2)
                        {
                            BaseInteractiveTui.SecondPaneCurrentSelection--;
                            if (BaseInteractiveTui.SecondPaneCurrentSelection < 1)
                                BaseInteractiveTui.SecondPaneCurrentSelection = dataCount;
                            DebugWriter.WriteDebug(DebugLevel.I, "Selection: {0}", BaseInteractiveTui.SecondPaneCurrentSelection);
                        }
                        else
                        {
                            BaseInteractiveTui.FirstPaneCurrentSelection--;
                            if (BaseInteractiveTui.FirstPaneCurrentSelection < 1)
                                BaseInteractiveTui.FirstPaneCurrentSelection = dataCount;
                            DebugWriter.WriteDebug(DebugLevel.I, "Selection: {0}", BaseInteractiveTui.SecondPaneCurrentSelection);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (BaseInteractiveTui.CurrentPane == 2)
                        {
                            BaseInteractiveTui.SecondPaneCurrentSelection++;
                            if (BaseInteractiveTui.SecondPaneCurrentSelection > dataCount)
                                BaseInteractiveTui.SecondPaneCurrentSelection = 1;
                            DebugWriter.WriteDebug(DebugLevel.I, "Selection: {0}", BaseInteractiveTui.SecondPaneCurrentSelection);
                        }
                        else
                        {
                            BaseInteractiveTui.FirstPaneCurrentSelection++;
                            if (BaseInteractiveTui.FirstPaneCurrentSelection > dataCount)
                                BaseInteractiveTui.FirstPaneCurrentSelection = 1;
                            DebugWriter.WriteDebug(DebugLevel.I, "Selection: {0}", BaseInteractiveTui.SecondPaneCurrentSelection);
                        }
                        break;
                    case ConsoleKey.PageUp:
                        if (BaseInteractiveTui.CurrentPane == 2)
                            BaseInteractiveTui.SecondPaneCurrentSelection = 1;
                        else
                            BaseInteractiveTui.FirstPaneCurrentSelection = 1;
                        DebugWriter.WriteDebug(DebugLevel.I, "Selection: 1");
                        break;
                    case ConsoleKey.PageDown:
                        if (BaseInteractiveTui.CurrentPane == 2)
                            BaseInteractiveTui.SecondPaneCurrentSelection = dataCount;
                        else
                            BaseInteractiveTui.FirstPaneCurrentSelection = dataCount;
                        DebugWriter.WriteDebug(DebugLevel.I, "Selection: {0}", dataCount);
                        break;
                    case ConsoleKey.I:
                        if (pressedKey.Modifiers.HasFlag(ConsoleModifiers.Shift) && !string.IsNullOrEmpty(_finalInfoRendered))
                        {
                            // User needs more information in the infobox
                            DebugWriter.WriteDebug(DebugLevel.I, "Rendering information in the infobox...");
                            InfoBoxColor.WriteInfoBox(_finalInfoRendered, BaseInteractiveTui.BoxForegroundColor, BaseInteractiveTui.BoxBackgroundColor);
                            BaseInteractiveTui.RedrawRequired = true;
                        }
                        break;
                    case ConsoleKey.Escape:
                        // User needs to exit
                        DebugWriter.WriteDebug(DebugLevel.I, "Exiting...");
                        interactiveTui.HandleExit();
                        interactiveTui.isExiting = true;
                        break;
                    default:
                        var implementedBindings = interactiveTui.Bindings.Where((binding) => binding.BindingKeyName == pressedKey.Key);
                        foreach (var implementedBinding in implementedBindings)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Executing implemented binding {0}...", implementedBinding.BindingKeyName.ToString(), implementedBinding.BindingName);
                            implementedBinding.BindingAction.Invoke(selectedData, paneCurrentSelection - 1);
                        }
                        break;
                }
            }
            catch (KernelException kex) when (kex.ExceptionType == KernelExceptionType.ConsoleReadTimeout)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Refreshing...");
            }
        }
    }
}
