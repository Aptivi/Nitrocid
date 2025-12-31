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

namespace Nitrocid.Analyzers.Kernel.Time.Renderers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class KernelDateTimeToStringUtcUsageAnalyzer : DiagnosticAnalyzer
    {
        // Some constants
        public const string DiagnosticId = "NKS0033";
        private const string Category = "Kernel";

        // Some strings
        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(AnalyzerResources.KernelDateTimeToStringUtcUsageAnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(AnalyzerResources.KernelDateTimeToStringUtcUsageAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(AnalyzerResources.KernelDateTimeToStringUtcUsageAnalyzerDescription), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));

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
            context.RegisterSyntaxNodeAction(AnalyzeKernelDateTimeToStringUtcUsage, SyntaxKind.SimpleMemberAccessExpression);
        }

        private static void AnalyzeKernelDateTimeToStringUtcUsage(SyntaxNodeAnalysisContext context)
        {
            // Now, check for the usage of string.Format()
            var exp = (MemberAccessExpressionSyntax)context.Node;

            // Check the ToString() call
            if (exp.Parent is InvocationExpressionSyntax parentIes)
            {
                var parentExp = (MemberAccessExpressionSyntax)parentIes.Expression;
                if (parentExp.Name.ToString() != nameof(ToString))
                    return;
            }
            else if (exp.Parent is MemberAccessExpressionSyntax parentMaes)
            {
                if (parentMaes.Name.ToString() != nameof(ToString))
                    return;
            }

            // Analyze!
            if (exp.Expression is IdentifierNameSyntax identifier)
            {
                var location = exp.Parent?.GetLocation();
                if (identifier.Identifier.Text == "TimeDateTools")
                {
                    // Let's see if the caller tries to access TimeDateTools.KernelDateTimeUtc.ToString.
                    var name = (IdentifierNameSyntax)exp.Name;
                    var idName = name.Identifier.Text;
                    if (idName == "KernelDateTimeUtc")
                    {
                        var diagnostic = Diagnostic.Create(Rule, location);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
