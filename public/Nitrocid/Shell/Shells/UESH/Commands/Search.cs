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

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Nitrocid.Files;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Terminaux.Base.Extensions;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Searches for a string in a specified file
    /// </summary>
    /// <remarks>
    /// Searching for strings in files is a common practice to find messages, unused messages, and hidden messages in files and executables, especially games. The command is found to make this practice much easier to access. It searches for a specified string in a specified file, and returns all matches. This command uses regular expressions.
    /// </remarks>
    class SearchCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "search";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SEARCH_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "regexp", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SEARCH_ARGUMENT_REGEXP_DESC"
                    }),
                    new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SEARCH_ARGUMENT_FILE_DESC"
                    }),
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string regexPattern = parameters.ArgumentsList[0];
            string file = parameters.ArgumentsList[1];
            try
            {
                var matches = FilesystemTools.SearchFileForStringRegexpMatches(file, new Regex(regexPattern, RegexOptions.IgnoreCase));
                foreach ((string, MatchCollection) matchTuple in matches)
                {
                    string matchLine = matchTuple.Item1;
                    var matchCollection = matchTuple.Item2;

                    // Iterate through each match collection to get their values so that we can replace the text with the text that
                    // contains VT sequences to colorize the matches.
                    var matchColor = ThemeColorsTools.GetColor(ThemeColorType.Success);
                    var normalColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
                    foreach (Match match in matchCollection.Cast<Match>())
                    {
                        string toReplaceWith = $"{matchColor.VTSequenceForeground()}{match.Value}{normalColor.VTSequenceForeground()}";

                        // We want to avoid repetitions here
                        if (!matchLine.Contains(toReplaceWith))
                            matchLine = matchLine.Replace(match.Value, toReplaceWith);
                    }
                    TextWriterColor.Write(matchLine);
                }
                return 0;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to search {0} for {1}", vars: [regexPattern, file]);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SEARCH_FAILED") + " {2}", true, ThemeColorType.Error, regexPattern, file, ex.Message);
                return ex.GetHashCode();
            }
        }

    }
}
