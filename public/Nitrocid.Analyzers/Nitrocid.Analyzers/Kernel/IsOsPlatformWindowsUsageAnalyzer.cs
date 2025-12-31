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
using System.Runtime.InteropServices;

namespace Nitrocid.Analyzers.Kernel
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IsOsPlatformWindowsUsageAnalyzer : DiagnosticAnalyzer
    {
        // Some constants
        public const string DiagnosticId = "NKS0039";
        private const string Category = "Kernel";

        // Some strings
        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(AnalyzerResources.IsOsPlatformWindowsUsageAnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(AnalyzerResources.IsOsPlatformWindowsUsageAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(AnalyzerResources.IsOsPlatformWindowsUsageAnalyzerDescription), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));

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
            context.RegisterSyntaxNodeAction(AnalyzeIsOsPlatformWindowsUsage, SyntaxKind.SimpleMemberAccessExpression);
        }

        private static void AnalyzeIsOsPlatformWindowsUsage(SyntaxNodeAnalysisContext context)
        {
            // Now, check for the usage of string.Format()
            var exp = (MemberAccessExpressionSyntax)context.Node;
            var location = exp.Parent?.GetLocation();
            bool found = false;
            if (exp.Expression is IdentifierNameSyntax identifier)
            {
                if (identifier.Identifier.Text == nameof(RuntimeInformation))
                {
                    // Let's see if the caller tries to access RuntimeInformation.IsOSPlatform.
                    var name = (IdentifierNameSyntax)exp.Name;
                    var idName = name.Identifier.Text;
                    if (idName == nameof(RuntimeInformation.IsOSPlatform))
                        found = true;
                }
            }
            if (!found)
                return;
            if (exp.Parent is InvocationExpressionSyntax invocation)
            {
                var args = invocation.ArgumentList;
                var argsList = args.Arguments;
                if (argsList.Count == 1)
                {
                    var argSyntax = (MemberAccessExpressionSyntax)argsList[0].Expression;
                    if (argSyntax.Expression is IdentifierNameSyntax argIdentifier)
                    {
                        if (argIdentifier.Identifier.Text == nameof(OSPlatform))
                        {
                            // Let's see if the caller tries to access OSPlatform.Windows.
                            var name = (IdentifierNameSyntax)argSyntax.Name;
                            var idName = name.Identifier.Text;
                            if (idName == nameof(OSPlatform.Windows))
                            {
                                var diagnostic = Diagnostic.Create(Rule, location);
                                context.ReportDiagnostic(diagnostic);
                            }
                        }
                    }
                }
            }
        }
    }
}
