//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Base.Files;
using Nitrocid.Base.Languages;
using Nitrocid.Extras.BassBoom.Animations.Lyrics;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;

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
        public override string Command => 
            "playlyric";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_BASSBOOM_COMMAND_PLAYLYRIC_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "lyric.lrc", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_BASSBOOM_COMMAND_ARGUMENT_LYRICLRC_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
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
