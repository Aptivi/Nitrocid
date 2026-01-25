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

#if NKS_EXTENSIONS
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Nitrocid.Base.Files;
using Nitrocid.Base.Files.Instances;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Base.Security.Signing;
using Terminaux.Writer.CyclicWriters.Graphical.Shapes;

namespace Nitrocid.Base.Kernel.Extensions
{
    internal static class AddonTools
    {
        internal static readonly List<string> probedAddons = [];
        private static readonly List<AddonInfo> addons = [];
        private const string windowsSuffix = ".Windows";

        internal static List<AddonInfo> ListAddons() =>
            new(addons);

        internal static AddonInfo? GetAddon(string addonName)
        {
            AddonInfo? addon = addons.Find((ai) => ai.AddonName == addonName);
            return addon;
        }

        internal static string[] GetAddons() =>
            addons.Select((ai) => ai.AddonName).ToArray();

        internal static void ProcessAddons(ModLoadPriority type)
        {
            var addonFolder = type == ModLoadPriority.Important ? PathsManagement.AddonsEssentialsPath : PathsManagement.AddonsPath;
            if (!FilesystemTools.FolderExists(addonFolder))
                return;
            var addonFolders = FilesystemTools.GetFilesystemEntries(addonFolder);
            DebugWriter.WriteDebug(DebugLevel.I, "Found {0} files under the addon folder {1}.", vars: [addonFolders.Length, addonFolder]);
            for (int i = 0; i < addonFolders.Length; i++)
            {
                string addon = addonFolders[i];
                ProcessAddon(addon, i + 1, addonFolders.Length);
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded all addons!");
            probedAddons.Clear();
        }

        internal static void ProcessAddon(string addon, int current = 1, int length = 1)
        {
            try
            {
                // First, check the platform
                string windowsAddonPath = addon + windowsSuffix;
                if (KernelPlatform.IsOnWindows() && FilesystemTools.FolderExists(windowsAddonPath))
                    addon = windowsAddonPath;
                if (addon.EndsWith(windowsSuffix) && !KernelPlatform.IsOnWindows())
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping addon entry {0} because it's built for Windows...", vars: [addon]);
                    return;
                }

                // Get the folder info and recurse through them to get actual addon file
                DebugWriter.WriteDebug(DebugLevel.I, "Processing addon entry {0}...", vars: [addon]);
                var folderInfo = new FileSystemEntry(addon);
                if (folderInfo.Type != FileSystemEntryType.Directory)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping addon entry {0}...", vars: [addon]);
                    if (folderInfo.Type != FileSystemEntryType.Directory)
                        DebugWriter.WriteDebug(DebugLevel.W, "Addon entry {0} is not a directory!", vars: [addon]);
                    if (!folderInfo.Exists)
                        DebugWriter.WriteDebug(DebugLevel.W, "Addon entry {0} doesn't exist!", vars: [addon]);
                    return;
                }

                // Now, guess and check the addon path
                DebugWriter.WriteDebug(DebugLevel.I, "Guessing addon path {0}...", vars: [addon]);
                string addonPath = $"{addon}/Nitrocid.{Path.GetFileName($"{addon}.dll")}";
                SplashReport.ReportProgress($"[{current}/{length}] " + LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_ADDONS_CHECKING") + " {0}...", Path.GetFileName(addonPath));
                DebugWriter.WriteDebug(DebugLevel.I, "Addon entry {0} is using path [{1}].", vars: [addon, addonPath]);
                if (!FilesystemTools.FileExists(addonPath))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping addon entry {0} because of nonexistent file [{1}]...", vars: [addon, addonPath]);
                    return;
                }
                if (!ReflectionCommon.IsDotnetAssemblyFile(addonPath, out AssemblyName? asmName))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping addon entry {0} because of invalid .NET assembly file [{1}]...", vars: [addon, addonPath]);
                    return;
                }
                if (asmName is null)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping addon entry {0} because of no assembly name [{1}]...", vars: [addon, addonPath]);
                    return;
                }

