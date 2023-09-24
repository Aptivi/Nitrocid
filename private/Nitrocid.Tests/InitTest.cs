
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
using System.IO;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel.Configuration;
using NUnit.Framework;
using KS.Files.Operations;
using KS.Drivers;
using KS.Drivers.DebugLogger;
using KS.Kernel.Debugging;
using KS.Kernel.Extensions;

namespace Nitrocid.Tests
{
    [SetUpFixture]
    public class InitTest
    {
        internal static string PathToTestSlotFolder = "";

        /// <summary>
        /// Initialize everything that is required before starting unit tests
        /// </summary>
        [OneTimeSetUp]
        public static void ReadyEverything()
        {
            if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.Configuration)))
            {
                // Check to see if we have an appdata folder for KS
                if (!Checking.FolderExists(Paths.AppDataPath))
                    Making.MakeDirectory(Paths.AppDataPath, false);

                // Now, create config
                Config.CreateConfig();
            }
            else
            {
                if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.Configuration) + ".old"))
                    File.Move(Paths.GetKernelPath(KernelPathType.Configuration), Paths.GetKernelPath(KernelPathType.Configuration) + ".old");
                Config.CreateConfig();
            }
            Config.ReadConfig(ConfigType.Kernel, Paths.GetKernelPath(KernelPathType.Configuration));

            // NUnit sets current directory to a wrong directory, so set it to the test context directory
            string TestAssemblyDir = TestContext.CurrentContext.TestDirectory;
            Environment.CurrentDirectory = TestAssemblyDir;
            PathToTestSlotFolder = Path.GetFullPath("FilesystemSlot");
            PathToTestSlotFolder = Filesystem.NeutralizePath(PathToTestSlotFolder);

            // Make a slot for filesystem-related tests
            if (!Checking.FolderExists(PathToTestSlotFolder))
                Making.MakeDirectory(PathToTestSlotFolder, false);

            // Enable debugging
            string debugPath = TestAssemblyDir + "/UnitTestDebug.log";
            DebugWriter.DebugPath = debugPath;
            KernelFlags.DebugMode = true;

            // Load necessary addons for testing
            AddonTools.ProcessAddons(AddonType.Important);
            AddonTools.ProcessAddons(AddonType.Optional);
        }

        /// <summary>
        /// Clean up everything that the unit tests made
        /// </summary>
        [OneTimeTearDown]
        public static void CleanEverything()
        {
            if (Checking.FolderExists(Path.GetFullPath("ResultSlot")))
                Removing.RemoveDirectory(Path.GetFullPath("ResultSlot"));
            Directory.Move(PathToTestSlotFolder, Path.GetFullPath("ResultSlot"));
            if (Checking.FileExists(Paths.GetKernelPath(KernelPathType.Configuration) + ".old"))
            {
                if (Checking.FileExists(Paths.GetKernelPath(KernelPathType.Configuration)))
                    File.Delete(Paths.GetKernelPath(KernelPathType.Configuration));
                File.Move(Paths.GetKernelPath(KernelPathType.Configuration) + ".old", Paths.GetKernelPath(KernelPathType.Configuration));
            }
        }
    }
}
