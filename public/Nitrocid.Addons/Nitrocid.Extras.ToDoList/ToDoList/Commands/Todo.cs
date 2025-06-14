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
using System.Linq;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Extras.ToDoList.ToDoList.Commands
{
    /// <summary>
    /// Manages your to-do list
    /// </summary>
    /// <remarks>
    /// This is a master application for the to-do list manager.
    /// </remarks>
    class TodoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string Action = parameters.ArgumentsList[0];

            // Enumerate based on action
            var ActionArguments = parameters.ArgumentsList.Skip(1).ToArray();
            switch (Action)
            {
                case "add":
                    {
                        // User chose to add a task
                        if (ActionArguments.Length != 0)
                        {
                            try
                            {
                                ToDoManager.AddTask(ActionArguments[0]);
                            }
                            catch (Exception ex)
                            {
                                DebugWriter.WriteDebugStackTrace(ex);
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_TODO_ADDREMOVEFAILED", "Nitrocid.Extras.ToDoList") + " {0}", true, KernelColorType.Error, ex.Message);
                                return ex.GetHashCode();
                            }
                        }
                        else
                        {
                            TextWriters.Write(LanguageTools.GetLocalized("NKS_TODO_NAMENEEDED", "Nitrocid.Extras.ToDoList"), true, KernelColorType.Error);
                            return 5;
                        }

                        return 0;
                    }
                case "remove":
                    {
                        // User chose to remove a task
                        if (ActionArguments.Length != 0)
                        {
                            try
                            {
                                ToDoManager.RemoveTask(ActionArguments[0]);
                            }
                            catch (Exception ex)
                            {
                                DebugWriter.WriteDebugStackTrace(ex);
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_TODO_ADDREMOVEFAILED", "Nitrocid.Extras.ToDoList") + " {0}", true, KernelColorType.Error, ex.Message);
                                return ex.GetHashCode();
                            }
                        }
                        else
                        {
                            TextWriters.Write(LanguageTools.GetLocalized("NKS_TODO_NAMENEEDED", "Nitrocid.Extras.ToDoList"), true, KernelColorType.Error);
                            return 5;
                        }

                        return 0;
                    }
                case "done":
                    {
                        // User chose to mark a task as done
                        if (ActionArguments.Length != 0)
                        {
                            try
                            {
                                ToDoManager.SetDone(ActionArguments[0]);
                            }
                            catch (Exception ex)
                            {
                                DebugWriter.WriteDebugStackTrace(ex);
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_TODO_MARKFAILED", "Nitrocid.Extras.ToDoList") + " {0}", true, KernelColorType.Error, ex.Message);
                                return ex.GetHashCode();
                            }
                        }
                        else
                        {
                            TextWriters.Write(LanguageTools.GetLocalized("NKS_TODO_NAMENEEDED", "Nitrocid.Extras.ToDoList"), true, KernelColorType.Error);
                            return 5;
                        }

                        return 0;
                    }
                case "undone":
                    {
                        // User chose to mark a task as undone
                        if (ActionArguments.Length != 0)
                        {
                            try
                            {
                                ToDoManager.SetUndone(ActionArguments[0]);
                            }
                            catch (Exception ex)
                            {
                                DebugWriter.WriteDebugStackTrace(ex);
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_TODO_MARKFAILED", "Nitrocid.Extras.ToDoList") + " {0}", true, KernelColorType.Error, ex.Message);
                                return ex.GetHashCode();
                            }
                        }
                        else
                        {
                            TextWriters.Write(LanguageTools.GetLocalized("NKS_TODO_NAMENEEDED", "Nitrocid.Extras.ToDoList"), true, KernelColorType.Error);
                            return 5;
                        }

                        return 0;
                    }
                case "list":
                    {
                        // User chose to list tasks
                        var tasks = ToDoManager.GetTaskNames()
                            .Select((name) => ToDoManager.GetTask(name).TaskDone ? $"[*] {name}" : $"[ ] {name}")
                            .ToArray();
                        TextWriters.WriteList(tasks);
                        return 0;
                    }
                case "save":
                    {
                        // User chose to save tasks
                        ToDoManager.SaveTasks();
                        return 0;
                    }
                case "load":
                    {
                        // User chose to load tasks
                        ToDoManager.LoadTasks();
                        return 0;
                    }
                default:
                    {
                        // Invalid action.
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_TODO_INVALIDACTION", "Nitrocid.Extras.ToDoList"), true, KernelColorType.Error);
                        return 5;
                    }
            }
        }

    }
}
