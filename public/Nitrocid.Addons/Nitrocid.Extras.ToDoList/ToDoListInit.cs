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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Extras.ToDoList.ToDoList;
using Nitrocid.Extras.ToDoList.Localized;
using Nitrocid.Extras.ToDoList.ToDoList.Commands;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;
using Nitrocid.Languages;

namespace Nitrocid.Extras.ToDoList
{
    internal class ToDoListInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("todo", "NKS_TODO_COMMAND_TODO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "add/remove/done/undone", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["add", "remove", "done", "undone"],
                            ArgumentDescription = "NKS_TODO_COMMAND_TODO_ARGUMENT_ACTION_DESC"
                        }),
                        new CommandArgumentPart(true, "taskname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_TODO_COMMAND_TODO_ARGUMENT_TASKNAME_DESC"
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "list", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["list", "save", "load"],
                            ArgumentDescription = "NKS_TODO_COMMAND_TODO_ARGUMENT_LISTSAVELOAD_DESC"
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

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.ToDoList", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.ToDoList");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
        }
    }
}
