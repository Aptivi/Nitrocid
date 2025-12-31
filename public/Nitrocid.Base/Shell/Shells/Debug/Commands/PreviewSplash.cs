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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using System;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Shell.Shells.Debug.Commands
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
            bool splashOut = parameters.ContainsSwitch("-splashout");
            bool customContext = parameters.ContainsSwitch("-context");
            string contextName =
                customContext ?
                SwitchManager.GetSwitchValue(parameters.SwitchesList, "-context") :
                nameof(SplashContext.Showcase);
            SplashContext context = SplashContext.Showcase;
            bool contextValid =
                !customContext || Enum.TryParse(contextName, out context);
            if (!contextValid)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_PREVIEWSPLASH_CONTEXTINVALID"), true, ThemeColorType.Error);
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
            ListWriterColor.WriteList(splashes);
        }

    }
}
