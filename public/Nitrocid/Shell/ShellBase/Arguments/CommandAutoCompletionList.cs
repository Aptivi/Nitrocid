//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Nitrocid.ConsoleBase.Themes;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Misc.Splash;
using Nitrocid.Modifications;
using Nitrocid.Security.Permissions;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Scripting;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Users;
using Nitrocid.Users.Groups;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Shell.ShellBase.Arguments
{
    /// <summary>
    /// The list of known command auto completion patterns
    /// </summary>
    public static class CommandAutoCompletionList
    {
        private static readonly Dictionary<string, Func<string[], string[]>> completions = new()
        {
            { "user",       (_) => UserManagement.ListAllUsers().ToArray() },
            { "username",   (_) => UserManagement.ListAllUsers().ToArray() },
            { "group",      (_) => GroupManagement.AvailableGroups.Select((group) => group.GroupName).ToArray() },
            { "groupname",  (_) => GroupManagement.AvailableGroups.Select((group) => group.GroupName).ToArray() },
            { "modname",    (_) => ModManager.ListMods().Keys.ToArray() },
            { "splashname", (_) => SplashManager.GetNamesOfSplashes() },
            { "saver",      (_) => ScreensaverManager.GetScreensaverNames() },
            { "theme",      (_) => ThemeTools.GetInstalledThemes().Keys.ToArray() },
            { "$variable",  (_) => UESHVariables.Variables.Keys.ToArray() },
            { "perm",       (_) => Enum.GetNames<PermissionTypes>() },
            { "cmd",        (_) => PopulateCommands() },
            { "command",    (_) => PopulateCommands() },
            { "shell",      (_) => ShellManager.AvailableShells.Keys.ToArray() },
        };

        /// <summary>
        /// Gets a completion function for a known expression
        /// </summary>
        /// <param name="expression">An expression to query</param>
        /// <returns>A function that points to the completion, or null if not found.</returns>
        public static Func<string[], string[]>? GetCompletionFunction(string expression)
        {
            expression = expression.ToLower();
            if (!completions.TryGetValue(expression, out Func<string[], string[]>? func))
                return null;
            return func;
        }

        private static string[] PopulateCommands()
        {
            var shellType = ShellManager.CurrentShellType;
            var ShellCommands = CommandManager.GetCommandNames(shellType);
            return ShellCommands;
        }
    }
}
