
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
using KS.Misc.Writers.ConsoleWriters;
using Newtonsoft.Json.Linq;

namespace KS.Languages
{
    public static class LanguageManager
    {

        // Variables
        public static string CurrentLanguage = "eng"; // Default to English
        internal static Dictionary<string, LanguageInfo> BaseLanguages = new();
        internal static Dictionary<string, LanguageInfo> CustomLanguages = new();
        private static bool NotifyCodepageError;
        private readonly static JToken LanguageMetadata = JToken.Parse(Properties.Resources.Resources.LanguageMetadata);

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

                    // If the language is not found in the base languages cache dictionary, add it
                    if (!BaseLanguages.ContainsKey(LanguageName))
                    {
                        var LanguageInfo = new LanguageInfo(LanguageName, LanguageFullName, LanguageTransliterable);
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
        /// Sets a system language permanently
        /// </summary>
        /// <param name="lang">A specified language</param>
        /// <returns>True if successful, False if unsuccessful.</returns>
        public static bool SetLang(string lang)
        {
            if (Languages.ContainsKey(lang))
            {
                // Set appropriate codepage for incapable terminals
                try
                {
                    switch (lang ?? "")
                    {
                        case "arb-T":
                            {
                                ConsoleBase.ConsoleWrapper.OutputEncoding = System.Text.Encoding.GetEncoding(1256);
                                ConsoleBase.ConsoleWrapper.InputEncoding = System.Text.Encoding.GetEncoding(1256);
                                DebugWriter.Wdbg(DebugLevel.I, "Encoding set successfully for Arabic to {0}.", ConsoleBase.ConsoleWrapper.OutputEncoding.EncodingName);
                                break;
                            }
                        case "chi-T":
                            {
                                ConsoleBase.ConsoleWrapper.OutputEncoding = System.Text.Encoding.GetEncoding(936);
                                ConsoleBase.ConsoleWrapper.InputEncoding = System.Text.Encoding.GetEncoding(936);
                                DebugWriter.Wdbg(DebugLevel.I, "Encoding set successfully for Chinese to {0}.", ConsoleBase.ConsoleWrapper.OutputEncoding.EncodingName);
                                break;
                            }
                        case "jpn":
                            {
                                ConsoleBase.ConsoleWrapper.OutputEncoding = System.Text.Encoding.GetEncoding(932);
                                ConsoleBase.ConsoleWrapper.InputEncoding = System.Text.Encoding.GetEncoding(932);
                                DebugWriter.Wdbg(DebugLevel.I, "Encoding set successfully for Japanese to {0}.", ConsoleBase.ConsoleWrapper.OutputEncoding.EncodingName);
                                break;
                            }
                        case "kor-T":
                            {
                                ConsoleBase.ConsoleWrapper.OutputEncoding = System.Text.Encoding.GetEncoding(949);
                                ConsoleBase.ConsoleWrapper.InputEncoding = System.Text.Encoding.GetEncoding(949);
                                DebugWriter.Wdbg(DebugLevel.I, "Encoding set successfully for Korean to {0}.", ConsoleBase.ConsoleWrapper.OutputEncoding.EncodingName);
                                break;
                            }
                        case "rus-T":
                            {
                                ConsoleBase.ConsoleWrapper.OutputEncoding = System.Text.Encoding.GetEncoding(866);
                                ConsoleBase.ConsoleWrapper.InputEncoding = System.Text.Encoding.GetEncoding(866);
                                DebugWriter.Wdbg(DebugLevel.I, "Encoding set successfully for Russian to {0}.", ConsoleBase.ConsoleWrapper.OutputEncoding.EncodingName);
                                break;
                            }
                        case "vtn":
                            {
                                ConsoleBase.ConsoleWrapper.OutputEncoding = System.Text.Encoding.GetEncoding(1258);
                                ConsoleBase.ConsoleWrapper.InputEncoding = System.Text.Encoding.GetEncoding(1258);
                                DebugWriter.Wdbg(DebugLevel.I, "Encoding set successfully for Vietnamese to {0}.", ConsoleBase.ConsoleWrapper.OutputEncoding.EncodingName);
                                break;
                            }

                        default:
                            {
                                ConsoleBase.ConsoleWrapper.OutputEncoding = System.Text.Encoding.GetEncoding(65001);
                                ConsoleBase.ConsoleWrapper.InputEncoding = System.Text.Encoding.GetEncoding(65001);
                                DebugWriter.Wdbg(DebugLevel.I, "Encoding set successfully to {0}.", ConsoleBase.ConsoleWrapper.OutputEncoding.EncodingName);
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    NotifyCodepageError = true;
                    DebugWriter.Wdbg(DebugLevel.W, "Codepage can't be set. {0}", ex.Message);
                    DebugWriter.WStkTrc(ex);
                }

                // Set current language
                try
                {
                    string OldModDescGeneric = Translate.DoTranslation("Command defined by ");
                    DebugWriter.Wdbg(DebugLevel.I, "Translating kernel to {0}.", lang);
                    CurrentLanguage = lang;
                    var Token = ConfigTools.GetConfigCategory(Config.ConfigCategory.General);
                    ConfigTools.SetConfigValue(Config.ConfigCategory.General, Token, "Language", CurrentLanguage);
                    DebugWriter.Wdbg(DebugLevel.I, "Saved new language.");

                    // Update Culture if applicable
                    if (Flags.LangChangeCulture)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Updating culture.");
                        CultureManager.UpdateCulture();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Language can't be set. {0}", ex.Message);
                    DebugWriter.WStkTrc(ex);
                }
            }
            else
            {
                throw new Kernel.Exceptions.NoSuchLanguageException(Translate.DoTranslation("Invalid language") + " {0}", lang);
            }
            return false;
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
                DebugWriter.Wdbg(DebugLevel.I, "Forced {0}", Force);
                if (!Force)
                {
                    if (lang.EndsWith("-T")) // The condition prevents tricksters from using "chlang <lang>-T", if not forced.
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Trying to bypass prompt.");
                        return;
                    }
                    else
                    {
                        // Check to see if the language is transliterable
                        DebugWriter.Wdbg(DebugLevel.I, "Transliterable? {0}", Languages[lang].Transliterable);
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
                                TextWriterColor.Write(Translate.DoTranslation("The language you've selected contains two variants. Select one:") + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.Neutral);
                                TextWriterColor.Write(" 1) " + Translate.DoTranslation("Transliterated version", lang), true, ColorTools.ColTypes.Option);
                                TextWriterColor.Write(" 2) " + Translate.DoTranslation("Translated version", lang + "-T") + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.Option);
                                var LanguageSet = default(bool);
                                while (!LanguageSet)
                                {
                                    TextWriterColor.Write(">> ", false, ColorTools.ColTypes.Input);
                                    int Answer;
                                    string AnswerString = Input.ReadLine(false);
                                    if (int.TryParse(AnswerString, out Answer))
                                    {
                                        DebugWriter.Wdbg(DebugLevel.I, "Choice: {0}", Answer);
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
                                                    TextWriterColor.Write(Translate.DoTranslation("Invalid choice. Try again."), true, ColorTools.ColTypes.Error);
                                                    break;
                                                }
                                        }
                                    }
                                    else
                                    {
                                        TextWriterColor.Write(Translate.DoTranslation("The answer must be numeric."), true, ColorTools.ColTypes.Error);
                                    }
                                }
                            }
                        }
                    }
                }

