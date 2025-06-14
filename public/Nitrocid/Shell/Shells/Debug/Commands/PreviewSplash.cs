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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Splash;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
using System;

namespace Nitrocid.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// Previews the splash
    /// </summary>
    /// <remarks>
    /// This command previews either the current splash as set in the kernel settings or the specified splash. Refer the current splash list found in <see cref="SplashManager.Splashes"/>.
    /// </remarks>
    class PreviewSplashCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool splashOut = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-splashout");
            bool customContext = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-context");
            string contextName =
                customContext ?
                SwitchManager.GetSwitchValue(parameters.SwitchesList, "-context") :
                nameof(SplashContext.Showcase);
            SplashContext context = SplashContext.Showcase;
            bool contextValid =
                !customContext || Enum.TryParse(contextName, out context);
            if (!contextValid)
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_PREVIEWSPLASH_CONTEXTINVALID"), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Splash);
            }

            if (parameters.ArgumentsList.Length > 0)
                SplashManager.PreviewSplash(parameters.ArgumentsList[0], splashOut, context);
            else
                SplashManager.PreviewSplash(splashOut, context);
            return 0;
        }

        public override void HelpHelper()
        {
            var splashes = SplashManager.GetNamesOfSplashes();
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_PREVIEWSPLASH_HELPER"));
            TextWriters.WriteList(splashes);
        }

    }
}
