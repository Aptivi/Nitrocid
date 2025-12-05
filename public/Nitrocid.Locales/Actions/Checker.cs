//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nitrocid.Locales.Actions.Analyzers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using static Nitrocid.Locales.Serializer.VisualStudioInstanceSelector;

namespace Nitrocid.Locales.Actions
{
    internal static class Checker
    {
        internal static readonly HashSet<string> localizationList = [];

        internal static void Execute()
        {
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

                // Check for Nitrocid solution
                var solutionPath = "../../../../Nitrocid.sln";
                if (!File.Exists(solutionPath))
                {
                    TextWriterColor.WriteColor("Can't find Nitrocid solution. Make sure that it's run from the Nitrocid repo.", true, ConsoleColors.Red);
                    return;
                }

                // Attach progress reporter so we print projects as they are loaded.
                TextWriterColor.Write($"Loading solution {solutionPath}...");
                var solution = workspace.OpenSolutionAsync(solutionPath, new ConsoleProgressReporter()).Result;
                TextWriterColor.WriteColor($"Finished loading solution {solutionPath}!", true, ConsoleColors.Green);

                // Find the English JSON stream and open it.
                var stream = EntryPoint.thisAssembly.GetManifestResourceStream("Nitrocid.Locales.eng.json") ??
                    throw new Exception("Opening the eng.json resource stream has failed.");
                var reader = new StreamReader(stream);
                var jsonReader = new JsonTextReader(reader);
                var localizationsDoc = JToken.Load(jsonReader) ??
                    throw new Exception("Unable to parse JSON for English localizations.");
                var localizations = localizationsDoc["Localizations"]?.Values<string>() ??
                    throw new Exception("Unable to get localizations.");

                // Now, add all localizations to a separate array
                foreach (var localization in localizations)
                {
                    if (localization is null)
                        throw new Exception("There is no localization.");
                    string localizationString = localization.ToString();
                    localizationList.Add(localizationString);
                }

                // Add the Nitrocid analyzer to all the projects
                var projects = solution.Projects;
                List<string> totalUnlocalized = [];
                foreach (var project in projects)
                {
                    var documents = project.Documents;
                    foreach (var document in documents)
                    {
                        foreach (var analyzer in AnalyzersCommon.analyzers)
                        {
                            try
                            {
                                if (analyzer.Analyze(document, out string[] unlocalized))
                                {
                                    foreach (string unlocalizedString in unlocalized)
                                    {
                                        if (!totalUnlocalized.Contains(unlocalizedString))
                                            totalUnlocalized.Add(unlocalizedString);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                TextWriterColor.WriteColor($"Analyzer failed: {ex.Message}", true, ConsoleColors.Red);
                            }
                        }
                    }
                }

                // Now, analyze also the Nitrocid resources
                string[] unlocalizedResourceStrings = LocalizableResourcesAnalyzer.GetUnlocalizedStrings();
                foreach (string unlocalizedString in unlocalizedResourceStrings)
                {
                    if (!totalUnlocalized.Contains(unlocalizedString))
                        totalUnlocalized.Add(unlocalizedString);
                }

                // Save all unlocalized strings
                File.WriteAllLines("unlocalized.txt", totalUnlocalized);
                TextWriterColor.WriteColor($"Total unlocalized strings: {totalUnlocalized.Count}", true, totalUnlocalized.Count > 0 ? ConsoleColors.Red : ConsoleColors.Lime);
            }
            catch (Exception ex)
            {
                TextWriterColor.WriteColor($"General analysis failure: {ex.Message}", true, ConsoleColors.Red);
            }
        }
    }
}
