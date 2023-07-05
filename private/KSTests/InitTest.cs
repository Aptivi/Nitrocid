
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
using KS.Users;
using KS.Files.Operations;

namespace KSTests
{
    [SetUpFixture]
    public class InitTest
    {
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
        }

        /// <summary>
        /// Clean up everything that the unit tests made
        /// </summary>
        [OneTimeTearDown]
        public static void CleanEverything()
        {
            if (Checking.FileExists(Paths.HomePath + "/Documents/TestText.txt"))
                File.Delete(Paths.HomePath + "/Documents/TestText.txt");
            if (Checking.FileExists(Paths.HomePath + "/Documents/Text.txt"))
                File.Delete(Paths.HomePath + "/Documents/Text.txt");
            if (Checking.FileExists(Paths.HomePath + "/NewFile.txt"))
                File.Delete(Paths.HomePath + "/NewFile.txt");
            if (Checking.FileExists(Paths.HomePath + "/NewFile.json"))
                File.Delete(Paths.HomePath + "/NewFile.json");
            if (Checking.FileExists(Paths.HomePath + "/1mb-test.csv"))
                File.Delete(Paths.HomePath + "/1mb-test.csv");
            if (Checking.FolderExists(Paths.HomePath + "/TestMovedDir2"))
                Directory.Delete(Paths.HomePath + "/TestMovedDir2", true);
            if (Checking.FolderExists(Paths.HomePath + "/NewDirectory"))
                Directory.Delete(Paths.HomePath + "/NewDirectory", true);
            if (Checking.FolderExists(Paths.HomePath + "/TestDir"))
                Directory.Delete(Paths.HomePath + "/TestDir", true);
            if (Checking.FolderExists(Paths.HomePath + "/TestDir2"))
                Directory.Delete(Paths.HomePath + "/TestDir2", true);
            if (Checking.FileExists(Paths.GetKernelPath(KernelPathType.Configuration) + ".old"))
            {
                if (Checking.FileExists(Paths.GetKernelPath(KernelPathType.Configuration)))
                    File.Delete(Paths.GetKernelPath(KernelPathType.Configuration));
                File.Move(Paths.GetKernelPath(KernelPathType.Configuration) + ".old", Paths.GetKernelPath(KernelPathType.Configuration));
            }
        }
    }
}
