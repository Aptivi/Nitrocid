
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.MiscWriters;

namespace Nitrocid.StandaloneAnalyzer.Analyzers
{
    internal class NKS0004 : IAnalyzer
    {
        public bool Analyze(Document document)
        {
            var tree = document.GetSyntaxTreeAsync().Result;
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
                        // Let's see if the caller tries to access Console.ForegroundColor.
                        var name = (IdentifierNameSyntax)exp.Name;
                        var idName = name.Identifier.Text;
                        if (idName == nameof(Console.ForegroundColor))
                        {
                            var lineSpan = location.GetLineSpan();
                            TextWriterColor.Write($"{GetType().Name}: {document.FilePath} ({lineSpan.StartLinePosition} -> {lineSpan.EndLinePosition}): Caller uses Console.ForegroundColor instead of SetConsoleColor(Color)", true, ConsoleColors.Yellow);
                            if (!string.IsNullOrEmpty(document.FilePath))
                                LineHandleWriter.PrintLineWithHandle(document.FilePath, lineSpan.StartLinePosition.Line + 1, lineSpan.StartLinePosition.Character + 1);
                            found = true;
                        }
                    }
                }
            }
            return found;
        }

        public async Task SuggestAsync(Document document, CancellationToken cancellationToken = default)
        {
            var tree = document.GetSyntaxTreeAsync().Result;
            var syntaxNodeNodes = tree.GetRoot().DescendantNodesAndSelf().OfType<SyntaxNode>().ToList();
            foreach (var syntaxNode in syntaxNodeNodes)
            {
                if (syntaxNode is not MemberAccessExpressionSyntax exp)
                    continue;
                if (exp.Expression is IdentifierNameSyntax identifier)
                {
                    // Build the replacement syntax
                    var classSyntax = SyntaxFactory.IdentifierName("KernelColorTools");
                    var methodSyntax = SyntaxFactory.IdentifierName("SetConsoleColor");
                    if (identifier.Identifier.Text != nameof(Console))
                        continue;
                    var maeSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, classSyntax, methodSyntax);
                    var parentSyntax = (AssignmentExpressionSyntax)exp.Parent;
                    var valueSyntax = SyntaxFactory.Argument(parentSyntax.Right.ReplaceNode(((MemberAccessExpressionSyntax)parentSyntax.Right).Expression, SyntaxFactory.IdentifierName("ConsoleColors")));
                    var valuesSyntax = SyntaxFactory.ArgumentList().AddArguments(valueSyntax);
                    var resultSyntax = SyntaxFactory.InvocationExpression(maeSyntax, valuesSyntax);

                    // Actually replace
                    var node = await document.GetSyntaxRootAsync(cancellationToken);
                    var finalNode = node.ReplaceNode(parentSyntax, resultSyntax);
                    TextWriterColor.Write("Here's what the replacement would look like (with no Roslyn trivia):", true, ConsoleColors.Yellow);
                    TextWriterColor.Write($"  - {exp}", true, ConsoleColors.Red);
                    TextWriterColor.Write($"  + {resultSyntax.ToFullString()}", true, ConsoleColors.Green);

                    // Check the imports
                    var compilation = finalNode as CompilationUnitSyntax;
                    if (compilation?.Usings.Any(u => u.Name.ToString() == "KS.ConsoleBase.Colors") == false)
                    {
                        var name = SyntaxFactory.QualifiedName(
                            SyntaxFactory.IdentifierName("KS.ConsoleBase"),
                            SyntaxFactory.IdentifierName("Colors"));
                        var directive = SyntaxFactory.UsingDirective(name).NormalizeWhitespace();
                        TextWriterColor.Write("Additionally, the suggested fix will add the following using statements:", true, ConsoleColors.Yellow);
                        TextWriterColor.Write($"  + {directive.ToFullString()}", true, ConsoleColors.Green);
                    }
                    if (compilation?.Usings.Any(u => u.Name.ToString() == "Terminaux.Colors") == false)
                    {
                        var name = SyntaxFactory.QualifiedName(
                            SyntaxFactory.IdentifierName("Terminaux"),
                            SyntaxFactory.IdentifierName("Colors"));
                        var directive = SyntaxFactory.UsingDirective(name).NormalizeWhitespace();
                        TextWriterColor.Write($"  + {directive.ToFullString()}", true, ConsoleColors.Green);
                    }
                }
            }
        }
    }
}
