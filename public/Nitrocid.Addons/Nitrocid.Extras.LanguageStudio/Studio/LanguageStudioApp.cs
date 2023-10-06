
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

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Files.Operations;
using KS.Files.Operations.Querying;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using Newtonsoft.Json.Linq;
using Nitrocid.LocaleGen.Core.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.LanguageStudio.Studio
{
    static class LanguageStudioApp
    {
        public static void StartLanguageStudio(string pathToTranslations)
        {
            // Neutralize the translations path
            pathToTranslations = Filesystem.NeutralizePath(pathToTranslations);
            string initialManifestFile = $"{Paths.ExecPath}/Translations/Metadata.json";
            string initialEnglishFile = $"{Paths.ExecPath}/Translations/eng.txt";
            string manifestFile = $"{pathToTranslations}/Metadata.json";
            string englishFile = $"{pathToTranslations}/eng.txt";

            // Check the translations path and the two necessary files
            if (!Checking.FolderExists(pathToTranslations))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Path to translations, {0}, is going to be created", pathToTranslations);
                Making.MakeDirectory(pathToTranslations);
            }
            if (!Checking.FileExists(manifestFile))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Manifest file, {0}, is going to be copied", manifestFile);
                Copying.CopyFileOrDir(initialManifestFile, manifestFile);
            }

            if (!Checking.FileExists(englishFile))
            {
                string answer = ChoiceStyle.PromptChoice(Translate.DoTranslation("The base English strings file doesn't exist. Do you want to create an empty one? Or do you want to use Nitrocid's English strings?"), "E/N").ToLower();
                switch (answer)
                {
                    case "E":
                        // User chose Nitrocid's English strings
                        DebugWriter.WriteDebug(DebugLevel.I, "English strings, {0}, is going to be copied", englishFile);
                        Copying.CopyFileOrDir(initialEnglishFile, englishFile);
                        break;
                    default:
                        // User chose to create a new one
                        DebugWriter.WriteDebug(DebugLevel.I, "Empty English strings file...");
                        Making.MakeFile(englishFile, false);
                        break;
                }
            }

            // Check the provided languages
            JToken metadata = JObject.Parse(Reading.ReadContentsText(manifestFile));
            string[] finalLangs = metadata
                .Select((token) => token.Path)
                .Where(LanguageManager.Languages.ContainsKey)
                .ToArray();
            DebugWriter.WriteDebug(DebugLevel.I, "finalLangs = {0}.", finalLangs.Length);
            if (finalLangs.Length == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "No languages!");
                TextWriterColor.Write(Translate.DoTranslation("No valid language is specified."), true, KernelColorType.Error);
                return;
            }

            // Populate the English strings and fill the translated lines
            List<string> englishLines = Reading.ReadContents(englishFile).ToList();
            Dictionary<string, List<string>> translatedLines = new();
            foreach (string language in finalLangs)
            {
                // Populate the existing translations
                string languagePath = $"{pathToTranslations}/{language}.txt";
                List<string> finalLangLines = new();
                DebugWriter.WriteDebug(DebugLevel.I, "Language path is {0}", languagePath);
                if (Checking.FileExists(languagePath))
                    finalLangLines.AddRange(Reading.ReadContents(languagePath));
                else
                    finalLangLines.AddRange(new string[englishLines.Count]);

                // If the size is smaller than the English lines, add empty entries. If the size is larger than the English lines,
                // remove extra entries.
                if (finalLangLines.Count < englishLines.Count)
                    finalLangLines.AddRange(new string[englishLines.Count - finalLangLines.Count]);
                else if (finalLangLines.Count > englishLines.Count)
                    finalLangLines.RemoveRange(englishLines.Count, finalLangLines.Count - englishLines.Count);

                // Now, add the translated lines
                DebugWriter.WriteDebug(DebugLevel.I, "Final lines {0}", finalLangLines.Count);
                translatedLines.Add(language, finalLangLines);
            }

            // Loop until exit is requested
            while (true)
            {
                // Populate the choices with English strings
                List<InputChoiceInfo> choices = new();
                for (int i = 0; i < englishLines.Count; i++)
                {
                    string englishLine = englishLines[i];
                    choices.Add(new InputChoiceInfo($"{i + 1}", englishLine));
                }

                // Now, show all strings to select, as well as several options
                string finalTitle = Translate.DoTranslation("Welcome to the Language Studio!");
                List<InputChoiceInfo> altChoices = new()
                {
                    new InputChoiceInfo($"{englishLines.Count + 1}", Translate.DoTranslation("New string")),
                    new InputChoiceInfo($"{englishLines.Count + 2}", Translate.DoTranslation("Remove string")),
                    new InputChoiceInfo($"{englishLines.Count + 3}", Translate.DoTranslation("Save translations")),
                    new InputChoiceInfo($"{englishLines.Count + 4}", Translate.DoTranslation("Exit")),
                };
                List<InputChoiceInfo> altChoicesRemove = new()
                {
                    new InputChoiceInfo($"{englishLines.Count + 1}", Translate.DoTranslation("Go Back...")),
                };
                int selectedStringNum = SelectionStyle.PromptSelection("- " + finalTitle + " " + new string('-', ConsoleWrapper.WindowWidth - ("- " + finalTitle + " ").Length) + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation("Select a string to translate:"), choices, altChoices);

                // Check the answer
                if (selectedStringNum == englishLines.Count + 1)
                {
                    // User chose to make a new string.
                    string newString = Input.ReadLine(Translate.DoTranslation("Enter a new string") + ": ");
                    englishLines.Add(newString);
                    foreach (var translatedLang in translatedLines.Keys)
                        translatedLines[translatedLang].Add(translatedLang == "eng" ? newString : "???");
                }
                else if (selectedStringNum == englishLines.Count + 2)
                {
                    // User chose to remove a string.
                    finalTitle = Translate.DoTranslation("Remove string");
                    int selectedRemovedStringNum = SelectionStyle.PromptSelection("- " + finalTitle + " " + new string('-', ConsoleWrapper.WindowWidth - ("- " + finalTitle + " ").Length) + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation("Select a string to remove:"), choices, altChoicesRemove);
                    if (selectedRemovedStringNum == englishLines.Count + 1 || selectedRemovedStringNum == -1)
                        continue;
                    else
                    {
                        englishLines.RemoveAt(selectedRemovedStringNum - 1);
                        foreach (var translatedLang in translatedLines.Keys)
                            translatedLines[translatedLang].RemoveAt(selectedRemovedStringNum - 1);
                    }
                }
                else if (selectedStringNum == englishLines.Count + 3)
                {
                    // User chose to save the translations. Invoke LocaleGen for this.
                    TextWriterColor.Write(Translate.DoTranslation("Saving language..."));
                    foreach (var translatedLine in translatedLines)
                    {
                        string language = translatedLine.Key;
                        List<string> localizations = translatedLine.Value;
                        string languagePath = $"{pathToTranslations}/{language}.txt";
                        Writing.WriteContents(languagePath, localizations.ToArray());
                    }
                    LanguageGenerator.GenerateLocaleFiles(pathToTranslations);
                }
                else if (selectedStringNum == englishLines.Count + 4 || selectedStringNum == -1)
                {
                    // User chose to exit.
                    break;
                }
                else
                {
                    // User chose a string to translate.
                    HandleStringTranslation(englishLines, selectedStringNum - 1, finalLangs, ref translatedLines);
                }
            }
        }

        private static void HandleStringTranslation(List<string> strings, int index, string[] targetLanguages, ref Dictionary<string, List<string>> translatedLines)
        {
            // Get a string
            DebugCheck.Assert(!(index < 0 || index >= strings.Count), $"attempted to access English string out of range because index was {index} out of {strings.Count - 1}");
            string str = strings[index];

            while (true)
            {
                // Choose a language first
                List<InputChoiceInfo> choices = new();
                for (int i = 0; i < targetLanguages.Length; i++)
                {
                    string language = targetLanguages[i];
                    choices.Add(new InputChoiceInfo($"{i + 1}", $"{language} [{translatedLines[language][index]}]"));
                }
                List<InputChoiceInfo> altChoices = new()
                {
                    new InputChoiceInfo($"{targetLanguages.Length + 1}", Translate.DoTranslation("Go Back...")),
                };
                string finalTitle = Translate.DoTranslation("Select language");
                int selectedLangNum = SelectionStyle.PromptSelection("- " + finalTitle + " " + new string('-', ConsoleWrapper.WindowWidth - ("- " + finalTitle + " ").Length) + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation("Select a language to translate this string to:"), choices, altChoices);
                if (selectedLangNum == targetLanguages.Length + 1 || selectedLangNum == -1)
                    return;

                // Try to get a language and prompt the user for the translation
                string selectedLang = targetLanguages[selectedLangNum - 1];
                string translated = Input.ReadLine(Translate.DoTranslation("Write your translation of") + $" \"{str}\": ");
                translatedLines[selectedLang][index] = translated;
            }
        }
    }
}
