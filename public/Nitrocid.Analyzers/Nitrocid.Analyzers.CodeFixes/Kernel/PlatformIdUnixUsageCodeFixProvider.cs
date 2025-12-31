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

namespace Nitrocid.Analyzers.Kernel
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PlatformIdUnixUsageCodeFixProvider)), Shared]
    public class PlatformIdUnixUsageCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(PlatformIdUnixUsageAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() =>
            WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root?.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<BinaryExpressionSyntax>().First();
            if (declaration is null)
                return;

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.PlatformIdUnixUsageCodeFixTitle,
                    createChangedSolution: c => UseTextToolsFormatStringAsync(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.PlatformIdUnixUsageCodeFixTitle)),
                diagnostic);
        }

        private async Task<Solution> UseTextToolsFormatStringAsync(Document document, BinaryExpressionSyntax typeDecl, CancellationToken cancellationToken)
        {
            // We need to have a syntax that calls KernelPlatform.IsOnUnix
            var classSyntax = SyntaxFactory.IdentifierName("KernelPlatform");
            var methodSyntax = SyntaxFactory.IdentifierName("IsOnUnix");
            var maeSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, classSyntax, methodSyntax);
            var valuesSyntax = SyntaxFactory.ArgumentList().AddArguments();
            var resultSyntax = SyntaxFactory.InvocationExpression(maeSyntax, valuesSyntax);
            var replacedSyntax = resultSyntax
                .WithLeadingTrivia(resultSyntax.GetLeadingTrivia())
                .WithTrailingTrivia(resultSyntax.GetTrailingTrivia());

            // Actually replace
            var node = await document.GetSyntaxRootAsync(cancellationToken);
            var finalNode = node?.ReplaceNode(typeDecl, replacedSyntax);

            // Check the imports
            if (finalNode is not CompilationUnitSyntax compilation)
                    return document.Project.Solution;
            if (compilation.Usings.Any(u => u.Name?.ToString() == $"{AnalysisTools.rootNameSpace}.Kernel") == false)
            {
                var name = SyntaxFactory.QualifiedName(
                    SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName(AnalysisTools.firstRootNameSpace), SyntaxFactory.IdentifierName("Base")),
                    SyntaxFactory.IdentifierName("Kernel"));
                compilation = compilation
                    .AddUsings(SyntaxFactory.UsingDirective(name));
            }

            var finalDoc = document.WithSyntaxRoot(compilation);
            return finalDoc.Project.Solution;
        }
    }
}
