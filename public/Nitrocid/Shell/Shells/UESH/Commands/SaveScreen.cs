//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Misc.Screensaver;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Switches;
using System.Linq;

namespace KS.Shell.Shells.UESH.Commands
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
                // Get the names and build the choices
                var saverNames = ScreensaverManager.GetScreensaverNames();
                var saverNums = saverNames.Select((_, idx) => $"{idx + 1}").ToArray();
                var saverChoices = InputChoiceTools.GetInputChoices(saverNums, saverNames);
                var saverExit = new InputChoiceInfo[]
                {
                    new($"{saverNames.Length + 1}", Translate.DoTranslation("Exit"))
                };

                // Main loop
                while (true)
                {
                    // Prompt for screensaver selection
                    int selected = SelectionStyle.PromptSelection(Translate.DoTranslation("Select a screensaver to showcase"), [.. saverChoices], saverExit);
                    if (selected == -1 || selected == saverNames.Length + 1)
                        break;

                    // Now, showcase the selected screensaver
                    string name = saverNames[selected - 1];
                    ScreensaverManager.ShowSavers(name);
                    PressAndBailHelper();
                }
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
            TextWriterColor.Write(Translate.DoTranslation("Available screensavers:"));
            ListWriterColor.WriteList(ScreensaverManager.GetScreensaverNames());
        }

        private void PressAndBailHelper()
        {
            if (ScreensaverManager.inSaver)
            {
                Input.DetectKeypress();
                ScreensaverDisplayer.BailFromScreensaver();
            }
        }

    }
}
