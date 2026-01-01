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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
using Terminaux.Inputs.Interactive;
using Nitrocid.Misc.Interactives;
using System.Threading;
using Terminaux.Inputs.Pointer;
using System;
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;

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
            {
                var tui = new ScreensaverCli();
                tui.Bindings.Add(new InteractiveTuiBinding<string>(Translate.DoTranslation("Preview"), ConsoleKey.Enter, (saver, _, _, _) => tui.PressAndBailHelper(saver)));
                InteractiveTuiTools.OpenInteractiveTui(tui);
            }
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
            var screensavers = ScreensaverManager.GetScreensaverNames();
            TextWriterColor.Write(Translate.DoTranslation("Available screensavers:"));
            TextWriters.WriteList(screensavers);
        }

        private void PressAndBailHelper()
        {
            if (ScreensaverManager.inSaver)
            {
                SpinWait.SpinUntil(() => Input.InputAvailable);
                while (Input.InputAvailable)
                {
                    var descriptor = Input.ReadPointerOrKey();
                    if (descriptor.Item1 is not null)
                    {
                        switch (descriptor.Item1.Button)
                        {
                            case PointerButton.Left:
                            case PointerButton.Right:
                            case PointerButton.Middle:
                                if (descriptor.Item1.ButtonPress == PointerButtonPress.Clicked)
                                    Input.ReadPointer();
                                break;
                        }
                    }
                }
                ScreensaverDisplayer.BailFromScreensaver();
            }
        }

    }
}
