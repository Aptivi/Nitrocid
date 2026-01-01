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

using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Extensions;
using System;

namespace Nitrocid.Languages
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
            if (LanguageManager.Languages.TryGetValue(lang, out LanguageInfo? langInfo))
                return DoTranslation(text, langInfo);
            else
            {
                // We might have this language from a mod
                DebugWriter.WriteDebug(DebugLevel.W, "\"{0}\" with string \"{1}\" isn't in language list. It might be a custom language in a mod.", vars: [lang]);
                try
                {
                    var modManagerType = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasMods, "Nitrocid.Extras.Mods.Modifications.ModManager");
                    string result = (string?)InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasMods, "GetLocalizedText", modManagerType, text, lang) ?? text;
                    DebugWriter.WriteDebug(DebugLevel.I, "Got \"{0}\".", vars: [result]);
                    return result;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Error trying to get string \"{0}\" from one of the mods: {1}", vars: [text, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return text;
                }
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
            if (lang.Strings.TryGetValue(text, out string? translated))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Translating string to {0}: {1}", vars: [langname, text]);
                return translated;
            }
            else
            {
                // We might have this language from a mod
                DebugWriter.WriteDebug(DebugLevel.W, "\"{0}\" with string \"{1}\" isn't in language list. It might be a custom language in a mod.", vars: [lang]);
                try
                {
                    var modManagerType = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasMods, "Nitrocid.Extras.Mods.Modifications.ModManager");
                    string result = (string?)InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasMods, "GetLocalizedText", modManagerType, text, lang.ThreeLetterLanguageName) ?? text;
                    DebugWriter.WriteDebug(DebugLevel.I, "Got \"{0}\".", vars: [result]);
                    return result;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Error trying to get string \"{0}\" from one of the mods: {1}", vars: [text, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return text;
                }
            }
        }

    }
}
