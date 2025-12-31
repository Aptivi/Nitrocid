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

using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Nitrocid.Analyzers.Test.Verifiers;
using System;

namespace Nitrocid.Analyzers.Test
{
    public static partial class CSharpAnalyzerVerifier<TAnalyzer>
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        public class Test : CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
        {
            public Test()
            {
                SolutionTransforms.Add((solution, projectId) =>
                {
                    var compilationOptions = solution.GetProject(projectId)?.CompilationOptions ??
                        throw new Exception("Can't get compilation options.");
                    compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(
                        compilationOptions.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.NullableWarnings));
                    solution = solution.WithProjectCompilationOptions(projectId, compilationOptions);

                    return solution;
                });
            }
        }
    }
}
