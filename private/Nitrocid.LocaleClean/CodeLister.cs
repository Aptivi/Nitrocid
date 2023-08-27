
//  Nitrocid KS  Copyright (C) 2018-2023  Aptivi
//
//  This file is part of Nitrocid KS
//
//  Nitrocid KS is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  Nitrocid KS is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;

namespace Nitrocid.LocaleClean
{
    internal static class CodeLister
    {
        private static string[] ListCodeFilesForKS()
        {
            // Check to see if we have the Nitrocid KS folder
            string kernelSimulatorSource = "../../../../../public/Nitrocid/";
            if (Directory.Exists(kernelSimulatorSource))
            {
                // Iterate through all the source files for Nitrocid KS
                string[] files = Directory.GetFiles(kernelSimulatorSource, "*.cs", SearchOption.AllDirectories);
                return files;
            }
            return Array.Empty<string>();
        }

        private static string[] ListDataFilesForKS()
        {
            // Check to see if we have the Nitrocid KS folder
            string kernelSimulatorDataSource = "../../../../../public/Nitrocid/Resources/Settings/";
            if (Directory.Exists(kernelSimulatorDataSource))
            {
                // Iterate through all the data files for Nitrocid KS
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
