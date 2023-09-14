
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
    internal class NKS0001 : IAnalyzer
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
                if (exp.Expression is PredefinedTypeSyntax expression)
                {
                    var name = (IdentifierNameSyntax)exp.Name;
                    var location = syntaxNode.GetLocation();
                    if (expression.Keyword.Text == "string" && name.Identifier.Text == nameof(string.Format))
                    {
                        var lineSpan = location.GetLineSpan();
                        TextWriterColor.Write($"{GetType().Name}: {document.FilePath} ({lineSpan.StartLinePosition} -> {lineSpan.EndLinePosition}): Caller uses string.Format() instead of TextTools.FormatString()", true, ConsoleColors.Yellow);
                        if (!string.IsNullOrEmpty(document.FilePath))
                            LineHandleWriter.PrintLineWithHandle(document.FilePath, lineSpan.StartLinePosition.Line + 1, lineSpan.StartLinePosition.Character + 1);
                        found = true;
                    }
                }
                else if (exp.Expression is IdentifierNameSyntax identifier)
                {
                    if (exp.Name is IdentifierNameSyntax name)
                    {
                        var location = syntaxNode.GetLocation();
                        if (identifier.Identifier.Text == nameof(String) && name.Identifier.Text == nameof(string.Format))
                        {
                            var lineSpan = location.GetLineSpan();
                            TextWriterColor.Write($"{GetType().Name}: {document.FilePath} ({lineSpan.StartLinePosition} -> {lineSpan.EndLinePosition}): Caller uses string.Format() instead of TextTools.FormatString()", true, ConsoleColors.Yellow);
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

                // We need to have a syntax that calls TextTools.FormatString
                var classSyntax = SyntaxFactory.IdentifierName("TextTools");
                var methodSyntax = SyntaxFactory.IdentifierName("FormatString");
                var resultSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, classSyntax, methodSyntax);
                var replacedSyntax = resultSyntax
                    .WithLeadingTrivia(resultSyntax.GetLeadingTrivia())
                    .WithTrailingTrivia(resultSyntax.GetTrailingTrivia());

                // Actually replace
                var node = await document.GetSyntaxRootAsync(cancellationToken);
                var finalNode = node.ReplaceNode(syntaxNode, replacedSyntax);
                TextWriterColor.Write("Here's what the replacement would look like (with no Roslyn trivia):", true, ConsoleColors.Yellow);
                TextWriterColor.Write($"  - {exp}", true, ConsoleColors.Red);
                TextWriterColor.Write($"  + {replacedSyntax.ToFullString()}", true, ConsoleColors.Green);

                // Check the imports
                var compilation = finalNode as CompilationUnitSyntax;
                if (compilation?.Usings.Any(u => u.Name.ToString() == "KS.Misc.Text") == false)
                {
                    var name = SyntaxFactory.QualifiedName(
                        SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName("KS"), SyntaxFactory.IdentifierName("Misc")),
                        SyntaxFactory.IdentifierName("Text"));
                    var directive = SyntaxFactory.UsingDirective(name).NormalizeWhitespace();
                    TextWriterColor.Write("Additionally, the suggested fix will add the following using statement:", true, ConsoleColors.Yellow);
                    TextWriterColor.Write($"  + {directive.ToFullString()}", true, ConsoleColors.Green);
                }
            }
        }
    }
}
