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

using Nitrocid.StandaloneAnalyzer.Analyzers;

namespace Nitrocid.StandaloneAnalyzer
{
    internal static class AnalyzersList
    {
        // For contributors: If you're going to add a new analyzer, you need to copy the implementation from Analyzers to here,
        // and make a dedicated diagnostic class for the standalone analyzer to recognize your new analyzer.
        internal static readonly IAnalyzer[] analyzers =
        [
            new NKS0001(),
            new NKS0002(),
            new NKS0003(),
            new NKS0004(),
            new NKS0005(),
            new NKS0006(),
            new NKS0007(),
            new NKS0008(),
            new NKS0009(),
            new NKS0010(),
            new NKS0011(),
            new NKS0012(),
            new NKS0013(),
            new NKS0014(),
            new NKS0015(),
            new NKS0016(),
            new NKS0017(),
            new NKS0018(),
            new NKS0019(),
            new NKS0020(),
            new NKS0021(),
            new NKS0022(),
            new NKS0023(),
            new NKS0024(),
            new NKS0025(),
            new NKS0026(),
            new NKS0027(),
            new NKS0031(),
            new NKS0032(),
            new NKS0033(),
            new NKS0037(),
            new NKS0038(),
            new NKS0039(),
            new NKS0040(),
            new NKS0041(),
            new NKS0042(),
            new NKS0043(),
            new NKS0052(),
            new NKS0053(),
            new NKS0044(),
            new NKS0045(),
            new NKS0046(),
            new NKS0051(),
            new NKS0047(),
            new NKS0048(),
            new NKS0049(),
            new NKS0050(),
            new NKS0054(),
            new NKS0055(),
        ];
    }
}
