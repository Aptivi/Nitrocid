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

using Microsoft.CodeAnalysis.Testing;
using NuGet.Frameworks;
using System;
using System.IO;

namespace Nitrocid.Analyzers.Test.Verifiers
{
    internal class References
    {
        private static readonly Lazy<ReferenceAssemblies> _lazyNetRef =
             new(() =>
             {
                 if (!NuGetFramework.Parse("net10.0").IsPackageBased)
                     throw new NotSupportedException("Upgrade NuGet to use .NET 10.0.");

                 // Return a new instance of the .NET 10.0 reference assemblies
                 return new ReferenceAssemblies(
                     "net10.0",
                     new PackageIdentity(
                         "Microsoft.NETCore.App.Ref",
                         "10.0.0"),
                     Path.Combine("ref", "net10.0"));
             });

        public static ReferenceAssemblies NetRef => _lazyNetRef.Value;
    }
}
