//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Kernel.Debugging;
using KS.Kernel.Extensions;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Nitrocid.Extras.ToDoList.ToDoList;
using Nitrocid.Extras.ToDoList.ToDoList.Commands;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.ToDoList
{
    internal class ToDoListInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "todo",
                new CommandInfo("todo", /* Localizable */ "To-do task manager",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "add"),
                            new CommandArgumentPart(true, "taskname"),
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "remove"),
                            new CommandArgumentPart(true, "taskname"),
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "done"),
                            new CommandArgumentPart(true, "taskname"),
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "undone"),
                            new CommandArgumentPart(true, "taskname"),
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "list"),
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "save"),
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "load"),
                        }),
                    }, new TodoCommand())
            },
        };

        string IAddon.AddonName => "Extras - To-do List";

        AddonType IAddon.AddonType => AddonType.Optional;

        void IAddon.FinalizeAddon()
        {
            // Initialize to-do tasks
            ToDoManager.LoadTasks();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded tasks.");
        }

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());
    }
}
