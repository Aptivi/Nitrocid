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

using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using Terminaux.Inputs.Interactive;
using Textify.General;
using System;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Threading;

namespace Nitrocid.Base.Misc.Interactives
{
    /// <summary>
    /// Task manager class
    /// </summary>
    public class TaskManagerCli : BaseInteractiveTui<(int, object)>, IInteractiveTui<(int, object)>
    {
        internal string taskStatus = "";
        internal bool osThreadMode = false;

        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                HelpTitle = /* Localizable */ "NKS_MISC_INTERACTIVES_TASKMANUI_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_MISC_INTERACTIVES_TASKMANUI_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANUI_HELP01_BODY") + "\n\n" +
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/advanced-and-power-users/inner-workings/inner-essentials/kernel-threads",
            },
            new()
            {
                HelpTitle = /* Localizable */ "NKS_MISC_INTERACTIVES_TASKMANUI_HELP02_TITLE",
                HelpDescription = /* Localizable */ "NKS_MISC_INTERACTIVES_TASKMANUI_HELP02_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANUI_HELP02_BODY") + "\n\n" +
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/advanced-and-power-users/inner-workings/inner-essentials/kernel-threads",
            }
        ];

        /// <inheritdoc/>
        public override IEnumerable<(int, object)> PrimaryDataSource
        {
            get
            {
                IEnumerable threads = osThreadMode ? ThreadManager.OperatingSystemThreads : ThreadManager.KernelThreads;
                List<(int, object)> result = [];
                if (threads is ProcessThreadCollection osThreads)
                {
                    // The level is always zero here, because the OS thread instance doesn't provide info about
                    // managed threads as they don't always have a 1:1 relationship with the OS thread.
                    foreach (var thread in osThreads)
                        result.Add((0, thread));
                }
                else
                {
                    int nestLevel = 0;
                    if (threads is not List<KernelThread> managedThreads)
                        return [];
                    foreach (var thread in managedThreads)
                    {
                        void HandleChildThreads(KernelThread thread)
                        {
                            nestLevel++;
                            var childThreads = thread.ChildThreads;
                            foreach (var childThread in childThreads)
                            {
                                result.Add((nestLevel, childThread));
                                HandleChildThreads(childThread);
                            }
                            nestLevel--;
                        }

                        result.Add((nestLevel, thread));
                        HandleChildThreads(thread);
                    }
                }
                return result;
            }
        }

        /// <inheritdoc/>
        public override int RefreshInterval =>
            3000;

        /// <inheritdoc/>
        public override string GetInfoFromItem((int, object) item)
        {
            if (osThreadMode)
            {
                ProcessThread selectedThread = (ProcessThread)item.Item2;
                string finalRenderedTaskID = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_OSTASKID") + $": {selectedThread.Id}";
                string finalRenderedTaskPPT = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_OSPRIVCPUTIME") + $": {selectedThread.PrivilegedProcessorTime}";
                string finalRenderedTaskUPT = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_OSUSERCPUTIME") + $": {selectedThread.UserProcessorTime}";
                string finalRenderedTaskTPT = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_OSTOTALCPUTIME") + $": {selectedThread.TotalProcessorTime}";
                string finalRenderedTaskState = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_OSTASKSTATE") + $": {selectedThread.ThreadState}";
                string finalRenderedTaskPriority = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_OSPRIORITY") + $": {selectedThread.CurrentPriority}";
                string finalRenderedTaskMemAddress = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_OSTASKMEMADDRESS") + $": 0x{selectedThread.StartAddress:X16}";
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
                KernelThread selectedThread = (KernelThread)item.Item2;
                string finalRenderedTaskName = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_KERNELTASKNAME") + $": {selectedThread.Name}";
                string finalRenderedTaskAlive = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_KERNELALIVE") + $": {selectedThread.IsAlive}";
                string finalRenderedTaskBackground = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_KERNELBG") + $": {selectedThread.IsBackground}";
                string finalRenderedTaskCritical = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_KERNELCRITICAL") + $": {selectedThread.IsCritical}";
                string finalRenderedTaskReady = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_KERNELREADY") + $": {selectedThread.IsReady}";
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
        public override string GetStatusFromItem((int, object) item)
        {
            string status = "";
            if (osThreadMode)
            {
                ProcessThread thread = (ProcessThread)item.Item2;
                if (string.IsNullOrEmpty(taskStatus))
                    status = $"{thread.Id}";
                else
                    status = $"{thread.Id} - {taskStatus}";
            }
            else
            {
                KernelThread thread = (KernelThread)item.Item2;
                if (string.IsNullOrEmpty(taskStatus))
                    status = $"{thread.Name}";
                else
                    status = $"{thread.Name} - {taskStatus}";
            }
            taskStatus = "";
            return status;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem((int, object) item)
        {
            if (osThreadMode)
            {
                ProcessThread thread = (ProcessThread)item.Item2;
                return $"{thread.Id}";
            }
            else
            {
                KernelThread thread = (KernelThread)item.Item2;
                return $"{new string(' ', item.Item1 * 2)}{thread.Name}";
            }
        }

        internal void KillThread(object id)
        {
            if (!osThreadMode)
            {
                (int, object) item = ((int, object))id;
                KernelThread thread = (KernelThread)item.Item2;
                if (!thread.IsCritical && thread.IsAlive)
                    thread.Stop();
                else if (!thread.IsAlive)
                    taskStatus = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_TASKALREADYKILLED");
                else
                    taskStatus = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_TASKCRITICAL");
            }
            else
                taskStatus = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_TASKOSTHREADSCANTKILL");
        }

        internal void SwitchMode()
        {
            osThreadMode = !osThreadMode;
            InteractiveTuiTools.SelectionMovement(this, 1);
        }

        internal static void OpenTaskManagerCli()
        {
            var tui = new TaskManagerCli();
            tui.Bindings.Add(new InteractiveTuiBinding<(int, object)>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_KEYBINDING_KILL"), ConsoleKey.F1, (thread, _, _, _) => tui.KillThread(thread)));
            tui.Bindings.Add(new InteractiveTuiBinding<(int, object)>(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_SWITCH"), ConsoleKey.F2, (_, _, _, _) => tui.SwitchMode()));
            InteractiveTuiTools.OpenInteractiveTui(tui);
        }
    }
}
