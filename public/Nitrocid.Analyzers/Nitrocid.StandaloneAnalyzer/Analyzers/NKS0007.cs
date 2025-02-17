﻿//
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
    internal class NKS0007 : IAnalyzer
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
                if (exp.Expression is IdentifierNameSyntax identifier)
                {
                    var location = syntaxNode.GetLocation();
                    if (identifier.Identifier.Text == nameof(Console))
                    {
                        // Let's see if the caller tries to access Console.Write.
                        var name = (IdentifierNameSyntax)exp.Name;
                        var idName = name.Identifier.Text;
                        if (idName == nameof(Console.Write))
                        {
                            AnalyzerTools.PrintFromLocation(location, document, GetType(), "Caller uses Console.Write instead of TWC.Write()");
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
                if (exp.Expression is IdentifierNameSyntax identifier && exp.Name is IdentifierNameSyntax idName)
                {
                    // Build the replacement syntax
                    var classSyntax = SyntaxFactory.IdentifierName("TextWriterColor");
                    var methodSyntax = SyntaxFactory.IdentifierName("Write");
                    if (identifier.Identifier.Text != nameof(Console))
                        continue;
                    if (idName.Identifier.Text != nameof(Console.Write))
                        continue;
                    var parentSyntax = (InvocationExpressionSyntax?)exp.Parent;
                    if (parentSyntax is null)
                        return;
                    var argsListSyntax = parentSyntax.ArgumentList.Arguments;
                    var falseArg = SyntaxFactory.Argument(SyntaxFactory.ParseExpression("false"));
                    if (argsListSyntax.Count > 1)
                    {
                        // ...akin to TWC.Write("MyText {0}", false, myarg)
                        argsListSyntax = argsListSyntax.Insert(1, falseArg);
                    }
                    else if (argsListSyntax.Count == 1)
                    {
                        // ...akin to TWC.Write("MyText", false)
                        argsListSyntax = argsListSyntax.Add(falseArg);
                    }
                    var valuesSyntax = SyntaxFactory.ArgumentList(argsListSyntax);
                    var maeSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, classSyntax, methodSyntax);
                    var resultSyntax = SyntaxFactory.InvocationExpression(maeSyntax, valuesSyntax);

                    // Actually replace
                    var node = await document.GetSyntaxRootAsync(cancellationToken);
                    var finalNode = node?.ReplaceNode(parentSyntax, resultSyntax);
                    TextWriterColor.WriteColor("Here's what the replacement would look like (with no Roslyn trivia):", true, ConsoleColors.Yellow);
                    TextWriterColor.WriteColor($"  - {parentSyntax}", true, ConsoleColors.Red);
                    TextWriterColor.WriteColor($"  + {resultSyntax.ToFullString()}", true, ConsoleColors.Green);

                    // Check the imports
                    var compilation = finalNode as CompilationUnitSyntax;
                    if (compilation?.Usings.Any(u => u.Name?.ToString() == $"{AnalysisTools.rootNameSpace}.ConsoleBase.Writers.ConsoleWriters") == false)
                    {
                        var name = SyntaxFactory.QualifiedName(
                            SyntaxFactory.QualifiedName(
                                SyntaxFactory.QualifiedName(
                                    SyntaxFactory.IdentifierName(AnalysisTools.rootNameSpace),
                                    SyntaxFactory.IdentifierName("ConsoleBase")),
                                SyntaxFactory.IdentifierName("Writers")),
                            SyntaxFactory.IdentifierName("ConsoleWriters"));
                        var directive = SyntaxFactory.UsingDirective(name).NormalizeWhitespace();
                        TextWriterColor.WriteColor("Additionally, the suggested fix will add the following using statement:", true, ConsoleColors.Yellow);
                        TextWriterColor.WriteColor($"  + {directive.ToFullString()}", true, ConsoleColors.Green);
                    }
                }
            }
        }
    }
}
