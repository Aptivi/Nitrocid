//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using System.Diagnostics;
using System.Collections;
using Nitrocid.Kernel.Threading;
using Nitrocid.Languages;
using Terminaux.Inputs.Interactive;
using Textify.General;

namespace Nitrocid.Misc.Interactives
{
    /// <summary>
    /// Task manager class
    /// </summary>
    public class TaskManagerCli : BaseInteractiveTui<object>, IInteractiveTui<object>
    {

        private static string taskStatus = "";
        private static bool osThreadMode = false;

        /// <summary>
        /// Task manager bindings
        /// </summary>
        public override InteractiveTuiBinding[] Bindings { get; } =
        [
            // Operations
            new InteractiveTuiBinding("Kill", ConsoleKey.F1,
                (_, index) => KillThread(index)),
            new InteractiveTuiBinding("Switch", ConsoleKey.F2,
                (_, _) => SwitchMode())
        ];

        /// <inheritdoc/>
        public override IEnumerable<object> PrimaryDataSource
        {
            get
            {
                IEnumerable objects = osThreadMode ? ThreadManager.OperatingSystemThreads : ThreadManager.KernelThreads;
                List<object> result = [];
                foreach (object obj in objects)
                    result.Add(obj);
                return result;
            }
        }

        /// <inheritdoc/>
        public override int RefreshInterval =>
            3000;

        /// <inheritdoc/>
        public override string GetInfoFromItem(object item)
        {
            if (osThreadMode)
            {
                ProcessThread selectedThread = (ProcessThread)item;
                string finalRenderedTaskID = Translate.DoTranslation("Task ID") + $": {selectedThread.Id}";
                string finalRenderedTaskPPT = Translate.DoTranslation("Privileged processor time") + $": {selectedThread.PrivilegedProcessorTime}";
                string finalRenderedTaskUPT = Translate.DoTranslation("User processor time") + $": {selectedThread.UserProcessorTime}";
                string finalRenderedTaskTPT = Translate.DoTranslation("Total processor time") + $": {selectedThread.TotalProcessorTime}";
                string finalRenderedTaskState = Translate.DoTranslation("Task state") + $": {selectedThread.ThreadState}";
                string finalRenderedTaskPriority = Translate.DoTranslation("Priority level") + $": {selectedThread.CurrentPriority}";
                string finalRenderedTaskMemAddress = Translate.DoTranslation("Task memory address") + $": 0x{selectedThread.StartAddress:X16}";
                return
                    finalRenderedTaskID + CharManager.NewLine +
                    finalRenderedTaskPPT + CharManager.NewLine +
                    finalRenderedTaskUPT + CharManager.NewLine +
                    finalRenderedTaskTPT + CharManager.NewLine +
                    finalRenderedTaskState + CharManager.NewLine +
                    finalRenderedTaskPriority + CharManager.NewLine +
                    finalRenderedTaskMemAddress
                ;
            }
            else
            {
                KernelThread selectedThread = (KernelThread)item;
                string finalRenderedTaskName = Translate.DoTranslation("Task name") + $": {selectedThread.Name}";
                string finalRenderedTaskAlive = Translate.DoTranslation("Alive") + $": {selectedThread.IsAlive}";
                string finalRenderedTaskBackground = Translate.DoTranslation("Background") + $": {selectedThread.IsBackground}";
                string finalRenderedTaskCritical = Translate.DoTranslation("Critical") + $": {selectedThread.IsCritical}";
                string finalRenderedTaskReady = Translate.DoTranslation("Ready") + $": {selectedThread.IsReady}";
                return
                    finalRenderedTaskName + CharManager.NewLine +
                    finalRenderedTaskAlive + CharManager.NewLine +
                    finalRenderedTaskBackground + CharManager.NewLine +
                    finalRenderedTaskCritical + CharManager.NewLine +
                    finalRenderedTaskReady
                ;
            }
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem(object item)
        {
            string status = "";
            if (osThreadMode)
            {
                ProcessThread thread = (ProcessThread)item;
                if (string.IsNullOrEmpty(taskStatus))
                    status = $"{thread.Id}";
                else
                    status = $"{thread.Id} - {taskStatus}";
            }
            else
            {
                KernelThread thread = (KernelThread)item;
                if (string.IsNullOrEmpty(taskStatus))
                    status = $"{thread.Name}";
                else
                    status = $"{thread.Name} - {taskStatus}";
            }
            taskStatus = "";
            return status;
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
                    taskStatus = Translate.DoTranslation("Kernel task is already killed.");
                else
                    taskStatus = Translate.DoTranslation("Kernel task is critical and can't be killed.");
            else
                taskStatus = Translate.DoTranslation("OS threads can't be killed.");
        }

        private static void SwitchMode()
        {
            osThreadMode = !osThreadMode;
            InteractiveTuiTools.SelectionMovement(Instance, 1);
        }
    }
}
