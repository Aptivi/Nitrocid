
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Themes;
using KS.Misc.Screensaver;
using KS.Misc.Splash;
using KS.Modifications;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Scripting;
using KS.Shell.ShellBase.Shells;
using KS.Users;
using KS.Users.Groups;
using KS.Users.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.Shell.ShellBase.Arguments
{
    /// <summary>
    /// The list of known command auto completion patterns
    /// </summary>
    public static class CommandAutoCompletionList
    {
        private static readonly Dictionary<string, Func<string[]>> completions = new()
        {
            { "user",       () => UserManagement.ListAllUsers().ToArray() },
            { "username",   () => UserManagement.ListAllUsers().ToArray() },
            { "group",      () => GroupManagement.AvailableGroups.Select((group) => group.GroupName).ToArray() },
            { "groupname",  () => GroupManagement.AvailableGroups.Select((group) => group.GroupName).ToArray() },
            { "modname",    () => ModManager.ListMods().Keys.ToArray() },
            { "splashname", () => SplashManager.Splashes.Keys.ToArray() },
            { "saver",      () => ScreensaverManager.Screensavers.Keys.ToArray() },
            { "theme",      () => ThemeTools.GetInstalledThemes().Keys.ToArray() },
            { "$variable",  () => UESHVariables.Variables.Keys.ToArray() },
            { "perm",             Enum.GetNames<PermissionTypes> },
            { "cmd",              PopulateCommands },
            { "command",          PopulateCommands },
        };

        /// <summary>
        /// Gets a completion function for a known expression
        /// </summary>
        /// <param name="expression">An expression to query</param>
        /// <returns>A function that points to the completion, or null if not found.</returns>
        public static Func<string[]> GetCompletionFunction(string expression)
        {
            expression = expression.ToLower();
            if (!completions.ContainsKey(expression))
                return null;
            return completions[expression];
        }

        private static string[] PopulateCommands()
        {
            var shellType = ShellStart.ShellStack[^1].ShellType;
            var ShellCommands = CommandManager.GetCommands(shellType);
            return ShellCommands.Keys.ToArray();
        }
    }
}
