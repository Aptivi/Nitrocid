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
using Newtonsoft.Json;
using System.Diagnostics;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages.Decoy;
using Nitrocid.Misc.Reflection.Internal;

namespace Nitrocid.Languages
{
    /// <summary>
    /// Language information
    /// </summary>
    [DebuggerDisplay("[{threeLetterLanguageName}] {fullLanguageName}")]
    public class LanguageInfo
    {

        [JsonProperty(nameof(ThreeLetterLanguageName))]
        private readonly string threeLetterLanguageName;
        [JsonProperty(nameof(FullLanguageName))]
        private readonly string fullLanguageName;
        [JsonProperty(nameof(Codepage))]
        private readonly int codepage;
        [JsonProperty(nameof(Country))]
        private readonly string country;
        [JsonIgnore]
        private readonly bool transliterable;
        [JsonIgnore]
        private readonly bool custom;
        [JsonIgnore]
        private readonly Dictionary<string, string> strings;

        /// <summary>
        /// The three-letter language name found in resources. Some languages have translated variants, and they usually end with "_T" in resources and "-T" in KS.
        /// </summary>
        [JsonIgnore]
        public string ThreeLetterLanguageName =>
            threeLetterLanguageName;

        /// <summary>
        /// The full name of language without the country specifier.
        /// </summary>
        [JsonIgnore]
        public string FullLanguageName =>
            fullLanguageName;

        /// <summary>
        /// The codepage number for the language
        /// </summary>
        [JsonIgnore]
        public int Codepage =>
            codepage;

        /// <summary>
        /// Country
        /// </summary>
        [JsonIgnore]
        public string Country =>
            country;

        /// <summary>
        /// Whether or not the language is transliterable (Arabic, Korea, ...)
        /// </summary>
        [JsonIgnore]
        public bool Transliterable =>
            transliterable;

        /// <summary>
        /// Whether the language is custom
        /// </summary>
        [JsonIgnore]
        public bool Custom =>
            custom;

        /// <summary>
        /// The localization information containing KS strings
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, string> Strings =>
            strings;

        /// <summary>
        /// Initializes the new instance of language information
        /// </summary>
        /// <param name="LangName">The three-letter language name found in resources. Some languages have translated variants, and they usually end with "_T" in resources and "-T" in KS.</param>
        /// <param name="FullLanguageName">The full name of language without the country specifier.</param>
        /// <param name="Transliterable">Whether or not the language is transliterable (Arabic, Korea, ...)</param>
        /// <param name="Codepage">Appropriate codepage number for language</param>
        /// <param name="country">The country</param>
        public LanguageInfo(string LangName, string FullLanguageName, bool Transliterable, int Codepage = 65001, string country = "")
        {
            // Check to see if the language being installed is found in resources
            string localizationTokenValue = ResourcesManager.ConvertToString(ResourcesManager.GetData($"{LangName}.json", ResourcesType.Languages) ??
                throw new KernelException(KernelExceptionType.LanguageManagement, Translate.DoTranslation("Failed to load language resource for") + $" {LangName}"));
            if (!string.IsNullOrEmpty(localizationTokenValue))
            {
                // Install values to the object instance
                threeLetterLanguageName = LangName;
                fullLanguageName = FullLanguageName;
                transliterable = Transliterable;
                codepage = Codepage;
                this.country = string.IsNullOrEmpty(country) ? "World" : country;

                // Get instance of language resource
                var localizations = JsonConvert.DeserializeObject<LanguageLocalizations>(localizationTokenValue) ??
                    throw new KernelException(KernelExceptionType.LanguageManagement, Translate.DoTranslation("Can't get localized text"));
                string[] LanguageResource = localizations.Localizations;
                var englishData = ResourcesManager.ConvertToString(ResourcesManager.GetData("eng.json", ResourcesType.Languages) ??
                    throw new KernelException(KernelExceptionType.LanguageManagement, Translate.DoTranslation("Can't get English localizations")));
                var englishLocalizations = JsonConvert.DeserializeObject<LanguageLocalizations>(englishData) ??
                    throw new KernelException(KernelExceptionType.LanguageManagement, Translate.DoTranslation("Can't get English text"));
                string[] LanguageResourceEnglish = englishLocalizations.Localizations;
                custom = false;

                // Populate language strings
                var langStrings = new Dictionary<string, string>();
                for (int i = 0; i < LanguageResourceEnglish.Length; i++)
                {
                    string UntranslatedProperty = LanguageResourceEnglish[i];
                    string TranslatedProperty = LanguageResource[i];
                    if (!langStrings.TryAdd(UntranslatedProperty, TranslatedProperty))
                        DebugWriter.WriteDebug(DebugLevel.W, "Fix: Duplicate string found in line {0}: {1}.", vars: [i + 1, UntranslatedProperty]);
                }
                DebugWriter.WriteDebug(DebugLevel.I, "{0} strings.", vars: [langStrings.Count]);
                strings = langStrings;
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
        /// <param name="country">The country</param>
        public LanguageInfo(string LangName, string FullLanguageName, bool Transliterable, string[]? LanguageToken, string country = "")
        {
            // Install values to the object instance
            threeLetterLanguageName = LangName;
            fullLanguageName = FullLanguageName;
            transliterable = Transliterable;
            this.country = string.IsNullOrEmpty(country) ? "World" : country;

            // Install it
            var englishData = ResourcesManager.ConvertToString(ResourcesManager.GetData("eng.json", ResourcesType.Languages) ??
                throw new KernelException(KernelExceptionType.LanguageManagement, Translate.DoTranslation("Can't get English localizations")));
            var englishLocalizations = JsonConvert.DeserializeObject<LanguageLocalizations>(englishData) ??
                throw new KernelException(KernelExceptionType.LanguageManagement, Translate.DoTranslation("Can't get English text"));
            if (LanguageToken is null)
                throw new KernelException(KernelExceptionType.LanguageManagement, Translate.DoTranslation("Language token must be specified"));
            string[] LanguageResourceEnglish = englishLocalizations.Localizations;
            custom = true;
            DebugWriter.WriteDebug(DebugLevel.I, "{0} should be {1} from English strings list.", vars: [LanguageToken.Length, LanguageResourceEnglish.Length]);
            if (LanguageToken.Length == LanguageResourceEnglish.Length)
            {
                // Populate language strings
                var langStrings = new Dictionary<string, string>();
                for (int i = 0; i < LanguageResourceEnglish.Length; i++)
                {
                    string UntranslatedProperty = LanguageResourceEnglish[i];
                    string TranslatedProperty = LanguageToken[i];
                    if (!langStrings.TryAdd(UntranslatedProperty, TranslatedProperty))
                        DebugWriter.WriteDebug(DebugLevel.W, "Fix: Duplicate string found in line {0}: {1}.", vars: [i + 1, UntranslatedProperty]);
                }

                DebugWriter.WriteDebug(DebugLevel.I, "{0} strings.", vars: [langStrings.Count]);
                strings = langStrings;
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Expected {0} lines according to the English string list, but got {1}.", vars: [LanguageResourceEnglish.Length, LanguageToken.Length]);
                throw new KernelException(KernelExceptionType.LanguageParse, Translate.DoTranslation("Length of the English language doesn't match the length of the language token provided."));
            }
        }

    }
}
