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

using BenchmarkDotNet.Running;
using Nitrocid.Base.Kernel;

namespace Nitrocid.Benchmarks
{
    internal static class FixtureManager
    {
        internal static void RunBenchmark(string fixture)
        {
            // Get the benchmark fixture and run it
            var fixtureType = Type.GetType($"{KernelReleaseInfo.rootNameSpace}.Benchmarks.Fixtures.{fixture}") ??
                throw new Exception($"No fixture type called {fixture}.");
            BenchmarkRunner.Run(fixtureType);
        }
    }
}
