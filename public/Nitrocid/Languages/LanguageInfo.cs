
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
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KS.Resources;

namespace KS.Languages
{
    /// <summary>
    /// Language information
    /// </summary>
    public class LanguageInfo
    {

        /// <summary>
        /// The three-letter language name found in resources. Some languages have translated variants, and they usually end with "_T" in resources and "-T" in KS.
        /// </summary>
        public readonly string ThreeLetterLanguageName;
        /// <summary>
        /// The full name of language without the country specifier.
        /// </summary>
        public readonly string FullLanguageName;
        /// <summary>
        /// The codepage number for the language
        /// </summary>
        public readonly int Codepage;
        /// <summary>
        /// Culture code to use. If blank, the language manager will find the appropriate culture.
        /// </summary>
        public readonly string CultureCode;
        /// <summary>
        /// Whether or not the language is transliterable (Arabic, Korea, ...)
        /// </summary>
        [JsonIgnore]
        public readonly bool Transliterable;
        /// <summary>
        /// Whether the language is custom
        /// </summary>
        [JsonIgnore]
        public readonly bool Custom;
        /// <summary>
        /// The localization information containing KS strings
        /// </summary>
        [JsonIgnore]
        public readonly Dictionary<string, string> Strings;
        /// <summary>
        /// List of cultures of language
        /// </summary>
        [JsonIgnore]
        public readonly List<CultureInfo> Cultures;

        /// <summary>
        /// Initializes the new instance of language information
        /// </summary>
        /// <param name="LangName">The three-letter language name found in resources. Some languages have translated variants, and they usually end with "_T" in resources and "-T" in KS.</param>
        /// <param name="FullLanguageName">The full name of language without the country specifier.</param>
        /// <param name="Transliterable">Whether or not the language is transliterable (Arabic, Korea, ...)</param>
        /// <param name="Codepage">Appropriate codepage number for language</param>
        /// <param name="cultureCode">Culture code to use. If blank, the language manager will find the appropriate culture.</param>
        public LanguageInfo(string LangName, string FullLanguageName, bool Transliterable, int Codepage = 65001, string cultureCode = "")
        {
            // Check to see if the language being installed is found in resources
            string localizationTokenValue = LanguageResources.ResourceManager.GetString(LangName.Replace("-", "_"));
            if (!string.IsNullOrEmpty(localizationTokenValue))
            {
                // Install values to the object instance
                ThreeLetterLanguageName = LangName;
                this.FullLanguageName = FullLanguageName;
                this.Transliterable = Transliterable;
                this.Codepage = Codepage;

                // Get all cultures associated with the language and install the parsed values. Additionally, it checks if the necessary cultures were added. If not,
                // the current culture is assumed.
                var Cults = CultureInfo.GetCultures(CultureTypes.AllCultures);
                var Cultures = new List<CultureInfo>();
                foreach (CultureInfo Cult in Cults)
                {
                    if (Cult.EnglishName.ToLower().Contains(FullLanguageName.ToLower()) || Cult.Name == cultureCode)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Adding culture {0} found from {1} to list...", Cult.EnglishName.ToLower(), FullLanguageName.ToLower());
                        Cultures.Add(Cult);
                    }
                }
                if (Cultures.Count == 0)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Adding current culture because we can't find any culture with the name of {0}...", FullLanguageName.ToLower());
                    Cultures.Add(CultureInfo.CurrentCulture);
                }
                this.Cultures = Cultures;
                CultureCode = cultureCode;

                // Get instance of language resource
                JArray LanguageResource = (JArray)JObject.Parse(localizationTokenValue).SelectToken("Localizations");
                JArray LanguageResourceEnglish = (JArray)JObject.Parse(LanguageResources.eng).SelectToken("Localizations");
                Custom = false;

                // Populate language strings
                var langStrings = new Dictionary<string, string>();
                for (int i = 0; i < LanguageResourceEnglish.Count; i++)
                {
                    JToken UntranslatedProperty = LanguageResourceEnglish[i];
                    JToken TranslatedProperty = LanguageResource[i];
                    langStrings.Add((string)UntranslatedProperty, (string)TranslatedProperty);
                }
                DebugWriter.WriteDebug(DebugLevel.I, "{0} strings.", langStrings.Count);
                Strings = langStrings;
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "No such language or invalid language. Perhaps, you should use the second overload that takes the LanguageToken for your custom languages?");
                throw new KernelException(KernelExceptionType.NoSuchLanguage, Translate.DoTranslation("Invalid language") + " {0}", LangName);
            }
        }

        /// <summary>
        /// Initializes the new instance of language information
        /// </summary>
        /// <param name="LangName">The three-letter language name found in resources. Some languages have translated variants, and they usually end with "_T" in resources and "-T" in KS.</param>
        /// <param name="FullLanguageName">The full name of language without the country specifier.</param>
        /// <param name="Transliterable">Whether or not the language is transliterable (Arabic, Korea, ...)</param>
        /// <param name="LanguageToken">The language token containing localization information</param>
        /// <param name="cultureCode">Culture code to use. If blank, the language manager will find the appropriate culture.</param>
        public LanguageInfo(string LangName, string FullLanguageName, bool Transliterable, JArray LanguageToken, string cultureCode = "")
        {
            // Install values to the object instance
            ThreeLetterLanguageName = LangName;
            this.FullLanguageName = FullLanguageName;
            this.Transliterable = Transliterable;

            // Get all cultures associated with the language and install the parsed values. Additionally, it checks if the necessary cultures were added. If not,
            // the current culture is assumed.
            var Cults = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var Cultures = new List<CultureInfo>();
            foreach (CultureInfo Cult in Cults)
            {
                if (Cult.EnglishName.ToLower().Contains(FullLanguageName.ToLower()) || Cult.Name == cultureCode)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Adding culture {0} found from {1} to list...", Cult.EnglishName.ToLower(), FullLanguageName.ToLower());
                    Cultures.Add(Cult);
                }
            }
            if (Cultures.Count == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Adding current culture because we can't find any culture with the name of {0}...", FullLanguageName.ToLower());
                Cultures.Add(CultureInfo.CurrentCulture);
            }
            this.Cultures = Cultures;
            CultureCode = cultureCode;

            // Install it
            JArray LanguageResourceEnglish = (JArray)JObject.Parse(LanguageResources.eng).SelectToken("Localizations");
            Custom = true;
            DebugWriter.WriteDebug(DebugLevel.I, "{0} should be {1} from English strings list.", LanguageToken.Count, LanguageResourceEnglish.Count);
            if (LanguageToken.Count == LanguageResourceEnglish.Count)
            {
                // Populate language strings
                var langStrings = new Dictionary<string, string>();
                for (int i = 0; i < LanguageResourceEnglish.Count; i++)
                {
                    JToken UntranslatedProperty = LanguageResourceEnglish[i];
                    JToken TranslatedProperty = LanguageToken[i];
                    langStrings.Add((string)UntranslatedProperty, (string)TranslatedProperty);
                }

                DebugWriter.WriteDebug(DebugLevel.I, "{0} strings.", langStrings.Count);
                Strings = langStrings;
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Expected {0} lines according to the English string list, but got {1}.", LanguageResourceEnglish.Count, LanguageToken.Count);
                throw new KernelException(KernelExceptionType.LanguageParse, Translate.DoTranslation("Length of the English language doesn't match the length of the language token provided."));
            }
        }

    }
}
