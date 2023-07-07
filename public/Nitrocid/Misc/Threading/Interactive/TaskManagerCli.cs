
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

using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using System;
using System.Collections.Generic;
using KS.Misc.Threading.Interactive;
using KS.Misc.Threading;
using System.Diagnostics;
using KS.Misc.Interactive;
using System.Collections;

namespace KS.Files.Interactive
{
    /// <summary>
    /// Task manager class
    /// </summary>
    public class TaskManagerCli : BaseInteractiveTui, IInteractiveTui
    {
        private static bool osThreadMode = false;

        /// <summary>
        /// Task manager bindings
        /// </summary>
        public override List<InteractiveTuiBinding> Bindings { get; set; } = new()
        {
            // Operations
            new InteractiveTuiBinding(/* Localizable */ "Kill",   ConsoleKey.F1,     (_, index) => KillThread(index), true),
            new InteractiveTuiBinding(/* Localizable */ "Switch", ConsoleKey.Tab,    (_, _) => SwitchMode(), true)
        };

        /// <summary>
        /// Task manager background color
        /// </summary>
        public static new Color BackgroundColor => TaskManagerCliColors.TaskManagerBackgroundColor;
        /// <summary>
        /// Task manager foreground color
        /// </summary>
        public static new Color ForegroundColor => TaskManagerCliColors.TaskManagerForegroundColor;
        /// <summary>
        /// Task manager pane background color
        /// </summary>
        public static new Color PaneBackgroundColor => TaskManagerCliColors.TaskManagerPaneBackgroundColor;
        /// <summary>
        /// Task manager pane separator color
        /// </summary>
        public static new Color PaneSeparatorColor => TaskManagerCliColors.TaskManagerPaneSeparatorColor;
        /// <summary>
        /// Task manager pane selected Task color (foreground)
        /// </summary>
        public static new Color PaneSelectedItemForeColor => TaskManagerCliColors.TaskManagerPaneSelectedTaskForeColor;
        /// <summary>
        /// Task manager pane selected Task color (background)
        /// </summary>
        public static new Color PaneSelectedItemBackColor => TaskManagerCliColors.TaskManagerPaneSelectedTaskBackColor;
        /// <summary>
        /// Task manager pane Task color (foreground)
        /// </summary>
        public static new Color PaneItemForeColor => TaskManagerCliColors.TaskManagerPaneTaskForeColor;
        /// <summary>
        /// Task manager pane Task color (background)
        /// </summary>
        public static new Color PaneItemBackColor => TaskManagerCliColors.TaskManagerPaneTaskBackColor;
        /// <summary>
        /// Task manager option background color
        /// </summary>
        public static new Color OptionBackgroundColor => TaskManagerCliColors.TaskManagerOptionBackgroundColor;
        /// <summary>
        /// Task manager key binding in option color
        /// </summary>
        public static new Color KeyBindingOptionColor => TaskManagerCliColors.TaskManagerKeyBindingOptionColor;
        /// <summary>
        /// Task manager option foreground color
        /// </summary>
        public static new Color OptionForegroundColor => TaskManagerCliColors.TaskManagerOptionForegroundColor;
        /// <summary>
        /// Task manager box background color
        /// </summary>
        public static new Color BoxBackgroundColor => TaskManagerCliColors.TaskManagerBoxBackgroundColor;
        /// <summary>
        /// Task manager box foreground color
        /// </summary>
        public static new Color BoxForegroundColor => TaskManagerCliColors.TaskManagerBoxForegroundColor;

        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource =>
            osThreadMode ? ThreadManager.OperatingSystemThreads : ThreadManager.KernelThreads;

