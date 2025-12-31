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
using System.Globalization;
using System.Collections.Immutable;

namespace Nitrocid.Analyzers.Languages
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CurrentUICultureGetNameUsageAnalyzer : DiagnosticAnalyzer
    {
        // Some constants
        public const string DiagnosticId = "NKS0045";
        private const string Category = "Languages";

        // Some strings
        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(AnalyzerResources.CurrentUICultureGetNameUsageAnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(AnalyzerResources.CurrentUICultureGetNameUsageAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(AnalyzerResources.CurrentUICultureGetNameUsageAnalyzerDescription), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));

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
            context.RegisterSyntaxNodeAction(AnalyzeCurrentUICultureGetNameUsage, SyntaxKind.SimpleMemberAccessExpression);
        }

        private static void AnalyzeCurrentUICultureGetNameUsage(SyntaxNodeAnalysisContext context)
        {
            // Now, check for the usage of string.Format()
            var exp = (MemberAccessExpressionSyntax)context.Node;
            if (exp.Expression is MemberAccessExpressionSyntax maes)
            {
                var maesLeft = maes.Expression;
                var maesRight = maes.Name;
                var location = context.Node.GetLocation();
                if (exp.Name is not IdentifierNameSyntax identifier)
                    return;
                if (maesLeft is not IdentifierNameSyntax identifierLeft)
                    return;
                if (maesRight is not IdentifierNameSyntax identifierRight)
                    return;

                // RS1035 occurs when we try to use nameof(CultureInfo.CurrentUICulture). Use "CurrentUICulture" instead.
                if (identifierLeft.Identifier.Text == nameof(CultureInfo) &&
                    identifierRight.Identifier.Text == "CurrentUICulture")
                {
                    // Let's see if the caller tries to access CultureInfo.CurrentUICulture.
                    var idName = identifier.Identifier.Text;
                    if (idName == "Name")
                    {
                        var diagnostic = Diagnostic.Create(Rule, location);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
