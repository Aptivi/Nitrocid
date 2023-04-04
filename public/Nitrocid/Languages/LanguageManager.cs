
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

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Extensification.DictionaryExts;
using FluentFTP.Helpers;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using Newtonsoft.Json.Linq;
using KS.Kernel.Events;
using KS.Users;
using KS.Users.Login;

namespace KS.Languages
{
    /// <summary>
    /// Lanaguage management module
    /// </summary>
    public static class LanguageManager
    {

        internal static Dictionary<string, LanguageInfo> BaseLanguages = new();
        internal static Dictionary<string, LanguageInfo> CustomLanguages = new();
        private static bool NotifyCodepageError;
        private readonly static JToken LanguageMetadata = JToken.Parse(Properties.Resources.Resources.LanguageMetadata);

        /// <summary>
        /// Current language
        /// </summary>
        public static string CurrentLanguage =>
            Config.MainConfig.CurrentLanguage;
        internal static LanguageInfo currentLanguage = Languages[CurrentLanguage];
        internal static LanguageInfo currentUserLanguage = Languages[CurrentLanguage];
        /// <summary>
        /// Current language
        /// </summary>
        public static LanguageInfo CurrentLanguageInfo => Flags.LoggedIn ? currentUserLanguage : currentLanguage;

        /// <summary>
        /// The installed languages list.
        /// </summary>
        public static Dictionary<string, LanguageInfo> Languages
        {
            get
            {
                var InstalledLanguages = new Dictionary<string, LanguageInfo>();

                // For each language, get information for localization and cache them
                foreach (JToken Language in LanguageMetadata)
                {
                    string LanguageName = Language.Path;
                    string LanguageFullName = (string)Language.First.SelectToken("name");
                    bool LanguageTransliterable = (bool)Language.First.SelectToken("transliterable");
                    int LanguageCodepage = (int)(Language.First.SelectToken("codepage") ?? 65001);

                    // If the language is not found in the base languages cache dictionary, add it
                    if (!BaseLanguages.ContainsKey(LanguageName))
                    {
                        var LanguageInfo = new LanguageInfo(LanguageName, LanguageFullName, LanguageTransliterable, LanguageCodepage);
                        BaseLanguages.AddIfNotFound(LanguageName, LanguageInfo);
                    }
                }

                // Add the base languages to the final dictionary
                foreach (string BaseLanguage in BaseLanguages.Keys)
                    InstalledLanguages.Add(BaseLanguage, BaseLanguages[BaseLanguage]);

                // Now, get the custom languages and add them to the languages list
                foreach (string CustomLanguage in CustomLanguages.Keys)
                    InstalledLanguages.Add(CustomLanguage, CustomLanguages[CustomLanguage]);

                // Return the list
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
            if (Languages.ContainsKey(lang))
            {
                // Set appropriate codepage for incapable terminals
                try
                {
                    int Codepage = Languages[lang].Codepage;
                    ConsoleBase.ConsoleWrapper.OutputEncoding = System.Text.Encoding.GetEncoding(Codepage);
                    ConsoleBase.ConsoleWrapper.InputEncoding = System.Text.Encoding.GetEncoding(Codepage);
                    DebugWriter.WriteDebug(DebugLevel.I, "Encoding set successfully for {0} to {1}.", lang, ConsoleBase.ConsoleWrapper.OutputEncoding.EncodingName);
                }
                catch (Exception ex)
                {
                    NotifyCodepageError = true;
                    DebugWriter.WriteDebug(DebugLevel.W, "Codepage can't be set. {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }

                // Set current language
                try
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Translating kernel to {0}.", lang);
                    currentLanguage = Languages[lang];
                    Config.MainConfig.CurrentLanguage = lang;

                    // Update Culture if applicable
                    if (Flags.LangChangeCulture)
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
            Config.CreateConfig(lang);
            DebugWriter.WriteDebug(DebugLevel.I, "Saved new language. Updating culture...");
            CultureManager.UpdateCulture();
            return true;
        }

        /// <summary>
        /// Prompt for setting language
        /// </summary>
        /// <param name="lang">A specified language</param>
        /// <param name="Force">Force changes</param>
        /// <param name="AlwaysTransliterated">The language is always transliterated</param>
        /// <param name="AlwaysTranslated">The language is always translated</param>
        public static void PromptForSetLang(string lang, bool Force = false, bool AlwaysTransliterated = false, bool AlwaysTranslated = false)
        {
            if (Languages.ContainsKey(lang))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Forced {0}", Force);
                if (!Force)
                {
                    if (lang.EndsWith("-T")) // The condition prevents tricksters from using "chlang <lang>-T", if not forced.
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Trying to bypass prompt.");
                        return;
                    }
                    else
                    {
                        // Check to see if the language is transliterable
                        DebugWriter.WriteDebug(DebugLevel.I, "Transliterable? {0}", Languages[lang].Transliterable);
                        if (Languages[lang].Transliterable)
                        {
                            if (AlwaysTransliterated)
                            {
                                lang.RemovePostfix("-T");
                            }
                            else if (AlwaysTranslated)
                            {
                                if (!lang.EndsWith("-T"))
                                    lang += "-T";
                            }
                            else
                            {
                                TextWriterColor.Write(Translate.DoTranslation("The language you've selected contains two variants. Select one:") + CharManager.NewLine);
                                TextWriterColor.Write(" 1) " + Translate.DoTranslation("Transliterated version", lang), true, KernelColorType.Option);
                                TextWriterColor.Write(" 2) " + Translate.DoTranslation("Translated version", lang + "-T") + CharManager.NewLine, true, KernelColorType.Option);
                                var LanguageSet = false;
                                while (!LanguageSet)
                                {
                                    TextWriterColor.Write(">> ", false, KernelColorType.Input);
                                    string AnswerString = Input.ReadLine(false);
                                    if (int.TryParse(AnswerString, out int Answer))
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Choice: {0}", Answer);
                                        switch (Answer)
                                        {
                                            case 1:
                                            case 2:
                                                {
                                                    if (Answer == 2)
                                                        lang += "-T";
                                                    LanguageSet = true;
                                                    break;
                                                }

                                            default:
                                                {
                                                    TextWriterColor.Write(Translate.DoTranslation("Invalid choice. Try again."), true, KernelColorType.Error);
                                                    break;
                                                }
                                        }
                                    }
                                    else
                                    {
                                        TextWriterColor.Write(Translate.DoTranslation("The answer must be numeric."), true, KernelColorType.Error);
                                    }
                                }
                            }
                        }
                    }
                }

                // Gangsta language contains strong language, so warn the user before setting
                if (lang == "pla")
                {
                    TextWriterColor.Write(Translate.DoTranslation("The gangsta language contains strong language that may make you feel uncomfortable reading it. Are you sure that you want to set the language anyways?"), true, KernelColorType.Warning);
                    if (Input.DetectKeypress().Key != ConsoleKey.Y)
                    {
                        return;
                    }
                }

                // Now, set the language!
                TextWriterColor.Write(Translate.DoTranslation("Changing from: {0} to {1}..."), CurrentLanguageInfo.ThreeLetterLanguageName, lang);
                if (!SetLang(lang))
                {
                    TextWriterColor.Write(Translate.DoTranslation("Failed to set language."), true, KernelColorType.Error);
                }
                if (NotifyCodepageError)
                {
                    NotifyCodepageError = false;
                    TextWriterColor.Write(Translate.DoTranslation("Unable to set codepage. The language may not display properly."), true, KernelColorType.Error);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Invalid language") + " {0}", true, KernelColorType.Error, lang);
            }
        }

        /// <summary>
        /// Installs the custom language to the installed languages
        /// </summary>
        /// <param name="LanguageName">The custom three-letter language name found in KSLanguages directory</param>
        /// <param name="ThrowOnAlreadyInstalled">If the custom language is already installed, throw an exception</param>
        public static void InstallCustomLanguage(string LanguageName, bool ThrowOnAlreadyInstalled = true)
        {
            if (!Flags.SafeMode)
            {
                try
                {
                    string LanguagePath = Paths.GetKernelPath(KernelPathType.CustomLanguages) + LanguageName + ".json";
                    if (Checking.FileExists(LanguagePath))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Language {0} exists in {1}", LanguageName, LanguagePath);

                        // Check the metadata to see if it has relevant information for the language
                        JToken MetadataToken = JObject.Parse(File.ReadAllText(LanguagePath));
                        DebugWriter.WriteDebug(DebugLevel.I, "MetadataToken is null: {0}", MetadataToken is null);
                        if (MetadataToken is not null)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Metadata exists!");

                            // Parse the values and install the language
                            string ParsedLanguageName = (string)(MetadataToken.SelectToken("Name") ?? LanguageName);
                            bool ParsedLanguageTransliterable = (bool)(MetadataToken.SelectToken("Transliterable") ?? false);
                            var ParsedLanguageLocalizations = MetadataToken.SelectToken("Localizations");
                            DebugWriter.WriteDebug(DebugLevel.I, "Metadata says: Name: {0}, Transliterable: {1}", ParsedLanguageName, ParsedLanguageTransliterable);

                            // Check the localizations...
                            DebugWriter.WriteDebug(DebugLevel.I, "Checking localizations... (Null: {0})", ParsedLanguageLocalizations is null);
                            if (ParsedLanguageLocalizations is not null)
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Valid localizations found! Length: {0}", ParsedLanguageLocalizations.Count());

                                // Try to install the language info
                                var ParsedLanguageInfo = new LanguageInfo(LanguageName, ParsedLanguageName, ParsedLanguageTransliterable, (JObject)ParsedLanguageLocalizations);
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
            if (!Flags.SafeMode)
            {
                try
                {
                    // Enumerate all the JSON files generated by KSJsonifyLocales
                    foreach (string Language in Listing.GetFilesystemEntries(Paths.GetKernelPath(KernelPathType.CustomLanguages), "*.json"))
                    {
                        // Install a custom language
                        string LanguageName = Path.GetFileNameWithoutExtension(Language);
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
        public static void UninstallCustomLanguage(string LanguageName)
        {
            if (!Flags.SafeMode)
            {
                try
                {
                    string LanguagePath = Paths.GetKernelPath(KernelPathType.CustomLanguages) + LanguageName + ".json";
                    if (Checking.FileExists(LanguagePath))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Language {0} exists in {1}", LanguageName, LanguagePath);

                        // Now, check the metadata to see if it has relevant information for the language
                        JToken MetadataToken = JObject.Parse(File.ReadAllText(LanguagePath));
                        DebugWriter.WriteDebug(DebugLevel.I, "MetadataToken is null: {0}", MetadataToken is null);
                        if (MetadataToken is not null)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Metadata exists!");

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
            if (!Flags.SafeMode)
            {
                try
                {
                    // Enumerate all the installed languages and query for the custom status to uninstall the custom languages
                    for (int LanguageIndex = Languages.Count - 1; LanguageIndex <= 0; LanguageIndex++)
                    {
                        string Language = Languages.Keys.ElementAtOrDefault(LanguageIndex);
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
                    ListedLanguages.Add(LanguageName, Languages[LanguageName]);
                }
            }
            return ListedLanguages;
        }

    }
}
