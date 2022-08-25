
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

using KS.Misc.Writers.WriterBase.PlainWriters;
using System.Collections.Generic;

namespace KS.Misc.Writers.WriterBase
{
    /// <summary>
    /// Plain writer management module
    /// </summary>
    public static class WriterPlainManager
    {
        internal readonly static Dictionary<string, IWriterPlain> plains = new()
        {
            { "Console", new ConsolePlainWriter() },
            { "File", new FilePlainWriter() },
            { "Null", new NullPlainWriter() }
        };
        internal static string currentPlainName = "Console";
        internal static IWriterPlain currentPlain = plains[currentPlainName];

        /// <summary>
        /// Gets the current plain writer name
        /// </summary>
        public static string CurrentPlainName { get => currentPlainName; }

        /// <summary>
        /// Changes the plain writer
        /// </summary>
        /// <param name="plainName">The plain writer name. Usually found in <see cref="plains"/></param>
        public static void ChangePlain(string plainName)
        {
            if (plains.ContainsKey(plainName))
            {
                currentPlainName = plainName;
                currentPlain = plains[plainName];
            }
            else
            {
                currentPlainName = "Console";
                currentPlain = plains[currentPlainName];
            }
        }
    }
}
