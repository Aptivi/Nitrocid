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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Nitrocid.Analyzers.Test.CSharpCodeFixVerifier<
    Nitrocid.Analyzers.Kernel.ScreenUsageAnalyzer,
    Nitrocid.Analyzers.Kernel.ScreenUsageCodeFixProvider>;

namespace Nitrocid.Analyzers.Test.Kernel
{
    [TestClass]
    public class ScreenUsageAnalyzerUnitTests
    {
        [TestMethod]
        public async Task TestEnsureNoAnalyzer()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task TestAnalyzeThisDiagnostic()
        {
            var test = """
                using System;
                using System.Collections.Generic;
                using System.IO;
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;
                using System.Diagnostics;
                using System.Runtime.InteropServices;

                namespace ConsoleApplication1
                {
                    class MyMod
                    {   
                        public static void Main()
                        {
                            var value = [|Environment.GetEnvironmentVariable("STY")|];
                        }
                    }
                }
                """;

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task TestFixThisDiagnostic()
        {
            var test = """
                using System;
                using System.Collections.Generic;
                using System.IO;
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;
                using System.Diagnostics;
                using System.Runtime.InteropServices;

                namespace ConsoleApplication1
                {
                    class MyMod
                    {   
                        public static void Main()
                        {
                            var value = [|Environment.GetEnvironmentVariable("STY")|];
                        }
                    }
                }
                """;

            var fixtest = """
                using System;
                using System.Collections.Generic;
                using System.IO;
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;
                using System.Diagnostics;
                using System.Runtime.InteropServices;
                using Nitrocid.Base.Kernel;

                namespace ConsoleApplication1
                {
                    class MyMod
                    {   
                        public static void Main()
                        {
                            var value = KernelPlatform.IsRunningFromScreen();
                        }
                    }
                }
                """;

            await VerifyCS.VerifyCodeFixAsync(test, fixtest);
        }
    }
}
