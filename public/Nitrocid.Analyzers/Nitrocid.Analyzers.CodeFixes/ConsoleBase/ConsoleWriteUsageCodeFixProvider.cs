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
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nitrocid.Analyzers.Resources;

namespace Nitrocid.Analyzers.ConsoleBase
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConsoleWriteUsageCodeFixProvider)), Shared]
    public class ConsoleWriteUsageCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(ConsoleWriteUsageAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() =>
            WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root?.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<MemberAccessExpressionSyntax>().First();
            if (declaration is null)
                return;

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.ConsoleWriteUsageCodeFixTitle,
                    createChangedSolution: c => UseTextToolsFormatStringAsync(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.ConsoleWriteUsageCodeFixTitle)),
                diagnostic);
        }

        private async Task<Solution> UseTextToolsFormatStringAsync(Document document, MemberAccessExpressionSyntax typeDecl, CancellationToken cancellationToken)
        {
            if (typeDecl.Expression is IdentifierNameSyntax identifier)
            {
                // Build the replacement syntax
                var classSyntax = SyntaxFactory.IdentifierName("TextWriterColor");
                var methodSyntax = SyntaxFactory.IdentifierName("Write");
                var parentSyntax = (InvocationExpressionSyntax?)typeDecl.Parent;
                if (parentSyntax is null)
                    return document.Project.Solution;
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

                // Check the imports
                if (finalNode is not CompilationUnitSyntax compilation)
                    return document.Project.Solution;
                if (compilation.Usings.Any(u => u.Name?.ToString() == "Terminaux.Writer.ConsoleWriters") == false)
                {
                    var name = SyntaxFactory.QualifiedName(
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.IdentifierName("Terminaux"),
                            SyntaxFactory.IdentifierName("Writer")),
                        SyntaxFactory.IdentifierName("ConsoleWriters"));
                    compilation = compilation
                        .AddUsings(SyntaxFactory.UsingDirective(name));
                }

                var finalDoc = document.WithSyntaxRoot(compilation);
                return finalDoc.Project.Solution;
            }
            return document.Project.Solution;
        }
    }
}
