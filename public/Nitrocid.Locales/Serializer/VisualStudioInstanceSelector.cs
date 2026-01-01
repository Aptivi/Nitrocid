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

using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.IO;
using Terminaux.Colors.Data;
using Terminaux.Reader;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Locales.Serializer
{
    internal static class VisualStudioInstanceSelector
    {
        internal static VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
        {
            // Check to see if the user has installed .NET SDK
            if (visualStudioInstances.Length == 0)
                throw new Exception("You should install a .NET SDK compatible with Nitrocid KS to continue.");

            TextWriterColor.Write("Select a Visual Studio instance:");
            for (int i = 0; i < visualStudioInstances.Length; i++)
            {
                TextWriterColor.Write($"Instance {i + 1}");
                TextWriterColor.Write($"  - Name: {visualStudioInstances[i].Name}");
                TextWriterColor.Write($"  - Version: {visualStudioInstances[i].Version}");
                TextWriterColor.Write($"  - MSBuild Path: {visualStudioInstances[i].MSBuildPath}");
            }

            while (true)
            {
                var userResponse = TermReader.Read(">> ");
                if (int.TryParse(userResponse, out int instanceNumber) &&
                    instanceNumber > 0 &&
                    instanceNumber <= visualStudioInstances.Length)
                {
                    return visualStudioInstances[instanceNumber - 1];
                }
                TextWriterColor.WriteColor("Input not accepted, try again.", true, ConsoleColors.Red);
            }
        }

        internal class ConsoleProgressReporter : IProgress<ProjectLoadProgress>
        {
            public void Report(ProjectLoadProgress loadProgress)
            {
                var projectDisplay = Path.GetFileName(loadProgress.FilePath);
                if (loadProgress.TargetFramework != null)
                    projectDisplay += $" ({loadProgress.TargetFramework})";

                TextWriterColor.Write($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
            }
        }
    }
}