                // Verify that the addon holds the same key as the Nitrocid main executable
                if (!AssemblySigning.IsStronglySigned(asmName))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping addon entry {0} because of no public key signing [{1}]...", vars: [addon, addonPath]);
                    return;
                }
                var mainKey = AssemblySigning.PublicKeyToken(Assembly.GetExecutingAssembly());
                var addonKey = AssemblySigning.PublicKeyToken(asmName);
                if (!mainKey.SequenceEqual(addonKey))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping addon entry {0} because of key mismatch [{1}]...", vars: [addon, addonPath]);
                    DebugWriter.WriteDebug(DebugLevel.W, "Expected key: {0}", vars: [string.Join(", ", mainKey)]);
                    DebugWriter.WriteDebug(DebugLevel.W, "Actual key:   {1}", vars: [string.Join(", ", addonKey)]);
                    return;
                }

                // Now, process the assembly
                if (probedAddons.Contains(addonPath))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping addon entry {0} because of conflicts with the already-loaded addon in the queue [{1}]...", vars: [addon, addonPath]);
                    return;
                }
                SplashReport.ReportProgress($"[{current}/{length}] " + LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_ADDONS_INITIALIZING") + " {0}...", asmName.Name ?? LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_ADDONS_INITIALIZING_UNKNOWN"));
                DebugWriter.WriteDebug(DebugLevel.I, $"[{current}/{length}] Initializing kernel addon {Path.GetFileName(addon)}...");
                probedAddons.Add(addonPath);
                AssemblyLookup.baseAssemblyLookupPaths.Add(addon);
                IAddon addonInstance;

                // Try to load the addon assembly
                var alc = new AssemblyLoadContext(addon, true);
                var asm = alc.LoadFromAssemblyPath(addonPath);
                addonInstance = GetAddonInstance(asm) ??
                    throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_ADDONS_EXCEPTION_ADDONINVALID") + $" {addonPath}");

                // Call the start function
                try
                {
                    SplashReport.ReportProgress($"[{current}/{length}] " + LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_ADDONS_STARTING") + " {0}...", addonInstance.AddonTranslatedName);
                    using (var context = alc.EnterContextualReflection())
                    {
                        addonInstance.StartAddon();
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "Started!");

                    // Add the addon
                    AddonInfo info = new(addonInstance, alc);
                    if (!addons.Where((addon) => addonInstance.AddonName == addon.AddonName).Any())
                        addons.Add(info);
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded addon!");
                    SplashReport.ReportProgress($"[{current}/{length}] " + LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_ADDONS_STARTED") + " {0}!", 1, addonInstance.AddonTranslatedName);
                }
                catch (Exception ex)
                {
                    SplashReport.ReportProgressError($"[{current}/{length}] " + LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_ADDONS_STARTFAILED") + " {0}.", addonInstance.AddonTranslatedName);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to start addon {0}. {1}", vars: [addon, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            catch (Exception ex)
            {
                SplashReport.ReportProgressError($"[{current}/{length}] " + LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_ADDONS_INITIALIZEFAILED") + " {0}.", Path.GetFileName(addon));
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to load addon {0}. {1}", vars: [addon, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        internal static void FinalizeAddons()
        {
            Dictionary<string, string> errors = [];
            foreach (var addonInfo in addons)
            {
                try
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Finalizing addon {0}...", vars: [addonInfo.AddonName]);
                    addonInfo.Addon.FinalizeAddon();
                    DebugWriter.WriteDebug(DebugLevel.I, "Finalized addon {0}!", vars: [addonInfo.AddonName]);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to finalize addon {0}. {1}", vars: [addonInfo.AddonName, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    errors.Add(addonInfo.AddonName, ex is KernelException kex ? kex.OriginalExceptionMessage : ex.Message);
                }
            }
            if (errors.Count != 0)
                throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_ADDONS_EXCEPTION_FINALIZEFAILED") + $"\n  - {string.Join("\n  - ", errors.Select((kvp) => $"{kvp.Key}: {kvp.Value}"))}");
        }

        internal static void UnloadAddons()
        {
            Dictionary<string, string> errors = [];
            for (int addonIdx = addons.Count - 1; addonIdx >= 0; addonIdx--)
            {
                var addonInfo = addons[addonIdx];
                var addonInstance = addonInfo.Addon;
                var alc = addonInfo.alc;
                try
                {
                    using var context = alc.EnterContextualReflection();
                    addonInstance.StopAddon();
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to stop addon {0}. {1}", vars: [addonInfo.AddonName, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    errors.Add(addonInfo.AddonName, ex is KernelException kex ? kex.OriginalExceptionMessage : ex.Message);
                }
                finally
                {
                    // Unload the assembly on garbage collection
                    alc.Unload();
                    addons.RemoveAt(addonIdx);
                }
            }

            // Unload all addon assemblies
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            if (errors.Count != 0)
                throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_ADDONS_EXCEPTION_STOPFAILED") + $"\n  - {string.Join("\n  - ", errors.Select((kvp) => $"{kvp.Key}: {kvp.Value}"))}");
        }

        /// <summary>
        /// Gets the addon instance from compiled assembly
        /// </summary>
        /// <param name="Assembly">An assembly</param>
        private static IAddon? GetAddonInstance(Assembly Assembly)
        {
            foreach (Type t in Assembly.GetTypes())
            {
                if (t.GetInterface(typeof(IAddon).Name) is not null)
                    return (IAddon?)Assembly.CreateInstance(t.FullName ?? "");
            }
            return null;
        }
    }
}
#endif
