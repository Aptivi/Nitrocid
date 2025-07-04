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
using System.IO;
using System.Linq;
using System.Globalization;
using Newtonsoft.Json;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Users.Login;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Files.Operations;
using Nitrocid.Files.Folders;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Events;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Languages.Decoy;
using Nitrocid.Misc.Reflection.Internal;
using LocaleStation.Tools;

namespace Nitrocid.Languages
{
    /// <summary>
    /// Lanaguage management module
    /// </summary>
    public static class LanguageManager
    {

        internal static Dictionary<string, LanguageInfo> BaseLanguages = [];
        internal static Dictionary<string, LanguageInfo> CustomLanguages = [];
        internal static LanguageInfo currentLanguage = Languages[Config.MainConfig.CurrentLanguage];
        internal static LanguageInfo currentUserLanguage = Languages[Config.MainConfig.CurrentLanguage];

        /// <summary>
        /// Current language
        /// </summary>
        public static LanguageInfo CurrentLanguageInfo
        {
            get
            {
                var lang = Login.LoggedIn ? currentUserLanguage : currentLanguage;
                LanguageCommon.Language = lang.ThreeLetterLanguageName;
                return lang;
            }
        }

        /// <summary>
        /// The installed languages list.
        /// </summary>
        public static Dictionary<string, LanguageInfo> Languages
        {
            get
            {
                var InstalledLanguages = new Dictionary<string, LanguageInfo>();

                // For each language, get information for localization and cache them
                var languageData = ResourcesManager.GetData("Metadata.json", ResourcesType.Languages) ??
                    throw new KernelException(KernelExceptionType.LanguageManagement, Translate.DoTranslation("Can't get language metadata"));
                LanguageMetadata[] LanguageMetadata = JsonConvert.DeserializeObject<LanguageMetadata[]>(languageData) ??
                    throw new KernelException(KernelExceptionType.LanguageManagement, Translate.DoTranslation("Can't get language metadata array"));
                foreach (var Language in LanguageMetadata)
                    AddBaseLanguage(Language);

                // Add the base languages to the final dictionary
                foreach (string BaseLanguage in BaseLanguages.Keys)
                    InstalledLanguages.Add(BaseLanguage, BaseLanguages[BaseLanguage]);

                // Now, get the custom languages and add them to the languages list
                foreach (string CustomLanguage in CustomLanguages.Keys)
                    InstalledLanguages.Add(CustomLanguage, CustomLanguages[CustomLanguage]);

                // Return the list
                DebugWriter.WriteDebug(DebugLevel.I, "{0} installed languages in total", InstalledLanguages.Count);
                return InstalledLanguages;
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
                // Set appropriate codepage for incapable terminals
                try
                {
                    if (KernelPlatform.IsOnWindows() && Config.MainConfig.SetCodepage)
                    {
                        int Codepage = langInfo.Codepage;
                        Console.OutputEncoding = System.Text.Encoding.GetEncoding(Codepage);
                        Console.InputEncoding = System.Text.Encoding.GetEncoding(Codepage);
                        DebugWriter.WriteDebug(DebugLevel.I, "Encoding set successfully for {0} to {1}.", lang, Console.OutputEncoding.EncodingName);
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Codepage can't be set. {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }

                // Set current language
                try
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Translating kernel to {0}.", lang);
                    currentLanguage = langInfo;

                    // Update Culture if applicable
                    if (Config.MainConfig.LangChangeCulture)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Updating culture.");
                        CultureManager.UpdateCultureDry();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Language can't be set. {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.NoSuchLanguage, Translate.DoTranslation("Invalid language") + " {0}", lang);
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
            DebugWriter.WriteDebug(DebugLevel.I, "Saved new language. Updating culture...");
            CultureManager.UpdateCulture();
            return true;
        }

        /// <summary>
        /// Installs the custom language to the installed languages
        /// </summary>
        /// <param name="LanguageName">The custom three-letter language name found in KSLanguages directory</param>
        /// <param name="ThrowOnAlreadyInstalled">If the custom language is already installed, throw an exception</param>
        public static void InstallCustomLanguageByName(string LanguageName, bool ThrowOnAlreadyInstalled = true)
        {
            string LanguagePath = PathsManagement.GetKernelPath(KernelPathType.CustomLanguages) + LanguageName + ".json";
            InstallCustomLanguage(LanguagePath, ThrowOnAlreadyInstalled);
        }

        /// <summary>
        /// Installs the custom language to the installed languages
        /// </summary>
        /// <param name="LanguagePath">Path to the JSON file of a language</param>
        /// <param name="ThrowOnAlreadyInstalled">If the custom language is already installed, throw an exception</param>
        public static void InstallCustomLanguage(string LanguagePath, bool ThrowOnAlreadyInstalled = true)
        {
            if (!KernelEntry.SafeMode)
            {
                string LanguageName = Path.GetFileNameWithoutExtension(LanguagePath);
                try
                {
                    if (Checking.FileExists(LanguagePath))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Language {0} exists in {1}", LanguageName, LanguagePath);

                        // Check the metadata to see if it has relevant information for the language
                        var locs = Reading.ReadContentsText(LanguagePath);
                        var localization = JsonConvert.DeserializeObject<LanguageLocalizations>(locs);
                        if (localization is not null)
                        {
                            // Parse the values and install the language
                            string ParsedLanguageName = localization.Name ?? LanguageName;
                            bool ParsedLanguageTransliterable = localization.Transliterable;
                            var ParsedLanguageLocalizations = localization.Localizations;
                            DebugWriter.WriteDebug(DebugLevel.I, "Metadata says: Name: {0}, Transliterable: {1}", ParsedLanguageName, ParsedLanguageTransliterable);

                            // Check the localizations...
                            DebugWriter.WriteDebug(DebugLevel.I, "Checking localizations... (Null: {0})", ParsedLanguageLocalizations is null);
                            if (ParsedLanguageLocalizations is not null)
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Valid localizations found! Length: {0}", ParsedLanguageLocalizations.Length);

                                // Try to install the language info
                                var ParsedLanguageInfo = new LanguageInfo(LanguageName, ParsedLanguageName, ParsedLanguageTransliterable, ParsedLanguageLocalizations);
                                DebugWriter.WriteDebug(DebugLevel.I, "Made language info! Checking for existence... (Languages.ContainsKey returns {0})", Languages.ContainsKey(LanguageName));
                                if (!Languages.ContainsKey(LanguageName))
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Language exists. Installing...");
                                    CustomLanguages.Add(LanguageName, ParsedLanguageInfo);
                                    EventsManager.FireEvent(EventType.LanguageInstalled, LanguageName);
                                }
                                else if (ThrowOnAlreadyInstalled)
                                {
                                    DebugWriter.WriteDebug(DebugLevel.E, "Can't add existing language.");
                                    throw new KernelException(KernelExceptionType.LanguageInstall, Translate.DoTranslation("The language already exists and can't be overwritten."));
                                }
                            }
                            else
                            {
                                DebugWriter.WriteDebug(DebugLevel.E, "Metadata doesn't contain valid localizations!");
                                throw new KernelException(KernelExceptionType.LanguageInstall, Translate.DoTranslation("The metadata information needed to install the custom language doesn't provide the necessary localizations needed."));
                            }
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Metadata for language doesn't exist!");
                            throw new KernelException(KernelExceptionType.LanguageInstall, Translate.DoTranslation("The metadata information needed to install the custom language doesn't exist."));
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to install custom language {0}: {1}", LanguageName, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    EventsManager.FireEvent(EventType.LanguageInstallError, LanguageName, ex);
                    throw new KernelException(KernelExceptionType.LanguageInstall, Translate.DoTranslation("Failed to install custom language {0}."), ex, LanguageName);
                }
            }
        }

        /// <summary>
        /// Installs all the custom languages found in KSLanguages
        /// </summary>
        public static void InstallCustomLanguages()
        {
            if (!KernelEntry.SafeMode)
            {
                try
                {
                    // Enumerate all the JSON files generated by Nitrocid.LocaleGen
                    foreach (string Language in Listing.GetFilesystemEntries(PathsManagement.GetKernelPath(KernelPathType.CustomLanguages), "*.json"))
                    {
                        // Install a custom language
                        string LanguageName = Path.GetFileNameWithoutExtension(Language);
                        DebugWriter.WriteDebug(DebugLevel.I, "Custom language {0} [{1}] is to be installed.", LanguageName, Language);
                        InstallCustomLanguage(LanguageName, false);
                    }
                    EventsManager.FireEvent(EventType.LanguagesInstalled);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to install custom languages: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    EventsManager.FireEvent(EventType.LanguagesInstallError, ex);
                    throw new KernelException(KernelExceptionType.LanguageInstall, Translate.DoTranslation("Failed to install custom languages."), ex);
                }
            }
        }

        /// <summary>
        /// Uninstalls the custom language to the installed languages
        /// </summary>
        /// <param name="LanguageName">The custom three-letter language name found in KSLanguages directory</param>
        public static void UninstallCustomLanguageByName(string LanguageName)
        {
            string LanguagePath = PathsManagement.GetKernelPath(KernelPathType.CustomLanguages) + LanguageName + ".json";
            UninstallCustomLanguage(LanguagePath);
        }

        /// <summary>
        /// Uninstalls the custom language to the installed languages
        /// </summary>
        /// <param name="LanguagePath">Path to the JSON file of a language</param>
        public static void UninstallCustomLanguage(string LanguagePath)
        {
            if (!KernelEntry.SafeMode)
            {
                string LanguageName = Path.GetFileNameWithoutExtension(LanguagePath);
                try
                {
                    if (Checking.FileExists(LanguagePath))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Language {0} exists in {1}", LanguageName, LanguagePath);

                        // Now, check the metadata to see if it has relevant information for the language
                        var locs = Reading.ReadContentsText(LanguagePath);
                        var localization = JsonConvert.DeserializeObject<LanguageLocalizations>(locs);
                        if (localization is not null)
                        {
                            // Uninstall the language
                            if (!CustomLanguages.Remove(LanguageName))
                            {
                                DebugWriter.WriteDebug(DebugLevel.E, "Failed to uninstall custom language");
                                throw new KernelException(KernelExceptionType.LanguageUninstall, Translate.DoTranslation("Failed to uninstall custom language. It most likely doesn't exist."));
                            }
                            EventsManager.FireEvent(EventType.LanguageUninstalled, LanguageName);
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Metadata for language doesn't exist!");
                            throw new KernelException(KernelExceptionType.LanguageUninstall, Translate.DoTranslation("The metadata information needed to uninstall the custom language doesn't exist."));
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to uninstall custom language {0}: {1}", LanguageName, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    EventsManager.FireEvent(EventType.LanguageUninstallError, LanguageName, ex);
                    throw new KernelException(KernelExceptionType.LanguageUninstall, Translate.DoTranslation("Failed to uninstall custom language {0}."), ex, LanguageName);
                }
            }
        }

        /// <summary>
        /// Uninstalls all the custom languages found in KSLanguages
        /// </summary>
        public static void UninstallCustomLanguages()
        {
            if (!KernelEntry.SafeMode)
            {
                try
                {
                    // Enumerate all the installed languages and query for the custom status to uninstall the custom languages
                    for (int LanguageIndex = Languages.Count - 1; LanguageIndex <= 0; LanguageIndex++)
                    {
                        string Language = Languages.Keys.ElementAt(LanguageIndex);
                        var LanguageInfo = Languages[Language];

                        // Check the status
                        if (LanguageInfo.Custom)
                        {
                            // Actually uninstall
                            if (!CustomLanguages.Remove(Language))
                            {
                                DebugWriter.WriteDebug(DebugLevel.E, "Failed to uninstall custom languages");
                                throw new KernelException(KernelExceptionType.LanguageUninstall, Translate.DoTranslation("Failed to uninstall custom languages."));
                            }
                        }
                        EventsManager.FireEvent(EventType.LanguagesUninstalled);
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to uninstall custom languages: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    EventsManager.FireEvent(EventType.LanguagesUninstallError, ex);
                    throw new KernelException(KernelExceptionType.LanguageUninstall, Translate.DoTranslation("Failed to uninstall custom languages. See the inner exception for more info."), ex);
                }
            }
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
            foreach (string LanguageName in Languages.Keys)
            {
                if (LanguageName.Contains(SearchTerm))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Adding language {0} to list... Search term: {1}", LanguageName, SearchTerm);
                    ListedLanguages.Add(LanguageName, Languages[LanguageName]);
                }
            }
            return ListedLanguages;
        }

        /// <summary>
        /// Lists all languages with their country specifiers
        /// </summary>
        public static Dictionary<string, LanguageInfo> ListAllLanguagesWithCountry() =>
            ListLanguagesWithCountry("");

        /// <summary>
        /// Lists the languages with their country specifiers
        /// </summary>
        /// <param name="SearchTerm">Search term</param>
        public static Dictionary<string, LanguageInfo> ListLanguagesWithCountry(string SearchTerm)
        {
            var ListedLanguages = new Dictionary<string, LanguageInfo>();

            // List the Languages using the search term
            foreach (var Language in Languages)
            {
                string LanguageName = Language.Key;
                var LanguageValue = Language.Value;
                if (LanguageName.Contains(SearchTerm))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Adding language {0} to list... Search term: {1}", LanguageName, SearchTerm);
                    ListedLanguages.Add($"{LanguageName} [{LanguageValue.Country}]", Languages[LanguageName]);
                }
            }
            return ListedLanguages;
        }

        /// <summary>
        /// Infers the language from the system's current culture settings
        /// </summary>
        /// <returns>Language name if the system culture can be used to infer the language. Otherwise, English (eng).</returns>
        public static string InferLanguageFromSystem()
        {
            string currentCult = CultureInfo.CurrentUICulture.Name;
            DebugWriter.WriteDebug(DebugLevel.I, "Inferring language from current UI culture {0}, {1}...", currentCult);

            // Get all the languages and compare
            var langs = ListAllLanguages();
            string finalLang = "eng";
            foreach (var language in langs.Keys)
            {
                // Get the available cultures
                var cults = CultureManager.GetCulturesFromLang(language);
                if (cults is null)
                    continue;
                foreach (var cult in cults)
                {
                    if (cult.Name == currentCult)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Found language {0} from culture {1}...", language, currentCult);
                        finalLang = language;
                        return finalLang;
                    }
                }
            }

            // Return the result
            return finalLang;
        }

        internal static string[] ProbeLocalizations(LanguageLocalizations loc)
        {
            DebugCheck.Assert(loc.Localizations.Length != 0, "language has no localizations!!!");
            DebugWriter.WriteDebug(DebugLevel.I, "{0} strings probed from localizations token.", loc.Localizations.Length);
            return loc.Localizations;
        }

        internal static void AddBaseLanguage(LanguageMetadata Language, bool useLocalizationObject = false, string[]? localizations = null)
        {
            string shortName = Language.ThreeLetterLanguageName;
            string LanguageFullName = Language.Name;
            bool LanguageTransliterable = Language.Transliterable;
            int LanguageCodepage = Language.Codepage;
            string LanguageCultureCode = Language.Culture ?? "";
            string LanguageCountry = Language.Country ?? "";

            // If the language is not found in the base languages cache dictionary, add it
            if (!BaseLanguages.ContainsKey(shortName))
            {
                LanguageInfo LanguageInfo;
                if (useLocalizationObject)
                    LanguageInfo = new LanguageInfo(shortName, LanguageFullName, LanguageTransliterable, localizations, LanguageCultureCode, LanguageCountry);
                else
                    LanguageInfo = new LanguageInfo(shortName, LanguageFullName, LanguageTransliterable, LanguageCodepage, LanguageCultureCode, LanguageCountry);
                DebugWriter.WriteDebug(DebugLevel.I, "Adding language to base languages. {0}, {1}, {2}", shortName, LanguageFullName, LanguageTransliterable);
                BaseLanguages.Add(shortName, LanguageInfo);
            }
        }
    }
}
