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

namespace Nitrocid.Analyzers.ConsoleBase
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ConsoleBackColorUsageAnalyzer : DiagnosticAnalyzer
    {
        // Some constants
        public const string DiagnosticId = "NKS0005";
        private const string Category = "ConsoleBase";

        // Some strings
        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(AnalyzerResources.ConsoleBackColorUsageAnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(AnalyzerResources.ConsoleBackColorUsageAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(AnalyzerResources.ConsoleBackColorUsageAnalyzerDescription), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));

        // A rule
        private static readonly DiagnosticDescriptor Rule =
            new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: Description);

        // Supported diagnostics
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeConsoleBackColorUsage, SyntaxKind.SimpleMemberAccessExpression);
        }

        private static void AnalyzeConsoleBackColorUsage(SyntaxNodeAnalysisContext context)
        {
            // Now, check for the usage of string.Format()
            var exp = (MemberAccessExpressionSyntax)context.Node;
            if (exp.Expression is IdentifierNameSyntax identifier)
            {
                var location = context.Node.GetLocation();
                if (identifier.Identifier.Text == nameof(Console))
                {
                    // Let's see if the caller tries to access Console.BackgroundColor.
                    var name = (IdentifierNameSyntax)exp.Name;
                    var idName = name.Identifier.Text;

                    // False positive, because we're actually getting the name of the Console.BackgroundColor property.
                    // Since that is always constant, stringify it.
                    if (idName == "BackgroundColor")
                    {
                        var diagnostic = Diagnostic.Create(Rule, location);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
