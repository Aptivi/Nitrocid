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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Terminaux.Inputs.Interactive;
using Nitrocid.Base.Misc.Interactives;
using Terminaux.Inputs.Pointer;
using System;
using Terminaux.Inputs;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Screensaver;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
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
            bool selectionMode = parameters.ContainsSwitch("-select");
            bool randomMode = parameters.ContainsSwitch("-random");
            if (selectionMode)
            {
                var tui = new ScreensaverCli();
                tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_PREVIEW"), ConsoleKey.Enter, (saver, _, _, _) => tui.PressAndBailHelper(saver)));
                InteractiveTuiTools.OpenInteractiveTui(tui);
            }
            else
            {
                if (parameters.ArgumentsList.Length != 0)
                    ScreensaverManager.ShowSavers(parameters.ArgumentsList[0]);
                else if (randomMode)
                    ScreensaverManager.ShowSavers("random");
                else
                    ScreensaverManager.ShowSavers();
                PressAndBailHelper();
            }
            return 0;
        }

        public override void HelpHelper()
        {
            var screensavers = ScreensaverManager.GetScreensaverNames();
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SAVESCREEN_LISTING"));
            ListWriterColor.WriteList(screensavers);
        }

        private void PressAndBailHelper()
        {
            if (ScreensaverManager.inSaver)
            {
                InputEventInfo eventInfo = Input.ReadPointerOrKey();
                if (eventInfo.PointerEventContext is not null && eventInfo.PointerEventContext.ButtonPress == PointerButtonPress.Clicked)
                {
                    while (eventInfo.PointerEventContext is not null && eventInfo.PointerEventContext.ButtonPress != PointerButtonPress.Released)
                        eventInfo = Input.ReadPointerOrKey(InputEventType.Mouse);
                }
                ScreensaverDisplayer.BailFromScreensaver();
            }
        }

    }
}
