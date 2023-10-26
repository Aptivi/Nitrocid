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
using KS.Files.Operations.Querying;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using Nitrocid.Extras.BassBoom.Animations.Lyrics;

namespace Nitrocid.Extras.BassBoom.Commands
{
    /// <summary>
    /// Plays a lyric file
    /// </summary>
    /// <remarks>
    /// This command allows you to play a lyric file by showing you the basic lyrics visualizer.
    /// </remarks>
    class PlayLyricCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string pathToLyrics = parameters.ArgumentsList[0];

            // If there is no lyric path, or if it doesn't exist, tell the user that they have to provide a path to the
            // lyrics folder.
            if (string.IsNullOrWhiteSpace(pathToLyrics) || !Checking.FileExists(pathToLyrics))
            {
                TextWriterColor.Write(Translate.DoTranslation("Make sure to specify the path to a directory containing your lyric files in the LRC format. You can also specify a custom path to your music library folder containing the lyric files."));
                return 17;
            }

            // Visualize it!
            Lyrics.VisualizeLyric(pathToLyrics);
            return 0;
        }

    }
}
