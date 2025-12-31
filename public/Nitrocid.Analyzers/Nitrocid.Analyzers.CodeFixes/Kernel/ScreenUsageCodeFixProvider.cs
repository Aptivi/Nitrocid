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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ScreenUsageCodeFixProvider)), Shared]
    public class ScreenUsageCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(ScreenUsageAnalyzer.DiagnosticId);

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
                    title: CodeFixResources.ScreenUsageCodeFixTitle,
                    createChangedSolution: c => UseTextToolsFormatStringAsync(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.ScreenUsageCodeFixTitle)),
                diagnostic);
        }

        private async Task<Solution> UseTextToolsFormatStringAsync(Document document, MemberAccessExpressionSyntax typeDecl, CancellationToken cancellationToken)
        {
            if (typeDecl.Expression is IdentifierNameSyntax identifier)
            {
                // Get the method
                var idName = ((IdentifierNameSyntax)typeDecl.Name).Identifier.Text;

                // We need to have a syntax that calls KernelPlatform.IsRunningFromScreen
                var classSyntax = SyntaxFactory.IdentifierName("KernelPlatform");
                var methodSyntax = SyntaxFactory.IdentifierName("IsRunningFromScreen");
                var maesSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, classSyntax, methodSyntax);
                var argsSyntax = SyntaxFactory.ArgumentList();
                var resultSyntax = SyntaxFactory.InvocationExpression(maesSyntax, argsSyntax);
                var replacedSyntax = resultSyntax
                    .WithLeadingTrivia(resultSyntax.GetLeadingTrivia())
                    .WithTrailingTrivia(resultSyntax.GetTrailingTrivia());

                // Actually replace
                if (typeDecl.Parent is null)
                    return document.Project.Solution;
                var node = await document.GetSyntaxRootAsync(cancellationToken);
                var finalNode = node?.ReplaceNode(typeDecl.Parent, replacedSyntax);

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
            return document.Project.Solution;
        }
    }
}
