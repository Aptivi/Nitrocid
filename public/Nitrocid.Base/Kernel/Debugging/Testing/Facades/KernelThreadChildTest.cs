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

using Nitrocid.Base.Kernel.Debugging.Testing.Facades.FacadeData;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Languages;
using System.Threading;

namespace Nitrocid.Base.Kernel.Debugging.Testing.Facades
{
    internal class KernelThreadChildTest : TestFacade
    {
        public override string TestName => LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_KERNELTHREADCHILDTEST_DESC");
        public override TestSection TestSection => TestSection.Kernel;
        public override void Run()
        {
            KernelThread thread = new("Test thread", true, KernelThreadTestData.WriteHello);
            thread.AddChild("Test child thread", true, KernelThreadTestData.WriteHello);
            thread.AddChild("Test child thread #2", true, KernelThreadTestData.WriteHello);
            thread.AddChild("Test child thread #3", true, KernelThreadTestData.WriteHello);
            thread.Start();
            Thread.Sleep(3000);
            thread.Stop();
        }
    }
}
