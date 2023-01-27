
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using ColorSeq;
using Extensification.StringExts;
using FluentFTP.Helpers;
using KS.ConsoleBase;
using KS.Files.Folders;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.TimeDate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KS.ConsoleBase.Inputs;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;
using KS.Misc.Threading.Interactive;
using KS.Misc.Threading;
using System.Diagnostics;

namespace KS.Files.Interactive
{
    /// <summary>
    /// Task manager class
    /// </summary>
    public static class TaskManagerCli
    {
        private static bool redrawRequired = true;
        private static bool isExiting = false;
        private static bool osThreadMode = false;
        private static int paneCurrentSelection = 1;
        private static string status = "";
        private static readonly List<TaskManagerBinding> TaskManagerBindings = new()
        {
            // Operations
            new TaskManagerBinding("Kill",   ConsoleKey.F1,  KillThread),
            new TaskManagerBinding("Switch", ConsoleKey.Tab, (_) => osThreadMode = !osThreadMode),

            // Misc bindings
            new TaskManagerBinding("Exit"  , ConsoleKey.Escape, (_) => isExiting = true)
        };

        /// <summary>
        /// Task manager background color
        /// </summary>
        public static Color TaskManagerBackgroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkBlue));
        /// <summary>
        /// Task manager foreground color
        /// </summary>
        public static Color TaskManagerForegroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Yellow));
        /// <summary>
        /// Task manager pane background color
        /// </summary>
        public static Color TaskManagerPaneBackgroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Blue3));
        /// <summary>
        /// Task manager pane separator color
        /// </summary>
        public static Color TaskManagerPaneSeparatorColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkGreen_005f00));
        /// <summary>
        /// Task manager pane selected task color (foreground)
        /// </summary>
        public static Color TaskManagerPaneSelectedTaskForeColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Yellow));
        /// <summary>
        /// Task manager pane selected task color (background)
        /// </summary>
        public static Color TaskManagerPaneSelectedTaskBackColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkBlue));
        /// <summary>
        /// Task manager pane task color (foreground)
        /// </summary>
        public static Color TaskManagerPaneTaskForeColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkYellow));
        /// <summary>
        /// Task manager pane task color (background)
        /// </summary>
        public static Color TaskManagerPaneTaskBackColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Blue3));
        /// <summary>
        /// Task manager option background color
        /// </summary>
        public static Color TaskManagerOptionBackgroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkCyan));
        /// <summary>
        /// Task manager key binding in option color
        /// </summary>
        public static Color TaskManagerKeyBindingOptionColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Black));
        /// <summary>
        /// Task manager option foreground color
        /// </summary>
        public static Color TaskManagerOptionForegroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Cyan));

        /// <summary>
        /// Opens the task manager
        /// </summary>
        public static void OpenMain()
        {
            isExiting = false;
            redrawRequired = true;
            status = Translate.DoTranslation("Ready");

            while (!isExiting)
            {
                // Prepare the console
                ConsoleWrapper.CursorVisible = false;
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = (ConsoleWrapper.WindowWidth / 2) - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMinimumHeightInterior = 2;
                int SeparatorMaximumHeight = ConsoleWrapper.WindowHeight - 2;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

                // Redraw the entire task manager screen
                if (redrawRequired)
                {
                    ColorTools.LoadBack(TaskManagerBackgroundColor, true);

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
                    BorderColor.WriteBorder(0, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, TaskManagerPaneSeparatorColor, TaskManagerPaneBackgroundColor);
                    BorderColor.WriteBorder(SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, TaskManagerPaneSeparatorColor, TaskManagerPaneBackgroundColor);

                    // Render the key bindings
                    ConsoleWrapper.CursorLeft = 0;
                    foreach (TaskManagerBinding binding in TaskManagerBindings)
                    {
                        // First, check to see if the rendered binding info is going to exceed the console window width
                        if (!($" {binding.BindingKeyName} {binding.BindingName}  ".Length + ConsoleWrapper.CursorLeft >= ConsoleWrapper.WindowWidth))
                        {
                            TextWriterWhereColor.WriteWhere($" {binding.BindingKeyName} ", ConsoleWrapper.CursorLeft + 0, ConsoleWrapper.WindowHeight - 1, TaskManagerKeyBindingOptionColor, TaskManagerOptionBackgroundColor);
                            TextWriterWhereColor.WriteWhere($"{binding.BindingName}  ", ConsoleWrapper.CursorLeft + 1, ConsoleWrapper.WindowHeight - 1, TaskManagerOptionForegroundColor, TaskManagerBackgroundColor);
                        }
                    }

                    // Don't require redraw
                    redrawRequired = false;
                }

                // Render the task lists
                var threads = ThreadManager.KernelThreads;
                var unmanagedThreads = ThreadManager.OperatingSystemThreads;
                int threadsCount = osThreadMode ? unmanagedThreads.Count : threads.Count;
                int pages = threadsCount / SeparatorMaximumHeightInterior;
                int answersPerPage = SeparatorMaximumHeightInterior - 1;
                int currentPage = (paneCurrentSelection - 1) / answersPerPage;
                int startIndex = answersPerPage * currentPage;
                int endIndex = answersPerPage * (currentPage + 1);
                for (int i = 0; i <= answersPerPage; i++)
                {
                    // Populate the first pane
                    string finalEntry = "";
                    int finalIndex = i + startIndex;
                    if (osThreadMode)
                    {
                        if (finalIndex <= unmanagedThreads.Count - 1)
                        {
                            ProcessThread thread = unmanagedThreads[finalIndex];
                            finalEntry = $"{thread.Id}".Truncate(SeparatorHalfConsoleWidthInterior - 4);
                        }
                    }
                    else
                    {
                        if (finalIndex <= threads.Count - 1)
                        {
                            KernelThread thread = threads[finalIndex];
                            finalEntry = $"{thread.Name}".Truncate(SeparatorHalfConsoleWidthInterior - 4);
                        }
                    }

                    var finalForeColor = finalIndex == paneCurrentSelection - 1 ? TaskManagerPaneSelectedTaskForeColor : TaskManagerPaneTaskForeColor;
                    var finalBackColor = finalIndex == paneCurrentSelection - 1 ? TaskManagerPaneSelectedTaskBackColor : TaskManagerPaneTaskBackColor;
                    TextWriterWhereColor.WriteWhere(finalEntry + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalEntry.Length), 1, SeparatorMinimumHeightInterior + finalIndex - startIndex, finalForeColor, finalBackColor);
                }
                ProgressBarVerticalColor.WriteVerticalProgress(100 * ((double)paneCurrentSelection / threadsCount), SeparatorHalfConsoleWidthInterior - 1, 1, 2, 2, false);

                // Write status and task info
                string finalInfoRendered = "";
                try
                {
                    if (osThreadMode)
                    {
                        var selectedThread = unmanagedThreads[paneCurrentSelection - 1];
                        string finalRenderedTaskID = Translate.DoTranslation("Task ID") + $": {selectedThread.Id}".Truncate(ConsoleWrapper.WindowWidth - 3);
                        string finalRenderedTaskPPT = Translate.DoTranslation("Privileged processor time") + $": {selectedThread.PrivilegedProcessorTime}".Truncate(ConsoleWrapper.WindowWidth - 3);
                        string finalRenderedTaskUPT = Translate.DoTranslation("User processor time") + $": {selectedThread.UserProcessorTime}".Truncate(ConsoleWrapper.WindowWidth - 3);
                        string finalRenderedTaskTPT = Translate.DoTranslation("Total processor time") + $": {selectedThread.TotalProcessorTime}".Truncate(ConsoleWrapper.WindowWidth - 3);
                        string finalRenderedTaskState = Translate.DoTranslation("Task state") + $": {selectedThread.ThreadState}".Truncate(ConsoleWrapper.WindowWidth - 3);
                        string finalRenderedTaskPriority = Translate.DoTranslation("Priority level") + $": {selectedThread.CurrentPriority}".Truncate(ConsoleWrapper.WindowWidth - 3);
                        string finalRenderedTaskMemAddress = Translate.DoTranslation("Task memory address") + $": 0x{selectedThread.StartAddress:X8}".Truncate(ConsoleWrapper.WindowWidth - 3);
                        TextWriterWhereColor.WriteWhere(finalRenderedTaskID + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskID.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 0, TaskManagerForegroundColor, TaskManagerPaneTaskBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedTaskPPT + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskPPT.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 2, TaskManagerForegroundColor, TaskManagerPaneTaskBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedTaskUPT + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskUPT.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 3, TaskManagerForegroundColor, TaskManagerPaneTaskBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedTaskTPT + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskTPT.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 4, TaskManagerForegroundColor, TaskManagerPaneTaskBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedTaskState + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskState.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 5, TaskManagerForegroundColor, TaskManagerPaneTaskBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedTaskPriority + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskPriority.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 6, TaskManagerForegroundColor, TaskManagerPaneTaskBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedTaskMemAddress + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskMemAddress.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 7, TaskManagerForegroundColor, TaskManagerPaneTaskBackColor);
                    }
                    else
                    {
                        var selectedThread = threads[paneCurrentSelection - 1];
                        string finalRenderedTaskName = Translate.DoTranslation("Task name") + $": {selectedThread.Name}".Truncate(ConsoleWrapper.WindowWidth - 3);
                        string finalRenderedTaskAlive = Translate.DoTranslation("Alive") + $": {selectedThread.IsAlive}".Truncate(ConsoleWrapper.WindowWidth - 3);
                        string finalRenderedTaskBackground = Translate.DoTranslation("Background") + $": {selectedThread.IsBackground}".Truncate(ConsoleWrapper.WindowWidth - 3);
                        string finalRenderedTaskCritical = Translate.DoTranslation("Critical") + $": {selectedThread.IsCritical}".Truncate(ConsoleWrapper.WindowWidth - 3);
                        string finalRenderedTaskReady = Translate.DoTranslation("Ready") + $": {selectedThread.IsReady}".Truncate(ConsoleWrapper.WindowWidth - 3);
                        TextWriterWhereColor.WriteWhere(finalRenderedTaskName + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskName.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 0, TaskManagerForegroundColor, TaskManagerPaneTaskBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedTaskAlive + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskAlive.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 2, TaskManagerForegroundColor, TaskManagerPaneTaskBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedTaskBackground + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskBackground.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 3, TaskManagerForegroundColor, TaskManagerPaneTaskBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedTaskCritical + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskCritical.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 4, TaskManagerForegroundColor, TaskManagerPaneTaskBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedTaskReady + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskReady.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 5, TaskManagerForegroundColor, TaskManagerPaneTaskBackColor);
                        TextWriterWhereColor.WriteWhere(" ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskReady.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 6, TaskManagerForegroundColor, TaskManagerPaneTaskBackColor);
                        TextWriterWhereColor.WriteWhere(" ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskReady.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 7, TaskManagerForegroundColor, TaskManagerPaneTaskBackColor);
                    }
                    
                    finalInfoRendered = $" {status}";
                    status = Translate.DoTranslation("Ready");
                }
                catch (Exception ex)
                {
                    finalInfoRendered = Translate.DoTranslation("Failed to get task information.");
                    DebugWriter.WriteDebug(DebugLevel.E, "Error trying to get task information in taskman: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
                TextWriterWhereColor.WriteWhere(finalInfoRendered.Truncate(ConsoleWrapper.WindowWidth - 3), 0, 0, TaskManagerForegroundColor, TaskManagerBackgroundColor);
                ConsoleExtensions.ClearLineToRight();

                // Wait for key
                ConsoleKey pressedKey = Input.DetectKeypress().Key;
                switch (pressedKey)
                {
                    case ConsoleKey.UpArrow:
                        paneCurrentSelection--;
                        if (paneCurrentSelection < 1)
                            paneCurrentSelection = threadsCount;
                        break;
                    case ConsoleKey.DownArrow:
                        paneCurrentSelection++;
                        if (paneCurrentSelection > threadsCount)
                            paneCurrentSelection = 1;
                        break;
                    case ConsoleKey.PageUp:
                        paneCurrentSelection = 1;
                        break;
                    case ConsoleKey.PageDown:
                        paneCurrentSelection = threadsCount;
                        break;
                    default:
                        var implementedBindings = TaskManagerBindings.Where((binding) => binding.BindingKeyName == pressedKey);
                        foreach (var implementedBinding in implementedBindings)
                            implementedBinding.BindingAction.Invoke(paneCurrentSelection - 1);
                        break;
                }
            }

            // Clear the console to clean up
            ColorTools.LoadBack();
        }

        private static void KillThread(int id)
        {
            if (!osThreadMode)
                if (!ThreadManager.kernelThreads[id].IsCritical && ThreadManager.kernelThreads[id].IsAlive)
                    ThreadManager.kernelThreads[id].Stop();
                else if (!ThreadManager.kernelThreads[id].IsAlive)
                    status = Translate.DoTranslation("Kernel task is already killed.");
                else
                    status = Translate.DoTranslation("Kernel task is critical and can't be killed.");
            else
                status = Translate.DoTranslation("OS threads can't be killed.");
        }
    }
}
