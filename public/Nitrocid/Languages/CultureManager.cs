
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

using System.Collections.Generic;
using System.Globalization;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;

namespace KS.Languages
{
    /// <summary>
    /// Culture management module
    /// </summary>
    public static class CultureManager
    {

        /// <summary>
        /// Current culture
        /// </summary>
        public static string CurrentCultStr { get; set; } = "en-US";
        /// <summary>
        /// Current culture
        /// </summary>
        public static CultureInfo CurrentCult => new(CurrentCultStr);

        /// <summary>
        /// Updates current culture based on current language. If there are no cultures in the curent language, assume current culture.
        /// </summary>
        public static void UpdateCulture()
        {
            string StrCult = !(GetCulturesFromCurrentLang().Count == 0) ? GetCulturesFromCurrentLang()[0].EnglishName : CultureInfo.CurrentCulture.EnglishName;
            DebugWriter.WriteDebug(DebugLevel.I, "Culture for {0} is {1}", LanguageManager.CurrentLanguage, StrCult);
            var Cults = CultureInfo.GetCultures(CultureTypes.AllCultures);
            DebugWriter.WriteDebug(DebugLevel.I, "Parsing {0} cultures for {1}", Cults.Length, StrCult);
            foreach (CultureInfo Cult in Cults)
            {
                if (Cult.EnglishName == StrCult)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Found. Changing culture...");
                    CurrentCultStr = Cult.Name;
                    var Token = ConfigTools.GetConfigCategory(ConfigCategory.General);
                    ConfigTools.SetConfigValue(ConfigCategory.General, Token, "Culture", CurrentCult.Name);
                    DebugWriter.WriteDebug(DebugLevel.I, "Saved new culture.");
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
            var Cultures = GetCulturesFromCurrentLang();
            foreach (CultureInfo Cult in Cultures)
            {
                if (Cult.EnglishName == Culture)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Found. Changing culture...");
                    CurrentCultStr = Cult.Name;
                    var Token = ConfigTools.GetConfigCategory(ConfigCategory.General);
                    ConfigTools.SetConfigValue(ConfigCategory.General, Token, "Culture", CurrentCult.Name);
                    DebugWriter.WriteDebug(DebugLevel.I, "Saved new culture.");
                    break;
                }
            }
        }

        /// <summary>
        /// Gets all cultures available for the current language
        /// </summary>
        public static List<CultureInfo> GetCulturesFromCurrentLang() => LanguageManager.CurrentLanguage.Cultures;

        /// <summary>
        /// Gets all cultures available for the current language
        /// </summary>
        public static List<CultureInfo> GetCulturesFromLang(string Language)
        {
            if (LanguageManager.Languages.ContainsKey(Language))
            {
                return LanguageManager.CurrentLanguage.Cultures;
            }
            return null;
        }

    }
}
