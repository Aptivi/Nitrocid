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

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;

namespace Nitrocid.Languages
{
    /// <summary>
    /// Culture management module
    /// </summary>
    public static class CultureManager
    {

        /// <summary>
        /// Current culture
        /// </summary>
        public static CultureInfo CurrentCult =>
            new(Config.MainConfig.CurrentCultStr);

        /// <summary>
        /// Updates current culture based on current language. If there are no cultures in the current language, assume current culture.
        /// </summary>
        public static void UpdateCultureDry()
        {
            var cultures = GetCulturesFromCurrentLang();
            string StrCult =
                !(cultures.Length == 0 && cultures.Any((ci) => ci.EnglishName.Contains(LanguageManager.CurrentLanguageInfo.FullLanguageName))) ?
                cultures.First((ci) => ci.EnglishName.Contains(LanguageManager.CurrentLanguageInfo.FullLanguageName)).EnglishName :
                CultureInfo.CurrentCulture.EnglishName;
            DebugWriter.WriteDebug(DebugLevel.I, "Culture for {0} is {1}", LanguageManager.CurrentLanguageInfo, StrCult);
            var Cults = CultureInfo.GetCultures(CultureTypes.AllCultures);
            DebugWriter.WriteDebug(DebugLevel.I, "Parsing {0} cultures for {1}", Cults.Length, StrCult);
            foreach (CultureInfo Cult in Cults)
            {
                if (Cult.EnglishName == StrCult)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Found. Changing culture...");
                    Config.MainConfig.CurrentCultStr = Cult.Name;
                    break;
                }
            }
        }

        /// <summary>
        /// Updates current culture based on current language. If there are no cultures in the curent language, assume current culture.
        /// </summary>
        public static void UpdateCulture()
        {
            UpdateCultureDry();
            Config.CreateConfig();
            DebugWriter.WriteDebug(DebugLevel.I, "Saved new culture.");
        }

        /// <summary>
        /// Updates current culture based on current language and custom culture
        /// </summary>
        /// <param name="Culture">Full culture name</param>
        public static void UpdateCultureDry(string Culture)
        {
            var Cultures = GetCulturesFromCurrentLang();
            foreach (CultureInfo Cult in Cultures)
            {
                if (Cult.EnglishName == Culture)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Found. Changing culture...");
                    Config.MainConfig.CurrentCultStr = Cult.Name;
                    break;
                }
            }
        }

        /// <summary>
        /// Updates current culture based on current language and custom culture
        /// </summary>
        /// <param name="Culture">Full culture name</param>
        public static void UpdateCulture(string Culture)
        {
            UpdateCultureDry(Culture);
            Config.CreateConfig();
            DebugWriter.WriteDebug(DebugLevel.I, "Saved new culture.");
        }

        /// <summary>
        /// Gets all cultures available for the current language
        /// </summary>
        public static CultureInfo[] GetCulturesFromCurrentLang() =>
            LanguageManager.CurrentLanguageInfo.Cultures;

        /// <summary>
        /// Gets all cultures available for the current language
        /// </summary>
        public static CultureInfo[]? GetCulturesFromLang(string Language)
        {
            if (LanguageManager.Languages.TryGetValue(Language, out LanguageInfo? langInfo))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Returning cultures for lang {0}", Language);
                return langInfo.Cultures;
            }
            return null;
        }

        /// <summary>
        /// Gets all culture names available for the current language
        /// </summary>
        public static List<string> GetCultureNamesFromCurrentLang() =>
            LanguageManager.CurrentLanguageInfo.Cultures.Select((culture) => culture.Name).ToList();

        /// <summary>
        /// Gets all culture names available for the current language
        /// </summary>
        public static List<string>? GetCultureNamesFromLang(string Language)
        {
            if (LanguageManager.Languages.TryGetValue(Language, out LanguageInfo? langInfo))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Returning culture names for lang {0}", Language);
                return langInfo.Cultures.Select((culture) => culture.Name).ToList();
            }
            return null;
        }

    }
}
