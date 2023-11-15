//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Files;
using KS.Files.Folders;
using KS.Files.Instances;
using KS.Files.Operations;
using KS.Files.Operations.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Reflection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace KS.Kernel.Extensions
{
    internal static class AddonTools
    {
        internal static readonly List<string> probedAddons = [];
        private static readonly List<AddonInfo> addons = [];
        private const string windowsSuffix = ".Windows";

        internal static List<AddonInfo> ListAddons() =>
            new(addons);

        internal static AddonInfo GetAddon(string addonName)
        {
            AddonInfo addon = addons.Find((ai) => ai.AddonName == addonName);
            return addon;
        }

        internal static void ProcessAddons(AddonType type)
        {
            var addonFolder = Paths.AddonsPath;
            if (!Checking.FolderExists(addonFolder))
                return;
            var addonFolders = Listing.GetFilesystemEntries(addonFolder);
            DebugWriter.WriteDebug(DebugLevel.I, "Found {0} files under the addon folder {1}.", addonFolders.Length, addonFolder);
            foreach (var addon in addonFolders)
                ProcessAddon(addon, type);
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded all addons!");
            probedAddons.Clear();
        }

        internal static void ProcessAddon(string addon, AddonType type)
        {
            try
            {
                // First, check the platform
                string windowsAddonPath = addon + windowsSuffix;
                if (KernelPlatform.IsOnWindows() && Checking.FolderExists(windowsAddonPath))
                    addon = windowsAddonPath;
                if (addon.EndsWith(windowsSuffix) && !KernelPlatform.IsOnWindows())
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping addon entry {0} because it's built for Windows...", addon);
                    return;
                }

                // Get the folder info and recurse through them to get actual addon file
                DebugWriter.WriteDebug(DebugLevel.I, "Processing addon entry {0}...", addon);
                var folderInfo = new FileSystemEntry(addon);
                if (folderInfo.Type != FileSystemEntryType.Directory)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping addon entry {0}...", addon);
                    if (folderInfo.Type != FileSystemEntryType.Directory)
                        DebugWriter.WriteDebug(DebugLevel.W, "Addon entry {0} is not a directory!", addon);
                    if (!folderInfo.Exists)
                        DebugWriter.WriteDebug(DebugLevel.W, "Addon entry {0} doesn't exist!", addon);
                    return;
                }

                // Now, verify that we have the addon metadata
                string metadataPath = $"{addon}/AddonMetadata.json";
                DebugWriter.WriteDebug(DebugLevel.I, "Verifying addon folder {0} for metadata...", addon);
                if (!(Checking.FileExists(metadataPath) && Parsing.IsJson(metadataPath)))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping addon entry {0} because of invalid metadata [{1}]...", addon, metadataPath);
                    return;
                }

                // Read the metadata
                DebugWriter.WriteDebug(DebugLevel.I, "Metadata {0} found!", metadataPath);
                string metadataContents = Reading.ReadContentsText(metadataPath);
                JToken metadataToken = JToken.Parse(metadataContents);

                // Check the metadata value
                string addonPath = $"{addon}/{(string)(metadataToken["DllPath"] ?? Path.GetFileName($"{addon}.dll"))}";

                // Now, check the addon path
                if (!Checking.FileExists(addonPath))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping addon entry {0} because of nonexistent file [{1}]...", addon, addonPath);
                    return;
                }
                if (!ReflectionCommon.IsDotnetAssemblyFile(addonPath, out AssemblyName asmName))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping addon entry {0} because of invalid .NET assembly file [{1}]...", addon, addonPath);
                    return;
                }

                // Now, process the assembly
                if (probedAddons.Contains(addonPath))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping addon entry {0} because of conflicts with the already-loaded addon in the queue [{1}]...", addon, addonPath);
                    return;
                }
                probedAddons.Add(addonPath);
                var asm = Assembly.LoadFrom(addonPath);
                var addonInstance = GetAddonInstance(asm) ??
                    throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("This addon is not a valid addon.") + $" {addonPath}");

                // Check to see if the types match
                if (addonInstance.AddonType == type)
                {
                    // Call the start function
                    try
                    {
                        addonInstance.StartAddon();
                        DebugWriter.WriteDebug(DebugLevel.I, "Started!");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to start addon {0}. {1}", addon, ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                    }

                    // Add the addon
                    AddonInfo info = new(addonInstance);
                    if (!addons.Where((addon) => addonInstance.AddonName == addon.AddonName).Any())
                        addons.Add(info);
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded addon!");
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to load addon {0}. {1}", addon, ex.Message);
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
                    DebugWriter.WriteDebug(DebugLevel.I, "Finalizing addon {0}...", addonInfo.AddonName);
                    addonInfo.Addon.FinalizeAddon();
                    DebugWriter.WriteDebug(DebugLevel.I, "Finalized addon {0}!", addonInfo.AddonName);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to finalize addon {0}. {1}", addonInfo.AddonName, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    errors.Add(addonInfo.AddonName, ex is KernelException kex ? kex.OriginalExceptionMessage : ex.Message);
                }
            }
            if (errors.Any())
                throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("Failed to finalize addons. Below addons have failed to finalize:") + $"\n  - {string.Join("\n  - ", errors.Select((kvp) => $"{kvp.Key}: {kvp.Value}"))}");
        }

        internal static void UnloadAddons()
        {
            Dictionary<string, string> errors = [];
            for (int addonIdx = addons.Count - 1; addonIdx >= 0; addonIdx--)
            {
                var addonInstance = addons[addonIdx].Addon;
                try
                {
                    addonInstance.StopAddon();
                    addons.RemoveAt(addonIdx);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to stop addon {0}. {1}", addonInstance.AddonName, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    errors.Add(addonInstance.AddonName, ex is KernelException kex ? kex.OriginalExceptionMessage : ex.Message);
                }
            }
            if (errors.Any())
                throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("Failed to stop addons. Below addons have failed to stop:") + $"\n  - {string.Join("\n  - ", errors.Select((kvp) => $"{kvp.Key}: {kvp.Value}"))}");
        }

        /// <summary>
        /// Gets the addon instance from compiled assembly
        /// </summary>
        /// <param name="Assembly">An assembly</param>
        private static IAddon GetAddonInstance(Assembly Assembly)
        {
            foreach (Type t in Assembly.GetTypes())
            {
                if (t.GetInterface(typeof(IAddon).Name) is not null)
                    return (IAddon)Assembly.CreateInstance(t.FullName);
            }
            return null;
        }
    }
}
