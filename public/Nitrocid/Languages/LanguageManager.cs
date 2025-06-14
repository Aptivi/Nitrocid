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
using Newtonsoft.Json;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Users.Login;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Events;
using Nitrocid.Languages.Decoy;
using Nitrocid.Misc.Reflection.Internal;
using Nitrocid.Files;

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
        public static LanguageInfo CurrentLanguageInfo =>
            Login.LoggedIn ? currentUserLanguage : currentLanguage;

        /// <summary>
        /// The installed languages list.
        /// </summary>
        public static Dictionary<string, LanguageInfo> Languages
        {
            get
            {
                var InstalledLanguages = new Dictionary<string, LanguageInfo>();

                // For each language, get information for localization and cache them
                var languageData = ResourcesManager.ConvertToString(ResourcesManager.GetData("Metadata.json", ResourcesType.Languages) ??
                    throw new KernelException(KernelExceptionType.LanguageManagement, LanguageTools.GetLocalized("NKS_LANGUAGES_EXCEPTION_NOMETADATA")));
                LanguageMetadata[] LanguageMetadata = JsonConvert.DeserializeObject<LanguageMetadata[]>(languageData) ??
                    throw new KernelException(KernelExceptionType.LanguageManagement, LanguageTools.GetLocalized("NKS_LANGUAGES_EXCEPTION_NOMETADATAARRAY"));
                foreach (var Language in LanguageMetadata)
                    AddBaseLanguage(Language);

                // Add the base languages to the final dictionary
                foreach (string BaseLanguage in BaseLanguages.Keys)
                    InstalledLanguages.Add(BaseLanguage, BaseLanguages[BaseLanguage]);

                // Now, get the custom languages and add them to the languages list
                foreach (string CustomLanguage in CustomLanguages.Keys)
                    InstalledLanguages.Add(CustomLanguage, CustomLanguages[CustomLanguage]);

                // Return the list
                DebugWriter.WriteDebug(DebugLevel.I, "{0} installed languages in total", vars: [InstalledLanguages.Count]);
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
                        DebugWriter.WriteDebug(DebugLevel.I, "Encoding set successfully for {0} to {1}.", vars: [lang, Console.OutputEncoding.EncodingName]);
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Codepage can't be set. {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                }

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
                    if (FilesystemTools.FileExists(LanguagePath))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Language {0} exists in {1}", vars: [LanguageName, LanguagePath]);

                        // Check the metadata to see if it has relevant information for the language
                        var locs = FilesystemTools.ReadContentsText(LanguagePath);
                        var localization = JsonConvert.DeserializeObject<LanguageLocalizations>(locs);
                        if (localization is not null)
                        {
                            // Parse the values and install the language
                            string ParsedLanguageName = localization.Name ?? LanguageName;
                            bool ParsedLanguageTransliterable = localization.Transliterable;
                            var ParsedLanguageLocalizations = localization.Localizations;
                            DebugWriter.WriteDebug(DebugLevel.I, "Metadata says: Name: {0}, Transliterable: {1}", vars: [ParsedLanguageName, ParsedLanguageTransliterable]);

                            // Check the localizations...
                            DebugWriter.WriteDebug(DebugLevel.I, "Checking localizations... (Null: {0})", vars: [ParsedLanguageLocalizations is null]);
                            if (ParsedLanguageLocalizations is not null)
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Valid localizations found! Length: {0}", vars: [ParsedLanguageLocalizations.Length]);

                                // Try to install the language info
                                var ParsedLanguageInfo = new LanguageInfo(LanguageName, ParsedLanguageName, ParsedLanguageTransliterable, ParsedLanguageLocalizations);
                                DebugWriter.WriteDebug(DebugLevel.I, "Made language info! Checking for existence... (Languages.ContainsKey returns {0})", vars: [Languages.ContainsKey(LanguageName)]);
                                if (!Languages.ContainsKey(LanguageName))
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Language exists. Installing...");
                                    CustomLanguages.Add(LanguageName, ParsedLanguageInfo);
                                    EventsManager.FireEvent(EventType.LanguageInstalled, LanguageName);
                                }
                                else if (ThrowOnAlreadyInstalled)
                                {
                                    DebugWriter.WriteDebug(DebugLevel.E, "Can't add existing language.");
                                    throw new KernelException(KernelExceptionType.LanguageInstall, LanguageTools.GetLocalized("NKS_LANGUAGES_EXCEPTION_NOOVERWRITE"));
                                }
                            }
                            else
                            {
                                DebugWriter.WriteDebug(DebugLevel.E, "Metadata doesn't contain valid localizations!");
                                throw new KernelException(KernelExceptionType.LanguageInstall, LanguageTools.GetLocalized("NKS_LANGUAGES_EXCEPTION_METADATAHASNOLOCS"));
                            }
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Metadata for language doesn't exist!");
                            throw new KernelException(KernelExceptionType.LanguageInstall, LanguageTools.GetLocalized("NKS_LANGUAGES_EXCEPTION_NOMETADATA_INSTALL"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to install custom language {0}: {1}", vars: [LanguageName, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    EventsManager.FireEvent(EventType.LanguageInstallError, LanguageName, ex);
                    throw new KernelException(KernelExceptionType.LanguageInstall, LanguageTools.GetLocalized("NKS_LANGUAGES_EXCEPTION_INSTALLCUSTLANGFAILED"), ex, LanguageName);
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
                    foreach (string Language in FilesystemTools.GetFilesystemEntries(PathsManagement.GetKernelPath(KernelPathType.CustomLanguages), "*.json"))
                    {
                        // Install a custom language
                        string LanguageName = Path.GetFileNameWithoutExtension(Language);
                        DebugWriter.WriteDebug(DebugLevel.I, "Custom language {0} [{1}] is to be installed.", vars: [LanguageName, Language]);
                        InstallCustomLanguage(LanguageName, false);
                    }
                    EventsManager.FireEvent(EventType.LanguagesInstalled);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to install custom languages: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    EventsManager.FireEvent(EventType.LanguagesInstallError, ex);
                    throw new KernelException(KernelExceptionType.LanguageInstall, LanguageTools.GetLocalized("NKS_LANGUAGES_EXCEPTION_INSTALLCUSTLANGSFAILED"), ex);
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
                    if (FilesystemTools.FileExists(LanguagePath))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Language {0} exists in {1}", vars: [LanguageName, LanguagePath]);

                        // Now, check the metadata to see if it has relevant information for the language
                        var locs = FilesystemTools.ReadContentsText(LanguagePath);
                        var localization = JsonConvert.DeserializeObject<LanguageLocalizations>(locs);
                        if (localization is not null)
                        {
                            // Uninstall the language
                            if (!CustomLanguages.Remove(LanguageName))
                            {
                                DebugWriter.WriteDebug(DebugLevel.E, "Failed to uninstall custom language");
                                throw new KernelException(KernelExceptionType.LanguageUninstall, LanguageTools.GetLocalized("NKS_LANGUAGES_EXCEPTION_UNINSTALLCUSTLANGFAILED2"));
                            }
                            EventsManager.FireEvent(EventType.LanguageUninstalled, LanguageName);
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Metadata for language doesn't exist!");
                            throw new KernelException(KernelExceptionType.LanguageUninstall, LanguageTools.GetLocalized("NKS_LANGUAGES_EXCEPTION_NOMETADATA_UNINSTALL"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to uninstall custom language {0}: {1}", vars: [LanguageName, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    EventsManager.FireEvent(EventType.LanguageUninstallError, LanguageName, ex);
                    throw new KernelException(KernelExceptionType.LanguageUninstall, LanguageTools.GetLocalized("NKS_LANGUAGES_EXCEPTION_UNINSTALLCUSTLANGFAILED1"), ex, LanguageName);
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
                                throw new KernelException(KernelExceptionType.LanguageUninstall, LanguageTools.GetLocalized("NKS_LANGUAGES_EXCEPTION_UNINSTALLCUSTLANGSFAILED1"));
                            }
                        }
                        EventsManager.FireEvent(EventType.LanguagesUninstalled);
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to uninstall custom languages: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    EventsManager.FireEvent(EventType.LanguagesUninstallError, ex);
                    throw new KernelException(KernelExceptionType.LanguageUninstall, LanguageTools.GetLocalized("NKS_LANGUAGES_EXCEPTION_UNINSTALLCUSTLANGSFAILED2"), ex);
                }
            }
        }

        /// <summary>
        /// Lists all languages
        /// </summary>
        /// <param name="withCountry">Whether to include country specifiers in the key</param>
        public static Dictionary<string, LanguageInfo> ListAllLanguages(bool withCountry = false) =>
            ListLanguages("", withCountry);

        /// <summary>
        /// Lists the languages
        /// </summary>
        /// <param name="SearchTerm">Search term</param>
        /// <param name="withCountry">Whether to include country specifiers in the key</param>
        public static Dictionary<string, LanguageInfo> ListLanguages(string SearchTerm, bool withCountry = false)
        {
            var ListedLanguages = new Dictionary<string, LanguageInfo>();

            // List the Languages using the search term
            foreach (var Language in Languages)
            {
                string LanguageName = Language.Key;
                var LanguageValue = Language.Value;
                if (LanguageName.Contains(SearchTerm))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Adding language {0} to list... Search term: {1}", vars: [LanguageName, SearchTerm]);
                    ListedLanguages.Add($"{LanguageName}{(withCountry ? $" [{LanguageValue.Country}]" : "")}", Languages[LanguageName]);
                }
            }
            return ListedLanguages;
        }

        /// <summary>
        /// Lists all countries
        /// </summary>
        public static Dictionary<string, LanguageInfo[]> ListAllCountries() =>
            ListCountries("");

        /// <summary>
        /// Lists the countries
        /// </summary>
        /// <param name="SearchTerm">Search term</param>
        public static Dictionary<string, LanguageInfo[]> ListCountries(string SearchTerm)
        {
            var listedCountries = new Dictionary<string, List<LanguageInfo>>();

            // List the countries using the search term
            foreach (var language in Languages)
            {
                string languageName = language.Key;
                var languageValue = language.Value;
                if (languageValue.Country.Contains(SearchTerm))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Adding language {0} for country {1} to list... Search term: {2}", vars: [languageName, languageValue.Country, SearchTerm]);
                    if (listedCountries.TryGetValue(languageValue.Country, out List<LanguageInfo>? languageInfoList))
                        languageInfoList.Add(Languages[languageName]);
                    else
                        listedCountries.Add(languageValue.Country, [Languages[languageName]]);
                }
            }
            return listedCountries.ToDictionary((kvp) => kvp.Key, (kvp) => kvp.Value.ToArray());
        }

        internal static string[] ProbeLocalizations(LanguageLocalizations loc)
        {
            DebugCheck.Assert(loc.Localizations.Length != 0, "language has no localizations!!!");
            DebugWriter.WriteDebug(DebugLevel.I, "{0} strings probed from localizations token.", vars: [loc.Localizations.Length]);
            return loc.Localizations;
        }

        internal static void AddBaseLanguage(LanguageMetadata Language, bool useLocalizationObject = false, string[]? localizations = null)
        {
            string shortName = Language.ThreeLetterLanguageName;
            string LanguageFullName = Language.Name;
            bool LanguageTransliterable = Language.Transliterable;
            int LanguageCodepage = Language.Codepage;
            string LanguageCountry = Language.Country ?? "";

            // If the language is not found in the base languages cache dictionary, add it
            if (!BaseLanguages.ContainsKey(shortName))
            {
                LanguageInfo LanguageInfo;
                if (useLocalizationObject)
                    LanguageInfo = new LanguageInfo(shortName, LanguageFullName, LanguageTransliterable, localizations, LanguageCountry);
                else
                    LanguageInfo = new LanguageInfo(shortName, LanguageFullName, LanguageTransliterable, LanguageCodepage, LanguageCountry);
                DebugWriter.WriteDebug(DebugLevel.I, "Adding language to base languages. {0}, {1}, {2}", vars: [shortName, LanguageFullName, LanguageTransliterable]);
                BaseLanguages.Add(shortName, LanguageInfo);
            }
        }
    }
}
