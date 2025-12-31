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
    public class CurrentUICultureSetUsageAnalyzer : DiagnosticAnalyzer
    {
        // Some constants
        public const string DiagnosticId = "NKS0046";
        private const string Category = "Languages";

        // Some strings
        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(AnalyzerResources.CurrentUICultureSetUsageAnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(AnalyzerResources.CurrentUICultureSetUsageAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(AnalyzerResources.CurrentUICultureSetUsageAnalyzerDescription), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));

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
            context.RegisterSyntaxNodeAction(AnalyzeCurrentUICultureSetUsage, SyntaxKind.SimpleMemberAccessExpression);
        }

        private static void AnalyzeCurrentUICultureSetUsage(SyntaxNodeAnalysisContext context)
        {
            // Now, check for the usage of string.Format()
            var exp = (MemberAccessExpressionSyntax)context.Node;
            if (exp.Expression is IdentifierNameSyntax identifier && exp.Parent is AssignmentExpressionSyntax assignment)
            {
                var rightExpInvocation = (InvocationExpressionSyntax)assignment.Right;
                var location = exp.Parent?.GetLocation();
                var rightExp = (MemberAccessExpressionSyntax)rightExpInvocation.Expression;
                var rightExpIdExp = (IdentifierNameSyntax)rightExp.Expression;
                var rightExpIdName = (IdentifierNameSyntax)rightExp.Name;
                if (identifier.Identifier.Text == nameof(CultureInfo) && rightExpIdExp.Identifier.Text == nameof(CultureInfo))
                {
                    // Let's see if the caller tries to access CultureInfo.CurrentUICulture.
                    var name = (IdentifierNameSyntax)exp.Name;
                    var idName = name.Identifier.Text;

                    // RS1035 occurs when we try to use nameof(CultureInfo.CurrentUICulture). Use "CurrentUICulture" instead.
                    if (idName == "CurrentUICulture" && rightExpIdName.Identifier.Text == nameof(CultureInfo.GetCultureInfo))
                    {
                        var diagnostic = Diagnostic.Create(Rule, location);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
