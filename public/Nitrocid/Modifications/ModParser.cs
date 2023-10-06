
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using KS.Files;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Splash;
using KS.Modifications.ManPages;
using KS.Kernel.Events;
using Newtonsoft.Json.Linq;
using SemanVer.Instance;
using KS.Modifications.Dependencies;
using KS.Kernel.Configuration;
using KS.Files.Operations;
using KS.Files.Operations.Querying;

namespace KS.Modifications
{
    /// <summary>
    /// Mod parsing module
    /// </summary>
    public static class ModParser
    {

        internal static List<string> queued = new();

        /// <summary>
        /// Gets the mod instance from compiled assembly
        /// </summary>
        /// <param name="Assembly">An assembly</param>
        public static IMod GetModInstance(Assembly Assembly)
        {
            foreach (Type t in Assembly.GetTypes())
            {
                if (t.GetInterface(typeof(IMod).Name) is not null)
                    return (IMod)Assembly.CreateInstance(t.FullName);
            }
            return null;
        }

        /// <summary>
        /// Starts to parse the mod, and configures it so it can be used
        /// </summary>
        /// <param name="modFile">Mod file name with extension. It should end with .dll</param>
        public static void ParseMod(string modFile)
        {
            string ModPath = Paths.GetKernelPath(KernelPathType.Mods);
            if (modFile.EndsWith(".dll"))
            {
                // Mod is a dynamic DLL
                try
                {
                    // Load the mod assembly
                    var modAsm = Assembly.LoadFrom(ModPath + modFile);

                    // Check the public key
                    var modAsmName = new AssemblyName(modAsm.FullName);
                    var modAsmPublicKey = modAsmName.GetPublicKeyToken();
                    if (modAsmPublicKey is null || modAsmPublicKey.Length == 0)
                    {
                        if (KernelFlags.AllowUntrustedMods)
                            SplashReport.ReportProgressWarning(Translate.DoTranslation("The mod is not strongly signed. It may contain untrusted code."));
                        else
                            throw new KernelException(KernelExceptionType.ModManagement, Translate.DoTranslation("The mod is not strongly signed. It may contain untrusted code."));
                    }

                    // Check to see if the DLL is actually a mod
                    var script = GetModInstance(modAsm) ??
                        throw new KernelException(KernelExceptionType.InvalidMod, Translate.DoTranslation("The modfile is invalid."));

                    // Finalize the mod
                    FinalizeMods(script, modFile);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", modFile, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Mod can't be loaded because of the following: "));
                    foreach (Exception LoaderException in ex.LoaderExceptions)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Loader exception: {0}", LoaderException.Message);
                        DebugWriter.WriteDebugStackTrace(LoaderException);
                        SplashReport.ReportProgressError(LoaderException.Message);
                    }
                    SplashReport.ReportProgressError(Translate.DoTranslation("Contact the vendor of the mod to upgrade the mod to the compatible version."));
                }
                catch (TargetInvocationException ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", modFile, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Mod can't be loaded because there's an incompatibility between this version of the kernel and this mod:") + $" {ex.Message}");
                    SplashReport.ReportProgressError(Translate.DoTranslation("Here's a list of errors that may help you investigate this incompatibility:"));
                    Exception inner = ex.InnerException;
                    while (inner != null)
                    {
                        SplashReport.ReportProgressError(inner.Message);
                        inner = inner.InnerException;
                    }
                    SplashReport.ReportProgressError(Translate.DoTranslation("Contact the vendor of the mod to upgrade the mod to the compatible version."));
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", modFile, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Mod can't be loaded because of the following: ") + ex.Message);
                }
            }
            else
            {
                // Ignore unsupported files
                DebugWriter.WriteDebug(DebugLevel.W, "Unsupported file type for mod file {0}.", modFile);
            }
        }

        /// <summary>
        /// Configures the mod so it can be used
        /// </summary>
        /// <param name="script">Instance of script</param>
        /// <param name="modFile">Mod file name with extension. It should end with .dll</param>
        public static void FinalizeMods(IMod script, string modFile)
        {
            var ModParts = new Dictionary<string, ModPartInfo>();
            ModInfo ModInstance;
            ModPartInfo PartInstance;

            // Try to finalize mod
            if (script is not null)
            {
                string ModPath = Paths.GetKernelPath(KernelPathType.Mods);
                string modFilePath = Filesystem.NeutralizePath(modFile, ModPath);
                EventsManager.FireEvent(EventType.ModParsed, modFile);
                try
                {
                    // Add mod dependencies folder (if any) to the private appdomain lookup folder
                    string ModDepPath = ModPath + "Deps/" + Path.GetFileNameWithoutExtension(modFile) + "-" + FileVersionInfo.GetVersionInfo(ModPath + modFile).FileVersion + "/";
                    AssemblyLookup.AddPathToAssemblySearchPath(ModDepPath);

                    // Check the API version defined by mod to ensure that we don't load mods that are API incompatible
                    try
                    {
                        if (KernelTools.KernelApiVersion != script.MinimumSupportedApiVersion)
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Trying to load mod {0} that requires minimum api version {1} on api {2}", modFile, script.MinimumSupportedApiVersion.ToString(), KernelTools.KernelApiVersion.ToString());
                            SplashReport.ReportProgressError(Translate.DoTranslation("Mod {0} requires exactly an API version {1}, but you have version {2}. Upgrading Nitrocid KS and/or the mod usually helps. Mod parsing failed."), modFile, script.MinimumSupportedApiVersion.ToString(), KernelTools.KernelApiVersion.ToString());
                            return;
                        }
                    }
                    catch
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Trying to load mod {0} that has undeterminable minimum API version.", modFile);
                        SplashReport.ReportProgress(Translate.DoTranslation("Mod {0} may not work properly with this API version. Mod may fail to start up. Contact the mod vendor to get a latest copy."), 0, modFile);
                    }

                    // Locate the mod's localization files
                    string ModLocalizationPath = ModPath + "Localization/" + Path.GetFileNameWithoutExtension(modFile) + "-" + FileVersionInfo.GetVersionInfo(ModPath + modFile).FileVersion + "/";
                    Dictionary<string, Dictionary<string, string>> localizations = new();
                    if (Checking.FolderExists(ModLocalizationPath))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Found mod localization collection in {0}", ModLocalizationPath);
                        foreach (string ModManualFile in Directory.EnumerateFiles(ModLocalizationPath, "*.json", SearchOption.AllDirectories))
                        {
                            // This json file, as always, contains "Name" (ignored), "Transliterable" (ignored), and "Localizations" keys.
                            string LanguageName = Path.GetFileNameWithoutExtension(ModManualFile);
                            string ModManualFileContents = Reading.ReadContentsText(ModManualFile);
                            JToken MetadataToken = JObject.Parse(ModManualFileContents);
                            DebugWriter.WriteDebug(DebugLevel.I, "MetadataToken is null: {0}", MetadataToken is null);
                            if (MetadataToken is not null)
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Metadata exists!");

                                // Parse the values and install the language
                                var ParsedLanguageLocalizations = MetadataToken.SelectToken("Localizations");

                                // Check to see if we have that language...
                                if (!LanguageManager.Languages.ContainsKey(LanguageName))
                                {
                                    DebugWriter.WriteDebug(DebugLevel.E, "Metadata contains nonexistent language!");
                                    SplashReport.ReportProgressError(Translate.DoTranslation("Invalid language") + " {0} [{1}]", LanguageName, LanguageName);
                                    return;
                                }

                                // Check the localizations...
                                DebugWriter.WriteDebug(DebugLevel.I, "Checking localizations... (Null: {0})", ParsedLanguageLocalizations is null);
                                if (ParsedLanguageLocalizations is not null)
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Valid localizations found! Length: {0}", ParsedLanguageLocalizations.Count());

                                    // Try to install the localizations
                                    if (!localizations.ContainsKey(LanguageName))
                                        localizations.Add(LanguageName, LanguageManager.ProbeLocalizations((JObject)MetadataToken));
                                }
                                else
                                {
                                    DebugWriter.WriteDebug(DebugLevel.E, "Metadata doesn't contain valid localizations!");
                                    SplashReport.ReportProgressError(Translate.DoTranslation("The metadata information needed to install the custom language doesn't provide the necessary localizations needed."));
                                    return;
                                }
                            }
                            else
                            {
                                DebugWriter.WriteDebug(DebugLevel.E, "Metadata for language doesn't exist!");
                                SplashReport.ReportProgressError(Translate.DoTranslation("The metadata information needed to install the custom language doesn't exist."));
                                return;
                            }
                        }
                    }

                    // See if the mod has part name
                    if (string.IsNullOrWhiteSpace(script.ModPart))
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "No part name for {0}", modFile);
                        SplashReport.ReportProgressError(Translate.DoTranslation("Mod {0} does not have the part name. Mod parsing failed. Review the source code."), modFile);
                        return;
                    }

                    // See if the mod has name
                    string ModName = script.Name;
                    if (string.IsNullOrWhiteSpace(ModName))
                    {
                        // Mod has no name! Give it a file name.
                        ModName = modFile;
                        DebugWriter.WriteDebug(DebugLevel.W, "No name for {0}", modFile);
                        SplashReport.ReportProgress(Translate.DoTranslation("Mod {0} does not have the name. Review the source code."), 0, modFile);
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "There is a name for {0}", modFile);
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "Mod name: {0}", ModName);

                    // Check to see if there is a part under the same name.
                    bool modFound = ModManager.Mods.ContainsKey(ModName);
                    var Parts = modFound ? ModManager.Mods[ModName].ModParts : ModParts;
                    DebugWriter.WriteDebug(DebugLevel.I, "Adding mod part {0}...", script.ModPart);
                    if (Parts.ContainsKey(script.ModPart))
                    {
                        // Append the number to the end of the name
                        DebugWriter.WriteDebug(DebugLevel.W, "There is a conflict with {0}. Appending item number...", script.ModPart);
                        script.ModPart = $"{script.ModPart} [{Parts.Count}]";
                    }

                    // See if the mod has version
                    if (string.IsNullOrWhiteSpace(script.Version))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "{0}.Version = \"\" | {0}.Name = {1}", modFile, script.Name);
                        SplashReport.ReportProgress(Translate.DoTranslation("Mod {0} does not have the version. Setting as") + " 1.0.0...", 0, script.Name);
                        script.Version = "1.0.0";
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "{0}.Version = {2} | {0}.Name = {1}", modFile, script.Name, script.Version);
                        try
                        {
                            // Parse the semantic version of the mod
                            var versionInfo = SemVer.Parse(script.Version);
                            SplashReport.ReportProgress(Translate.DoTranslation("{0} v{1} started") + " ({2})", 0, script.Name, script.Version, script.ModPart);
                        }
                        catch (Exception ex)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse mod version {0}: {1}", script.Version, ex.Message);
                            DebugWriter.WriteDebugStackTrace(ex);
                            SplashReport.ReportProgressError(Translate.DoTranslation("Mod {0} contains invalid version. Mod parsing failed. Version was") + ": {1}\n{2}", modFile, script.Version, ex.Message);
                            return;
                        }
                    }

                    // Prepare the mod and part instances
                    PartInstance = new ModPartInfo(ModName, script.ModPart, modFile, modFilePath, script);
                    Parts.Add(script.ModPart, PartInstance);
                    queued.Add(modFilePath);
                    ModInstance = new ModInfo(ModName, modFile, modFilePath, Parts, script.Version, localizations);

                    // Satisfy the dependencies
                    ModDependencySatisfier.SatisfyDependencies(ModInstance);

                    // Start the mod
                    script.StartMod();
                    DebugWriter.WriteDebug(DebugLevel.I, "script.StartMod() initialized. Mod name: {0} | Mod part: {1} | Version: {2}", script.Name, script.ModPart, script.Version);

                    // Now, add the part
                    if (!modFound)
                        ModManager.Mods.Add(ModName, ModInstance);

                    // Check for accompanying manual pages for mods
                    string ModManualPath = Filesystem.NeutralizePath(modFile + ".manual", ModPath);
                    if (Checking.FolderExists(ModManualPath))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Found manual page collection in {0}", ModManualPath);
                        foreach (string ModManualFile in Directory.EnumerateFiles(ModManualPath, "*.man", SearchOption.AllDirectories))
                            PageParser.InitMan(ModName, ModManualFile);
                    }

                    // Raise event
                    EventsManager.FireEvent(EventType.ModFinalized, modFile);
                }
                catch (Exception ex)
                {
                    EventsManager.FireEvent(EventType.ModFinalizationFailed, modFile, ex.Message);
                    DebugWriter.WriteDebug(DebugLevel.E, "Finalization failed for {0}: {1}", modFile, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Failed to finalize mod {0}: {1}"), modFile, ex.Message);
                }
                finally
                {
                    queued.Remove(modFilePath);
                }
            }
            else
            {
                EventsManager.FireEvent(EventType.ModParseError, modFile);
                DebugWriter.WriteDebug(DebugLevel.E, "Script is not provided to finalize {0}!", modFile);
            }
        }

    }
}
