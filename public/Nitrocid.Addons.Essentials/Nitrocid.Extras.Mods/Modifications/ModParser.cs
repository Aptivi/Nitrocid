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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Textify.Versioning;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel;
using Nitrocid.Files;
using Nitrocid.Misc.Reflection;
using Nitrocid.Misc.Splash;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Events;
using Nitrocid.Security.Signing;
using Nitrocid.Languages.Decoy;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Extras.Mods.Modifications.Dependencies;

namespace Nitrocid.Extras.Mods.Modifications
{
    /// <summary>
    /// Mod parsing module
    /// </summary>
    public static class ModParser
    {

        internal static List<string> queued = [];

        /// <summary>
        /// Gets the mod instance from compiled assembly
        /// </summary>
        /// <param name="Assembly">An assembly</param>
        public static IMod? GetModInstance(Assembly Assembly)
        {
            foreach (Type t in Assembly.GetTypes())
            {
                if (t.GetInterface(typeof(IMod).Name) is not null)
                    return (IMod?)Assembly.CreateInstance(t.FullName ?? "");
            }
            return null;
        }

        /// <summary>
        /// Starts to parse the mod, and configures it so it can be used
        /// </summary>
        /// <param name="modFile">Mod file name with extension. It should end with .dll</param>
        /// <param name="priority">Specifies the mod load priority</param>
        public static void ParseMod(string modFile, ModLoadPriority priority = ModLoadPriority.Optional)
        {
            string ModPath = PathsManagement.GetKernelPath(KernelPathType.Mods);
            if (Path.HasExtension(modFile) && Path.GetExtension(modFile) == ".dll")
            {
                // Mod is a dynamic DLL
                try
                {
                    // Check for signing
                    bool signed = AssemblySigning.IsStronglySigned(ModPath + modFile);
                    if (!signed)
                    {
                        if (ModsInit.ModsConfig.AllowUntrustedMods)
                            SplashReport.ReportProgressWarning(Translate.DoTranslation("The mod is not strongly signed. It may contain untrusted code."));
                        else
                            throw new KernelException(KernelExceptionType.ModManagement, Translate.DoTranslation("The mod is not strongly signed. It may contain untrusted code."));
                    }

                    // Check to see if the DLL is actually a mod
                    var modAsm = Assembly.LoadFrom(ModPath + modFile);
                    var script = GetModInstance(modAsm) ??
                        throw new KernelException(KernelExceptionType.InvalidMod, Translate.DoTranslation("The modfile is invalid."));

                    // Finalize the mod
                    if (script.LoadPriority == priority)
                        FinalizeMods(script, modFile);
                    else
                        DebugWriter.WriteDebug(DebugLevel.W, "Skipping dynamic mod {0} because priority [{1}] doesn't match required priority [{2}]", vars: [modFile, priority, script.LoadPriority]);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", vars: [modFile, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Mod can't be loaded because of the following: "));
                    foreach (Exception? LoaderException in ex.LoaderExceptions)
                    {
                        if (LoaderException is null)
                            continue;
                        DebugWriter.WriteDebug(DebugLevel.E, "Loader exception: {0}", vars: [LoaderException.Message]);
                        DebugWriter.WriteDebugStackTrace(LoaderException);
                        SplashReport.ReportProgressError(LoaderException.Message);
                    }
                    SplashReport.ReportProgressError(Translate.DoTranslation("Contact the vendor of the mod to upgrade the mod to the compatible version."));
                }
                catch (TargetInvocationException ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", vars: [modFile, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Mod can't be loaded because there's an incompatibility between this version of the kernel and this mod:") + $" {ex.Message}");
                    SplashReport.ReportProgressError(Translate.DoTranslation("Here's a list of errors that may help you investigate this incompatibility:"));
                    Exception? inner = ex.InnerException;
                    while (inner != null)
                    {
                        SplashReport.ReportProgressError(inner.Message);
                        inner = inner.InnerException;
                    }
                    SplashReport.ReportProgressError(Translate.DoTranslation("Contact the vendor of the mod to upgrade the mod to the compatible version."));
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", vars: [modFile, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Mod can't be loaded because of the following: ") + ex.Message);
                }
            }
            else
            {
                // Ignore unsupported files
                DebugWriter.WriteDebug(DebugLevel.W, "Unsupported file type for mod file {0}.", vars: [modFile]);
            }
        }

        /// <summary>
        /// Configures the mod so it can be used
        /// </summary>
        /// <param name="script">Instance of script</param>
        /// <param name="modFile">Mod file name with extension. It should end with .dll</param>
        public static void FinalizeMods(IMod script, string modFile)
        {
            ModInfo ModInstance;

            // Try to finalize mod
            if (script is not null)
            {
                string ModPath = PathsManagement.GetKernelPath(KernelPathType.Mods);
                string modFilePath = FilesystemTools.NeutralizePath(modFile, ModPath);
                EventsManager.FireEvent(EventType.ModParsed, modFile);
                try
                {
                    // Add mod dependencies folder (if any) to the private appdomain lookup folder
                    string ModDepPath = ModPath + "Deps/" + Path.GetFileNameWithoutExtension(modFile) + "-" + FileVersionInfo.GetVersionInfo(ModPath + modFile).FileVersion + "/";
                    AssemblyLookup.baseAssemblyLookupPaths.Add(ModDepPath);

                    // Check the API version defined by mod to ensure that we don't load mods that are API incompatible
                    try
                    {
                        if (KernelMain.ApiVersion != script.MinimumSupportedApiVersion)
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Trying to load mod {0} that requires minimum api version {1} on api {2}", vars: [modFile, script.MinimumSupportedApiVersion.ToString(), KernelMain.ApiVersion.ToString()]);
                            SplashReport.ReportProgressError(Translate.DoTranslation("Mod {0} requires exactly an API version {1}, but you have version {2}. Upgrading Nitrocid KS and/or the mod usually helps. Mod parsing failed."), modFile, script.MinimumSupportedApiVersion.ToString(), KernelMain.ApiVersion.ToString());
                            return;
                        }
                    }
                    catch
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Trying to load mod {0} that has undeterminable minimum API version.", vars: [modFile]);
                        SplashReport.ReportProgress(Translate.DoTranslation("Mod {0} may not work properly with this API version. Mod may fail to start up. Contact the mod vendor to get a latest copy."), modFile);
                    }

                    // Locate the mod's localization files
                    string ModLocalizationPath = ModPath + "Localization/" + Path.GetFileNameWithoutExtension(modFile) + "-" + FileVersionInfo.GetVersionInfo(ModPath + modFile).FileVersion + "/";
                    Dictionary<string, string[]> localizations = [];
                    if (FilesystemTools.FolderExists(ModLocalizationPath))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Found mod localization collection in {0}", vars: [ModLocalizationPath]);
                        foreach (string ModLocFile in Directory.GetFiles(ModLocalizationPath, "*.json", SearchOption.AllDirectories))
                        {
                            // This json file, as always, contains "Name" (ignored), "Transliterable" (ignored), and "Localizations" keys.
                            string LanguageName = Path.GetFileNameWithoutExtension(ModLocFile);
                            string ModLocFileContents = FilesystemTools.ReadContentsText(ModLocFile);
                            var modLocs = JsonConvert.DeserializeObject<LanguageLocalizations[]>(ModLocFileContents) ??
                                throw new KernelException(KernelExceptionType.ModManagement, Translate.DoTranslation("Can't load mod localizations"));
                            DebugWriter.WriteDebug(DebugLevel.I, "{0} localizations.", vars: [modLocs.Length]);
                            foreach (var modLoc in modLocs)
                            {
                                // Parse the values and install the language
                                var ParsedLanguageLocalizations = modLoc.Localizations;

                                // Check to see if we have that language...
                                if (!LanguageManager.Languages.ContainsKey(LanguageName))
                                {
                                    DebugWriter.WriteDebug(DebugLevel.E, "Metadata contains nonexistent language!");
                                    SplashReport.ReportProgressError(Translate.DoTranslation("Invalid language") + " {0} [{1}]", LanguageName, LanguageName);
                                    return;
                                }

                                // Check the localizations...
                                DebugWriter.WriteDebug(DebugLevel.I, "Checking localizations... (Null: {0})", vars: [ParsedLanguageLocalizations is null]);
                                if (ParsedLanguageLocalizations is not null)
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Valid localizations found! Length: {0}", vars: [ParsedLanguageLocalizations.Length]);

                                    // Try to install the localizations
                                    if (!localizations.ContainsKey(LanguageName))
                                        localizations.Add(LanguageName, LanguageManager.ProbeLocalizations(modLoc));
                                }
                                else
                                {
                                    DebugWriter.WriteDebug(DebugLevel.E, "Metadata doesn't contain valid localizations!");
                                    SplashReport.ReportProgressError(Translate.DoTranslation("The metadata information needed to install the custom language doesn't provide the necessary localizations needed."));
                                    return;
                                }
                            }
                            if (modLocs.Length == 0)
                            {
                                DebugWriter.WriteDebug(DebugLevel.E, "Metadata for language doesn't exist!");
                                SplashReport.ReportProgressError(Translate.DoTranslation("The metadata information needed to install the custom language doesn't exist."));
                                return;
                            }
                        }
                    }

                    // See if the mod has name
                    string ModName = script.Name;
                    if (string.IsNullOrWhiteSpace(ModName))
                    {
                        // Mod has no name!
                        DebugWriter.WriteDebug(DebugLevel.E, "No name for {0}", vars: [modFile]);
                        SplashReport.ReportProgressError(Translate.DoTranslation("Mod {0} does not have the name. Mod parsing failed. Review the source code."), modFile);
                        return;
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "Mod name: {0}", vars: [ModName]);

                    // See if the mod has version
                    if (string.IsNullOrWhiteSpace(script.Version))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "{0}.Version = \"\" | {0}.Name = {1}", vars: [modFile, script.Name]);
                        SplashReport.ReportProgressError(Translate.DoTranslation("Mod {0} does not have the version. Mod parsing failed. Review the source code."), modFile);
                        return;
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "{0}.Version = {2} | {0}.Name = {1}", vars: [modFile, script.Name, script.Version]);
                        try
                        {
                            // Parse the semantic version of the mod
                            var versionInfo = SemVer.Parse(script.Version);
                            SplashReport.ReportProgress(Translate.DoTranslation("{0} v{1} started"), script.Name, script.Version);
                        }
                        catch (Exception ex)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse mod version {0}: {1}", vars: [script.Version, ex.Message]);
                            DebugWriter.WriteDebugStackTrace(ex);
                            SplashReport.ReportProgressError(Translate.DoTranslation("Mod {0} contains invalid version. Mod parsing failed. Version was") + ": {1}\n{2}", modFile, script.Version, ex.Message);
                            return;
                        }
                    }

                    // Prepare the mod and part instances
                    queued.Add(modFilePath);
                    ModInstance = new ModInfo(ModName, modFile, modFilePath, script, script.Version, localizations);

                    // Satisfy the dependencies
                    ModDependencySatisfier.SatisfyDependencies(ModInstance);

                    // Start the mod
                    script.StartMod();
                    DebugWriter.WriteDebug(DebugLevel.I, "script.StartMod() initialized. Mod name: {0} | Version: {1}", vars: [script.Name, script.Version]);

                    // Now, add the part
                    bool modFound = ModManager.Mods.ContainsKey(ModName);
                    if (!modFound)
                        ModManager.Mods.Add(ModName, ModInstance);

                    // Raise event
                    EventsManager.FireEvent(EventType.ModFinalized, modFile);
                }
                catch (Exception ex)
                {
                    EventsManager.FireEvent(EventType.ModFinalizationFailed, modFile, ex.Message);
                    DebugWriter.WriteDebug(DebugLevel.E, "Finalization failed for {0}: {1}", vars: [modFile, ex.Message]);
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
                DebugWriter.WriteDebug(DebugLevel.E, "Script is not provided to finalize {0}!", vars: [modFile]);
            }
        }

    }
}
