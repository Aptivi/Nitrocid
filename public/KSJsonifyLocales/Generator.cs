
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

// Note: The new localizable strings during the development of the next version
//       of Kernel Simulator are found in the source code, which is:
//
//       Kernel Simulator Source -> Internal -> NewStrings.txt
//       which is found in: ProjectRoot/private/NewStrings.txt

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Misc.Writers.ConsoleWriters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KSJsonifyLocales
{
    static class LocaleGenerator
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] Args)
        {
            // Check console
            ConsoleChecker.CheckConsole();

            // Parse for arguments
            var arguments = new List<string>();
            var switches = new List<string>();
            bool custom = true;
            bool normal = true;
            var copyToResources = false;
            var singular = false;
            var quiet = false;
            string toSearch = "";
            if (Args.Length > 0)
            {
                // Separate between switches and arguments
                foreach (string Arg in Args)
                {
                    if (Arg.StartsWith("--"))
                    {
                        // It's a switch.
                        switches.Add(Arg);
                    }
                    else
                    {
                        // It's an argument.
                        arguments.Add(Arg);
                    }
                }

                // Change the values of custom and normal to match the switches provided
                custom = switches.Contains("--CustomOnly") | switches.Contains("--All");
                normal = switches.Contains("--NormalOnly") | switches.Contains("--All");
                copyToResources = switches.Contains("--CopyToResources");
                quiet = switches.Contains("--Quiet");

                // Check to see if we're going to parse one language
                singular = switches.Contains("--Singular");
                if (singular & arguments.Count > 0)
                {
                    // Select the language to be searched
                    toSearch = arguments[0];
                }
                else if (singular)
                {
                    // We can't be singular without providing the language!
                    Console.WriteLine("Provide a language to generate.");
                    Environment.Exit(1);
                }

                // Check to see if we're going to show help
                if (switches.Contains("--Help"))
                {
                    Console.WriteLine("{0} [--CustomOnly|--NormalOnly|--All|--Singular|--CopyToResources|--Help]", Path.GetFileName(Environment.GetCommandLineArgs()[0]));
                    Environment.Exit(1);
                }
            }

            var total = new Stopwatch();
            total.Start();
            try
            {
                // Enumerate the translations folder
                var toParse = new List<TargetLanguage>();
                string englishFile = Path.GetFullPath("Translations/eng.txt");
                var files = Directory.EnumerateFiles("Translations").ToArray();
                var customFiles = Directory.EnumerateFiles("CustomLanguages").ToArray();
                string metadataPath = Path.GetFullPath("Translations/Metadata.json");
                string customMetadataPath = Path.GetFullPath("CustomLanguages/Metadata.json");
                JToken metadata = JObject.Parse(File.ReadAllText(metadataPath));
                JToken customMetadata = JObject.Parse(File.ReadAllText(customMetadataPath));

                // Add languages to parse list
                if (normal)
                {
                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        string fileExtension = Path.GetExtension(file);

                        // Check the file and add if the localization file is a text file
                        if (fileExtension == ".txt")
                        {
                            if (!singular || (singular && fileName.Equals(toSearch)))
                            {
                                var LanguageInstance = new TargetLanguage(file, fileName, false);
                                Debug.WriteLine(file);
                                toParse.Add(LanguageInstance);
                            }
                        }
                    }
                }

                // Add the custom languages
                if (custom)
                {
                    foreach (string file in customFiles)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        string fileExtension = Path.GetExtension(file);

                        // Check the file and add if not a Readme and is a text file
                        if (fileExtension == ".txt" & !(fileName.ToLower() == "readme"))
                        {
                            if (!singular || (singular && fileName.Equals(toSearch)))
                            {
                                var LanguageInstance = new TargetLanguage(file, fileName, true);
                                Debug.WriteLine(file);
                                toParse.Add(LanguageInstance);
                            }
                        }
                    }
                }

                // Make a localized JSON file for target languages
                int fileNumber = 1;
                var fileLinesEng = File.ReadAllLines(englishFile);
                foreach (TargetLanguage language in toParse)
                {
                    // Initialize the stopwatch and the counter
                    var GenerationInterval = new Stopwatch();
                    GenerationInterval.Start();

                    // Initialize two arrays for localization
                    string file = language.FileName;
                    string fileName = language.LanguageName;
                    var fileLines = File.ReadAllLines(file);

                    // Show the generation message
                    Debug.WriteLine("Lines for {0} (Eng: {1}, Loc: {2})", fileName, fileLinesEng.Length, fileLines.Length);
                    if (!quiet)
                        TextWriterColor.Write($"[{fileNumber}/{toParse.Count}] " + "Generating locale JSON for " + $"{fileName}...", true, ColorTools.ColTypes.Progress);

                    // Make a JSON object for each language entry
                    var localizedJson = new JObject();
                    var localizationDataJson = new JObject();
                    for (int i = 0; i <= fileLines.Length - 1; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(fileLines[i]) & !string.IsNullOrWhiteSpace(fileLinesEng[i]))
                        {
                            try
                            {
                                Debug.WriteLine("Adding \"{0}, {1}\"...", fileLinesEng[i], fileLines[i]);
                                localizationDataJson.Add(fileLinesEng[i], fileLines[i]);
                            }
                            catch (Exception ex)
                            {
                                if (!quiet)
                                    TextWriterColor.Write($"[{fileNumber}/{toParse.Count}] " + "Malformed line" + $" {i + 1}: {fileLinesEng[i]} -> {fileLines[i]}", true, ColorTools.ColTypes.Error);
                                if (!quiet)
                                    TextWriterColor.Write($"[{fileNumber}/{toParse.Count}] " + "Error trying to parse above line:" + $" {ex.Message}", true, ColorTools.ColTypes.Error);
                            }
                        }
                    }

                    // Fetch the metadata and put their values
                    var languageMetadata = (language.CustomLanguage ? customMetadata : metadata).SelectToken(fileName);
                    var languageName = languageMetadata.SelectToken("name");
                    var languageTransliterable = languageMetadata.SelectToken("transliterable");
                    localizedJson.Add("Name", languageName);
                    localizedJson.Add("Transliterable", languageTransliterable);
                    localizedJson.Add("Localizations", localizationDataJson);

                    // Serialize the JSON object
                    string serializedLocale = JsonConvert.SerializeObject(localizedJson, Formatting.Indented);

                    // Save changes
                    Debug.WriteLine("Saving as {0}...", fileName + ".json");
                    if (language.CustomLanguage)
                    {
                        Directory.CreateDirectory(Paths.CustomLanguagesPath);
                        File.WriteAllText(Paths.CustomLanguagesPath + fileName + ".json", serializedLocale);
                    }
                    else if (copyToResources)
                    {
                        File.WriteAllText("../../Resources/Languages/" + fileName + ".json", serializedLocale);
                    }
                    else
                    {
                        Directory.CreateDirectory("Translations/Output");
                        File.WriteAllText("Translations/Output/" + fileName + ".json", serializedLocale);
                    }
                    if (!quiet)
                        TextWriterColor.Write($"[{fileNumber}/{toParse.Count}] " + "Saved new language JSON file to" + $" {fileName}.json!", true, ColorTools.ColTypes.Success);

                    // Show elapsed time and reset
                    if (!quiet)
                        TextWriterColor.Write($"[{fileNumber}/{toParse.Count}] " + "Time elapsed:" + $" {GenerationInterval.Elapsed}", true, ColorTools.ColTypes.StageTime);
                    fileNumber += 1;
                    GenerationInterval.Restart();
                }
            }
            catch (Exception ex)
            {
                if (!quiet)
                {
                    TextWriterColor.Write("Unexpected error in converter:" + $" {ex.Message}", true, ColorTools.ColTypes.Error);
                    TextWriterColor.Write(ex.StackTrace, true, ColorTools.ColTypes.Error);
                }
            }

            // Finish the program
            if (!quiet)
                TextWriterColor.Write("Finished in " + $"{total.Elapsed}");
            total.Reset();
        }
    }
}
