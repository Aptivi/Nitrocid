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

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Files.Operations.Querying;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using Nitrocid.Extras.BassBoom.Player;
using System;

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
                if (parameters.ArgumentsList.Length != 0)
                {
                    string musicPath = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);

                    // Check for existence.
                    if (string.IsNullOrEmpty(musicPath))
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Music file not specified."));
                        return 30;
                    }
                    if (!Checking.FileExists(musicPath))
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Music file '{0}' doesn't exist."), musicPath);
                        return 31;
                    }
                    if (!PlayerTui.musicFiles.Contains(musicPath))
                        PlayerTui.musicFiles.Add(musicPath);
                }

                // Now, open an interactive TUI
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
