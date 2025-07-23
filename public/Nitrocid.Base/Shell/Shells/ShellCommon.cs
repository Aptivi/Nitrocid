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

using Nitrocid.Base.Users.Groups;
using Terminaux.Colors.Themes;
using System.Linq;
using Nitrocid.Base.Security.Permissions;
using System.Collections.Generic;
using System;
using Nitrocid.Base.Users;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Shell.Arguments;
using System.Reflection;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Shell.Shells
{
    internal static class ShellCommon
    {
        private static readonly Dictionary<string, Func<string[], string[]>> completions = new()
        {
            { "user",       (_) => UserManagement.ListAllUsers().ToArray() },
            { "username",   (_) => UserManagement.ListAllUsers().ToArray() },
            { "group",      (_) => GroupManagement.AvailableGroups.Select((group) => group.GroupName).ToArray() },
            { "groupname",  (_) => GroupManagement.AvailableGroups.Select((group) => group.GroupName).ToArray() },
            { "splashname", (_) => SplashManager.GetNamesOfSplashes() },
            { "saver",      (_) => ScreensaverManager.GetScreensaverNames() },
            { "theme",      (_) => ThemeTools.GetInstalledThemes().Keys.ToArray() },
            { "perm",       (_) => Enum.GetNames<PermissionTypes>() },
        };

        internal static void RegisterCompletions()
        {
            // TODO: Publicize this on Terminaux 7.1 and replace below code.
            var completionsListType = typeof(CommandAutoCompletionList);
            var completionsField = completionsListType.GetField(nameof(completions), BindingFlags.Static | BindingFlags.NonPublic) ??
                throw new KernelException(KernelExceptionType.ShellOperation, "Can't get a list of completions");
            var completionsDict = (Dictionary<string, Func<string[], string[]>>?)completionsField.GetValue(null) ??
                throw new KernelException(KernelExceptionType.ShellOperation, "Can't get a list of completions");
            foreach (var completion in completions)
                completionsDict.Add(completion.Key, completion.Value);
        }

        internal static void UnregisterCompletions()
        {
            var completionsListType = typeof(CommandAutoCompletionList);
            var completionsField = completionsListType.GetField(nameof(completions), BindingFlags.Static | BindingFlags.NonPublic) ??
                throw new KernelException(KernelExceptionType.ShellOperation, "Can't get a list of completions");
            var completionsDict = (Dictionary<string, Func<string[], string[]>>?)completionsField.GetValue(null) ??
                throw new KernelException(KernelExceptionType.ShellOperation, "Can't get a list of completions");
            foreach (var completion in completions)
                completionsDict.Remove(completion.Key);
        }
    }
}
