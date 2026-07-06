//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Base.Kernel.Debugging.Testing.Facades.FacadeData;
using Threadify.Manager;
using Nitrocid.Base.Languages;
using System.Threading;

namespace Nitrocid.Base.Kernel.Debugging.Testing.Facades
{
    internal class ThreadInstanceChildTest : TestFacade
    {
        public override string TestName => LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_KERNELTHREADCHILDTEST_DESC");
        public override TestSection TestSection => TestSection.Kernel;
        public override void Run()
        {
            ThreadInstance thread = new("Test thread", true, ThreadInstanceTestData.WriteHello);
            thread.AddChild("Test child thread", true, ThreadInstanceTestData.WriteHello);
            thread.AddChild("Test child thread #2", true, ThreadInstanceTestData.WriteHello);
            thread.AddChild("Test child thread #3", true, ThreadInstanceTestData.WriteHello);
            thread.Start();
            Thread.Sleep(3000);
            thread.Stop();
        }
    }
}
