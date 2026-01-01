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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nitrocid.Locales.Serializer
{
    internal static class LocalizationLister
    {
        private static string[] ListLanguageFilesForKS()
        {
            // Check to see if we have the Nitrocid KS folder
            string ksJsonifyLocalesSource = "../../../Nitrocid.Locales/Translations";
            string ksJsonifyLocalesAddonSource = "../../../Nitrocid.Locales/AddonTranslations";
            List<string> translations = [];
            if (Directory.Exists(ksJsonifyLocalesSource))
            {
                // Iterate through all the source files for Nitrocid KS
                string[] files = Directory.GetFiles(ksJsonifyLocalesSource, "*.txt");
                translations.AddRange(files);
            }
            if (Directory.Exists(ksJsonifyLocalesAddonSource))
            {
                // Iterate through all the source files for Nitrocid KS addons
                string[] files = Directory.GetFiles(ksJsonifyLocalesAddonSource, "*.txt");
                translations.AddRange(files);
            }
            return [.. translations];
        }

        internal static Dictionary<string, List<string>> PopulateLanguages()
        {
            Dictionary<string, List<string>> sources = [];

            // List all code files to add the sources
            foreach (string source in ListLanguageFilesForKS())
                sources.Add(source, [.. File.ReadAllLines(source)]);

            return sources;
        }

        internal static LanguageMetadata[] PopulateLanguageMetadata()
        {
            string metadataFile = "../../../Nitrocid.Locales/Translations/Metadata.json";
            string addonMetadataFile = "../../../Nitrocid.Locales/AddonTranslations/Metadata.json";
            string metadata = File.ReadAllText(metadataFile);
            string addonMetadata = File.ReadAllText(addonMetadataFile);
            var languageMetadataToken = JsonConvert.DeserializeObject<LanguageMetadata[]>(metadata) ??
                throw new Exception("Failed to get language metadata");
            var languageAddonMetadataToken = JsonConvert.DeserializeObject<LanguageMetadata[]>(addonMetadata) ??
                throw new Exception("Failed to get addon language metadata");
            return languageMetadataToken.Union(languageAddonMetadataToken).ToArray();
        }
    }
}
