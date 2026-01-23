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

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Users.Login;

namespace Nitrocid.Languages
{
    /// <summary>
    /// Culture management module
    /// </summary>
    public static class CultureManager
    {
        internal static CultureInfo currentCulture = GetCulturesDictionary()[Config.MainConfig.CurrentCultureName];
        internal static CultureInfo currentUserCulture = GetCulturesDictionary()[Config.MainConfig.CurrentCultureName];

        /// <summary>
        /// Current culture
        /// </summary>
        public static CultureInfo CurrentCulture =>
            Login.LoggedIn ? currentUserCulture : currentCulture;

        /// <summary>
        /// Updates current culture based on selected culture
        /// </summary>
        /// <param name="culture">Full culture name or code</param>
        public static void UpdateCultureDry(string culture)
        {
            var cultures = GetCultures();
            foreach (CultureInfo cultureInfo in cultures)
            {
                if (cultureInfo.EnglishName == culture || cultureInfo.Name == culture)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Found. Changing culture...");
                    currentCulture = cultureInfo;
                    break;
                }
            }
        }

        /// <summary>
        /// Updates current culture based on selected culture
        /// </summary>
        /// <param name="Culture">Full culture name or code</param>
        public static void UpdateCulture(string Culture)
        {
            UpdateCultureDry(Culture);
            Config.MainConfig.CurrentCultureName = currentCulture.Name;
            Config.CreateConfig();
            DebugWriter.WriteDebug(DebugLevel.I, "Saved new culture.");
        }

        /// <summary>
        /// Gets the installed cultures according to <see cref="CultureInfo.GetCultures(CultureTypes)"/>
        /// </summary>
        /// <returns>An array of <see cref="CultureInfo"/> containing all installed lectures</returns>
        public static CultureInfo[] GetCultures()
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            return cultures;
        }

        /// <summary>
        /// Gets the culture names
        /// </summary>
        /// <returns>An array of culture names</returns>
        public static string[] GetCultureNames() =>
            GetCultures().Select((ci) => ci.EnglishName).ToArray();

        /// <summary>
        /// Gets the culture codes
        /// </summary>
        /// <returns>An array of culture codes</returns>
        public static string[] GetCultureCodes() =>
            GetCultures().Select((ci) => ci.Name).ToArray();

        /// <summary>
        /// Gets the culture dictionary
        /// </summary>
        /// <returns>An array of culture codes</returns>
        public static Dictionary<string, CultureInfo> GetCulturesDictionary() =>
            GetCultures().ToDictionary((ci) => ci.Name, (ci) => ci);
    }
}
