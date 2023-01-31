
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Misc.Threading;
using NUnit.Framework;
using Shouldly;

namespace KSTests.MiscTests
{

    [TestFixture]
    public class KernelThreadTests
    {

        private static KernelThread TargetThread;
        private static KernelThread TargetParameterizedThread;

        /// <summary>
        /// Tests initializing kernel thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeKernelThread() => TargetThread = new KernelThread("Unit test thread #1", true, KernelThreadTestHelper.WriteHello);

        /// <summary>
        /// Tests initializing kernel parameterized thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeKernelParameterizedThread() => TargetParameterizedThread = new KernelThread("Unit test thread #2", true, (_) => KernelThreadTestHelper.WriteHelloWithArgument("Hello"));

        /// <summary>
        /// Tests starting kernel thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestStartKernelThread() => TargetThread.Start();

        /// <summary>
        /// Tests starting kernel parameterized thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestStartKernelParameterizedThread() => TargetParameterizedThread.Start("Agustin");

        /// <summary>
        /// Tests stopping kernel thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestStopKernelThread()
        {
            System.Threading.Thread.Sleep(300);
            TargetThread.Stop();
            TargetThread.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests stopping kernel parameterized thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestStopKernelParameterizedThread()
        {
            System.Threading.Thread.Sleep(300);
            TargetParameterizedThread.Stop();
            TargetParameterizedThread.ShouldNotBeNull();
        }

    }
}
