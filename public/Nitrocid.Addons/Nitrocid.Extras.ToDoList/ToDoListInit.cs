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

using Terminaux.Shell.Arguments;
using Nitrocid.Extras.ToDoList.ToDoList;
using Nitrocid.Extras.ToDoList.Localized;
using Nitrocid.Extras.ToDoList.ToDoList.Commands;
using Nitrocid.Base.Kernel.Debugging;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Base.Kernel.Extensions;
using System.Linq;
using Nitrocid.Base.Languages;

namespace Nitrocid.Extras.ToDoList
{
    internal class ToDoListInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("todo", /* Localizable */ "NKS_TODO_COMMAND_TODO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "add/remove/done/undone", new CommandArgumentPartOptions()
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
                        new CommandArgumentPart(true, "list", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["list", "save", "load"],
                            ArgumentDescription = /* Localizable */ "NKS_TODO_COMMAND_TODO_ARGUMENT_LISTSAVELOAD_DESC"
                        }),
                    ]),
                ], new TodoCommand()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasToDoList);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasToDoList);

        public void FinalizeAddon()
        {
            // Initialize to-do tasks
            ToDoManager.LoadTasks();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded tasks.");
        }

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
        }
    }
}
