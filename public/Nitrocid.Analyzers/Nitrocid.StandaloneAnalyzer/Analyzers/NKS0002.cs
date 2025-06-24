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
    internal class NKS0002 : IAnalyzer
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
                        // Before making the diagnostic, we need to first check for any existing methods from the ConsoleWrapper class.
                        // For contributors: Update the below array when new functions get added to ConsoleWrapper or changed.
                        string[] wrapperFunctions =
                        [
                            nameof(Console.Out),
                            nameof(Console.CursorLeft),
                            nameof(Console.CursorTop),
                            nameof(Console.WindowWidth),
                            nameof(Console.WindowHeight),
                            nameof(Console.WindowTop),
                            nameof(Console.BufferWidth),
                            nameof(Console.BufferHeight),
                            nameof(Console.ForegroundColor),
                            nameof(Console.BackgroundColor),
                            nameof(Console.CursorVisible),
                            nameof(Console.OutputEncoding),
                            nameof(Console.InputEncoding),
                            nameof(Console.KeyAvailable),
                            nameof(Console.Clear),
                            nameof(Console.SetCursorPosition),
                            nameof(Console.ResetColor),
                            nameof(Console.OpenStandardInput),
                            nameof(Console.OpenStandardOutput),
                            nameof(Console.OpenStandardError),
                            nameof(Console.SetOut),
                            nameof(Console.Beep),
                            nameof(Console.ReadKey),
                            nameof(Console.Write),
                            nameof(Console.WriteLine),
                        ];

                        // Let's see if the caller tries to access one of the above Console functions and/or properties.
                        var name = (IdentifierNameSyntax)exp.Name;
                        var idName = name.Identifier.Text;
                        if (wrapperFunctions.Contains(idName))
                        {
                            AnalyzerTools.PrintFromLocation(location, document, GetType(), "Caller uses Console instead of ConsoleWrapper");
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
                if (exp.Expression is IdentifierNameSyntax identifier)
                {
                    // Get the method
                    var idName = ((IdentifierNameSyntax)exp.Name).Identifier.Text;
                    if (identifier.Identifier.Text != nameof(Console))
                        continue;

                    // We need to have a syntax that calls ConsoleWrapper.idName
                    var classSyntax = SyntaxFactory.IdentifierName("ConsoleWrapper");
                    var methodSyntax = SyntaxFactory.IdentifierName(idName);
                    var resultSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, classSyntax, methodSyntax);
                    var replacedSyntax = resultSyntax
                        .WithLeadingTrivia(resultSyntax.GetLeadingTrivia())
                        .WithTrailingTrivia(resultSyntax.GetTrailingTrivia());

                    // Actually replace
                    var node = await document.GetSyntaxRootAsync(cancellationToken);
                    var finalNode = node?.ReplaceNode(exp, replacedSyntax);
                    TextWriterColor.WriteColor("Here's what the replacement would look like (with no Roslyn trivia):", true, ConsoleColors.Yellow);
                    TextWriterColor.WriteColor($"  - {exp}", true, ConsoleColors.Red);
                    TextWriterColor.WriteColor($"  + {replacedSyntax.ToFullString()}", true, ConsoleColors.Green);

                    // Check the imports
                    var compilation = finalNode as CompilationUnitSyntax;
                    if (compilation?.Usings.Any(u => u.Name?.ToString() == $"{AnalysisTools.rootNameSpace}.ConsoleBase") == false)
                    {
                        var name = SyntaxFactory.QualifiedName(
                            SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName(AnalysisTools.firstRootNameSpace), SyntaxFactory.IdentifierName("Base")),
                            SyntaxFactory.IdentifierName("ConsoleBase"));
                        var directive = SyntaxFactory.UsingDirective(name).NormalizeWhitespace();
                        TextWriterColor.WriteColor("Additionally, the suggested fix will add the following using statement:", true, ConsoleColors.Yellow);
                        TextWriterColor.WriteColor($"  + {directive.ToFullString()}", true, ConsoleColors.Green);
                    }
                }
            }
        }
    }
}
