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
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nitrocid.Base.Files;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Base.Kernel.Starting;
using Nitrocid.Base.Misc.Reflection.Internal;
using Nitrocid.Base.Shell.Shells.Admin;
using Nitrocid.Base.Shell.Shells.Debug;
using Nitrocid.Base.Shell.Shells.Hex;
using Nitrocid.Base.Shell.Shells.Text;
using Nitrocid.Base.Shell.Shells.UESH;
using Terminaux.Shell.Shells;
using Terminaux.Themes;
using Textify.Tools.Placeholder;

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
            // We need not to use real user config directory
            PathsManagement.isTest = true;
            if (FilesystemTools.FolderExists(PathsManagement.AppDataPath))
                FilesystemTools.RemoveDirectory(PathsManagement.AppDataPath, false);
            FilesystemTools.MakeDirectory(PathsManagement.AppDataPath, false);

            // Initialize themes, shells, and placeholders
            KernelInitializers.InitializeEnvironmentComponents();

            // Create config
            Config.CreateConfig();

            // Populate the test slot folder
            PathToTestSlotFolder = Path.GetFullPath("FilesystemSlot");
            PathToTestSlotFolder = FilesystemTools.NeutralizePath(PathToTestSlotFolder);

            // Make a slot for filesystem-related tests
            if (!FilesystemTools.FolderExists(PathToTestSlotFolder))
                FilesystemTools.MakeDirectory(PathToTestSlotFolder, false);

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
            if (FilesystemTools.FolderExists(Path.GetFullPath("ResultSlot")))
                FilesystemTools.RemoveDirectory(Path.GetFullPath("ResultSlot"));
            Directory.Move(PathToTestSlotFolder, Path.GetFullPath("ResultSlot"));
        }
    }
}
