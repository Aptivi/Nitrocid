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

using System;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Nitrocid.Files;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Searches for a string in a specified file
    /// </summary>
    /// <remarks>
    /// Searching for strings in files is a common practice to find messages, unused messages, and hidden messages in files and executables, especially games. The command is found to make this practice much easier to access. It searches for a specified string in a specified file, and returns all matches.
    /// </remarks>
    class SearchWordCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string lookup = parameters.ArgumentsList[0];
            string fileName = parameters.ArgumentsList[1];

            try
            {
                var Matches = FilesystemTools.SearchFileForString(fileName, lookup);
                foreach (string Match in Matches)
                {
                    var matchColor = KernelColorTools.GetColor(KernelColorType.Success);
                    var normalColor = KernelColorTools.GetColor(KernelColorType.NeutralText);
                    string matchLine = Match;
                    string toReplaceWith = $"{matchColor.VTSequenceForeground}{lookup}{normalColor.VTSequenceForeground}";

                    // We want to avoid repetitions here
                    if (!matchLine.Contains(toReplaceWith))
                        matchLine = matchLine.Replace(lookup, toReplaceWith);
                    TextWriterColor.Write(matchLine);
                }
                return 0;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to search {0} for {1}", vars: [lookup, fileName]);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriters.Write(Translate.DoTranslation("Searching {0} for {1} failed.") + " {2}", true, KernelColorType.Error, parameters.ArgumentsList[0], parameters.ArgumentsList[1], ex.Message);
                return ex.GetHashCode();
            }
        }

    }
}
