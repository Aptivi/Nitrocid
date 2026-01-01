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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Shell.ShellBase.Commands;
using System.Linq;

namespace Nitrocid.Shell.Shells.UESH.Commands
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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string modPath = PathsManagement.GetKernelPath(KernelPathType.Mods);
            string finalSaverName = parameters.ArgumentsText.ToLower();
            if (ScreensaverManager.GetScreensaverNames().Contains(finalSaverName))
            {
                ScreensaverManager.SetDefaultScreensaver(finalSaverName);
                TextWriterColor.Write(Translate.DoTranslation("{0} is set to default screensaver."), finalSaverName);
                return 0;
            }
            else if (Checking.FileExists($"{modPath}{finalSaverName}") & !KernelEntry.SafeMode)
            {
                ScreensaverManager.SetDefaultScreensaver(finalSaverName);
                TextWriterColor.Write(Translate.DoTranslation("{0} is set to default screensaver."), finalSaverName);
                return 0;
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Screensaver {0} not found."), true, KernelColorType.Error, finalSaverName);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.NoSuchScreensaver);
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("where saver will be") + " {0}", string.Join(", ", ScreensaverManager.GetScreensaverNames()));
        }

    }
}
