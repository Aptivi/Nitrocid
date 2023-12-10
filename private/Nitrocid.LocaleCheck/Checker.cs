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

using Nitrocid.LocaleTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.LocaleCheck
{
    internal class Checker
    {
        static int Main()
        {
            // Check to see if we're running on the KS repo
            string engFile = "../../../../../public/Nitrocid.LocaleGen/Translations/eng.txt";
            if (File.Exists(engFile))
            {
                TextWriterColor.Write("Probing English file...");

                // Get all the lines from the file
                string[] engStrings = File.ReadAllLines(engFile);

                // Iterate through all the source code files for the main project
                TextWriterColor.Write("Checking for unlocalized strings...");
                var sources = CodeLister.PopulateSources();
                var dataSources = CodeLister.PopulateData();
                List<string> unlocalizedStrings = [];

                // Check the source content
                foreach ((string, string) sourceTuple in sources)
                {
                    // Check to see if the string is not localized yet
                    string source = sourceTuple.Item2;
                    string[] sourceLines = source
                        .Replace(Convert.ToChar(13).ToString(), "")
                        .Split(Convert.ToChar(10));
                    foreach (string sourceLine in sourceLines)
                    {
                        if (sourceLine.Contains($"DoTranslation(\"") || sourceLine.Contains($"/* Localizable */ \""))
                        {
                            // Possible translatable string found. Get the string.
                            string stringPattern =
                                 /* lang=regex */ @"/\* Localizable \*/ (""(?'string'[^()]*)"")|" +
                                 /* lang=regex */ @"DoTranslation\((""(?'string'[^()]*)"")";
                            var strMatches = Regex.Matches(sourceLine, stringPattern);
                            foreach (Match strMatch in strMatches.Cast<Match>())
                            {
                                string str = strMatch.Groups["string"].Value.Replace("\"", "\\\"");

                                // Check to see if the string is found in the eng.txt file
                                if (!engStrings.Contains(str) && !unlocalizedStrings.Contains(str) && !string.IsNullOrEmpty(str))
                                    unlocalizedStrings.Add(str);
                            }
                        }
                    }
                }

                // Now, check the data sources if not found yet
                foreach ((string, string) dataSourceTuple in dataSources)
                {
                    // Check to see if the string is not localized yet
                    string source = dataSourceTuple.Item2;
                    string[] sourceLines = source
                        .Replace(Convert.ToChar(13).ToString(), "")
                        .Split(Convert.ToChar(10));
                    foreach (string sourceLine in sourceLines)
                    {
                        if (source.Contains($"                \"Description\": \"") ||
                            source.Contains($"                \"Name\": \"") ||
                            source.Contains($"        \"DisplayAs\": \"") ||
                            source.Contains($"        \"Description\": \"") ||
                            source.Contains($"        \"Desc\": \""))
                        {
                            // Possible translatable string found. Get the string.
                            string stringPattern =
                                 /* lang=regex */ @"                ""Description"": (""(?'string'[^()]*)"")|" +
                                 /* lang=regex */ @"                ""Name"": (""(?'string'[^()]*)"")|" +
                                 /* lang=regex */ @"        ""DisplayAs"": (""(?'string'[^()]*)"")|" +
                                 /* lang=regex */ @"        ""Description"": (""(?'string'[^()]*)"")|" +
                                 /* lang=regex */ @"        ""Desc"": (""(?'string'[^()]*)"")|";
                            var strMatches = Regex.Matches(sourceLine, stringPattern);
                            foreach (Match strMatch in strMatches.Cast<Match>())
                            {
                                string str = strMatch.Groups["string"].Value.Replace("\"", "\\\"");

                                // Check to see if the string is found in the eng.txt file
                                if (!engStrings.Contains(str) && !unlocalizedStrings.Contains(str) && !string.IsNullOrEmpty(str))
                                    unlocalizedStrings.Add(str);
                            }
                        }
                    }
                }

                // Print unlocalized strings
                foreach (string unlocalizedString in unlocalizedStrings)
                {
                    TextWriterColor.Write($"Unlocalized string: {unlocalizedString}", true, ConsoleColors.Yellow);
                    File.AppendAllText("unlocalized.txt", $"{unlocalizedString}\n");
                }
                return 0;
            }
            else
            {
                TextWriterColor.Write("This internal program needs to be run within the Nitrocid KS repository.", true, ConsoleColors.Red);
                return 1;
            }
        }
    }
}
