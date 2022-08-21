
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
using System.Globalization;
using KS.Kernel.Configuration;
using KS.Misc.Writers.DebugWriters;

namespace KS.Languages
{
    public static class CultureManager
    {

        // Variables
        public static CultureInfo CurrentCult = new CultureInfo("en-US");

        /// <summary>
        /// Updates current culture based on current language. If there are no cultures in the curent language, assume current culture.
        /// </summary>
        public static void UpdateCulture()
        {
            string StrCult = !(GetCulturesFromCurrentLang().Count == 0) ? GetCulturesFromCurrentLang()[0].EnglishName : CultureInfo.CurrentCulture.EnglishName;
            DebugWriter.Wdbg(DebugLevel.I, "Culture for {0} is {1}", LanguageManager.CurrentLanguage, StrCult);
            var Cults = CultureInfo.GetCultures(CultureTypes.AllCultures);
            DebugWriter.Wdbg(DebugLevel.I, "Parsing {0} cultures for {1}", Cults.Length, StrCult);
            foreach (CultureInfo Cult in Cults)
            {
                if ((Cult.EnglishName ?? "") == (StrCult ?? ""))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Found. Changing culture...");
                    CurrentCult = Cult;
                    var Token = ConfigTools.GetConfigCategory(Config.ConfigCategory.General);
                    ConfigTools.SetConfigValue(Config.ConfigCategory.General, Token, "Culture", CurrentCult.Name);
                    DebugWriter.Wdbg(DebugLevel.I, "Saved new culture.");
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
                if ((Cult.EnglishName ?? "") == (Culture ?? ""))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Found. Changing culture...");
                    CurrentCult = Cult;
                    var Token = ConfigTools.GetConfigCategory(Config.ConfigCategory.General);
                    ConfigTools.SetConfigValue(Config.ConfigCategory.General, Token, "Culture", CurrentCult.Name);
                    DebugWriter.Wdbg(DebugLevel.I, "Saved new culture.");
                    break;
                }
            }
        }

        /// <summary>
        /// Gets all cultures available for the current language
        /// </summary>
        public static List<CultureInfo> GetCulturesFromCurrentLang()
        {
            return LanguageManager.Languages[LanguageManager.CurrentLanguage].Cultures;
        }

        /// <summary>
        /// Gets all cultures available for the current language
        /// </summary>
        public static List<CultureInfo> GetCulturesFromLang(string Language)
        {
            if (LanguageManager.Languages.ContainsKey(Language))
            {
                return LanguageManager.Languages[LanguageManager.CurrentLanguage].Cultures;
            }
            return null;
        }

    }
}