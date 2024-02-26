//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
using System.Linq;
using Terminaux.Inputs;
using Terminaux.Reader;
using Terminaux.Inputs.Interactive;
using Nitrocid.Misc.Interactives;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Launches your current screen saver
    /// </summary>
    /// <remarks>
    /// This command can protect your LCD screen from burn-in and shows you the current screensaver that is set by you or by the kernel. However it doesn't lock the user account, so we recommend to lock your screen for any purposes, unless you're testing your own screensaver from the screensaver modfile.
    /// </remarks>
    class SaveScreenCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool selectionMode = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-select");
            if (selectionMode)
                InteractiveTuiTools.OpenInteractiveTui(new ScreensaverTui());
            else
            {
                if (parameters.ArgumentsList.Length != 0)
                    ScreensaverManager.ShowSavers(parameters.ArgumentsList[0]);
                else
                    ScreensaverManager.ShowSavers();
                PressAndBailHelper();
            }
            return 0;
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("Available screensavers:"));
            ListWriterColor.WriteList(ScreensaverManager.GetScreensaverNames());
        }

        private void PressAndBailHelper()
        {
            if (ScreensaverManager.inSaver)
            {
                TermReader.ReadKey();
                ScreensaverDisplayer.BailFromScreensaver();
            }
        }

    }
}
