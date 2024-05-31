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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.BassBoom.Player;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Linq;

namespace Nitrocid.Extras.BassBoom.Commands
{
    /// <summary>
    /// Opens an interactive music player
    /// </summary>
    /// <remarks>
    /// This command allows you to play songs interactively.
    /// </remarks>
    class MusicPlayerCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            try
            {
                // First, prompt for the music path if no arguments are provided.
                bool isRadio = parameters.SwitchesList.Contains("-r");
                if (parameters.ArgumentsList.Length != 0)
                {
                    string musicPath = parameters.ArgumentsList[0];

                    // Check for existence.
                    if (string.IsNullOrEmpty(musicPath) || (!isRadio && !Checking.FileExists(musicPath)))
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Music file '{0}' doesn't exist."), musicPath);
                        return 31;
                    }
                    if (!isRadio)
                        PlayerTui.passedMusicPaths.Add(musicPath);
                }

                // Now, open an interactive TUI
                Common.exiting = false;
                if (isRadio)
                    Radio.RadioLoop();
                else
                    PlayerTui.PlayerLoop();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Can't start BassBoom Player: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("Fatal error in the BassBoom CLI addon.") + "\n\n" + ex.Message);
                return ex.HResult;
            }
            return 0;
        }

    }
}
