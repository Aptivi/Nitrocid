
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

using KS.Arguments.ArgumentBase;
using KS.Files.Querying;
using KS.Files;
using System.IO;
using System;

namespace KS.Arguments.CommandLineArguments
{
    class ResetArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            // Delete every single thing found in KernelPaths
            foreach (string PathName in Enum.GetNames(typeof(KernelPathType)))
            {
                string TargetPath = Paths.GetKernelPath((KernelPathType)Convert.ToInt32(PathName));
                if (Checking.FileExists(TargetPath))
                {
                    File.Delete(TargetPath);
                }
                else
                {
                    Directory.Delete(TargetPath, true);
                }
            }

            // Exit now.
            Environment.Exit(0);
        }
    }
}
