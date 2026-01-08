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

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nitrocid.Analyzers.Common;
using Nitrocid.LocaleChecker.Resources;
using static System.Net.Mime.MediaTypeNames;

namespace Nitrocid.LocaleChecker.Localization
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UnlocalizedStringAnalyzer : DiagnosticAnalyzer
    {
        // This assembly
        private static readonly Assembly thisAssembly = typeof(UnlocalizedStringAnalyzer).Assembly;
        private static readonly string[] languageManifestNames = [.. thisAssembly.GetManifestResourceNames().Where((name) => name.StartsWith("Nitrocid.Langs"))];

        // Some constants
        public const string DiagnosticId = "NLOC0001";
        public const string DiagnosticIdComment = "NLOC0002";
        public const string DiagnosticIdJson = "NLOC0003";
        public const string DiagnosticIdExtraJson = "NLOC0004";
        public const string DiagnosticIdExtraJsonSummary = "NLOC0005";
        private const string Category = "Localization";

        // Some strings
        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(AnalyzerResources.UnlocalizedStringAnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString ExtraJsonTitle =
            new LocalizableResourceString(nameof(AnalyzerResources.ExtraLocalizedJsonStringAnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString ExtraJsonSummaryTitle =
            new LocalizableResourceString(nameof(AnalyzerResources.ExtraLocalizedJsonSummaryStringAnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(AnalyzerResources.UnlocalizedStringAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormatComment =
            new LocalizableResourceString(nameof(AnalyzerResources.UnlocalizedCommentStringAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormatJson =
            new LocalizableResourceString(nameof(AnalyzerResources.UnlocalizedJsonStringAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormatExtraJson =
            new LocalizableResourceString(nameof(AnalyzerResources.ExtraLocalizedJsonStringAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormatExtraJsonSummary =
            new LocalizableResourceString(nameof(AnalyzerResources.ExtraLocalizedJsonSummaryStringAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(AnalyzerResources.UnlocalizedStringAnalyzerDescription), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString ExtraJsonDescription =
            new LocalizableResourceString(nameof(AnalyzerResources.ExtraLocalizedJsonStringAnalyzerDescription), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));

        // A rule
        private static readonly DiagnosticDescriptor Rule =
            new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description, customTags: ["CompilationEnd"]);
        private static readonly DiagnosticDescriptor RuleComment =
            new(DiagnosticIdComment, Title, MessageFormatComment, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description, customTags: ["CompilationEnd"]);
        private static readonly DiagnosticDescriptor RuleJson =
            new(DiagnosticIdJson, Title, MessageFormatJson, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description, customTags: ["CompilationEnd"]);
        private static readonly DiagnosticDescriptor RuleExtraJson =
            new(DiagnosticIdExtraJson, ExtraJsonTitle, MessageFormatExtraJson, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: ExtraJsonDescription, customTags: ["CompilationEnd"]);
        private static readonly DiagnosticDescriptor RuleExtraJsonSummary =
            new(DiagnosticIdExtraJsonSummary, ExtraJsonSummaryTitle, MessageFormatExtraJsonSummary, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: ExtraJsonDescription, customTags: ["CompilationEnd"]);

        // English localization list
        private readonly Dictionary<string, (List<string>, List<JToken>)> localizationList = [];

        // Found locs
        private static readonly DiagnosticDescriptor assemblyLocsDbg =
            new("DBG_NLOC", "Debug Diagnostic for localization", "DEBUG: {0} = {1}", Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, customTags: ["CompilationEnd"]);
        private readonly Dictionary<string, List<Location?>> assemblyLocs = [];

        // Supported diagnostics
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(Rule, RuleComment, RuleJson, RuleExtraJson, RuleExtraJsonSummary, assemblyLocsDbg);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1026:Enable concurrent execution", Justification = "Concurrency causes false positives related to assembly locs")]
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterCompilationStartAction(PopulateLocalizations);
        }

        private void PopulateLocalizations(CompilationStartAnalysisContext context)
        {
            foreach (string langStream in languageManifestNames)
            {
                // Find the JSON stream and open it.
                var stream = thisAssembly.GetManifestResourceStream(langStream) ??
                    throw new Exception($"Opening the {langStream} resource stream has failed.");
                var reader = new StreamReader(stream);
                var jsonReader = new JsonTextReader(reader);
                var document = JToken.Load(jsonReader) ??
                    throw new Exception("Unable to parse JSON for localizations.");
                var locs = document["locs"] ??
                    throw new Exception("Unable to get localizations.");
                var localizations = JsonConvert.DeserializeObject<LocalizationInfo[]>(locs.ToString()) ??
                    throw new Exception("Unable to get localizations.");
                string[] splitLangStream = langStream.Split(' ');
                string langFileName = splitLangStream[3].Contains("\\") ? splitLangStream[3].Substring(splitLangStream[3].LastIndexOf('\\') + 1) : splitLangStream[3].Substring(splitLangStream[3].LastIndexOf('/') + 1);
                string langName = langFileName.Remove(langFileName.IndexOf("."));
                string assemblyName =
                    splitLangStream[1].Contains("\\") ? splitLangStream[1].Split('\\')[4] :
                    splitLangStream[1].Contains("/") ? splitLangStream[1].Split('/')[4] : splitLangStream[1];
                string finalLangStream = $"Nitrocid.Langs {assemblyName} {splitLangStream[2]} {splitLangStream[3]}";
                if (assemblyName != context.Compilation.AssemblyName)
                    continue;

                // Now, add all localizations to a separate array
                string finalKey = langName + " - " + finalLangStream;
                if (!localizationList.ContainsKey(finalKey))
                    localizationList.Add(finalKey, ([], []));
                for (int i = 0; i < localizations.Length; i++)
                {
                    LocalizationInfo? localization = localizations[i] ??
                        throw new Exception("There is no localization.");
                    JToken? localizationToken = locs.ElementAt(i)["loc"] ??
                        throw new Exception("There is no localization.");
                    string localizationString = localization.Localization.ToString();
                    localizationList[finalKey].Item1.Add(localizationString);
                    localizationList[finalKey].Item2.Add(localizationToken);
                    localizationList[finalKey] = (localizationList[finalKey].Item1, localizationList[finalKey].Item2);
                }
            }

            // Register the localization analysis action
            context.RegisterSyntaxNodeAction(AnalyzeLocalization, SyntaxKind.InvocationExpression);
            context.RegisterSyntaxNodeAction(AnalyzeImplicitLocalization, SyntaxKind.CompilationUnit);
            context.RegisterCompilationEndAction(AnalyzeResourceLocalization);
            context.RegisterCompilationEndAction(CheckExtraJsonLocalizations);
        }

        private void AnalyzeLocalization(SyntaxNodeAnalysisContext context)
        {
            // Check for argument
            var exp = (InvocationExpressionSyntax)context.Node;
            var args = exp.ArgumentList.Arguments;
            if (args.Count < 1)
                return;
            var localizableStringArgument = args[0] ??
                throw new Exception("Can't get localizable string");

            // Now, check for the LanguageTools.GetLocalized() call
            if (exp.Expression is not MemberAccessExpressionSyntax expMaes)
                return;
            if (expMaes.Expression is IdentifierNameSyntax expIdentifier && expMaes.Name is IdentifierNameSyntax identifier)
            {
                // Verify that we're dealing with LanguageTools.GetLocalized()
                var location = context.Node.GetLocation();
                var idExpression = expIdentifier.Identifier.Text;
                var idName = identifier.Identifier.Text;
                if ((idExpression == "LanguageTools" || idExpression == "BaseLangTools") && idName == "GetLocalized")
                {
                    // Now, get the string representation from the argument count and compare it with the list of translations.
                    // You'll notice that we sometimes call LanguageTools.GetLocalized() with a variable instead of a string, so
                    // check that first, because they're usually obtained from a string representation usually prefixed with
                    // either the /* Localizable */ comment or in individual kernel resources. However, the resources don't
                    // have a prefix, so the key names alone are enough.
                    if (localizableStringArgument.Expression is LiteralExpressionSyntax literalText)
                    {
                        string text = literalText.ToString();
                        text = text.Substring(1, text.Length - 2).Replace("\\\"", "\"");
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            List<string> incompleteLangs = [];
                            foreach (var localization in localizationList.Keys)
                            {
                                ProcessAssemblyLoc(localization, text, out bool found);
                                if (!found && localization.Contains($"Nitrocid.Langs {context.SemanticModel.Compilation.AssemblyName} "))
                                    incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                            }
                            if (incompleteLangs.Count > 0)
                            {
                                var diagnostic = Diagnostic.Create(Rule, location, text, string.Join(", ", incompleteLangs));
                                context.ReportDiagnostic(diagnostic);
                            }
                        }
                    }
                }
            }
        }

        private void AnalyzeImplicitLocalization(SyntaxNodeAnalysisContext context)
        {
            // Since /* Localizable */ represents a multiline comment, we need to find them and find the string
            // next to each one.
            var exp = (CompilationUnitSyntax)context.Node;
            var triviaList = exp.DescendantTrivia();
            var multiLineComments = triviaList.Where((trivia) => trivia.IsKind(SyntaxKind.MultiLineCommentTrivia));
            foreach (var multiLineComment in multiLineComments)
            {
                string comment = multiLineComment.ToString();
                if (comment == "/* Localizable */")
                {
                    // We found a localizable string, but we need to find the string itself, so get all the possible
                    // tokens.
                    var node = exp.FindNode(multiLineComment.Span);
                    var tokens = node.DescendantTokens()
                        .Where(token => token.GetAllTrivia()
                            .Where((trivia) => trivia.IsKind(SyntaxKind.MultiLineCommentTrivia) && trivia.ToString() == "/* Localizable */")
                        .Count() > 0);

                    // Now, enumerate them to find the string
                    foreach (var token in tokens)
                    {
                        void Process(LiteralExpressionSyntax literalText)
                        {
                            // Process it.
                            var location = literalText.GetLocation();
                            string text = literalText.ToString();
                            text = text.Substring(1, text.Length - 2).Replace("\\\"", "\"");
                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                List<string> incompleteLangs = [];
                                foreach (var localization in localizationList.Keys)
                                {
                                    ProcessAssemblyLoc(localization, text, out bool found);
                                    if (!found && localization.Contains($"Nitrocid.Langs {context.SemanticModel.Compilation.AssemblyName} "))
                                        incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                                }
                                if (incompleteLangs.Count > 0)
                                {
                                    var diagnostic = Diagnostic.Create(RuleComment, location, text, string.Join(", ", incompleteLangs));
                                    context.ReportDiagnostic(diagnostic);
                                }
                            }
                        }

                        // Try to get a child
                        int start = token.FullSpan.End;
                        var parent = token.Parent;
                        if (parent is null)
                            continue;
                        if (parent is LiteralExpressionSyntax literalParent)
                        {
                            Process(literalParent);
                            continue;
                        }
                        if (parent is NameColonSyntax)
                            parent = parent.Parent;
                        if (parent is null)
                            continue;
                        var child = (SyntaxNode?)parent.ChildThatContainsPosition(start);
                        if (child is null)
                            continue;

                        // Now, check to see if it's a literal string
                        if (child is LiteralExpressionSyntax literalText)
                            Process(literalText);
                        else if (child is ArgumentSyntax argument && argument.Expression is LiteralExpressionSyntax literalArgText)
                            Process(literalArgText);
                    }
                }
            }
        }

        private void AnalyzeResourceLocalization(CompilationAnalysisContext context)
        {
            // Open every resource except the English translations file and the analyzer string resources
            var resourceNames = thisAssembly.GetManifestResourceNames().Except([
                .. languageManifestNames,
                "Nitrocid.LocaleChecker.Resources.AnalyzerResources.resources",
            ]).Where((path) => path.Contains("?"));
            foreach (var resourceName in resourceNames)
            {
                // Check the assembly name
                string[] pathArgs = resourceName.Split('?');
                string relativePath = pathArgs[0];
                string absolutePath = pathArgs[1];
                string assemblyName = context.Compilation.AssemblyName ?? "";
                if (relativePath.Contains("\\") || relativePath.Contains('/'))
                {
                    // Split the path and find "Resources"
                    string[] splitPath = relativePath.Contains('/') ? relativePath.Split('/') : relativePath.Split('\\');
                    int asmNameIdx = splitPath.IndexOf("Resources") - 1;
                    assemblyName = splitPath[asmNameIdx];
                }
                if (assemblyName != context.Compilation.AssemblyName && assemblyName != "Nitrocid.Generators.KnownAddons")
                    continue;

                // Open the resource and load it to a JSON token instance
                var stream = thisAssembly.GetManifestResourceStream(resourceName) ??
                    throw new Exception($"Opening the {resourceName} resource stream has failed.");
                var reader = new StreamReader(stream);
                var jsonReader = new JsonTextReader(reader);
                var document = JToken.Load(jsonReader) ??
                    throw new Exception($"Unable to parse JSON for {resourceName}.");

                // Determine if this is a theme or a settings entries list
                var themeMetadata = document.Type == JTokenType.Array ? null : document["Metadata"];
                if (themeMetadata is not null)
                {
                    // It's a theme. Get its description and its localizable boolean value
                    string descriptionOrig = (string?)themeMetadata["Description"] ?? "";
                    string description = descriptionOrig.Replace("\\\"", "\"");
                    var location = AnalyzerTools.GenerateLocation(themeMetadata["Description"], descriptionOrig, absolutePath, false);
                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        List<string> incompleteLangs = [];
                        foreach (var localization in localizationList.Keys)
                        {
                            ProcessAssemblyLoc(localization, description, out bool found);
                            if (!found && localization.Contains($"Nitrocid.Langs {assemblyName} "))
                                incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                        }
                        if (incompleteLangs.Count > 0)
                        {
                            var diagnostic = Diagnostic.Create(RuleJson, location, description, string.Join(", ", incompleteLangs));
                            context.ReportDiagnostic(diagnostic);
                        }
                    }
                }
                else if (document.Type == JTokenType.Array)
                {
                    // It's likely a settings entry list, but verify
                    foreach (var settingsEntryList in document)
                    {
                        // Check the description and the display
                        string descriptionOrig = (string?)settingsEntryList["Desc"] ?? "";
                        string displayAsOrig = (string?)settingsEntryList["DisplayAs"] ?? "";
                        string knownAddonDisplayOrig = (string?)settingsEntryList["display"] ?? "";
                        string description = descriptionOrig.Replace("\\\"", "\"");
                        string displayAs = displayAsOrig.Replace("\\\"", "\"");
                        string knownAddonDisplay = knownAddonDisplayOrig.Replace("\\\"", "\"");
                        if (!string.IsNullOrWhiteSpace(description))
                        {
                            List<string> incompleteLangs = [];
                            var location = AnalyzerTools.GenerateLocation(settingsEntryList["Desc"], descriptionOrig, absolutePath, false);
                            foreach (var localization in localizationList.Keys)
                            {
                                ProcessAssemblyLoc(localization, description, out bool found);
                                if (!found && localization.Contains($"Nitrocid.Langs {assemblyName} "))
                                    incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                            }
                            if (incompleteLangs.Count > 0)
                            {
                                var diagnostic = Diagnostic.Create(RuleJson, location, description, string.Join(", ", incompleteLangs));
                                context.ReportDiagnostic(diagnostic);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(displayAs))
                        {
                            List<string> incompleteLangs = [];
                            var location = AnalyzerTools.GenerateLocation(settingsEntryList["DisplayAs"], displayAsOrig, absolutePath, false);
                            foreach (var localization in localizationList.Keys)
                            {
                                ProcessAssemblyLoc(localization, displayAs, out bool found);
                                if (!found && localization.Contains($"Nitrocid.Langs {assemblyName} "))
                                    incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                            }
                            if (incompleteLangs.Count > 0)
                            {
                                var diagnostic = Diagnostic.Create(RuleJson, location, displayAs, string.Join(", ", incompleteLangs));
                                context.ReportDiagnostic(diagnostic);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(knownAddonDisplay))
                        {
                            List<string> incompleteLangs = [];
                            var location = AnalyzerTools.GenerateLocation(settingsEntryList["display"], knownAddonDisplayOrig, absolutePath, false);
                            foreach (var localization in localizationList.Keys)
                            {
                                ProcessAssemblyLoc(localization, knownAddonDisplay, out bool found);
                                if (!found && localization.Contains($"Nitrocid.Langs {assemblyName} "))
                                    incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                            }
                            if (incompleteLangs.Count > 0)
                            {
                                var diagnostic = Diagnostic.Create(RuleJson, location, knownAddonDisplay, string.Join(", ", incompleteLangs));
                                context.ReportDiagnostic(diagnostic);
                            }
                        }

                        // Helper function to check a key, because a key can be a multivar
                        void CheckKeys(JArray keys)
                        {
                            foreach (var key in keys)
                            {
                                string keyNameOrig = (string?)key["Name"] ?? "";
                                string keyType = (string?)key["Type"] ?? "";
                                string keyDescOrig = (string?)key["Description"] ?? "";
                                string keyTipOrig = (string?)key["Tip"] ?? "";
                                string keyName = keyNameOrig.Replace("\\\"", "\"");
                                string keyDesc = keyDescOrig.Replace("\\\"", "\"");
                                string keyTip = keyTipOrig.Replace("\\\"", "\"");
                                if (!string.IsNullOrWhiteSpace(keyName))
                                {
                                    List<string> incompleteLangs = [];
                                    var location = AnalyzerTools.GenerateLocation(key["Name"], keyNameOrig, absolutePath, false);
                                    foreach (var localization in localizationList.Keys)
                                    {
                                        ProcessAssemblyLoc(localization, keyName, out bool found);
                                        if (!found && localization.Contains($"Nitrocid.Langs {assemblyName} "))
                                            incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                                    }
                                    if (incompleteLangs.Count > 0)
                                    {
                                        var diagnostic = Diagnostic.Create(RuleJson, location, keyName, string.Join(", ", incompleteLangs));
                                        context.ReportDiagnostic(diagnostic);
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(keyDesc))
                                {
                                    List<string> incompleteLangs = [];
                                    var location = AnalyzerTools.GenerateLocation(key["Description"], keyDescOrig, absolutePath, false);
                                    foreach (var localization in localizationList.Keys)
                                    {
                                        ProcessAssemblyLoc(localization, keyDesc, out bool found);
                                        if (!found && localization.Contains($"Nitrocid.Langs {assemblyName} "))
                                            incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                                    }
                                    if (incompleteLangs.Count > 0)
                                    {
                                        var diagnostic = Diagnostic.Create(RuleJson, location, keyDesc, string.Join(", ", incompleteLangs));
                                        context.ReportDiagnostic(diagnostic);
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(keyTip))
                                {
                                    List<string> incompleteLangs = [];
                                    var location = AnalyzerTools.GenerateLocation(key["Tip"], keyTipOrig, absolutePath, false);
                                    foreach (var localization in localizationList.Keys)
                                    {
                                        ProcessAssemblyLoc(localization, keyTip, out bool found);
                                        if (!found && localization.Contains($"Nitrocid.Langs {assemblyName} "))
                                            incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                                    }
                                    if (incompleteLangs.Count > 0)
                                    {
                                        var diagnostic = Diagnostic.Create(RuleJson, location, keyTip, string.Join(", ", incompleteLangs));
                                        context.ReportDiagnostic(diagnostic);
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(keyType) && keyType == "SMultivar")
                                {
                                    var multiVarKeys = (JArray?)key["Variables"];
                                    if (multiVarKeys is null || multiVarKeys.Count == 0)
                                        continue;
                                    CheckKeys(multiVarKeys);
                                }
                            }
                        }

                        // Now, check the keys
                        JArray? keys = (JArray?)settingsEntryList["Keys"];
                        if (keys is null || keys.Count == 0)
                            continue;
                        CheckKeys(keys);
                    }
                }
            }
        }

        private void CheckExtraJsonLocalizations(CompilationAnalysisContext context)
        {
#if LOCALECHECK_DEBUG
            // Debug diagnostic
            var debugDiagnostic = Diagnostic.Create(assemblyLocsDbg, null, "DETECTED LOCALIZATIONS: assemblyLocs", string.Join(", ", assemblyLocs));
            context.ReportDiagnostic(debugDiagnostic);
#endif

            Dictionary<string, List<string>> summary = [];
            foreach (var localization in localizationList.Keys)
            {
                string name = localization.Split(' ')[0];
                var hashSet = localizationList[localization];
                summary.Add(name, []);
#if LOCALECHECK_DEBUG
                debugDiagnostic = Diagnostic.Create(assemblyLocsDbg, null, $"DETECTED JSON LOCALIZATIONS: localizationList[{localization}]", string.Join(", ", hashSet));
                context.ReportDiagnostic(debugDiagnostic);
#endif

                // List extras in Locs and JSON
                for (int i = 0; i < hashSet.Item1.Count; i++)
                {
                    string? loc = hashSet.Item1[i];
                    var locObj = hashSet.Item2[i];
                    if (!assemblyLocs.ContainsKey(loc))
                    {
                        summary[name].Add(loc);
                        string filePath = localization.Split(' ')[5];
                        var location = AnalyzerTools.GenerateLocation(locObj, loc, filePath);
                        var diagnostic = Diagnostic.Create(RuleExtraJson, location, filePath, loc);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
            foreach (var summaryKvp in summary)
            {
                if (summaryKvp.Value.Count > 0)
                {
                    var totalDiagnostic = Diagnostic.Create(RuleExtraJsonSummary, null, summaryKvp.Key, string.Join(", ", summaryKvp.Value));
                    context.ReportDiagnostic(totalDiagnostic);
                }
            }
        }

        private void ProcessAssemblyLoc(string localization, string assemblyLoc, out bool found)
        {
            var hashSets = localizationList[localization];
            var hashSet = hashSets.Item1;
            var hashSetObjs = hashSets.Item2;
            found = false;
            for (int i = 0; i < hashSetObjs.Count && !found; i++)
            {
                var locLocalization = hashSet[i];
                var locLocalizationObj = hashSetObjs[i];
                if (locLocalization == assemblyLoc)
                {
                    var locLocation = AnalyzerTools.GenerateLocation(locLocalizationObj, locLocalization, localization.Substring(localization.LastIndexOf(" ")));
                    if (assemblyLocs.ContainsKey(assemblyLoc))
                        assemblyLocs[assemblyLoc].Add(locLocation);
                    else
                        assemblyLocs.Add(assemblyLoc, [locLocation]);
                    found = true;
                }
            }
        }
    }
}
