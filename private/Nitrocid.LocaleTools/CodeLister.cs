//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using System.Collections.Generic;
using System.IO;

namespace Nitrocid.LocaleTools
{
    internal static class CodeLister
    {
        private static string[] ListCodeFilesForKS()
        {
            // Check to see if we have the Nitrocid KS folder
            string kernelSimulatorSource = "../../../../../public/Nitrocid/";
            string kernelSimulatorAddonsSource = "../../../../../public/Nitrocid.Addons/";
            List<string> sources = [];
            if (Directory.Exists(kernelSimulatorSource))
            {
                // Iterate through all the source files for Nitrocid KS
                string[] files = Directory.GetFiles(kernelSimulatorSource, "*.cs", SearchOption.AllDirectories);
                sources.AddRange(files);
            }
            if (Directory.Exists(kernelSimulatorAddonsSource))
            {
                // Iterate through all the source files for Nitrocid KS addons
                string[] files = Directory.GetFiles(kernelSimulatorAddonsSource, "*.cs", SearchOption.AllDirectories);
                sources.AddRange(files);
            }
            return [.. sources];
        }

        private static string[] ListDataFilesForKS()
        {
            // Check to see if we have the Nitrocid KS folder
            string kernelSimulatorDataSource = "../../../../../public/Nitrocid/Resources/Settings/";
            string kernelSimulatorDataAddonsSource = "../../../../../public/Nitrocid.Addons/";
            List<string> data = [];
            if (Directory.Exists(kernelSimulatorDataSource))
            {
                // Iterate through all the data files for Nitrocid KS
                string[] files = Directory.GetFiles(kernelSimulatorDataSource, "*Entries.json", SearchOption.AllDirectories);
                data.AddRange(files);
            }
            if (Directory.Exists(kernelSimulatorDataAddonsSource))
            {
                // Iterate through all the data files for Nitrocid KS
                string[] files = Directory.GetFiles(kernelSimulatorDataAddonsSource, "*Settings.json", SearchOption.AllDirectories);
                data.AddRange(files);
            }
            return [.. data];
        }

        internal static List<(string, string)> PopulateSources()
        {
            List<(string, string)> sources = [];

            // List all code files to add the sources
            foreach (string source in ListCodeFilesForKS())
                sources.Add((source, File.ReadAllText(source)));

            return sources;
        }

        internal static List<(string, string)> PopulateData()
        {
            List<(string, string)> sources = [];

            // List all code files to add the sources
            foreach (string source in ListDataFilesForKS())
                sources.Add((source, File.ReadAllText(source)));

            return sources;
        }
    }
}
