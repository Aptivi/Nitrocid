
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
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Nitrocid.LocaleGen.Serializer
{
    internal static class LanguageGenerator
    {
        internal static void GenerateLocaleFiles(string pathToTranslations, string toSearch = "", bool quiet = false, bool copyToResources = false)
        {
            var toParse = new List<(string, TargetLanguage[])>()
            {
                // Add languages to parse list
                (pathToTranslations, SerializerTools.GetTargetLanguages(pathToTranslations, toSearch))
            };

            // Make a localized JSON file for target languages
            var GenerationInterval = new Stopwatch();
            for (int i = 0; i < toParse.Count; i++)
            {
                try
                {
                    // Get the key value pair to process the language
                    (string, TargetLanguage[]) targetKeyValue = toParse[i];

                    // Initialize the stopwatch and the counter
                    GenerationInterval.Start();

                    // Now, generate and save
                    var serializedTargets = SerializerTools.SerializeTargetLanguages(targetKeyValue.Item2, targetKeyValue.Item1, quiet);
                    if (!quiet)
                        TextWriterColor.Write($"  - " + Translate.DoTranslation("Serialization time elapsed:") + $" {GenerationInterval.Elapsed}", true, KernelColorType.StageTime);
                    GenerationInterval.Restart();
                    foreach (var targetLanguage in serializedTargets)
                    {
                        var finalLang = targetLanguage.Item1;
                        var finalLocalization = targetLanguage.Item2;
                        SerializerTools.SaveTargetLanguage(finalLang, finalLocalization, targetKeyValue.Item1, quiet, copyToResources);
                    }

                    // Print the entire generation interval
                    if (!quiet)
                        TextWriterColor.Write($"  - " + Translate.DoTranslation("Export time elapsed:") + $" {GenerationInterval.Elapsed}", true, KernelColorType.StageTime);
                }
                catch (Exception ex)
                {
                    if (!quiet)
                        TextWriterColor.Write($"  - " + Translate.DoTranslation("Generation failed:") + $" {ex.Message}", true, KernelColorType.StageTime);
                }
                GenerationInterval.Restart();
            }
        }
    }
}
