
//  Nitrocid KS  Copyright (C) 2018-2023  Aptivi
//
//  This file is part of Nitrocid KS
//
//  Nitrocid KS is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  Nitrocid KS is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KSCleanUnusedLocales
{
    internal class Cleaner
    {
        static void Main(string[] Args)
        {
            // If on dry mode, set as appropriate
            bool dry = Args.Contains("-dry");

            // Check to see if we're running on the KS repo
            string engFile = "../../../../../public/Nitrocid.LocaleGen/Translations/eng.txt";
            if (File.Exists(engFile))
            {
                Console.WriteLine("Probing English file...");

                // Get all the lines from the file
                string[] engStrings = File.ReadAllLines(engFile);

                // Iterate through all the source code files for the main project
                Console.WriteLine("Checking for unused strings...");
                List<string> sources = CodeLister.PopulateSources();
                List<string> dataSources = CodeLister.PopulateData();
                List<int> redundantIndexes = new();
                int lineNumber = 1;
                foreach (string engString in engStrings)
                {
                    bool found = false;

                    // Check the source content
                    foreach (string source in sources)
                    {
                        // Check to see if the string exists in the source
                        if (source.Contains($"DoTranslation(\"{engString.Replace("\"", "\\\"")}\"") ||
                            source.Contains($"/* Localizable */ \"{engString.Replace("\"", "\\\"")}\", "))
                        {
                            found = true;
                            break;
                        }
                    }

                    // Now, check the data sources if not found yet
                    foreach (string dataSource in dataSources)
                    {
                        // If found, bail
                        if (found)
                            break;

                        // Now, check to see if the string exists in the data
                        if (dataSource.Contains($"                \"Description\": \"{engString.Replace("\"", "\\\"")}\"") ||
                            dataSource.Contains($"                \"Name\": \"{engString.Replace("\"", "\\\"")}\"") ||
                            dataSource.Contains($"        \"DisplayAs\": \"{engString.Replace("\"", "\\\"")}\"") ||
                            dataSource.Contains($"        \"Desc\": \"{engString.Replace("\"", "\\\"")}\""))
                        {
                            found = true;
                            break;
                        }
                    }

                    // If still not found, add the line number index to the redundant string list
                    if (!found)
                    {
                        redundantIndexes.Add(lineNumber - 1);
                        Console.WriteLine("Unused string found at eng.txt line {0}: {1}", lineNumber, engString);
                    }
                    lineNumber++;
                }

                // Now, list all localization files
                if (redundantIndexes.Count > 0 && !dry)
                {
                    Console.WriteLine("Cleaning up...");
                    var langs = LocalizationLister.PopulateLanguages();
                    foreach (string localizationFile in langs.Keys)
                    {
                        // Delete all line numbers listed
                        for (int i = redundantIndexes.Count; i > 0; i--)
                        {
                            int redundantIndex = redundantIndexes[i - 1];
                            langs[localizationFile].RemoveAt(redundantIndex);
                        }

                        // Save the modified list to the file
                        File.WriteAllLines(localizationFile, langs[localizationFile]);
                    }
                }

                // Done!
                Console.WriteLine("Done! Please use Nitrocid.LocaleGen to finalize the change.");
            }
            else
                Console.WriteLine("This internal program needs to be run within the Nitrocid KS repository.");
        }
    }
}
