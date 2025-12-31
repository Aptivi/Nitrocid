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

namespace Nitrocid.Analyzers.Kernel
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PlatformIdWin32NtUsageAnalyzer : DiagnosticAnalyzer
    {
        // Some constants
        public const string DiagnosticId = "NKS0037";
        private const string Category = "Kernel";

        // Some strings
        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(AnalyzerResources.PlatformIdWin32NtUsageAnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(AnalyzerResources.PlatformIdWin32NtUsageAnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(AnalyzerResources.PlatformIdWin32NtUsageAnalyzerDescription), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));

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
            context.RegisterSyntaxNodeAction(AnalyzePlatformIdWin32NtUsage, SyntaxKind.EqualsExpression);
        }

        private static void AnalyzePlatformIdWin32NtUsage(SyntaxNodeAnalysisContext context)
        {
            // Now, check for the usage of string.Format()
            var exp = (BinaryExpressionSyntax)context.Node;
            var leftExp = (MemberAccessExpressionSyntax)exp.Left;
            var rightExp = (MemberAccessExpressionSyntax)exp.Right;
            var location = exp.GetLocation();
            bool @continue = false;
            if (leftExp.Name is IdentifierNameSyntax identifierLeft)
            {
                if (identifierLeft.Identifier.Text == nameof(Platform))
                {
                    var platformExp = (MemberAccessExpressionSyntax)leftExp.Expression;
                    if (platformExp.Expression is IdentifierNameSyntax identifier &&
                        platformExp.Name is IdentifierNameSyntax identifierName)
                    {
                        // RS1035 occurs when we try to use nameof(Environment.OSVersion). Use "OSVersion" instead.
                        if (identifier.Identifier.Text == nameof(Environment) &&
                            identifierName.Identifier.Text == "OSVersion")
                            @continue = true;
                    }
                }
            }
            if (!@continue)
                return;
            if (rightExp.Expression is IdentifierNameSyntax identifierRight)
            {
                if (identifierRight.Identifier.Text == nameof(PlatformID))
                {
                    // Let's see if the caller tries to access PlatformID.Win32NT.
                    var name = (IdentifierNameSyntax)rightExp.Name;
                    var idName = name.Identifier.Text;
                    if (idName == nameof(PlatformID.Win32NT))
                    {
                        var diagnostic = Diagnostic.Create(Rule, location);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
