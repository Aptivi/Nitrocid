
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Extensification.StringExts;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;

namespace KS.Files.Querying
{
    public static class Searching
    {

        /// <summary>
        /// Searches a file for string
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="StringLookup">String to find</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static List<string> SearchFileForString(string FilePath, string StringLookup)
        {
            try
            {
                Filesystem.ThrowOnInvalidPath(FilePath);
                FilePath = Filesystem.NeutralizePath(FilePath);
                var Matches = new List<string>();
                var Filebyte = File.ReadAllLines(FilePath);
                int MatchNum = 1;
                int LineNumber = 1;
                foreach (string Str in Filebyte)
                {
                    if (Str.Contains(StringLookup))
                    {
                        Matches.Add($"[{LineNumber}] " + Translate.DoTranslation("Match {0}: {1}").FormatString(MatchNum, Str));
                        MatchNum += 1;
                    }
                    LineNumber += 1;
                }
                return Matches;
            }
            catch (Exception ex)
            {
                DebugWriter.WStkTrc(ex);
                throw new IOException(Translate.DoTranslation("Unable to find file to match string \"{0}\": {1}").FormatString(StringLookup, ex.Message));
            }
            return null;
        }

        /// <summary>
        /// Searches a file for string using regexp
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="StringLookup">String to find</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static List<string> SearchFileForStringRegexp(string FilePath, Regex StringLookup)
        {
            try
            {
                Filesystem.ThrowOnInvalidPath(FilePath);
                FilePath = Filesystem.NeutralizePath(FilePath);
                var Matches = new List<string>();
                var Filebyte = File.ReadAllLines(FilePath);
                int MatchNum = 1;
                int LineNumber = 1;
                foreach (string Str in Filebyte)
                {
                    if (StringLookup.IsMatch(Str))
                    {
                        Matches.Add($"[{LineNumber}] " + Translate.DoTranslation("Match {0}: {1}").FormatString(MatchNum, Str));
                        MatchNum += 1;
                    }
                    LineNumber += 1;
                }
                return Matches;
            }
            catch (Exception ex)
            {
                DebugWriter.WStkTrc(ex);
                throw new IOException(Translate.DoTranslation("Unable to find file to match string \"{0}\": {1}").FormatString(StringLookup, ex.Message));
            }
            return null;
        }

    }
}