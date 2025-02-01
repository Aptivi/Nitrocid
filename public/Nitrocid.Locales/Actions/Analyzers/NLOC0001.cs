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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nitrocid.Analyzers.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Locales.Actions.Analyzers
{
    internal class NLOC0001 : IAnalyzer
    {
        public bool Analyze(Document document, out string[] unlocalized)
        {
            unlocalized = [];
            var tree = document.GetSyntaxTreeAsync().Result;
            if (tree is null)
                return false;
            var syntaxNodeNodes = tree.GetRoot().DescendantNodesAndSelf().OfType<SyntaxNode>().ToList();
            bool found = false;
            List<string> unlocalizedStrings = [];
            foreach (var syntaxNode in syntaxNodeNodes)
            {
                // Check for argument
                if (syntaxNode is not InvocationExpressionSyntax exp)
                    continue;
                var args = exp.ArgumentList.Arguments;
                if (args.Count < 1)
                    continue;
                var localizableStringArgument = args[0] ??
                    throw new Exception("Can't get localizable string");

                // Now, check for the Translate.DoTranslation() call
                if (exp.Expression is not MemberAccessExpressionSyntax expMaes)
                    continue;
                if (expMaes.Expression is IdentifierNameSyntax expIdentifier && expMaes.Name is IdentifierNameSyntax identifier)
                {
                    // Verify that we're dealing with Translate.DoTranslation()
                    var location = syntaxNode.GetLocation();
                    var idExpression = expIdentifier.Identifier.Text;
                    var idName = identifier.Identifier.Text;
                    if (idExpression == "Translate" && idName == "DoTranslation")
                    {
                        // Now, get the string representation from the argument count and compare it with the list of translations.
                        // You'll notice that we sometimes call Translate.DoTranslation() with a variable instead of a string, so
                        // check that first, because they're usually obtained from a string representation usually prefixed with
                        // either the /* Localizable */ comment or in individual kernel resources. However, the resources don't
                        // have a prefix, so the key names alone are enough.
                        if (localizableStringArgument.Expression is LiteralExpressionSyntax literalText)
                        {
                            string text = literalText.ToString();
                            text = text[1..^1].Replace("\\\"", "\"");
                            if (!string.IsNullOrWhiteSpace(text) && !Checker.localizationList.Contains(text))
                            {
                                AnalyzerTools.PrintFromLocation(location, document, GetType(), $"Unlocalized string found: {text}");
                                found = true;
                                unlocalizedStrings.Add(text);
                            }
                        }
                    }
                }
            }
            unlocalized = [.. unlocalizedStrings];
            return found;
        }

        public bool ReverseAnalyze(Document document, out string[] localized)
        {
            localized = [];
            var tree = document.GetSyntaxTreeAsync().Result;
            if (tree is null)
                return false;
            var syntaxNodeNodes = tree.GetRoot().DescendantNodesAndSelf().OfType<SyntaxNode>().ToList();
            bool found = false;
            List<string> localizedStrings = [];
            foreach (var syntaxNode in syntaxNodeNodes)
            {
                // Check for argument
                if (syntaxNode is not InvocationExpressionSyntax exp)
                    continue;
                var args = exp.ArgumentList.Arguments;
                if (args.Count < 1)
                    continue;
                var localizableStringArgument = args[0] ??
                    throw new Exception("Can't get localizable string");

                // Now, check for the Translate.DoTranslation() call
                if (exp.Expression is not MemberAccessExpressionSyntax expMaes)
                    continue;
                if (expMaes.Expression is IdentifierNameSyntax expIdentifier && expMaes.Name is IdentifierNameSyntax identifier)
                {
                    // Verify that we're dealing with Translate.DoTranslation()
                    var location = syntaxNode.GetLocation();
                    var idExpression = expIdentifier.Identifier.Text;
                    var idName = identifier.Identifier.Text;
                    if (idExpression == "Translate" && idName == "DoTranslation")
                    {
                        // Now, get the string representation from the argument count and compare it with the list of translations.
                        // You'll notice that we sometimes call Translate.DoTranslation() with a variable instead of a string, so
                        // check that first, because they're usually obtained from a string representation usually prefixed with
                        // either the /* Localizable */ comment or in individual kernel resources. However, the resources don't
                        // have a prefix, so the key names alone are enough.
                        if (localizableStringArgument.Expression is LiteralExpressionSyntax literalText)
                        {
                            string text = literalText.ToString();
                            text = text[1..^1].Replace("\\\"", "\"");
                            if (Cleaner.localizationList.Contains(text))
                            {
                                found = true;
                                localizedStrings.Add(text);
                            }
                        }
                    }
                }
            }
            localized = [.. localizedStrings];
            return found;
        }
    }
}
