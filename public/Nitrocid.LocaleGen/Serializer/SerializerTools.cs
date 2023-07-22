
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

using KS.ConsoleBase.Colors;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Nitrocid.LocaleGen.Serializer
{
    internal static class SerializerTools
    {
        internal static (TargetLanguage, string)[] SerializeTargetLanguages(TargetLanguage[] targetLanguages, string pathToTranslations = "", bool quiet = false)
        {
            // Check to see if the translations directory exists
            if (!Checking.FolderExists(pathToTranslations))
                throw new KernelException(KernelExceptionType.LocaleGen, Translate.DoTranslation("Translations folder doesn't exist. Make sure that it exists, and that it contains both the metadata file containing language information and the eng.txt file containing English localizations for each string."));

            // Determine several paths
            pathToTranslations = string.IsNullOrEmpty(pathToTranslations) ? Path.GetFullPath("Translations") : pathToTranslations;
            string englishFile  = $"{pathToTranslations}/eng.txt";
            string metadataFile = $"{pathToTranslations}/Metadata.json";
            string outputFolder = $"{pathToTranslations}/Output";

            // Check to see if the required files exist
            if (!Checking.FileExists(metadataFile))
                throw new KernelException(KernelExceptionType.LocaleGen, Translate.DoTranslation("Metadata file doesn't exist."));
            if (!Checking.FileExists(englishFile))
                throw new KernelException(KernelExceptionType.LocaleGen, Translate.DoTranslation("English translations file doesn't exist."));
            if (!Checking.FolderExists(outputFolder))
                Making.MakeDirectory(outputFolder);

            // Now, iterate through each target language to generate JSON files
            var fileLinesEng = File.ReadAllLines(englishFile);
            JToken metadata = JObject.Parse(File.ReadAllText(metadataFile));
            var serializedTargets = new List<(TargetLanguage, string)>();
            var GenerationInterval = new Stopwatch();
            for (int fileIndex = 0; fileIndex < targetLanguages.Length; fileIndex++)
            {
                // Initialize two arrays for localization
                var language = targetLanguages[fileIndex];
                string file = language.FileName;
                string fileName = language.LanguageName;
                var fileLines = File.ReadAllLines(file);
                int fileNumber = fileIndex + 1;

                // Show the generation message
                if (!quiet)
                    TextWriterColor.Write($"[{fileNumber}/{targetLanguages.Length}] " + Translate.DoTranslation("Generating locale JSON for") + $" {fileName}...", true, KernelColorType.Progress);
                GenerationInterval.Start();

                // Make a JSON object for each language entry
                var localizedJson = new JObject();
                var localizationDataJson = new JObject();
                for (int i = 0; i <= fileLines.Length - 1; i++)
                {
                    if (!string.IsNullOrWhiteSpace(fileLines[i]) & !string.IsNullOrWhiteSpace(fileLinesEng[i]))
                    {
                        try
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Adding \"{0}, {1}\"...", fileLinesEng[i], fileLines[i]);
                            localizationDataJson.Add(fileLinesEng[i], fileLines[i]);
                        }
                        catch (Exception ex)
                        {
                            if (!quiet)
                            {
                                TextWriterColor.Write($"  - " + Translate.DoTranslation("Malformed line") + $" {i + 1}: {fileLinesEng[i]} -> {fileLines[i]}", true, KernelColorType.Error);
                                TextWriterColor.Write($"  - " + Translate.DoTranslation("Error trying to parse above line") + $": {ex.Message}", true, KernelColorType.Error);
                            }
                        }
                    }
                }

                // Fetch the metadata and put their values, but check the metadata first
                var languageMetadata = metadata.SelectToken(fileName) ??
                    throw new KernelException(KernelExceptionType.LocaleGen, Translate.DoTranslation("Language metadata is empty."));
                var languageName = languageMetadata.SelectToken("name");
                var languageTransliterable = languageMetadata.SelectToken("transliterable");
                localizedJson.Add("Name", languageName);
                localizedJson.Add("Transliterable", languageTransliterable);
                localizedJson.Add("Localizations", localizationDataJson);

                // Serialize the JSON object
                string serializedLocale = JsonConvert.SerializeObject(localizedJson, Formatting.Indented);
                serializedTargets.Add((language, serializedLocale));

                // Print the entire generation interval
                if (!quiet)
                    TextWriterColor.Write($"  - " + Translate.DoTranslation("Time elapsed:") + $" {GenerationInterval.Elapsed}", true, KernelColorType.StageTime);
                GenerationInterval.Restart();
            }
            return serializedTargets.ToArray();
        }

        internal static void SaveTargetLanguage(TargetLanguage target, string serializedLocale, string pathToTranslations = "", bool quiet = false, bool copyToResources = false)
        {
            // Check to see if the translations directory exists
            if (!Checking.FolderExists(pathToTranslations))
                throw new KernelException(KernelExceptionType.LocaleGen, Translate.DoTranslation("Translations folder doesn't exist. Make sure that it exists, and that it contains both the metadata file containing language information and the eng.txt file containing English localizations for each string."));

            // Determine several paths
            pathToTranslations = string.IsNullOrEmpty(pathToTranslations) ? Path.GetFullPath("Translations") : pathToTranslations;
            string outputFolder = copyToResources ? Path.GetFullPath("../../Resources/Languages") : $"{pathToTranslations}/Output";

            // Check to see if the output folder exists
            if (!Checking.FolderExists(outputFolder))
                Making.MakeDirectory(outputFolder);

            // Save changes
            string fileName = target.LanguageName;
            DebugWriter.WriteDebug(DebugLevel.I, "Saving as {0}...", fileName + ".json");
            if (!Generator.dry)
                File.WriteAllText($"{outputFolder}/" + fileName + ".json", serializedLocale);
            if (!quiet)
                TextWriterColor.Write("  - Saved new language JSON file to" + $" {outputFolder}/{fileName}.json!", true, KernelColorType.Success);
        }

        internal static TargetLanguage[] GetTargetLanguages(string pathToTranslations, string toSearch = "")
        {
            // Check to see if the translations directory exists
            if (!Checking.FolderExists(pathToTranslations))
                throw new KernelException(KernelExceptionType.LocaleGen, Translate.DoTranslation("Translations folder doesn't exist. Make sure that it exists, and that it contains both the metadata file containing language information and the eng.txt file containing English localizations for each string."));

            // Get all the language files
            bool custom = pathToTranslations == Path.GetFullPath("Translations");
            var toParse = new List<TargetLanguage>();
            var files = Directory.EnumerateFiles(pathToTranslations).ToArray();

            // Add languages to parse list
            bool singular = !string.IsNullOrEmpty(toSearch);
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string fileExtension = Path.GetExtension(file);

                // Check the file and add if the localization file is a text file
                if (fileExtension == ".txt")
                {
                    if (!singular || (singular && fileName.Equals(toSearch)))
                    {
                        var LanguageInstance = new TargetLanguage(file, fileName, custom);
                        toParse.Add(LanguageInstance);
                    }
                }
            }
            return toParse.ToArray();
        }
    }
}
