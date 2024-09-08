//
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
            new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

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
    }
}
