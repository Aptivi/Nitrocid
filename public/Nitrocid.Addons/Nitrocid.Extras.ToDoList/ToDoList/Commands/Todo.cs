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
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;

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
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_TODO_ADDREMOVEFAILED") + " {0}", true, ThemeColorType.Error, ex.Message);
                                return ex.GetHashCode();
                            }
                        }
                        else
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_TODO_NAMENEEDED"), true, ThemeColorType.Error);
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
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_TODO_ADDREMOVEFAILED") + " {0}", true, ThemeColorType.Error, ex.Message);
                                return ex.GetHashCode();
                            }
                        }
                        else
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_TODO_NAMENEEDED"), true, ThemeColorType.Error);
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
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_TODO_MARKFAILED") + " {0}", true, ThemeColorType.Error, ex.Message);
                                return ex.GetHashCode();
                            }
                        }
                        else
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_TODO_NAMENEEDED"), true, ThemeColorType.Error);
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
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_TODO_MARKFAILED") + " {0}", true, ThemeColorType.Error, ex.Message);
                                return ex.GetHashCode();
                            }
                        }
                        else
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_TODO_NAMENEEDED"), true, ThemeColorType.Error);
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
                        ListWriterColor.WriteList(tasks);
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
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_TODO_INVALIDACTION"), true, ThemeColorType.Error);
                        return 5;
                    }
            }
        }

    }
}
