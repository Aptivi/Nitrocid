
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using KS.ConsoleBase.Colors;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Searches for a string in a specified file
    /// </summary>
    /// <remarks>
    /// Searching for strings in files is a common practice to find messages, unused messages, and hidden messages in files and executables, especially games. The command is found to make this practice much easier to access. It searches for a specified string in a specified file, and returns all matches.
    /// </remarks>
    class SearchWordCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string lookup = ListArgsOnly[0];
            string fileName = ListArgsOnly[1];

            try
            {
                var Matches = Searching.SearchFileForString(fileName, lookup);
                foreach (string Match in Matches)
                {
                    var matchColor = ColorTools.GetColor(KernelColorType.Success);
                    var normalColor = ColorTools.GetColor(KernelColorType.NeutralText);
                    string matchLine = Match;
                    string toReplaceWith = $"{matchColor.VTSequenceForeground}{lookup}{normalColor.VTSequenceForeground}";

                    // We want to avoid repetitions here
                    if (!matchLine.Contains(toReplaceWith))
                        matchLine = matchLine.Replace(lookup, toReplaceWith);
                    TextWriterColor.Write(matchLine);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to search {0} for {1}", lookup, fileName);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("Searching {0} for {1} failed.") + " {2}", true, KernelColorType.Error, ListArgsOnly[0], ListArgsOnly[1], ex.Message);
            }
        }

    }
}
