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
using Nitrocid.Analyzers.Resources;
using System;
using System.Collections.Immutable;

namespace Nitrocid.Analyzers.Misc.Text
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class StringFormatAnalyzer : DiagnosticAnalyzer
    {
        // Some constants
        public const string DiagnosticId = "NKS0001";
        private const string Category = "Text";

        // Some strings
        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(AnalyzerResources.StringFormatAnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(AnalyzerResources.StringFormatAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(AnalyzerResources.StringFormatAnalyzerDescription), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));

        // A rule
        private static readonly DiagnosticDescriptor Rule =
            new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        // Supported diagnostics
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeStringFormatUsage, SyntaxKind.SimpleMemberAccessExpression);
        }

        private static void AnalyzeStringFormatUsage(SyntaxNodeAnalysisContext context)
        {
            // Now, check for the usage of string.Format()
            var exp = (MemberAccessExpressionSyntax)context.Node;
            if (exp.Expression is PredefinedTypeSyntax expression)
            {
                var name = (IdentifierNameSyntax)exp.Name;
                var location = context.Node.GetLocation();
                if (expression.Keyword.Text == "string" && name.Identifier.Text == nameof(string.Format))
                {
                    var diagnostic = Diagnostic.Create(Rule, location);
                    context.ReportDiagnostic(diagnostic);
                }
            }
            else if (exp.Expression is IdentifierNameSyntax identifier)
            {
                if (exp.Name is IdentifierNameSyntax name)
                {
                    var location = context.Node.GetLocation();
                    if (identifier.Identifier.Text == nameof(String) && name.Identifier.Text == nameof(string.Format))
                    {
                        var diagnostic = Diagnostic.Create(Rule, location);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
