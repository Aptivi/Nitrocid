
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

using KS.Kernel.Debugging;
using KS.Modifications;

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
        /// <param name="text">Any string that exists in Nitrocid KS's translation files</param>
        /// <returns>Translated string</returns>
        public static string DoTranslation(string text) =>
            DoTranslation(text, LanguageManager.CurrentLanguageInfo);

        /// <summary>
        /// Translates string into another language, or to English if the language wasn't specified or if it's invalid.
        /// </summary>
        /// <param name="text">Any string that exists in Nitrocid KS's translation files</param>
        /// <param name="lang">3 letter language</param>
        /// <returns>Translated string</returns>
        public static string DoTranslation(string text, string lang)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            if (string.IsNullOrWhiteSpace(lang))
                lang = "eng";

            if (lang == "eng")
                return text;

            // If the language is available, translate
            if (LanguageManager.Languages.ContainsKey(lang))
            {
                return DoTranslation(text, LanguageManager.Languages[lang]);
            }
            else
            {
                // We might have this string from a mod
                foreach (ModInfo mod in ModManager.ListMods().Values)
                {
                    if (mod.ModStrings.ContainsKey(lang) && mod.ModStrings[lang].ContainsKey(text))
                        return mod.ModStrings[lang][text];
                }

                // String wasn't found
                DebugWriter.WriteDebug(DebugLevel.E, "{0} isn't in language list", lang);
                return text;
            }
        }

        /// <summary>
        /// Translates string into another language, or to English if the language wasn't specified or if it's invalid.
        /// </summary>
        /// <param name="text">Any string that exists in Nitrocid KS's translation files</param>
        /// <param name="lang">Language info instance</param>
        /// <returns>Translated string</returns>
        public static string DoTranslation(string text, LanguageInfo lang)
        {
            // Some sanity checks
            if (string.IsNullOrWhiteSpace(text))
                return "";
            if (lang is null)
                return text;

            // Check to see if we're interacting with the English text
            string langname = lang.ThreeLetterLanguageName;
            if (langname == "eng")
                return text;

            // Do translation
            if (lang.Strings.ContainsKey(text))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Translating string to {0}: {1}", langname, text);
                return lang.Strings[text];
            }
            else
            {
                // We might have this string from a mod
                foreach (ModInfo mod in ModManager.ListMods().Values)
                {
                    if (mod.ModStrings.ContainsKey(langname) && mod.ModStrings[langname].ContainsKey(text))
                        return mod.ModStrings[langname][text];
                }

                // String wasn't found
                DebugWriter.WriteDebug(DebugLevel.W, "No string found in langlist. Lang: {0}, String: {1}", langname, text);
                return text;
            }
        }

    }
}
