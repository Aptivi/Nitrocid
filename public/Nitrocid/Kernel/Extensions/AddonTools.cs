
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

using KS.Files;
using KS.Files.Folders;
using KS.Files.Instances;
using KS.Files.Querying;
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
using System.Text;
using System.Threading.Tasks;

namespace KS.Kernel.Extensions
{
    internal static class AddonTools
    {
        private static readonly List<AddonInfo> addons = new();

        internal static void ProcessAddons()
        {
            var addonFolder = Paths.AddonsPath;
            if (!Checking.FolderExists(addonFolder))
                return;
            var addonFolders = Listing.GetFilesystemEntries(addonFolder);
            DebugWriter.WriteDebug(DebugLevel.I, "Found {0} files under the addon folder {1}.", addonFolders.Length, addonFolder);
            foreach (var addon in addonFolders)
                ProcessAddon(addon);
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded all addons!");
        }

        internal static void ProcessAddon(string addon)
        {
            try
            {
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
                string metadataContents = File.ReadAllText(metadataPath);
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

                // Now, process the assembly and call the start function
                var asm = Assembly.LoadFrom(addonPath);
                var addonInstance = GetAddonInstance(asm) ??
                    throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("This addon is not a valid addon.") + $" {addonPath}");
                addonInstance.StartAddon();

                // Add the addon
                AddonInfo info = new(addonInstance);
                if (!addons.Select((addon) => addon.AddonName).Any())
                    addons.Add(info);
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded addon!");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to load addon {0}. {1}", addon, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
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
