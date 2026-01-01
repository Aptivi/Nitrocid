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

using Nitrocid.Kernel;
using Nitrocid.Kernel.Debugging;
using System;

namespace Nitrocid.Arguments.CommandLineArguments
{
    class DebugArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(ArgumentParameters parameters)
        {
            KernelEntry.DebugMode = true;

            // Write headers for debug
            DebugWriter.WriteDebug(DebugLevel.I,
                "Kernel initialized, version {0}.\n" +
                "Kernel mod API version {1}.\n" +
                "OS: {2}",
                KernelMain.VersionFullStr, KernelMain.ApiVersion.ToString(), Environment.OSVersion.ToString()
            );
            if (KernelPlatform.IsOnUnixMusl())
                DebugWriter.WriteDebug(DebugLevel.I, "Running on musl");
        }
    }
}
