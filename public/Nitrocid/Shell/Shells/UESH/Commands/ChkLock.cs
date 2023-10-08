
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Switches;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can check your file to see if it's locked
    /// </summary>
    /// <remarks>
    /// If you want to know that your file is locked, you can point this command to your file.
    /// </remarks>
    class ChkLockCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string path = parameters.ArgumentsList[0];
            bool locked = Filesystem.IsLocked(path);
            bool waitForUnlock = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-waitforunlock");
            string waitForUnlockMsStr = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-waitforunlock");
            bool waitForUnlockTimed = !string.IsNullOrEmpty(waitForUnlockMsStr);
            int waitForUnlockMs = waitForUnlockTimed ? int.Parse(waitForUnlockMsStr) : 0;
            if (locked)
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("File or folder is already in use."), true, KernelColorType.Warning);
                if (waitForUnlock)
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Waiting until the file or the folder is unlocked..."), true, KernelColorType.Progress);
                    if (waitForUnlockTimed)
                        Filesystem.WaitForLockRelease(path, waitForUnlockMs);
                    else
                        Filesystem.WaitForLockReleaseIndefinite(path);
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("File or folder is not in use."), true, KernelColorType.Success);
                    return 0;
                }
                else
                    return 10000 + (int)KernelExceptionType.Filesystem;
            }
            else
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("File or folder is not in use."), true, KernelColorType.Success);
            return 0;
        }
    }
}
