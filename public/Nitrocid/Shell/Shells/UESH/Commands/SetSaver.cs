
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
using KS.Files.Querying;
using KS.Kernel.Configuration;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Screensaver;
using KS.Misc.Screensaver.Customized;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Sets your default screensaver as your own screensaver or built-in savers
    /// </summary>
    /// <remarks>
    /// You can set your default screensaver as your own screensaver by the modfile or built-in savers such as matrix, disco, colorMix, and so on, whose names found in <see cref="ScreensaverManager.Screensavers"/> can be used as the argument for this command.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class SetSaverCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            string modPath = Paths.GetKernelPath(KernelPathType.Mods);
            StringArgs = StringArgs.ToLower();
            if (ScreensaverManager.Screensavers.ContainsKey(StringArgs) | CustomSaverTools.CustomSavers.ContainsKey(StringArgs))
            {
                ScreensaverManager.SetDefaultScreensaver(StringArgs);
                TextWriterColor.Write(Translate.DoTranslation("{0} is set to default screensaver."), StringArgs);
                return 0;
            }
            else if (Checking.FileExists($"{modPath}{StringArgs}") & !KernelFlags.SafeMode)
            {
                ScreensaverManager.SetDefaultScreensaver(StringArgs);
                TextWriterColor.Write(Translate.DoTranslation("{0} is set to default screensaver."), StringArgs);
                return 0;
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Screensaver {0} not found."), true, KernelColorType.Error, StringArgs);
                return 10000 + (int)KernelExceptionType.NoSuchScreensaver;
            }
        }

        public override void HelpHelper()
        {
            if (CustomSaverTools.CustomSavers.Count > 0)
            {
                TextWriterColor.Write(Translate.DoTranslation("where customsaver will be") + " {0}", string.Join(", ", CustomSaverTools.CustomSavers.Keys));
            }
            TextWriterColor.Write(Translate.DoTranslation("where builtinsaver will be") + " {0}", string.Join(", ", ScreensaverManager.Screensavers.Keys));
        }

    }
}