        /// <inheritdoc/>
        public override string RenderInfoOnSecondPane(object item)
        {
            // Populate some positions
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = (ConsoleWrapper.WindowWidth / 2) - 2;
            int SeparatorMinimumHeightInterior = 2;

            if (osThreadMode)
            {
                ProcessThread selectedThread = (ProcessThread)item;
                string finalRenderedTaskID = (Translate.DoTranslation("Task ID") + $": {selectedThread.Id}").Truncate(SeparatorHalfConsoleWidthInterior - 3);
                string finalRenderedTaskPPT = (Translate.DoTranslation("Privileged processor time") + $": {selectedThread.PrivilegedProcessorTime}").Truncate(SeparatorHalfConsoleWidthInterior - 3);
                string finalRenderedTaskUPT = (Translate.DoTranslation("User processor time") + $": {selectedThread.UserProcessorTime}").Truncate(SeparatorHalfConsoleWidthInterior - 3);
                string finalRenderedTaskTPT = (Translate.DoTranslation("Total processor time") + $": {selectedThread.TotalProcessorTime}").Truncate(SeparatorHalfConsoleWidthInterior - 3);
                string finalRenderedTaskState = (Translate.DoTranslation("Task state") + $": {selectedThread.ThreadState}").Truncate(SeparatorHalfConsoleWidthInterior - 3);
                string finalRenderedTaskPriority = (Translate.DoTranslation("Priority level") + $": {selectedThread.CurrentPriority}").Truncate(SeparatorHalfConsoleWidthInterior - 3);
                string finalRenderedTaskMemAddress = (Translate.DoTranslation("Task memory address") + $": 0x{selectedThread.StartAddress:X8}").Truncate(SeparatorHalfConsoleWidthInterior - 3);
                TextWriterWhereColor.WriteWhere(finalRenderedTaskID + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskID.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 0, ForegroundColor, PaneItemBackColor);
                TextWriterWhereColor.WriteWhere(finalRenderedTaskPPT + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskPPT.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 2, ForegroundColor, PaneItemBackColor);
                TextWriterWhereColor.WriteWhere(finalRenderedTaskUPT + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskUPT.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 3, ForegroundColor, PaneItemBackColor);
                TextWriterWhereColor.WriteWhere(finalRenderedTaskTPT + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskTPT.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 4, ForegroundColor, PaneItemBackColor);
                TextWriterWhereColor.WriteWhere(finalRenderedTaskState + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskState.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 5, ForegroundColor, PaneItemBackColor);
                TextWriterWhereColor.WriteWhere(finalRenderedTaskPriority + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskPriority.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 6, ForegroundColor, PaneItemBackColor);
                TextWriterWhereColor.WriteWhere(finalRenderedTaskMemAddress + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskMemAddress.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 7, ForegroundColor, PaneItemBackColor);
            }
            else
            {
                KernelThread selectedThread = (KernelThread)item;
                string finalRenderedTaskName = (Translate.DoTranslation("Task name") + $": {selectedThread.Name}").Truncate(SeparatorHalfConsoleWidthInterior - 3);
                string finalRenderedTaskAlive = (Translate.DoTranslation("Alive") + $": {selectedThread.IsAlive}").Truncate(SeparatorHalfConsoleWidthInterior - 3);
                string finalRenderedTaskBackground = (Translate.DoTranslation("Background") + $": {selectedThread.IsBackground}").Truncate(SeparatorHalfConsoleWidthInterior - 3);
                string finalRenderedTaskCritical = (Translate.DoTranslation("Critical") + $": {selectedThread.IsCritical}").Truncate(SeparatorHalfConsoleWidthInterior - 3);
                string finalRenderedTaskReady = (Translate.DoTranslation("Ready") + $": {selectedThread.IsReady}").Truncate(SeparatorHalfConsoleWidthInterior - 3);
                TextWriterWhereColor.WriteWhere(finalRenderedTaskName + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskName.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 0, ForegroundColor, PaneItemBackColor);
                TextWriterWhereColor.WriteWhere(finalRenderedTaskAlive + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskAlive.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 2, ForegroundColor, PaneItemBackColor);
                TextWriterWhereColor.WriteWhere(finalRenderedTaskBackground + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskBackground.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 3, ForegroundColor, PaneItemBackColor);
                TextWriterWhereColor.WriteWhere(finalRenderedTaskCritical + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskCritical.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 4, ForegroundColor, PaneItemBackColor);
                TextWriterWhereColor.WriteWhere(finalRenderedTaskReady + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskReady.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 5, ForegroundColor, PaneItemBackColor);
                TextWriterWhereColor.WriteWhere(" ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskReady.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 6, ForegroundColor, PaneItemBackColor);
                TextWriterWhereColor.WriteWhere(" ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedTaskReady.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 7, ForegroundColor, PaneItemBackColor);
            }

            // Prepare the status
            Status = Translate.DoTranslation("Ready");
            string finalInfoRendered = $" {Status}";
            return finalInfoRendered;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item)
        {
            if (osThreadMode)
            {
                ProcessThread thread = (ProcessThread)item;
                return $"{thread.Id}";
            }
            else
            {
                KernelThread thread = (KernelThread)item;
                return $"{thread.Name}";
            }
        }

        private static void KillThread(int id)
        {
            if (!osThreadMode)
                if (!ThreadManager.kernelThreads[id].IsCritical && ThreadManager.kernelThreads[id].IsAlive)
                    ThreadManager.kernelThreads[id].Stop();
                else if (!ThreadManager.kernelThreads[id].IsAlive)
                    Status = Translate.DoTranslation("Kernel task is already killed.");
                else
                    Status = Translate.DoTranslation("Kernel task is critical and can't be killed.");
            else
                Status = Translate.DoTranslation("OS threads can't be killed.");
        }

        private static void SwitchMode()
        {
            osThreadMode = !osThreadMode;
            FirstPaneCurrentSelection = 1;
        }
    }
}
