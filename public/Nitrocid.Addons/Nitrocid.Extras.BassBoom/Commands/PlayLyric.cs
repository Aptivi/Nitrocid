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
using Nitrocid.Extras.BassBoom.Animations.Lyrics;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Files;

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

            // If there is no lyric file, bail.
            if (string.IsNullOrWhiteSpace(pathToLyrics) || !FilesystemTools.FileExists(pathToLyrics))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_NOLYRICFILE"));
                return 17;
            }

            // Visualize it!
            Lyrics.VisualizeLyric(pathToLyrics);
            return 0;
        }

    }
}
