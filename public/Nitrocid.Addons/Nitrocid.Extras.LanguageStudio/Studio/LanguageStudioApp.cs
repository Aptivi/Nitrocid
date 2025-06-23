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

using Newtonsoft.Json.Linq;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs.Styles.Choice;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Files;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using System.Collections.Generic;
using System.Linq;
using Textify.General;
using Terminaux.Base;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles;
using System;
using Terminaux.Inputs.Styles.Infobox;

namespace Nitrocid.Extras.LanguageStudio.Studio
{
    static class LanguageStudioApp
    {
        public static void StartLanguageStudio(string pathToTranslations, bool useTui = false)
        {
            // Neutralize the translations path
            pathToTranslations = FilesystemTools.NeutralizePath(pathToTranslations);
            string initialManifestFile = $"{PathsManagement.ExecPath}/Translations/Metadata.json";
            string initialEnglishFile = $"{PathsManagement.ExecPath}/Translations/eng.txt";
            string manifestFile = $"{pathToTranslations}/Metadata.json";
            string englishFile = $"{pathToTranslations}/eng.txt";

            // Check the translations path and the two necessary files
            if (!FilesystemTools.FolderExists(pathToTranslations))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Path to translations, {0}, is going to be created", vars: [pathToTranslations]);
                FilesystemTools.MakeDirectory(pathToTranslations);
            }
            if (!FilesystemTools.FileExists(manifestFile))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Manifest file, {0}, is going to be copied", vars: [manifestFile]);
                FilesystemTools.CopyFileOrDir(initialManifestFile, manifestFile);
            }

            if (!FilesystemTools.FileExists(englishFile))
            {
                string answer = ChoiceStyle.PromptChoice(
                    LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_BASEFILENOTFOUND"), [("e", "Empty"), ("n", "Nitrocid's English strings")]
                ).ToLower();
                switch (answer)
                {
                    case "E":
                        // User chose Nitrocid's English strings
                        DebugWriter.WriteDebug(DebugLevel.I, "English strings, {0}, is going to be copied", vars: [englishFile]);
                        FilesystemTools.CopyFileOrDir(initialEnglishFile, englishFile);
                        break;
                    default:
                        // User chose to create a new one
                        DebugWriter.WriteDebug(DebugLevel.I, "Empty English strings file...");
                        FilesystemTools.MakeFile(englishFile, false);
                        break;
                }
            }

