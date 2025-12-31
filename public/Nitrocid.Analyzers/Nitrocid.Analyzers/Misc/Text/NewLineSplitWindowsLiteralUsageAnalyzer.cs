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
using System.Collections.Immutable;

namespace Nitrocid.Analyzers.Misc.Text
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NewLineSplitWindowsLiteralUsageAnalyzer : DiagnosticAnalyzer
    {
        // Some constants
        public const string DiagnosticId = "NKS0048";
        private const string Category = "Text";

        // Some strings
        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(AnalyzerResources.NewLineSplitWindowsLiteralUsageAnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(AnalyzerResources.NewLineSplitWindowsLiteralUsageAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(AnalyzerResources.NewLineSplitWindowsLiteralUsageAnalyzerDescription), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));

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
            context.RegisterSyntaxNodeAction(AnalyzeNewLineSplitWindowsLiteralUsage, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeNewLineSplitWindowsLiteralUsage(SyntaxNodeAnalysisContext context)
        {
            // Now, check for the usage of string.Format()
            var exp = (InvocationExpressionSyntax)context.Node;
            var args = exp.ArgumentList.Arguments;
            if (args.Count != 1)
                return;
            if (args[0].Expression is LiteralExpressionSyntax les)
                if (les.Token.ValueText != "\r\n")
                    return;
            if (exp.Expression is not MemberAccessExpressionSyntax maes)
                return;
            if (maes.Name is IdentifierNameSyntax identifier)
            {
                var location = context.Node.GetLocation();

                // Let's see if the caller tries to access .Split.
                var idName = identifier.Identifier.Text;
                if (idName == nameof(string.Split))
                {
                    var diagnostic = Diagnostic.Create(Rule, location);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
