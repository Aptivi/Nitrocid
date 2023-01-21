
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using KS.Kernel.Debugging;

namespace KS.Languages
{
    /// <summary>
    /// Translation module
    /// </summary>
    public static class Translate
    {

        /// <summary>
        /// Translates string into current kernel language.
        /// </summary>
        /// <param name="text">Any string that exists in Kernel Simulator's translation files</param>
        /// <returns>Translated string</returns>
        public static string DoTranslation(string text) =>
            DoTranslation(text, LanguageManager.CurrentLanguage);

        /// <summary>
        /// Translates string into another language, or to English if the language wasn't specified or if it's invalid.
        /// </summary>
        /// <param name="text">Any string that exists in Kernel Simulator's translation files</param>
        /// <param name="lang">3 letter language</param>
        /// <returns>Translated string</returns>
        public static string DoTranslation(string text, string lang)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            if (string.IsNullOrWhiteSpace(lang))
                lang = "eng";

            // If the language is available, translate
            if (LanguageManager.Languages.ContainsKey(lang))
            {
                if (lang != "eng")
                    // Language is not English. Translate.
                    return DoTranslation(text, LanguageManager.Languages[lang]);
                else
                    return text;
            }
            else // If the language is invalid
            {
                DebugWriter.WriteDebug(DebugLevel.E, "{0} isn't in language list", lang);
                return text;
            }
        }

        /// <summary>
        /// Translates string into another language, or to English if the language wasn't specified or if it's invalid.
        /// </summary>
        /// <param name="text">Any string that exists in Kernel Simulator's translation files</param>
        /// <param name="lang">Language info instance</param>
        /// <returns>Translated string</returns>
        public static string DoTranslation(string text, LanguageInfo lang)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            // Do translation
            if (lang.Strings.ContainsKey(text))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Translating string to {0}: {1}", lang.ThreeLetterLanguageName, text);
                return lang.Strings[text];
            }
            else if (lang.ThreeLetterLanguageName == "eng")
                return text;
            else
            {
                // String wasn't found
                DebugWriter.WriteDebug(DebugLevel.W, "No string found in langlist. Lang: {0}, String: {1}", lang.ThreeLetterLanguageName, text);
                text = "(( " + text + " ))";
                return text;
            }
        }

    }
}
