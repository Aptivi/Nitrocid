
//  Kernel Simulator  Copyright (C) 2018-2023  Aptivi
//
//  This file is part of Kernel Simulator
//
//  Kernel Simulator is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  Kernel Simulator is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;

namespace KSCleanUnusedLocales
{
    internal static class CodeLister
    {
        private static string[] ListCodeFilesForKS()
        {
            // Check to see if we have the Kernel Simulator folder
            string kernelSimulatorSource = "../../../../../public/Kernel Simulator/";
            if (Directory.Exists(kernelSimulatorSource))
            {
                // Iterate through all the source files for Kernel Simulator
                string[] files = Directory.GetFiles(kernelSimulatorSource, "*.cs", SearchOption.AllDirectories);
                return files;
            }
            return Array.Empty<string>();
        }

        private static string[] ListDataFilesForKS()
        {
            // Check to see if we have the Kernel Simulator folder
            string kernelSimulatorDataSource = "../../../../../public/Kernel Simulator/Resources/Data/";
            if (Directory.Exists(kernelSimulatorDataSource))
            {
                // Iterate through all the data files for Kernel Simulator
                string[] files = Directory.GetFiles(kernelSimulatorDataSource, "*Entries.json", SearchOption.AllDirectories);
                return files;
            }
            return Array.Empty<string>();
        }

        internal static List<string> PopulateSources()
        {
            List<string> sources = new();

            // List all code files to add the sources
            foreach (string source in ListCodeFilesForKS())
                sources.Add(File.ReadAllText(source));

            return sources;
        }

        internal static List<string> PopulateData()
        {
            List<string> sources = new();

            // List all code files to add the sources
            foreach (string source in ListDataFilesForKS())
                sources.Add(File.ReadAllText(source));

            return sources;
        }
    }
}
