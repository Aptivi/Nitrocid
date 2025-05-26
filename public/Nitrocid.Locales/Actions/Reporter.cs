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
using System.Text;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using static Nitrocid.Locales.Serializer.VisualStudioInstanceSelector;

namespace Nitrocid.Locales.Actions
{
    internal static class Reporter
    {
        internal static readonly HashSet<string> localizationList = [];

        internal static void Execute(bool dry = false)
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
                workspace.WorkspaceFailed += (o, e) =>
                {
                    if (e.Diagnostic.Kind == WorkspaceDiagnosticKind.Warning)
                        TextWriterColor.WriteColor($"Warning while loading the workspace: [{e.Diagnostic.Kind}] {e.Diagnostic.Message}", true, ConsoleColors.Yellow);
                    else
                        TextWriterColor.WriteColor($"Failed to load the workspace: [{e.Diagnostic.Kind}] {e.Diagnostic.Message}", true, ConsoleColors.Red);
                };

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

                // Make a list of paths to print and to process
                List<string> paths = [];

                // Add the Nitrocid analyzer to all the projects
                var projects = solution.Projects;
                foreach (var project in projects)
                {
                    List<string> totalUnlocalized = [];
                    List<string> totalLocalized = [];
                    TextWriterColor.Write("==================================================================");
                    TextWriterColor.Write($"[{project.Name} - codebase] Processing...");
                    var documents = project.Documents;
                    foreach (var document in documents)
                    {
                        foreach (var analyzer in AnalyzersCommon.analyzers)
                        {
                            try
                            {
                                if (analyzer.Analyze(document, out string[] unlocalized, true))
                                {
                                    foreach (string unlocalizedString in unlocalized)
                                    {
                                        if (!totalUnlocalized.Contains(unlocalizedString))
                                            totalUnlocalized.Add(unlocalizedString);
                                    }
                                }
                                if (analyzer.ReverseAnalyze(document, out string[] localized, true))
                                {
                                    foreach (string localizedString in localized)
                                    {
                                        if (!totalLocalized.Contains(localizedString))
                                            totalLocalized.Add(localizedString);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                TextWriterColor.WriteColor($"Analyzer failed: {ex.Message}", true, ConsoleColors.Red);
                            }
                        }
                    }

                    // Build the unlocalized and the localized strings list for reporting
                    StringBuilder unlocalizedBuilder = new();
                    StringBuilder localizedBuilder = new();
                    foreach (var unlocalized in totalUnlocalized)
                        unlocalizedBuilder.AppendLine(unlocalized);
                    foreach (var localized in totalLocalized)
                        localizedBuilder.AppendLine(localized);
                    if (totalUnlocalized.Count > 0 || totalLocalized.Count > 0)
                    {
                        if (!dry)
                        {
                            File.WriteAllText($"unlocalized.{project.Name}.txt", unlocalizedBuilder.ToString());
                            paths.Add($"unlocalized.{project.Name}.txt");
                        }
                        TextWriterColor.WriteColor($"[{project.Name} - codebase] Total unlocalized strings: {totalUnlocalized.Count}", true, totalUnlocalized.Count > 0 ? ConsoleColors.Red : ConsoleColors.Lime);
                        if (!dry)
                        {
                            File.WriteAllText($"localized.{project.Name}.txt", localizedBuilder.ToString());
                            paths.Add($"localized.{project.Name}.txt");
                        }
                        TextWriterColor.WriteColor($"[{project.Name} - codebase] Total localized strings: {totalLocalized.Count}", true, totalLocalized.Count > 0 ? ConsoleColors.Lime : ConsoleColors.Red);
                    }
                    else
                        TextWriterColor.WriteColor($"[{project.Name} - codebase] Nothing to process", true, ConsoleColors.Yellow);
                }

                // Now, analyze also the Nitrocid resources
                var resourceNames = LocalizableResourcesAnalyzer.GetResourceNames();
                foreach (var resourceName in resourceNames)
                {
                    // Determine the project names
                    string projectName = DetermineProjectName(resourceName);
                    TextWriterColor.Write("==================================================================");
                    TextWriterColor.Write($"[{projectName} - resources] Processing...");

                    // Get unlocalized and localized resource strings
                    string[] unlocalizedResourceStrings = LocalizableResourcesAnalyzer.GetUnlocalizedStrings(resourceName, true);
                    string[] localizedResourceStrings = LocalizableResourcesAnalyzer.GetLocalizedStrings(resourceName, true);

                    // Build the unlocalized and the localized strings list for reporting
                    StringBuilder unlocalizedBuilder = new();
                    StringBuilder localizedBuilder = new();
                    foreach (var unlocalized in unlocalizedResourceStrings)
                        unlocalizedBuilder.AppendLine(unlocalized);
                    foreach (var localized in localizedResourceStrings)
                        localizedBuilder.AppendLine(localized);
                    if (unlocalizedResourceStrings.Length > 0 || localizedResourceStrings.Length > 0)
                    {
                        if (!dry)
                        {
                            File.AppendAllText($"unlocalized.{projectName}.txt", unlocalizedBuilder.ToString());
                            paths.Add($"unlocalized.{projectName}.txt");
                        }
                        TextWriterColor.WriteColor($"[{projectName} - resources] Total unlocalized strings: {unlocalizedResourceStrings.Length}", true, unlocalizedResourceStrings.Length > 0 ? ConsoleColors.Red : ConsoleColors.Lime);
                        if (!dry)
                        {
                            File.AppendAllText($"localized.{projectName}.txt", localizedBuilder.ToString());
                            paths.Add($"localized.{projectName}.txt");
                        }
                        TextWriterColor.WriteColor($"[{projectName} - resources] Total localized strings: {localizedResourceStrings.Length}", true, localizedResourceStrings.Length > 0 ? ConsoleColors.Lime : ConsoleColors.Red);
                    }
                    else
                        TextWriterColor.WriteColor($"[{projectName} - resources] Nothing to process", true, ConsoleColors.Yellow);
                }

                // Open the text files to remove duplicates
                TextWriterColor.Write("==================================================================");
                foreach (var textFile in paths.Distinct())
                {
                    string[] lines = [.. File.ReadAllLines(textFile).Distinct()];
                    File.WriteAllLines(textFile, lines);
                    TextWriterColor.Write($"Saved as {Path.GetFullPath(textFile)}...");
                }
            }
            catch (Exception ex)
            {
                TextWriterColor.WriteColor($"General analysis failure: {ex.Message}", true, ConsoleColors.Red);
            }
        }

        private static string DetermineProjectName(string resourceName)
        {
            string projectName = "Nitrocid";
            if (resourceName.Contains("Nitrocid.Base"))
                projectName = "Nitrocid.Base";
            else if (resourceName.Contains("Nitrocid.Core"))
                projectName = "Nitrocid.Core";
            else if (resourceName.Contains("Nitrocid.Addons") || resourceName.Contains("Nitrocid.Generators"))
            {
                int lastIdx = -1;
                if (resourceName.Contains("Nitrocid.Addons"))
                    lastIdx = resourceName.IndexOf("Nitrocid.Addons") + "Nitrocid.Addons".Length;
                else if (resourceName.Contains("Nitrocid.Generators"))
                    lastIdx = resourceName.IndexOf("Nitrocid.Generators") + "Nitrocid.Generators".Length;
                projectName = resourceName[lastIdx..];
                char delimiter = projectName[0];
                projectName = projectName[1..];
                projectName = projectName[..projectName.IndexOf(delimiter)];
            }
            return projectName;
        }
    }
}