                // Gangsta language contains strong language, so warn the user before setting
                if (lang == "pla")
                {
                    TextWriterColor.Write(Translate.DoTranslation("The gangsta language contains strong language that may make you feel uncomfortable reading it. Are you sure that you want to set the language anyways?"), true, ColorTools.ColTypes.Warning);
                    if (ConsoleBase.ConsoleWrapper.ReadKey(true).Key != ConsoleKey.Y)
                    {
                        return;
                    }
                }

                // Now, set the language!
                TextWriterColor.Write(Translate.DoTranslation("Changing from: {0} to {1}..."), true, ColorTools.ColTypes.Neutral, CurrentLanguage, lang);
                if (!SetLang(lang))
                {
                    TextWriterColor.Write(Translate.DoTranslation("Failed to set language."), true, ColorTools.ColTypes.Error);
                }
                if (NotifyCodepageError)
                {
                    TextWriterColor.Write(Translate.DoTranslation("Unable to set codepage. The language may not display properly."), true, ColorTools.ColTypes.Error);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Invalid language") + " {0}", true, ColorTools.ColTypes.Error, lang);
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
                        DebugWriter.Wdbg(DebugLevel.I, "Language {0} exists in {1}", LanguageName, LanguagePath);

                        // Check the metadata to see if it has relevant information for the language
                        JToken MetadataToken = JObject.Parse(File.ReadAllText(LanguagePath));
                        DebugWriter.Wdbg(DebugLevel.I, "MetadataToken is null: {0}", MetadataToken is null);
                        if (MetadataToken is not null)
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Metadata exists!");

                            // Parse the values and install the language
                            string ParsedLanguageName = (string)(MetadataToken.SelectToken("Name") ?? LanguageName);
                            bool ParsedLanguageTransliterable = (bool)(MetadataToken.SelectToken("Transliterable") ?? false);
                            var ParsedLanguageLocalizations = MetadataToken.SelectToken("Localizations");
                            DebugWriter.Wdbg(DebugLevel.I, "Metadata says: Name: {0}, Transliterable: {1}", ParsedLanguageName, ParsedLanguageTransliterable);

                            // Check the localizations...
                            DebugWriter.Wdbg(DebugLevel.I, "Checking localizations... (Null: {0})", ParsedLanguageLocalizations is null);
                            if (ParsedLanguageLocalizations is not null)
                            {
                                DebugWriter.Wdbg(DebugLevel.I, "Valid localizations found! Length: {0}", ParsedLanguageLocalizations.Count());

                                // Try to install the language info
                                var ParsedLanguageInfo = new LanguageInfo(LanguageName, ParsedLanguageName, ParsedLanguageTransliterable, (JObject)ParsedLanguageLocalizations);
                                DebugWriter.Wdbg(DebugLevel.I, "Made language info! Checking for existence... (Languages.ContainsKey returns {0})", Languages.ContainsKey(LanguageName));
                                if (!Languages.ContainsKey(LanguageName))
                                {
                                    DebugWriter.Wdbg(DebugLevel.I, "Language exists. Installing...");
                                    CustomLanguages.Add(LanguageName, ParsedLanguageInfo);
                                    Kernel.Kernel.KernelEventManager.RaiseLanguageInstalled(LanguageName);
                                }
                                else if (ThrowOnAlreadyInstalled)
                                {
                                    DebugWriter.Wdbg(DebugLevel.E, "Can't add existing language.");
                                    throw new Kernel.Exceptions.LanguageInstallException(Translate.DoTranslation("The language already exists and can't be overwritten."));
                                }
                            }
                            else
                            {
                                DebugWriter.Wdbg(DebugLevel.E, "Metadata doesn't contain valid localizations!");
                                throw new Kernel.Exceptions.LanguageInstallException(Translate.DoTranslation("The metadata information needed to install the custom language doesn't provide the necessary localizations needed."));
                            }
                        }
                        else
                        {
                            DebugWriter.Wdbg(DebugLevel.E, "Metadata for language doesn't exist!");
                            throw new Kernel.Exceptions.LanguageInstallException(Translate.DoTranslation("The metadata information needed to install the custom language doesn't exist."));
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Failed to install custom language {0}: {1}", LanguageName, ex.Message);
                    DebugWriter.WStkTrc(ex);
                    Kernel.Kernel.KernelEventManager.RaiseLanguageInstallError(LanguageName, ex);
                    throw new Kernel.Exceptions.LanguageInstallException(Translate.DoTranslation("Failed to install custom language {0}."), ex, LanguageName);
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
                    Kernel.Kernel.KernelEventManager.RaiseLanguagesInstalled();
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Failed to install custom languages: {0}", ex.Message);
                    DebugWriter.WStkTrc(ex);
                    Kernel.Kernel.KernelEventManager.RaiseLanguagesInstallError(ex);
                    throw new Kernel.Exceptions.LanguageInstallException(Translate.DoTranslation("Failed to install custom languages."), ex);
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
                        DebugWriter.Wdbg(DebugLevel.I, "Language {0} exists in {1}", LanguageName, LanguagePath);

                        // Now, check the metadata to see if it has relevant information for the language
                        JToken MetadataToken = JObject.Parse(File.ReadAllText(LanguagePath));
                        DebugWriter.Wdbg(DebugLevel.I, "MetadataToken is null: {0}", MetadataToken is null);
                        if (MetadataToken is not null)
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Metadata exists!");

                            // Uninstall the language
                            if (!CustomLanguages.Remove(LanguageName))
                            {
                                DebugWriter.Wdbg(DebugLevel.E, "Failed to uninstall custom language");
                                throw new Kernel.Exceptions.LanguageUninstallException(Translate.DoTranslation("Failed to uninstall custom language. It most likely doesn't exist."));
                            }
                            Kernel.Kernel.KernelEventManager.RaiseLanguageUninstalled(LanguageName);
                        }
                        else
                        {
                            DebugWriter.Wdbg(DebugLevel.E, "Metadata for language doesn't exist!");
                            throw new Kernel.Exceptions.LanguageUninstallException(Translate.DoTranslation("The metadata information needed to uninstall the custom language doesn't exist."));
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Failed to uninstall custom language {0}: {1}", LanguageName, ex.Message);
                    DebugWriter.WStkTrc(ex);
                    Kernel.Kernel.KernelEventManager.RaiseLanguageUninstallError(LanguageName, ex);
                    throw new Kernel.Exceptions.LanguageUninstallException(Translate.DoTranslation("Failed to uninstall custom language {0}."), ex, LanguageName);
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
                                DebugWriter.Wdbg(DebugLevel.E, "Failed to uninstall custom languages");
                                throw new Kernel.Exceptions.LanguageUninstallException(Translate.DoTranslation("Failed to uninstall custom languages."));
                            }
                        }
                        Kernel.Kernel.KernelEventManager.RaiseLanguagesUninstalled();
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Failed to uninstall custom languages: {0}", ex.Message);
                    DebugWriter.WStkTrc(ex);
                    Kernel.Kernel.KernelEventManager.RaiseLanguagesUninstallError(ex);
                    throw new Kernel.Exceptions.LanguageUninstallException(Translate.DoTranslation("Failed to uninstall custom languages. See the inner exception for more info."), ex);
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
