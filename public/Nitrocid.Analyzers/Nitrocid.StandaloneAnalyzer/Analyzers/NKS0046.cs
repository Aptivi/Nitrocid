//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using System.Globalization;
using Nitrocid.Analyzers.Common;

namespace Nitrocid.StandaloneAnalyzer.Analyzers
{
    internal class NKS0046 : IAnalyzer
    {
        public bool Analyze(Document document)
        {
            var tree = document.GetSyntaxTreeAsync().Result;
            if (tree is null)
                return false;
            var syntaxNodeNodes = tree.GetRoot().DescendantNodesAndSelf().OfType<SyntaxNode>().ToList();
            bool found = false;
            foreach (var syntaxNode in syntaxNodeNodes)
            {
                if (syntaxNode is not MemberAccessExpressionSyntax exp)
                    continue;

                // Analyze!
                if (exp.Expression is IdentifierNameSyntax identifier && exp.Parent is AssignmentExpressionSyntax assignment)
                {
                    var rightExpInvocation = (InvocationExpressionSyntax)assignment.Right;
                    var location = exp.Parent.GetLocation();
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
                            AnalyzerTools.PrintFromLocation(location, document, GetType(), "Caller uses CultureInfo.CurrentUICulture instead of CultureManager.UpdateCulture()");
                            found = true;
                        }
                    }
                }
            }
            return found;
        }

        public async Task SuggestAsync(Document document, CancellationToken cancellationToken = default)
        {
            var tree = document.GetSyntaxTreeAsync(cancellationToken).Result;
            if (tree is null)
                return;
            var syntaxNodeNodes = tree.GetRoot(cancellationToken).DescendantNodesAndSelf().OfType<SyntaxNode>().ToList();
            foreach (var syntaxNode in syntaxNodeNodes)
            {
                if (syntaxNode is not MemberAccessExpressionSyntax exp)
                    continue;

                // We need to have a syntax that calls CultureManager.UpdateCulture
                var classSyntax = SyntaxFactory.IdentifierName("CultureManager");
                var methodSyntax = SyntaxFactory.IdentifierName("UpdateCulture");
                var maeSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, classSyntax, methodSyntax);
                var parentSyntax = (AssignmentExpressionSyntax?)exp.Parent;
                if (parentSyntax is null)
                    return;
                var invocationSyntax = (InvocationExpressionSyntax)parentSyntax.Right;
                var argumentListSyntax = invocationSyntax.ArgumentList.Arguments;
                if (argumentListSyntax.Count != 1)
                    continue;
                var argumentSyntax = (LiteralExpressionSyntax)argumentListSyntax[0].Expression;
                var tokenSyntax = argumentSyntax.Token;
                var valueArgSyntax = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, tokenSyntax);
                var valueSyntax = SyntaxFactory.Argument(valueArgSyntax);
                var valuesSyntax = SyntaxFactory.ArgumentList().AddArguments(valueSyntax);
                var resultSyntax = SyntaxFactory.InvocationExpression(maeSyntax, valuesSyntax);
                var replacedSyntax = resultSyntax
                    .WithLeadingTrivia(resultSyntax.GetLeadingTrivia())
                    .WithTrailingTrivia(resultSyntax.GetTrailingTrivia());

                // Actually replace
                var node = await document.GetSyntaxRootAsync(cancellationToken);
                var finalNode = node?.ReplaceNode(parentSyntax, replacedSyntax);
                TextWriterColor.WriteColor("Here's what the replacement would look like (with no Roslyn trivia):", true, ConsoleColors.Yellow);
                TextWriterColor.WriteColor($"  - {exp}", true, ConsoleColors.Red);
                TextWriterColor.WriteColor($"  + {replacedSyntax.ToFullString()}", true, ConsoleColors.Green);

                // Check the imports
                var compilation = finalNode as CompilationUnitSyntax;
                if (compilation?.Usings.Any(u => u.Name?.ToString() == $"{AnalysisTools.rootNameSpace}.Languages") == false)
                {
                    var name = SyntaxFactory.QualifiedName(
                        SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName(AnalysisTools.firstRootNameSpace), SyntaxFactory.IdentifierName("Base")),
                        SyntaxFactory.IdentifierName("Languages"));
                    var directive = SyntaxFactory.UsingDirective(name).NormalizeWhitespace();
                    TextWriterColor.WriteColor("Additionally, the suggested fix will add the following using statement:", true, ConsoleColors.Yellow);
                    TextWriterColor.WriteColor($"  + {directive.ToFullString()}", true, ConsoleColors.Green);
                }
            }
        }
    }
}
