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
using Nitrocid.Analyzers.Common;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.StandaloneAnalyzer.Analyzers
{
    internal class NKS0043 : IAnalyzer
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
                var location = exp.Parent?.GetLocation();
                bool synFound = false;
                if (exp.Expression is IdentifierNameSyntax identifier)
                {
                    if (identifier.Identifier.Text == nameof(Environment))
                    {
                        // Let's see if the caller tries to access RuntimeInformation.GetEnvironmentVariable.
                        var name = (IdentifierNameSyntax)exp.Name;
                        var idName = name.Identifier.Text;
                        if (idName == nameof(Environment.GetEnvironmentVariable))
                            found = true;
                    }
                }
                if (!synFound)
                    continue;
                if (exp.Parent is InvocationExpressionSyntax invocation)
                {
                    var args = invocation.ArgumentList;
                    var argsList = args.Arguments;
                    if (argsList.Count == 1)
                    {
                        var argSyntax = (LiteralExpressionSyntax)argsList[0].Expression;
                        var argToken = argSyntax.Token;
                        if (argToken.ValueText == "TERM" && location is not null)
                        {
                            AnalyzerTools.PrintFromLocation(location, document, GetType(), "Caller uses RuntimeInformation.IsOSPlatform(OSPlatform.OSX) instead of KernelPlatform.IsOnMacOS()");
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

                // We need to have a syntax that calls KernelPlatform.GetTerminalType
                var classSyntax = SyntaxFactory.IdentifierName("KernelPlatform");
                var methodSyntax = SyntaxFactory.IdentifierName("GetTerminalType");
                var maesSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, classSyntax, methodSyntax);
                var argsSyntax = SyntaxFactory.ArgumentList();
                var resultSyntax = SyntaxFactory.InvocationExpression(maesSyntax, argsSyntax);
                var replacedSyntax = resultSyntax
                    .WithLeadingTrivia(resultSyntax.GetLeadingTrivia())
                    .WithTrailingTrivia(resultSyntax.GetTrailingTrivia());

                // Actually replace
                if (exp.Parent is null)
                    return;
                var node = await document.GetSyntaxRootAsync(cancellationToken);
                var finalNode = node?.ReplaceNode(exp.Parent, replacedSyntax);
                TextWriterColor.WriteColor("Here's what the replacement would look like (with no Roslyn trivia):", true, ConsoleColors.Yellow);
                TextWriterColor.WriteColor($"  - {exp}", true, ConsoleColors.Red);
                TextWriterColor.WriteColor($"  + {replacedSyntax.ToFullString()}", true, ConsoleColors.Green);

                // Check the imports
                var compilation = finalNode as CompilationUnitSyntax;
                if (compilation?.Usings.Any(u => u.Name?.ToString() == $"{AnalysisTools.rootNameSpace}.Kernel") == false)
                {
                    var name = SyntaxFactory.QualifiedName(
                        SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName(AnalysisTools.firstRootNameSpace), SyntaxFactory.IdentifierName("Base")),
                        SyntaxFactory.IdentifierName("Kernel"));
                    var directive = SyntaxFactory.UsingDirective(name).NormalizeWhitespace();
                    TextWriterColor.WriteColor("Additionally, the suggested fix will add the following using statement:", true, ConsoleColors.Yellow);
                    TextWriterColor.WriteColor($"  + {directive.ToFullString()}", true, ConsoleColors.Green);
                }
            }
        }
    }
}
