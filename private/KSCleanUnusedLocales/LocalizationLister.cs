
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
using System.Linq;

namespace KSCleanUnusedLocales
{
    internal static class LocalizationLister
    {
        private static string[] ListLanguageFilesForKS()
        {
            // Check to see if we have the Kernel Simulator folder
            string ksJsonifyLocalesSource = "../../../../../public/KSJsonifyLocales/Translations";
            if (Directory.Exists(ksJsonifyLocalesSource))
            {
                // Iterate through all the source files for Kernel Simulator
                string[] files = Directory.GetFiles(ksJsonifyLocalesSource, "*.txt");
                return files;
            }
            return Array.Empty<string>();
        }

        internal static Dictionary<string, List<string>> PopulateLanguages()
        {
            Dictionary<string, List<string>> sources = new();

            // List all code files to add the sources
            foreach (string source in ListLanguageFilesForKS())
                sources.Add(source, File.ReadAllLines(source).ToList());

            return sources;
        }
    }
}
