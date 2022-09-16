
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

using KS.Arguments.PreBootCommandLineArguments;
using System.Collections.Generic;

namespace KS.Arguments.ArgumentBase
{
    /// <summary>
    /// Pre-boot command line arguments parser
    /// </summary>
    public static class PreBootCommandLineArgsParse
    {

        /// <summary>
        /// Available pre-boot command line arguments
        /// </summary>
        public readonly static Dictionary<string, ArgumentInfo> AvailablePreBootCMDLineArgs = new()
        {
            { "reset", new ArgumentInfo("reset", ArgumentType.PreBootCommandLineArgs, "Resets the kernel to the factory settings", "", false, 0, new PreBootCommandLine_ResetArgument()) },
            { "newreader", new ArgumentInfo("newreader", ArgumentType.KernelArgs, "Opts in to new config reader", "", false, 0, new PreBootCommandLine_NewReaderArgument()) },
            { "bypasssizedetection", new ArgumentInfo("bypasssizedetection", ArgumentType.PreBootCommandLineArgs, "Bypasses the console size detection", "", false, 0, new PreBootCommandLine_BypassSizeDetectionArgument()) }
        };

    }
}
