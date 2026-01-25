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
using System.Runtime.Loader;
using Nitrocid.Base.Files;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Events;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Base.Security.Signing;
using Nitrocid.Extras.Mods.Modifications.Dependencies;
using Textify.Versioning;

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
                var alc = new AssemblyLoadContext(ModPath, true);
                bool failed = false;

                // Mod is a dynamic DLL
                try
                {
                    // Check for signing
                    string pathToMod = ModPath + modFile;
                    bool signed = AssemblySigning.IsStronglySigned(pathToMod);
                    if (!signed)
                    {
                        if (ModsInit.ModsConfig.AllowUntrustedMods)
                            SplashReport.ReportProgressWarning(LanguageTools.GetLocalized("NKS_MODS_EXCEPTION_UNSAFEMOD"));
                        else
                            throw new KernelException(KernelExceptionType.ModManagement, LanguageTools.GetLocalized("NKS_MODS_EXCEPTION_UNSAFEMOD"));
                    }

                    // Check to see if the DLL is actually a mod
                    var modAsm = alc.LoadFromAssemblyPath(pathToMod);
                    var script = GetModInstance(modAsm) ??
                        throw new KernelException(KernelExceptionType.InvalidMod, LanguageTools.GetLocalized("NKS_MODS_EXCEPTION_INVALIDMODFILE"));

                    // Finalize the mod
                    if (script.LoadPriority == priority)
                        FinalizeMods(script, modFile, alc);
                    else
                        DebugWriter.WriteDebug(DebugLevel.W, "Skipping dynamic mod {0} because priority [{1}] doesn't match required priority [{2}]", vars: [modFile, priority, script.LoadPriority]);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", vars: [modFile, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_MODS_MODCANTLOAD"));
                    foreach (Exception? LoaderException in ex.LoaderExceptions)
                    {
                        if (LoaderException is null)
                            continue;
                        DebugWriter.WriteDebug(DebugLevel.E, "Loader exception: {0}", vars: [LoaderException.Message]);
                        DebugWriter.WriteDebugStackTrace(LoaderException);
                        SplashReport.ReportProgressError(LoaderException.Message);
                    }
                    SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_MODS_MODNEEDSUPGRADE"));
                    failed = true;
                }
                catch (TargetInvocationException ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", vars: [modFile, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_MODS_MODINCOMPATIBLE1") + $" {ex.Message}");
                    SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_MODS_MODINCOMPATIBLE2"));
                    Exception? inner = ex.InnerException;
                    while (inner != null)
                    {
                        SplashReport.ReportProgressError(inner.Message);
                        inner = inner.InnerException;
                    }
                    SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_MODS_MODNEEDSUPGRADE"));
                    failed = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", vars: [modFile, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_MODS_MODCANTLOAD") + ex.Message);
                    failed = true;
                }
                if (failed)
                {
                    alc.Unload();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
            }
            else
            {
                // Ignore unsupported files
                DebugWriter.WriteDebug(DebugLevel.W, "Unsupported file type for mod file {0}.", vars: [modFile]);
            }
        }

        internal static void FinalizeMods(IMod script, string modFile, AssemblyLoadContext alc)
        {
            ModInfo ModInstance;

            // Try to finalize mod
            if (script is not null)
            {
                string ModPath = PathsManagement.GetKernelPath(KernelPathType.Mods);
                string modFilePath = FilesystemTools.NeutralizePath(modFile, ModPath);
                bool failed = false;
                EventsManager.FireEvent(EventType.ModParsed, modFile);
                try
                {
                    // Add mod dependencies folder (if any) to the private appdomain lookup folder
                    string ModDepPath = ModPath + "Deps/" + Path.GetFileNameWithoutExtension(modFile) + "-" + FileVersionInfo.GetVersionInfo(ModPath + modFile).FileVersion + "/";
                    AssemblyLookup.baseAssemblyLookupPaths.Add(ModDepPath);

                    // Check the API version defined by mod to ensure that we don't load mods that are API incompatible
                    try
                    {
                        if (KernelReleaseInfo.ApiVersion != script.MinimumSupportedApiVersion)
                        {
                            failed = true;
                            DebugWriter.WriteDebug(DebugLevel.W, "Trying to load mod {0} that requires minimum api version {1} on api {2}", vars: [modFile, script.MinimumSupportedApiVersion.ToString(), KernelReleaseInfo.ApiVersion.ToString()]);
                            throw new KernelException(KernelExceptionType.InvalidMod, LanguageTools.GetLocalized("NKS_MODS_MODNEEDSAPIEXACT"), modFile, script.MinimumSupportedApiVersion.ToString(), KernelReleaseInfo.ApiVersion.ToString());
                        }
                    }
                    catch
                    {
                        if (failed)
                            throw;
                        DebugWriter.WriteDebug(DebugLevel.W, "Trying to load mod {0} that has undeterminable minimum API version.", vars: [modFile]);
                        SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_MODS_MODHASNOAPIVERSION"), modFile);
                    }

                    // See if the mod has name
                    string ModName = script.Name;
                    if (string.IsNullOrWhiteSpace(ModName))
                    {
                        // Mod has no name!
                        failed = true;
                        DebugWriter.WriteDebug(DebugLevel.E, "No name for {0}", vars: [modFile]);
                        throw new KernelException(KernelExceptionType.InvalidMod, LanguageTools.GetLocalized("NKS_MODS_MODNEDSNAME"), modFile);
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "Mod name: {0}", vars: [ModName]);

                    // See if the mod has version
                    if (string.IsNullOrWhiteSpace(script.Version))
                    {
                        failed = true;
                        DebugWriter.WriteDebug(DebugLevel.I, "{0}.Version = \"\" | {0}.Name = {1}", vars: [modFile, script.Name]);
                        throw new KernelException(KernelExceptionType.InvalidMod, LanguageTools.GetLocalized("NKS_MODS_MODNEEDSVERSION"), modFile);
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "{0}.Version = {2} | {0}.Name = {1}", vars: [modFile, script.Name, script.Version]);
                        try
                        {
                            // Parse the semantic version of the mod
                            var versionInfo = SemVer.Parse(script.Version);
                            SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_MODS_MODSTARTED"), script.Name, script.Version);
                        }
                        catch (Exception ex)
                        {
                            failed = true;
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse mod version {0}: {1}", vars: [script.Version, ex.Message]);
                            DebugWriter.WriteDebugStackTrace(ex);
                            throw new KernelException(KernelExceptionType.InvalidMod, LanguageTools.GetLocalized("NKS_MODS_MODVERSIONINVALID") + ": {1}\n{2}", modFile, script.Version, ex.Message);
                        }
                    }

                    // Prepare the mod and part instances
                    queued.Add(modFilePath);
                    ModInstance = new ModInfo(ModName, modFile, modFilePath, script, script.Version, alc);

                    // Satisfy the dependencies
                    ModDependencySatisfier.SatisfyDependencies(ModInstance);

                    // Start the mod
                    using (var context = alc.EnterContextualReflection())
                    {
                        script.StartMod();
                    }
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
                    SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_MODS_CANTFINALIZE"), modFile, ex.Message);
                    failed = true;
                }
                finally
                {
                    queued.Remove(modFilePath);
                    if (failed)
                    {
                        alc.Unload();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                    }
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
