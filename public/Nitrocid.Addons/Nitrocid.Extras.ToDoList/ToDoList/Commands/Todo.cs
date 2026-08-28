//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Linq;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

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
        public override string Command =>
            "todo";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_TODO_COMMAND_TODO_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "mode", new CommandArgumentPartOptions()
                    {
                        ExactWording = ["add", "remove", "done", "undone"],
                        ArgumentDescription = /* Localizable */ "NKS_TODO_COMMAND_TODO_ARGUMENT_ACTION_DESC"
                    }),
                    new CommandArgumentPart(true, "taskname", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_TODO_COMMAND_TODO_ARGUMENT_TASKNAME_DESC"
                    }),
                ]),
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "mode", new CommandArgumentPartOptions()
                    {
                        ExactWording = ["list", "save", "load"],
                        ArgumentDescription = /* Localizable */ "NKS_TODO_COMMAND_TODO_ARGUMENT_LISTSAVELOAD_DESC"
                    }),
                ]),
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string Action = parameters.ArgumentsList[0];

            // Enumerate based on action
            var ActionArguments = parameters.ArgumentsList.Skip(1).ToArray();
            switch (Action)
            {
                case "add":
                    {
                        // User chose to add a task
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

                        return 0;
                    }
                case "remove":
                    {
                        // User chose to remove a task
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

                        return 0;
                    }
                case "done":
                    {
                        // User chose to mark a task as done
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

                        return 0;
                    }
                case "undone":
                    {
                        // User chose to mark a task as undone
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
