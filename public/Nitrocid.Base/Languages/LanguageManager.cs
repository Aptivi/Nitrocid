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

using System;
using System.Collections.Generic;
using Nitrocid.Base.Localized;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Users.Login;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Languages
{
    /// <summary>
    /// Lanaguage management module
    /// </summary>
    public static class LanguageManager
    {

        internal static Dictionary<string, LanguageInfo> baseLanguages = [];
        internal static LanguageInfo currentLanguage = Languages[Config.MainConfig.CurrentLanguage];
        internal static LanguageInfo currentUserLanguage = Languages[Config.MainConfig.CurrentLanguage];

        /// <summary>
        /// Current language
        /// </summary>
        public static LanguageInfo CurrentLanguageInfo =>
            Login.LoggedIn ? currentUserLanguage : currentLanguage;

        /// <summary>
        /// The installed languages list.
        /// </summary>
        public static Dictionary<string, LanguageInfo> Languages
        {
            get
            {
                if (baseLanguages.Count > 0)
                    return new(baseLanguages);

                // Open all localized strings list
                foreach (string lang in LocalStrings.Languages.Keys)
                {
                    Dictionary<string, string> strings = [];
                    foreach (string id in LocalStrings.Localizations)
                    {
                        if (LocalStrings.Exists(id, lang))
                        {
                            string localized = LocalStrings.Translate(id, lang);
                            strings.Add(id, localized);
                        }
                    }

                    // Get the language name
                    string langName = LocalStrings.Languages[lang];
                    DebugWriter.WriteDebug(DebugLevel.I, "Adding language to base languages. {0}, {1}", vars: [lang, langName]);
                    baseLanguages.Add(lang, new(lang, langName, strings));
                }

                // Return the list
                DebugWriter.WriteDebug(DebugLevel.I, "{0} installed languages in total", vars: [baseLanguages.Count]);
                return new(baseLanguages);
            }
        }

        /// <summary>
        /// Sets a system language temporarily
        /// </summary>
        /// <param name="lang">A specified language</param>
        /// <returns>True if successful, False if unsuccessful.</returns>
        public static bool SetLangDry(string lang)
        {
            // Settings app may have passed the language name with the country
            lang = lang.Contains(' ') ? lang.Split(' ')[0] : lang;
            if (Languages.TryGetValue(lang, out LanguageInfo? langInfo))
            {
                // Set current language
                try
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Translating kernel to {0}.", vars: [lang]);
                    currentLanguage = langInfo;
                    return true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Language can't be set. {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.NoSuchLanguage, LanguageTools.GetLocalized("NKS_LANGUAGES_EXCEPTION_INVALIDLANG") + " {0}", lang);
            }
            return false;
        }

        /// <summary>
        /// Sets a system language permanently
        /// </summary>
        /// <param name="lang">A specified language</param>
        /// <returns>True if successful, False if unsuccessful.</returns>
        public static bool SetLang(string lang)
        {
            SetLangDry(lang);
            Config.MainConfig.CurrentLanguage = lang;
            Config.CreateConfig();
            return true;
        }

        /// <summary>
        /// Lists all languages
        /// </summary>
        public static Dictionary<string, LanguageInfo> ListAllLanguages() =>
            ListLanguages("");

        /// <summary>
        /// Lists the languages
        /// </summary>
        /// <param name="SearchTerm">Search term</param>
        public static Dictionary<string, LanguageInfo> ListLanguages(string SearchTerm)
        {
            var ListedLanguages = new Dictionary<string, LanguageInfo>();

            // List the Languages using the search term
            foreach (var Language in Languages)
            {
                string LanguageName = Language.Key;
                if (LanguageName.Contains(SearchTerm))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Adding language {0} to list... Search term: {1}", vars: [LanguageName, SearchTerm]);
                    ListedLanguages.Add(LanguageName, Language.Value);
                }
            }
            return ListedLanguages;
        }
    }
}
