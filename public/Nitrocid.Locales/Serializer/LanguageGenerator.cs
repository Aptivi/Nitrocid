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
using System.Collections.Generic;

namespace Nitrocid.Locales.Serializer
{
    internal static class LanguageGenerator
    {
        internal static void GenerateLocaleFiles(string pathToTranslations, string toSearch = "", bool copyToResources = false, bool dry = false)
        {
            var toParse = new List<(string, TargetLanguage[])>()
            {
                // Add languages to parse list
                (pathToTranslations, SerializerTools.GetTargetLanguages(pathToTranslations, toSearch))
            };

            // Make a localized JSON file for target languages
            for (int i = 0; i < toParse.Count; i++)
            {
                try
                {
                    // Get the key value pair to process the language
                    (string, TargetLanguage[]) targetKeyValue = toParse[i];

                    // Now, generate and save
                    var serializedTargets = SerializerTools.SerializeTargetLanguages(targetKeyValue.Item2, targetKeyValue.Item1);
                    foreach (var targetLanguage in serializedTargets)
                    {
                        var finalLang = targetLanguage.Item1;
                        var finalLocalization = targetLanguage.Item2;
                        SerializerTools.SaveTargetLanguage(finalLang, finalLocalization, targetKeyValue.Item1, copyToResources, dry);
                        SerializerTools.SaveMetadata(finalLang, targetKeyValue.Item1, copyToResources, dry);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}
