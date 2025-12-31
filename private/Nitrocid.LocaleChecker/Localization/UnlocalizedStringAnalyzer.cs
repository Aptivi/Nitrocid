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

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using Nitrocid.LocaleChecker.Resources;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Nitrocid.Analyzers.Common;

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
        private const string Category = "Localization";

        // Some strings
        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(AnalyzerResources.UnlocalizedStringAnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(AnalyzerResources.UnlocalizedStringAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormatComment =
            new LocalizableResourceString(nameof(AnalyzerResources.UnlocalizedCommentStringAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormatJson =
            new LocalizableResourceString(nameof(AnalyzerResources.UnlocalizedJsonStringAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(AnalyzerResources.UnlocalizedStringAnalyzerDescription), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));

        // A rule
        private static readonly DiagnosticDescriptor Rule =
            new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description, customTags: ["CompilationEnd"]);
        private static readonly DiagnosticDescriptor RuleComment =
            new(DiagnosticIdComment, Title, MessageFormatComment, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description, customTags: ["CompilationEnd"]);
        private static readonly DiagnosticDescriptor RuleJson =
            new(DiagnosticIdJson, Title, MessageFormatJson, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description, customTags: ["CompilationEnd"]);

        // English localization list
        private static readonly Dictionary<string, HashSet<string>> localizationList = [];

        // Supported diagnostics
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(Rule, RuleComment, RuleJson);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
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
                string langFileName = langStream.Substring(langStream.LastIndexOf(" ") + 1);
                string langName = langFileName.Remove(langFileName.IndexOf("."));
                string[] splitLangStream = langStream.Split(' ');
                string assemblyName = langStream.Contains("\\") ? splitLangStream[1].Split('\\')[4] : splitLangStream[1];
                string finalLangStream = $"Nitrocid.Langs {assemblyName} {splitLangStream[2]}";

                // Now, add all localizations to a separate array
                var locHashSet = new HashSet<string>();
                foreach (var localization in localizations)
                {
                    if (localization is null)
                        throw new Exception("There is no localization.");
                    string localizationString = localization.Localization.ToString();
                    locHashSet.Add(localizationString);
                }
                if (!localizationList.ContainsKey(langName + " - " + finalLangStream))
                    localizationList.Add(langName + " - " + finalLangStream, locHashSet);
            }

            // Register the localization analysis action
            context.RegisterSyntaxNodeAction(AnalyzeLocalization, SyntaxKind.InvocationExpression);
            context.RegisterSyntaxNodeAction(AnalyzeImplicitLocalization, SyntaxKind.CompilationUnit);
            context.RegisterCompilationEndAction(AnalyzeResourceLocalization);
        }

        private static void AnalyzeLocalization(SyntaxNodeAnalysisContext context)
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
                if (idExpression == "LanguageTools" && idName == "GetLocalized")
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
                                var hashSet = localizationList[localization];
                                if (!hashSet.Contains(text) && localization.Contains($"Nitrocid.Langs {context.SemanticModel.Compilation.AssemblyName} "))
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
                                    var hashSet = localizationList[localization];
                                    if (!hashSet.Contains(text) && localization.Contains($"Nitrocid.Langs {context.SemanticModel.Compilation.AssemblyName} "))
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
                if (relativePath.Contains("\\"))
                {
                    // Split the path and find "Resources"
                    string[] splitPath = relativePath.Split('\\');
                    int asmNameIdx = splitPath.IndexOf("Resources") - 1;
                    assemblyName = splitPath[asmNameIdx];
                }
                if (assemblyName != context.Compilation.AssemblyName)
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
                    bool localizable = (bool?)themeMetadata["Localizable"] ?? false;
                    if (!string.IsNullOrWhiteSpace(description) && localizable)
                    {
                        List<string> incompleteLangs = [];
                        foreach (var localization in localizationList.Keys)
                        {
                            var hashSet = localizationList[localization];
                            if (!hashSet.Contains(description) && localization.Contains($"Nitrocid.Langs {assemblyName} "))
                                incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                        }
                        if (incompleteLangs.Count > 0)
                        {
                            var location = AnalyzerTools.GenerateLocation(themeMetadata["Description"], descriptionOrig, absolutePath, false);
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
                            foreach (var localization in localizationList.Keys)
                            {
                                var hashSet = localizationList[localization];
                                if (!hashSet.Contains(description) && localization.Contains($"Nitrocid.Langs {assemblyName} "))
                                    incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                            }
                            if (incompleteLangs.Count > 0)
                            {
                                var location = AnalyzerTools.GenerateLocation(settingsEntryList["Desc"], descriptionOrig, absolutePath, false);
                                var diagnostic = Diagnostic.Create(RuleJson, location, description, string.Join(", ", incompleteLangs));
                                context.ReportDiagnostic(diagnostic);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(displayAs))
                        {
                            List<string> incompleteLangs = [];
                            foreach (var localization in localizationList.Keys)
                            {
                                var hashSet = localizationList[localization];
                                if (!hashSet.Contains(displayAs) && localization.Contains($"Nitrocid.Langs {assemblyName} "))
                                    incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                            }
                            if (incompleteLangs.Count > 0)
                            {
                                var location = AnalyzerTools.GenerateLocation(settingsEntryList["DisplayAs"], displayAsOrig, absolutePath, false);
                                var diagnostic = Diagnostic.Create(RuleJson, location, displayAs, string.Join(", ", incompleteLangs));
                                context.ReportDiagnostic(diagnostic);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(knownAddonDisplay))
                        {
                            List<string> incompleteLangs = [];
                            foreach (var localization in localizationList.Keys)
                            {
                                var hashSet = localizationList[localization];
                                if (!hashSet.Contains(knownAddonDisplay) && localization.Contains($"Nitrocid.Langs {assemblyName} "))
                                    incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                            }
                            if (incompleteLangs.Count > 0)
                            {
                                var location = AnalyzerTools.GenerateLocation(settingsEntryList["display"], knownAddonDisplayOrig, absolutePath, false);
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
                                string keyName = keyNameOrig.Replace("\\\"", "\"");
                                string keyDesc = keyDescOrig.Replace("\\\"", "\"");
                                if (!string.IsNullOrWhiteSpace(keyName))
                                {
                                    List<string> incompleteLangs = [];
                                    foreach (var localization in localizationList.Keys)
                                    {
                                        var hashSet = localizationList[localization];
                                        if (!hashSet.Contains(keyName) && localization.Contains($"Nitrocid.Langs {assemblyName} "))
                                            incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                                    }
                                    if (incompleteLangs.Count > 0)
                                    {
                                        var location = AnalyzerTools.GenerateLocation(key["Name"], keyNameOrig, absolutePath, false);
                                        var diagnostic = Diagnostic.Create(RuleJson, location, keyName, string.Join(", ", incompleteLangs));
                                        context.ReportDiagnostic(diagnostic);
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(keyDesc))
                                {
                                    List<string> incompleteLangs = [];
                                    foreach (var localization in localizationList.Keys)
                                    {
                                        var hashSet = localizationList[localization];
                                        if (!hashSet.Contains(keyDesc) && localization.Contains($"Nitrocid.Langs {assemblyName} "))
                                            incompleteLangs.Add(localization.Substring(0, localization.IndexOf(" ")));
                                    }
                                    if (incompleteLangs.Count > 0)
                                    {
                                        var location = AnalyzerTools.GenerateLocation(key["Description"], keyDescOrig, absolutePath, false);
                                        var diagnostic = Diagnostic.Create(RuleJson, location, keyDesc, string.Join(", ", incompleteLangs));
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
    }
}
