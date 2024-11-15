﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using System.Globalization;
using System.Collections.Immutable;
using Nitrocid.LocaleChecker.Resources;
using System.Diagnostics;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Text;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Nitrocid.LocaleChecker.Localization
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UnlocalizedStringAnalyzer : DiagnosticAnalyzer
    {
        // This assembly
        private readonly Assembly thisAssembly = typeof(UnlocalizedStringAnalyzer).Assembly;

        // Some constants
        public const string DiagnosticId = "NLOC0001";
        private const string Category = "Localization";

        // Some strings
        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(AnalyzerResources.UnlocalizedStringAnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(AnalyzerResources.UnlocalizedStringAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(AnalyzerResources.UnlocalizedStringAnalyzerDescription), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));

        // A rule
        private static readonly DiagnosticDescriptor Rule =
            new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description, customTags: ["CompilationEnd"]);

        // English localization list
        private static readonly HashSet<string> localizationList = [];

        // Supported diagnostics
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationStartAction(PopulateEnglishLocalizations);
        }

        private void PopulateEnglishLocalizations(CompilationStartAnalysisContext context)
        {
            // Find the English JSON stream and open it.
            var stream = thisAssembly.GetManifestResourceStream("Nitrocid.LocaleChecker.eng.json") ??
                throw new Exception("Opening the eng.json resource stream has failed.");
            var reader = new StreamReader(stream);
            var jsonReader = new JsonTextReader(reader);
            var document = JToken.Load(jsonReader) ??
                throw new Exception("Unable to parse JSON for English localizations.");
            var localizations = document["Localizations"]?.Values<string>() ??
                throw new Exception("Unable to get localizations.");

            // Now, add all localizations to a separate array
            foreach (var localization in localizations)
            {
                if (localization is null)
                    throw new Exception("There is no localization.");
                string localizationString = localization.ToString();
                localizationList.Add(localizationString);
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

            // Now, check for the Translate.DoTranslation() call
            if (exp.Expression is not MemberAccessExpressionSyntax expMaes)
                return;
            if (expMaes.Expression is IdentifierNameSyntax expIdentifier && expMaes.Name is IdentifierNameSyntax identifier)
            {
                // Verify that we're dealing with Translate.DoTranslation()
                var location = context.Node.GetLocation();
                var idExpression = expIdentifier.Identifier.Text;
                var idName = identifier.Identifier.Text;
                if (idExpression == "Translate" && idName == "DoTranslation")
                {
                    // Now, get the string representation from the argument count and compare it with the list of translations.
                    // You'll notice that we sometimes call Translate.DoTranslation() with a variable instead of a string, so
                    // check that first, because they're usually obtained from a string representation usually prefixed with
                    // either the /* Localizable */ comment or in individual kernel resources. However, the resources don't
                    // have a prefix, so the key names alone are enough.
                    if (localizableStringArgument.Expression is LiteralExpressionSyntax literalText)
                    {
                        string text = literalText.ToString();
                        text = text.Substring(1, text.Length - 2).Replace("\\\"", "\"");
                        if (!localizationList.Contains(text))
                        {
                            var diagnostic = Diagnostic.Create(Rule, location, text);
                            context.ReportDiagnostic(diagnostic);
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
                            if (!localizationList.Contains(text))
                            {
                                var diagnostic = Diagnostic.Create(Rule, location, text);
                                context.ReportDiagnostic(diagnostic);
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
            // Just launch once
            if (context.Compilation.Assembly.Name != "Nitrocid")
                return;

            // Open every resource except the English translations file and the analyzer string resources
            var resourceNames = thisAssembly.GetManifestResourceNames().Except([
                "Nitrocid.LocaleChecker.eng.json",
                "Nitrocid.LocaleChecker.Resources.AnalyzerResources.resources",
            ]);
            foreach (var resourceName in resourceNames)
            {
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
                    string description = ((string?)themeMetadata["Description"] ?? "").Replace("\\\"", "\"");
                    bool localizable = (bool?)themeMetadata["Localizable"] ?? false;
                    if (!string.IsNullOrEmpty(description) && localizable && !localizationList.Contains(description))
                    {
                        var diagnostic = Diagnostic.Create(Rule, null, description);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
                else if (document.Type == JTokenType.Array)
                {
                    // It's likely a settings entry list, but verify
                    foreach (var settingsEntryList in document)
                    {
                        // Check the description and the display
                        string description = ((string?)settingsEntryList["Desc"] ?? "").Replace("\\\"", "\"");
                        string displayAs = ((string?)settingsEntryList["DisplayAs"] ?? "").Replace("\\\"", "\"");
                        string knownAddonDisplay = ((string?)settingsEntryList["display"] ?? "").Replace("\\\"", "\"");
                        if (!string.IsNullOrEmpty(description) && !localizationList.Contains(description))
                        {
                            var diagnostic = Diagnostic.Create(Rule, null, description);
                            context.ReportDiagnostic(diagnostic);
                        }
                        if (!string.IsNullOrEmpty(displayAs) && !localizationList.Contains(displayAs))
                        {
                            var diagnostic = Diagnostic.Create(Rule, null, displayAs);
                            context.ReportDiagnostic(diagnostic);
                        }
                        if (!string.IsNullOrEmpty(knownAddonDisplay) && !localizationList.Contains(knownAddonDisplay))
                        {
                            var diagnostic = Diagnostic.Create(Rule, null, knownAddonDisplay);
                            context.ReportDiagnostic(diagnostic);
                        }

                        // Now, check the keys
                        JArray? keys = (JArray?)settingsEntryList["Keys"];
                        if (keys is null || keys.Count == 0)
                            continue;
                        foreach (var key in keys)
                        {
                            string keyName = ((string?)key["Name"] ?? "").Replace("\\\"", "\"");
                            string keyDesc = ((string?)key["Description"] ?? "").Replace("\\\"", "\"");
                            if (!string.IsNullOrEmpty(keyName) && !localizationList.Contains(keyName))
                            {
                                var diagnostic = Diagnostic.Create(Rule, null, keyName);
                                context.ReportDiagnostic(diagnostic);
                            }
                            if (!string.IsNullOrEmpty(keyDesc) && !localizationList.Contains(keyDesc))
                            {
                                var diagnostic = Diagnostic.Create(Rule, null, keyDesc);
                                context.ReportDiagnostic(diagnostic);
                            }
                        }
                    }
                }
            }
        }
    }
}