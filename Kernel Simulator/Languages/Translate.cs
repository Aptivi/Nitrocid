
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

using System.Collections.Generic;
using KS.Misc.Writers.DebugWriters;
using Newtonsoft.Json.Linq;

namespace KS.Languages
{
    public static class Translate
    {

        /// <summary>
        /// Translates string into current kernel language.
        /// </summary>
        /// <param name="text">Any string that exists in Kernel Simulator's translation files</param>
        /// <returns>Translated string</returns>
        public static string DoTranslation(string text)
        {
            return DoTranslation(text, LanguageManager.CurrentLanguage);
        }

        /// <summary>
        /// Translates string into another language, or to English if the language wasn't specified or if it's invalid.
        /// </summary>
        /// <param name="text">Any string that exists in Kernel Simulator's translation files</param>
        /// <param name="lang">3 letter language</param>
        /// <returns>Translated string</returns>
        public static string DoTranslation(string text, string lang)
        {
            if (string.IsNullOrWhiteSpace(lang))
                lang = "eng";
            // Get language string and translate
            Dictionary<string, string> translatedString;

            // If the language is available and is not English, translate
            if (LanguageManager.Languages.ContainsKey(lang) & lang != "eng")
            {
                // Prepare dictionary
                translatedString = PrepareDict(lang);
                DebugWriter.Wdbg(DebugLevel.I, "Dictionary size: {0}", translatedString.Count);

                // Do translation
                if (translatedString.ContainsKey(text))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Translating string to {0}: {1}", lang, text);
                    return translatedString[text];
                }
                else // String wasn't found
                {
                    DebugWriter.Wdbg(DebugLevel.W, "No string found in langlist. Lang: {0}, String: {1}", lang, text);
                    text = "(( " + text + " ))";
                    return text;
                }
            }
            else if (LanguageManager.Languages.ContainsKey(lang) & lang == "eng") // If the language is available, but is English, don't translate
            {
                return text;
            }
            else // If the language is invalid
            {
                DebugWriter.Wdbg(DebugLevel.E, "{0} isn't in language list", lang);
                return text;
            }
        }

        /// <summary>
        /// Prepares the translation dictionary for a language
        /// </summary>
        /// <param name="lang">A specified language</param>
        /// <returns>A dictionary of English strings and translated strings</returns>
        public static Dictionary<string, string> PrepareDict(string lang)
        {
            var langStrings = new Dictionary<string, string>();

            // Move final translations to dictionary
            foreach (JProperty TranslatedProperty in LanguageManager.Languages[lang].LanguageResource.Properties())
                langStrings.Add(TranslatedProperty.Name, (string)TranslatedProperty.Value);
            return langStrings;
        }

    }
}