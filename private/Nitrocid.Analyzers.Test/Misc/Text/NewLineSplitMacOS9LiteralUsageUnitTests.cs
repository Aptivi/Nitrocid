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
    Nitrocid.Analyzers.Misc.Text.NewLineSplitMacOS9LiteralUsageAnalyzer,
    Nitrocid.Analyzers.Misc.Text.NewLineSplitMacOS9LiteralUsageCodeFixProvider>;
using VerifyCSAlt = Nitrocid.Analyzers.Test.CSharpCodeFixVerifier<
    Nitrocid.Analyzers.Misc.Text.NewLineSplitMacOS9LiteralUsageAnalyzer,
    Nitrocid.Analyzers.Misc.Text.NewLineSplitMacOS9LiteralUsageAltCodeFixProvider>;

namespace Nitrocid.Analyzers.Test.Misc.Text
{
    [TestClass]
    public class NewLineSplitMacOS9LiteralUnitTests
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
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;
                using System.Diagnostics;

                namespace ConsoleApplication1
                {
                    class MyMod
                    {   
                        public static void Main()
                        {
                            string var = "Hello\rWorld!";
                            var split = [|var.Split("\r")|];
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
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;
                using System.Diagnostics;

                namespace ConsoleApplication1
                {
                    class MyMod
                    {   
                        public static void Main()
                        {
                            string var = "Hello\rWorld!";
                            var split = [|var.Split("\r")|];
                        }
                    }
                }
                """;

            var fixtest = """
                using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;
                using System.Diagnostics;
                using Textify.General;

                namespace ConsoleApplication1
                {
                    class MyMod
                    {   
                        public static void Main()
                        {
                            string var = "Hello\rWorld!";
                            var split = var.SplitNewLines();
                        }
                    }
                }
                """;

            await VerifyCS.VerifyCodeFixAsync(test, fixtest);
        }

        [TestMethod]
        public async Task TestFixThisDiagnosticAlt()
        {
            var test = """
                using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;
                using System.Diagnostics;

                namespace ConsoleApplication1
                {
                    class MyMod
                    {   
                        public static void Main()
                        {
                            string var = "Hello\rWorld!";
                            var split = [|var.Split("\r")|];
                        }
                    }
                }
                """;

            var fixtest = """
                using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;
                using System.Diagnostics;
                using Textify.General;

                namespace ConsoleApplication1
                {
                    class MyMod
                    {   
                        public static void Main()
                        {
                            string var = "Hello\rWorld!";
                            var split = var.SplitNewLinesOld();
                        }
                    }
                }
                """;

            await VerifyCSAlt.VerifyCodeFixAsync(test, fixtest);
        }
    }
}
