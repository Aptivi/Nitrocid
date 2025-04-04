﻿//
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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Extras.ToDoList.ToDoList;
using Nitrocid.Extras.ToDoList.ToDoList.Commands;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;

namespace Nitrocid.Extras.ToDoList
{
    internal class ToDoListInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("todo", /* Localizable */ "To-do task manager",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "add/remove/done/undone", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["add", "remove", "done", "undone"],
                            ArgumentDescription = /* Localizable */ "Whether to add, remove, mark as done, or unmark as done on a task"
                        }),
                        new CommandArgumentPart(true, "taskname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Task name"
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "list", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["list", "save", "load"],
                            ArgumentDescription = /* Localizable */ "Whether to list, save, or load all tasks"
                        }),
                    ]),
                ], new TodoCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasToDoList);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        void IAddon.FinalizeAddon()
        {
            // Initialize to-do tasks
            ToDoManager.LoadTasks();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded tasks.");
        }

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
    }
}
