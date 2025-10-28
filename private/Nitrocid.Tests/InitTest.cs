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
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nitrocid.Kernel;
using Nitrocid.Files;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Files.Operations;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Modifications;
using Terminaux.Base.Checks;
using System.Reflection;

[assembly: DoNotParallelize]

namespace Nitrocid.Tests
{
    [TestClass]
    public static class InitTest
    {
        internal static string PathToTestSlotFolder = "";

        /// <summary>
        /// Initialize everything that is required before starting unit tests
        /// </summary>
        [AssemblyInitialize]
        public static void ReadyEverything(TestContext tc)
        {
            // Add this assembly to the console check whitelist
            var asm = Assembly.GetEntryAssembly();
            if (asm is not null)
                ConsoleChecker.AddToCheckWhitelist(asm);

            // We need not to use real user config directory
            PathsManagement.isTest = true;
            if (Checking.FolderExists(PathsManagement.AppDataPath))
                Removing.RemoveDirectory(PathsManagement.AppDataPath, false);
            Making.MakeDirectory(PathsManagement.AppDataPath, false);

            // Create config
            Config.CreateConfig();

            // Populate the test slot folder
            PathToTestSlotFolder = Path.GetFullPath("FilesystemSlot");
            PathToTestSlotFolder = FilesystemTools.NeutralizePath(PathToTestSlotFolder);

            // Make a slot for filesystem-related tests
            if (!Checking.FolderExists(PathToTestSlotFolder))
                Making.MakeDirectory(PathToTestSlotFolder, false);

            // Enable debugging
            KernelEntry.DebugMode = true;
            string debugPath = Environment.CurrentDirectory + "/UnitTestDebug.log";
            DebugWriter.InitializeDebug(debugPath);

            // Load necessary addons for testing
            AddonTools.ProcessAddons(ModLoadPriority.Important);
            AddonTools.ProcessAddons(ModLoadPriority.Optional);
            tc.WriteLine("Loaded all necessary test assets");
        }

        /// <summary>
        /// Clean up everything that the unit tests made
        /// </summary>
        [AssemblyCleanup]
        public static void CleanEverything()
        {
            if (Checking.FolderExists(Path.GetFullPath("ResultSlot")))
                Removing.RemoveDirectory(Path.GetFullPath("ResultSlot"));
            Directory.Move(PathToTestSlotFolder, Path.GetFullPath("ResultSlot"));
        }
    }
}
