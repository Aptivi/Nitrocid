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

using Nitrocid.Files;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can check your file to see if it's locked
    /// </summary>
    /// <remarks>
    /// If you want to know that your file is locked, you can point this command to your file.
    /// </remarks>
    class ChkLockCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "chklock";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHKLOCK_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "file", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_FILE_DESC"
                    }),
                ],
                [
                    new SwitchInfo("waitforunlock", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHKLOCK_SWITCH_WAITFORUNLOCK_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string path = parameters.ArgumentsList[0];
            bool locked = FilesystemTools.IsLocked(path);
            bool waitForUnlock = parameters.ContainsSwitch("-waitforunlock");
            string waitForUnlockMsStr = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-waitforunlock");
            bool waitForUnlockTimed = !string.IsNullOrEmpty(waitForUnlockMsStr);
            int waitForUnlockMs = waitForUnlockTimed ? int.Parse(waitForUnlockMsStr) : 0;
            if (locked)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHKLOCK_LOCKED"), true, ThemeColorType.Warning);
                if (waitForUnlock)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHKLOCK_WAITING"), true, ThemeColorType.Progress);
                    if (waitForUnlockTimed)
                        FilesystemTools.WaitForLockRelease(path, waitForUnlockMs);
                    else
                        FilesystemTools.WaitForLockReleaseIndefinite(path);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHKLOCK_UNLOCKED"), true, ThemeColorType.Success);
                    return 0;
                }
                else
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHKLOCK_UNLOCKED"), true, ThemeColorType.Success);
            return 0;
        }
    }
}
