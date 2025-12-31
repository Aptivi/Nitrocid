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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Terminaux.Colors.Data;
using Terminaux.Reader;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.StandaloneAnalyzer
{
    internal class EntryPoint
    {
        static async Task Main(string[] args)
        {
            // Check to see if we've been provided the path to a mod
            if (args.Length == 0)
            {
                TextWriterColor.WriteColor("Provide a path to your mod project.", true, ConsoleColors.Red);
                return;
            }

            try
            {
                // Attempt to set the version of MSBuild.
                var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
                var instance = visualStudioInstances.Length == 1 ? visualStudioInstances[0] : SelectVisualStudioInstance(visualStudioInstances);
                TextWriterColor.Write($"Build system is {instance.MSBuildPath}, {instance.Name}, version {instance.Version}");
                MSBuildLocator.RegisterInstance(instance);

                // Create a workspace using the instance
                using var workspace = MSBuildWorkspace.Create();
                workspace.RegisterWorkspaceFailedHandler((e) =>
                {
                    if (e.Diagnostic.Kind == WorkspaceDiagnosticKind.Warning)
                        TextWriterColor.WriteColor($"Warning while loading the workspace: [{e.Diagnostic.Kind}] {e.Diagnostic.Message}", true, ConsoleColors.Yellow);
                    else
                        TextWriterColor.WriteColor($"Failed to load the workspace: [{e.Diagnostic.Kind}] {e.Diagnostic.Message}", true, ConsoleColors.Red);
                });

                // Load the solution
                var solutionPath = args[0];
                TextWriterColor.Write($"Loading solution {solutionPath}...");

                // Attach progress reporter so we print projects as they are loaded.
                var solution = await workspace.OpenSolutionAsync(solutionPath, new ConsoleProgressReporter());
                TextWriterColor.WriteColor($"Finished loading solution {solutionPath}!", true, ConsoleColors.Green);

                // Add the Nitrocid analyzer to all the projects
                var projects = solution.Projects;
                foreach (var project in projects)
                {
                    var documents = project.Documents;
                    foreach (var document in documents)
                    {
                        foreach (var analyzer in AnalyzersList.analyzers)
                        {
                            try
                            {
                                if (analyzer.Analyze(document))
                                    await analyzer.SuggestAsync(document);
                            }
                            catch (Exception ex)
                            {
                                TextWriterColor.WriteColor($"Analyzer failed: {ex.Message}", true, ConsoleColors.Red);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TextWriterColor.WriteColor($"General analysis failure: {ex.Message}", true, ConsoleColors.Red);
            }
        }

        private static VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
        {
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

        private class ConsoleProgressReporter : IProgress<ProjectLoadProgress>
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
