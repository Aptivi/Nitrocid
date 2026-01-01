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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.BassBoom.Animations.Lyrics;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Nitrocid.Files;

namespace Nitrocid.Extras.BassBoom.Commands
{
    /// <summary>
    /// Lists all lyric lines
    /// </summary>
    /// <remarks>
    /// This command allows you to list all luric lines from a lyric file.
    /// </remarks>
    class LyricLinesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string pathToLyrics = parameters.ArgumentsList[0];

            // If there is no lyric file, bail.
            if (string.IsNullOrWhiteSpace(pathToLyrics) || !FilesystemTools.FileExists(pathToLyrics))
            {
                TextWriterColor.Write(Translate.DoTranslation("Make sure to specify the path to an LRC file."));
                return 27;
            }

            // Visualize it!
            var lines = Lyrics.GetLyricLines(pathToLyrics);
            foreach (var line in lines)
            {
                TextWriters.Write($"- [{line.LineSpan.Hours:00}:{line.LineSpan.Minutes:00}:{line.LineSpan.Seconds:00}.{line.LineSpan.Milliseconds:000}] ", false, KernelColorType.ListEntry);
                TextWriters.Write(line.Line, KernelColorType.ListValue);
            }
            return 0;
        }

    }
}
