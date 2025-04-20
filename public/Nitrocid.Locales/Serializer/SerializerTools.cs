//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Nitrocid.Locales.Serializer
{
    internal static class SerializerTools
    {
        internal static (TargetLanguage, string)[] SerializeTargetLanguages(TargetLanguage[] targetLanguages, string pathToTranslations = "")
        {
            // Check to see if the translations directory exists
            if (!Directory.Exists(pathToTranslations))
                throw new Exception("Translations folder doesn't exist. Make sure that it exists, and that it contains both the metadata file containing language information and the eng.txt file containing English localizations for each string.");

            // Determine several paths
            pathToTranslations = string.IsNullOrEmpty(pathToTranslations) ? Path.GetFullPath("Translations") : pathToTranslations;
            string englishFile = $"{pathToTranslations}/eng.txt";
            string metadataFile = $"{pathToTranslations}/Metadata.json";
            string outputFolder = $"{pathToTranslations}/Output";

            // Check to see if the required files exist
            if (!File.Exists(metadataFile))
                throw new Exception("Metadata file doesn't exist.");
            if (!File.Exists(englishFile))
                throw new Exception("English translations file doesn't exist.");
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            // Now, iterate through each target language to generate JSON files
            var fileLinesEng = File.ReadAllLines(englishFile);
            JArray metadatas = JArray.Parse(File.ReadAllText(metadataFile));
            var languageNames = metadatas.Select((token) => token.SelectToken("three")?.ToString());
            targetLanguages = targetLanguages.Where((tl) => languageNames.Contains(tl.LanguageName)).ToArray();
            var serializedTargets = new List<(TargetLanguage, string)>();
            for (int i = 0; i < metadatas.Count; i++)
            {
                // Initialize two arrays for localization
                JToken metadata = metadatas[i];
                var language = targetLanguages[i];
                string file = language.FileName;
                string fileName = language.LanguageName;
                var fileLines = File.ReadAllLines(file);

                // Make a JSON object for each language entry
                var localizedJson = new JObject();
                var localizationDataJson = new JArray();
                for (int l = 0; l <= fileLinesEng.Length - 1; l++)
                {
                    try
                    {
                        // First, check to see if the English file contains more text than your language's file
                        if (l >= fileLines.Length)
                            localizationDataJson.Add(fileLinesEng[l]);
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(fileLines[l]) & !string.IsNullOrWhiteSpace(fileLinesEng[l]))
                                localizationDataJson.Add(fileLines[l]);
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                // Fetch the metadata and put their values
                var languageName = metadata.SelectToken("name");
                var languageTransliterable = metadata.SelectToken("transliterable");
                localizedJson.Add("Name", languageName);
                localizedJson.Add("Transliterable", languageTransliterable);
                localizedJson.Add("Localizations", localizationDataJson);

                // Serialize the JSON object
                string serializedLocale = JsonConvert.SerializeObject(localizedJson, Formatting.Indented);
                serializedTargets.Add((language, serializedLocale));
            }
            return [.. serializedTargets];
        }

        internal static void SaveTargetLanguage(TargetLanguage target, string serializedLocale, string pathToTranslations = "", bool copyToResources = false, bool dry = false)
        {
            // Check to see if the translations directory exists
            if (!Directory.Exists(pathToTranslations))
                throw new Exception("Translations folder doesn't exist. Make sure that it exists, and that it contains both the metadata file containing language information and the eng.txt file containing English localizations for each string.");

            // Determine several paths
            pathToTranslations = string.IsNullOrEmpty(pathToTranslations) ? Path.GetFullPath("Translations") : pathToTranslations;
            string fileName = target.LanguageName;
            string outputFolder =
                !copyToResources ? $"{pathToTranslations}/Output" :
                fileName != "eng" ? Path.GetFullPath("../../../Nitrocid.Addons/Nitrocid.LanguagePacks/Resources/Languages") :
                Path.GetFullPath("../../Resources/Languages");

            // Check to see if the output folder exists
            if (!Directory.Exists(outputFolder) && !dry)
                Directory.CreateDirectory(outputFolder);

            // Save changes
            if (!dry)
                File.WriteAllText($"{outputFolder}/" + fileName + ".json", serializedLocale);
            else
                Debug.WriteLine($"Would be saved to: {outputFolder}/" + fileName + ".json");
        }

        internal static void SaveMetadata(TargetLanguage target, string pathToTranslations = "", bool copyToResources = false, bool dry = false)
        {
            // Check to see if the translations directory exists
            if (!Directory.Exists(pathToTranslations))
                throw new Exception("Translations folder doesn't exist. Make sure that it exists, and that it contains both the metadata file containing language information and the eng.txt file containing English localizations for each string.");

            // Determine several paths
            pathToTranslations = string.IsNullOrEmpty(pathToTranslations) ? Path.GetFullPath("Translations") : pathToTranslations;
            string metadataPath = pathToTranslations + "/Metadata.json";
            string fileName = target.LanguageName;
            string outputFolder =
                !copyToResources ? $"{pathToTranslations}/Output" :
                fileName != "eng" ? Path.GetFullPath("../../../Nitrocid.Addons/Nitrocid.LanguagePacks/Resources/Languages") :
                Path.GetFullPath("../../Resources/Languages");

            // Check to see if the metadata file exists
            if (!File.Exists(metadataPath))
                throw new Exception("Metadata file doesn't exist.");

            // Check to see if the output folder exists
            if (!Directory.Exists(outputFolder) && !dry)
                Directory.CreateDirectory(outputFolder);

            // Save metadata
            if (!dry)
            {
                File.Delete($"{outputFolder}/Metadata.json");
                File.Copy(metadataPath, $"{outputFolder}/Metadata.json");
            }
            else
                Debug.WriteLine($"Would be saved to: {outputFolder}/Metadata.json");
        }

        internal static TargetLanguage[] GetTargetLanguages(string pathToTranslations, string toSearch = "")
        {
            // Check to see if the translations directory exists
            if (!Directory.Exists(pathToTranslations))
                throw new Exception("Translations folder doesn't exist. Make sure that it exists, and that it contains both the metadata file containing language information and the eng.txt file containing English localizations for each string.");

            // Get all the language files
            bool custom = pathToTranslations != Path.GetFullPath("Translations") && pathToTranslations != Path.GetFullPath("AddonTranslations");
            var toParse = new List<TargetLanguage>();
            var files = Directory.GetFiles(pathToTranslations);

            // Add languages to parse list
            bool singular = !string.IsNullOrEmpty(toSearch);
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string fileExtension = Path.GetExtension(file);

                // Check the file and add if the localization file is a text file
                if (fileExtension == ".txt")
                {
                    if (!singular || singular && fileName.Equals(toSearch))
                    {
                        var LanguageInstance = new TargetLanguage(file, fileName, custom);
                        toParse.Add(LanguageInstance);
                    }
                }
            }
            return [.. toParse];
        }
    }
}