            // Check the provided languages
            string metadataStr = FilesystemTools.ReadContentsText(manifestFile);
            JArray metadata = JArray.Parse(metadataStr);
            string[] finalLangs = metadata
                .Select((token) => token["three"]?.ToString() ?? "")
                .Where(LanguageManager.Languages.ContainsKey)
                .ToArray();
            DebugWriter.WriteDebug(DebugLevel.I, "finalLangs = {0}.", vars: [finalLangs.Length]);
            if (finalLangs.Length == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "No languages!");
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_NOLANGSPECIFIED"), true, ThemeColorType.Error);
                return;
            }

            // Populate the English strings and fill the translated lines
            List<string> englishLines = [.. FilesystemTools.ReadContents(englishFile)];
            Dictionary<string, List<string>> translatedLines = [];
            foreach (string language in finalLangs)
            {
                // Populate the existing translations
                string languagePath = $"{pathToTranslations}/{language}.txt";
                List<string> finalLangLines = [];
                DebugWriter.WriteDebug(DebugLevel.I, "Language path is {0}", vars: [languagePath]);
                if (FilesystemTools.FileExists(languagePath))
                    finalLangLines.AddRange(FilesystemTools.ReadContents(languagePath));
                else
                    finalLangLines.AddRange(new string[englishLines.Count]);

                // If the size is smaller than the English lines, add empty entries. If the size is larger than the English lines,
                // remove extra entries.
                if (finalLangLines.Count < englishLines.Count)
                    finalLangLines.AddRange(new string[englishLines.Count - finalLangLines.Count]);
                else if (finalLangLines.Count > englishLines.Count)
                    finalLangLines.RemoveRange(englishLines.Count, finalLangLines.Count - englishLines.Count);

                // Now, add the translated lines
                DebugWriter.WriteDebug(DebugLevel.I, "Final lines {0}", vars: [finalLangLines.Count]);
                translatedLines.Add(language, finalLangLines);
            }

            // Check for TUI
            if (useTui)
            {
                var tui = new LanguageStudioCli()
                {
                    translatedLines = translatedLines,
                    pathToTranslations = pathToTranslations,
                    englishLines = englishLines,
                };
                new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_TUI_KEYBINDING_TRANSLATE"), ConsoleKey.Enter, (line, _, _, _) => tui.DoTranslate(line), true);
                new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_TUI_KEYBINDING_ADD"), ConsoleKey.A, (_, _, _, _) => tui.Add(), true);
                new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_TUI_KEYBINDING_REMOVE"), ConsoleKey.Delete, (_, idx, _, _) => tui.Remove(idx));
                new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_TUI_KEYBINDING_SAVE"), ConsoleKey.F1, (_, _, _, _) => tui.Save(), true);
                InteractiveTuiTools.OpenInteractiveTui(tui);
                return;
            }

            // Loop until exit is requested
            while (true)
            {
                // Populate the choices with English strings
                List<InputChoiceInfo> choices = [];
                for (int i = 0; i < englishLines.Count; i++)
                {
                    string englishLine = englishLines[i];
                    choices.Add(new InputChoiceInfo($"{i + 1}", englishLine));
                }

                // Now, show all strings to select, as well as several options
                string finalTitle = LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_HEADER");
                List<InputChoiceInfo> altChoices =
                [
                    new InputChoiceInfo($"{englishLines.Count + 1}", LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_NEWSTRING")),
                    new InputChoiceInfo($"{englishLines.Count + 2}", LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_REMOVESTRING")),
                    new InputChoiceInfo($"{englishLines.Count + 3}", LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_SAVE")),
                    new InputChoiceInfo($"{englishLines.Count + 4}", LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_EXIT")),
                ];
                List<InputChoiceInfo> altChoicesRemove =
                [
                    new InputChoiceInfo($"{englishLines.Count + 1}", LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_GOBACK")),
                ];
                int selectedStringNum = SelectionStyle.PromptSelection("\n  * " + finalTitle + " " + CharManager.NewLine + CharManager.NewLine + LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_SELECTSTRINGTRANSLATE"), [.. choices], [.. altChoices]);

                // Check the answer
                if (selectedStringNum == englishLines.Count + 1)
                {
                    // User chose to make a new string.
                    string newString = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_NEWSTRINGPROMPT") + ": ");
                    englishLines.Add(newString);
                    foreach (var translatedLang in translatedLines.Keys)
                        translatedLines[translatedLang].Add(translatedLang == "eng" ? newString : "???");
                }
                else if (selectedStringNum == englishLines.Count + 2)
                {
                    // User chose to remove a string.
                    finalTitle = LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_REMOVESTRING");
                    int selectedRemovedStringNum = SelectionStyle.PromptSelection("- " + finalTitle + " " + new string('-', ConsoleWrapper.WindowWidth - ("- " + finalTitle + " ").Length) + CharManager.NewLine + CharManager.NewLine + LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_SELECTSTRINGREMOVE"), [.. choices], [.. altChoicesRemove]);
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
                    // User chose to save the translations.
                    InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_SAVINGLANG"));
                    foreach (var translatedLine in translatedLines)
                    {
                        string language = translatedLine.Key;
                        List<string> localizations = translatedLine.Value;
                        string languagePath = $"{pathToTranslations}/{language}.txt";
                        FilesystemTools.WriteContents(languagePath, [.. localizations]);
                    }
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_SAVESUCCESS") + $" {pathToTranslations}");
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
                List<InputChoiceInfo> choices = [];
                for (int i = 0; i < targetLanguages.Length; i++)
                {
                    string language = targetLanguages[i];
                    choices.Add(new InputChoiceInfo($"{i + 1}", $"{language} [{translatedLines[language][index]}]"));
                }
                List<InputChoiceInfo> altChoices =
                [
                    new InputChoiceInfo($"{targetLanguages.Length + 1}", LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_GOBACK")),
                ];
                string finalTitle = LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_SELECTLANGTITLE");
                int selectedLangNum = SelectionStyle.PromptSelection("- " + finalTitle + " " + new string('-', ConsoleWrapper.WindowWidth - ("- " + finalTitle + " ").Length) + CharManager.NewLine + CharManager.NewLine + LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_SELECTLANGPROMPT"), [.. choices], [.. altChoices]);
                if (selectedLangNum == targetLanguages.Length + 1 || selectedLangNum == -1)
                    return;

                // Try to get a language and prompt the user for the translation
                string selectedLang = targetLanguages[selectedLangNum - 1];
                string translated = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_TRANSLATIONPROMPT") + $" \"{str}\": ");
                translatedLines[selectedLang][index] = translated;
            }
        }
    }
}
