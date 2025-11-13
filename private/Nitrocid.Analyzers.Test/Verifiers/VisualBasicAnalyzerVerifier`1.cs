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
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.VisualBasic.Testing;
using Nitrocid.Analyzers.Test.Verifiers;
using System.Threading;
using System.Threading.Tasks;

namespace Nitrocid.Analyzers.Test
{
    public static partial class VisualBasicAnalyzerVerifier<TAnalyzer>
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.Diagnostic()"/>
        public static DiagnosticResult Diagnostic()
            => VisualBasicAnalyzerVerifier<TAnalyzer, DefaultVerifier>.Diagnostic();

        /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.Diagnostic(string)"/>
        public static DiagnosticResult Diagnostic(string diagnosticId)
            => VisualBasicAnalyzerVerifier<TAnalyzer, DefaultVerifier>.Diagnostic(diagnosticId);

        /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.Diagnostic(DiagnosticDescriptor)"/>
        public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
            => VisualBasicAnalyzerVerifier<TAnalyzer, DefaultVerifier>.Diagnostic(descriptor);

        /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.VerifyAnalyzerAsync(string, DiagnosticResult[])"/>
        public static async Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
        {
            var test = new Test
            {
                TestCode = source,
            };

            test.ExpectedDiagnostics.AddRange(expected);
            test.ReferenceAssemblies = References.NetRef;
            test.TestState.AdditionalReferences.Add(MetadataReference.CreateFromFile("../../../public/Nitrocid/KSBuild/net10.0/Nitrocid.dll"));
            await test.RunAsync(CancellationToken.None);
        }
    }
}
