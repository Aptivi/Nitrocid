
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

namespace Nitrocid.LocaleClean
{
    internal class Cleaner
    {
        static int Main(string[] Args)
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
                var sources = CodeLister.PopulateSources();
                var dataSources = CodeLister.PopulateData();
                List<int> redundantIndexes = new();
                int lineNumber = 1;
                bool foundFalsePositives = false;
                foreach (string engString in engStrings)
                {
                    bool found = false;

                    // Check the source content
                    foreach ((string, string) sourceTuple in sources)
                    {
                        // Check to see if the string exists in the source
                        string source = sourceTuple.Item2;
                        if (source.Contains($"DoTranslation(\"{engString.Replace("\"", "\\\"")}\"") ||
                            source.Contains($"/* Localizable */ \"{engString.Replace("\"", "\\\"")}\""))
                        {
                            found = true;
                            break;
                        }
                    }

                    // Now, check the data sources if not found yet
                    foreach ((string, string) dataSourceTuple in dataSources)
                    {
                        // If found, bail
                        if (found)
                            break;

                        // Now, check to see if the string exists in the data
                        string dataSource = dataSourceTuple.Item2;
                        if (dataSource.Contains($"                \"Description\": \"{engString.Replace("\"", "\\\"")}\"") ||
                            dataSource.Contains($"                \"Name\": \"{engString.Replace("\"", "\\\"")}\"") ||
                            dataSource.Contains($"        \"DisplayAs\": \"{engString.Replace("\"", "\\\"")}\"") ||
                            dataSource.Contains($"        \"Description\": \"{engString.Replace("\"", "\\\"")}\"") ||
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

                        // Check to see if this detection is a false positive
                        bool falsePositive = false;
                        string falsePositiveSource = "";

                        // Check the source content
                        foreach ((string, string) sourceTuple in sources)
                        {
                            // Check to see if the string exists in the source
                            string source = sourceTuple.Item2;
                            if (source.Contains($"\"{engString.Replace("\"", "\\\"")}\""))
                            {
                                falsePositive = true;
                                falsePositiveSource = sourceTuple.Item1;
                                foundFalsePositives = true;
                                break;
                            }
                        }

                        // Now, check the data sources if not found yet
                        foreach ((string, string) dataSourceTuple in dataSources)
                        {
                            // If found, bail
                            if (falsePositive)
                                break;

                            // Now, check to see if the string exists in the data
                            string dataSource = dataSourceTuple.Item2;
                            if (dataSource.Contains($"\"{engString.Replace("\"", "\\\"")}\""))
                            {
                                falsePositive = true;
                                falsePositiveSource = dataSourceTuple.Item1;
                                foundFalsePositives = true;
                                break;
                            }
                        }

                        // Print possible false positive
                        if (falsePositive)
                            Console.WriteLine("  - Possible false positive in source {0} at eng.txt line {1}. Double-check the source.", falsePositiveSource, lineNumber);
                    }
                    lineNumber++;
                }

                // Now, list all localization files
                if (redundantIndexes.Count > 0 && !dry)
                {
                    if (foundFalsePositives)
                    {
                        Console.Write("Are you sure that you want to clear out unused strings and some of the used strings? [Y/N] ");
                        if (Console.ReadKey(true).Key != ConsoleKey.Y)
                        {
                            Console.WriteLine("\nCan't continue. Please double-check the false positive detection above.");
                            return 2;
                        }
                    }
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
                if (foundFalsePositives)
                    Console.WriteLine("WARNING: Cleared some of the used strings!");
                return 0;
            }
            else
            {
                Console.WriteLine("This internal program needs to be run within the Nitrocid KS repository.");
                return 1;
            }
        }
    }
}
