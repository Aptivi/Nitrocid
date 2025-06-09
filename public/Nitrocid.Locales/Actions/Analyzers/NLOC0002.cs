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
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Locales.Actions.Analyzers
{
    internal class NLOC0002 : IAnalyzer
    {
        public bool Analyze(Document document, out string[] unlocalized, bool reportMode = false)
        {
            unlocalized = [];
            var tree = document.GetSyntaxTreeAsync().Result;
            if (tree is null)
                return false;
            var syntaxNodeNodes = tree.GetRoot().DescendantNodesAndSelf().OfType<SyntaxNode>().ToList();
            bool found = false;
            List<string> unlocalizedStrings = [];
            HashSet<string> source =
                reportMode ? [.. Reporter.localizationList] : Checker.localizationList;

            foreach (var syntaxNode in syntaxNodeNodes)
            {
                // Check for argument
                if (syntaxNode is not CompilationUnitSyntax exp)
                    continue;
                var triviaList = exp.DescendantTrivia();
                var multiLineComments = triviaList.Where((trivia) => trivia.IsKind(SyntaxKind.MultiLineCommentTrivia));
                foreach (var multiLineComment in multiLineComments)
                {
                    string comment = multiLineComment.ToString();
                    if (comment == "/* Localizable */")
                    {
                        // We found a localizable string, but we need to find the string itself, so get all the possible
                        // tokens.
                        var node = exp.FindNode(multiLineComment.Span);
                        var tokens = node.DescendantTokens()
                            .Where(token => token.GetAllTrivia()
                                .Where((trivia) => trivia.IsKind(SyntaxKind.MultiLineCommentTrivia) && trivia.ToString() == "/* Localizable */").Any());

                        // Now, enumerate them to find the string
                        foreach (var token in tokens)
                        {
                            void Process(LiteralExpressionSyntax literalText)
                            {
                                // Process it.
                                var location = literalText.GetLocation();
                                string text = literalText.ToString();
                                text = text.Substring(1, text.Length - 2).Replace("\\\"", "\"");
                                if (!string.IsNullOrWhiteSpace(text) && !source.Contains(text))
                                {
                                    if (!reportMode)
                                        AnalyzerTools.PrintFromLocation(location, document, GetType(), $"Unlocalized string found: {text}");
                                    found = true;
                                    unlocalizedStrings.Add(text);
                                }
                            }

                            // Try to get a child
                            int start = token.FullSpan.End;
                            var parent = token.Parent;
                            if (parent is null)
                                continue;
                            if (parent is LiteralExpressionSyntax literalParent)
                            {
                                Process(literalParent);
                                continue;
                            }
                            if (parent is NameColonSyntax)
                                parent = parent.Parent;
                            if (parent is null)
                                continue;
                            var child = (SyntaxNode?)parent.ChildThatContainsPosition(start);
                            if (child is null)
                                continue;

                            // Now, check to see if it's a literal string
                            if (child is LiteralExpressionSyntax literalText)
                                Process(literalText);
                            else if (child is ArgumentSyntax argument && argument.Expression is LiteralExpressionSyntax literalArgText)
                                Process(literalArgText);
                        }
                    }
                }
            }
            unlocalized = [.. unlocalizedStrings.Distinct()];
            return found;
        }

        public bool ReverseAnalyze(Document document, out string[] localized, bool reportMode = false)
        {
            localized = [];
            var tree = document.GetSyntaxTreeAsync().Result;
            if (tree is null)
                return false;
            var syntaxNodeNodes = tree.GetRoot().DescendantNodesAndSelf().OfType<SyntaxNode>().ToList();
            bool found = false;
            List<string> localizedStrings = [];
            HashSet<string> source =
                reportMode ? [.. Reporter.localizationList] : [.. Cleaner.localizationList];

            foreach (var syntaxNode in syntaxNodeNodes)
            {
                // Check for argument
                if (syntaxNode is not CompilationUnitSyntax exp)
                    continue;
                var triviaList = exp.DescendantTrivia();
                var multiLineComments = triviaList.Where((trivia) => trivia.IsKind(SyntaxKind.MultiLineCommentTrivia));
                foreach (var multiLineComment in multiLineComments)
                {
                    string comment = multiLineComment.ToString();
                    if (comment == "/* Localizable */")
                    {
                        // We found a localizable string, but we need to find the string itself, so get all the possible
                        // tokens.
                        var node = exp.FindNode(multiLineComment.Span);
                        var tokens = node.DescendantTokens()
                            .Where(token => token.GetAllTrivia()
                                .Where((trivia) => trivia.IsKind(SyntaxKind.MultiLineCommentTrivia) && trivia.ToString() == "/* Localizable */").Any());

                        // Now, enumerate them to find the string
                        foreach (var token in tokens)
                        {
                            void Process(LiteralExpressionSyntax literalText)
                            {
                                // Process it.
                                var location = literalText.GetLocation();
                                string text = literalText.ToString();
                                text = text.Substring(1, text.Length - 2).Replace("\\\"", "\"");
                                if (source.Contains(text))
                                {
                                    found = true;
                                    localizedStrings.Add(text);
                                }
                            }

                            // Try to get a child
                            int start = token.FullSpan.End;
                            var parent = token.Parent;
                            if (parent is null)
                                continue;
                            if (parent is LiteralExpressionSyntax literalParent)
                            {
                                Process(literalParent);
                                continue;
                            }
                            if (parent is NameColonSyntax)
                                parent = parent.Parent;
                            if (parent is null)
                                continue;
                            var child = (SyntaxNode?)parent.ChildThatContainsPosition(start);
                            if (child is null)
                                continue;

                            // Now, check to see if it's a literal string
                            if (child is LiteralExpressionSyntax literalText)
                                Process(literalText);
                            else if (child is ArgumentSyntax argument && argument.Expression is LiteralExpressionSyntax literalArgText)
                                Process(literalArgText);
                        }
                    }
                }
            }
            localized = [.. localizedStrings.Distinct()];
            return found;
        }
    }
}
