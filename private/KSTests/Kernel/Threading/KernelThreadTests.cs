
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

using KS.Kernel.Threading;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;

namespace KSTests.Kernel.Threading
{

    [TestFixture]
    public class KernelThreadTests
    {

        internal static KernelThread TargetThread;
        internal static KernelThread TargetParameterizedThread;
        internal static KernelThread TargetThreadWithChild;
        internal static KernelThread TargetParameterizedThreadWithChild;

        /// <summary>
        /// Tests initializing kernel thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeKernelThread() =>
            TargetThread = new KernelThread("Unit test thread #1", true, KernelThreadTestHelper.WriteHello);

        /// <summary>
        /// Tests initializing kernel parameterized thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeKernelParameterizedThread() =>
            TargetParameterizedThread = new KernelThread("Unit test thread #2", true, (_) => KernelThreadTestHelper.WriteHelloWithArgument("Hello"));

        /// <summary>
        /// Tests initializing kernel thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeKernelThreadWithChildThread()
        {
            TargetThreadWithChild = new KernelThread("Unit test thread #3", true, KernelThreadTestHelper.WriteHelloWithChild);
            TargetThreadWithChild.AddChild("Unit test child thread #1 for parent thread #3", true, KernelThreadTestHelper.WriteHelloFromChild);
            TargetThreadWithChild.AddChild("Unit test child thread #2 for parent thread #3", true, KernelThreadTestHelper.WriteHelloFromChild);
            TargetThreadWithChild.AddChild("Unit test child thread #3 for parent thread #3", true, KernelThreadTestHelper.WriteHelloFromChild);
        }

        /// <summary>
        /// Tests initializing kernel parameterized thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeKernelParameterizedThreadWithChildThread()
        {
            TargetParameterizedThreadWithChild = new KernelThread("Unit test thread #4", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentWithChild("Hello"));
            TargetParameterizedThreadWithChild.AddChild("Unit test child thread #1 for parent thread #4", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChild("Hello"));
            TargetParameterizedThreadWithChild.AddChild("Unit test child thread #2 for parent thread #4", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChild("Hello"));
            TargetParameterizedThreadWithChild.AddChild("Unit test child thread #3 for parent thread #4", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChild("Hello"));
        }

        /// <summary>
        /// Tests starting kernel thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestStartKernelThread() =>
            TargetThread.Start();

        /// <summary>
        /// Tests starting kernel parameterized thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestStartKernelParameterizedThread() =>
            TargetParameterizedThread.Start("Agustin");

        /// <summary>
        /// Tests starting kernel thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestStartKernelThreadWithChildThread() =>
            TargetThreadWithChild.Start();

        /// <summary>
        /// Tests starting kernel parameterized thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestStartKernelParameterizedThreadWithChildThread() =>
            TargetParameterizedThreadWithChild.Start("Agustin");

        /// <summary>
        /// Tests stopping kernel thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestStopKernelThread()
        {
            Thread.Sleep(300);
            TargetThread.Stop();
            TargetThread.IsStopping.ShouldBeFalse();
            TargetThread.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests stopping kernel parameterized thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestStopKernelParameterizedThread()
        {
            Thread.Sleep(300);
            TargetParameterizedThread.Stop();
            TargetParameterizedThread.IsStopping.ShouldBeFalse();
            TargetParameterizedThread.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests stopping kernel thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestStopKernelThreadWithChildThread()
        {
            Thread.Sleep(500);
            Should.CompleteIn(TargetThreadWithChild.Stop, TimeSpan.FromSeconds(5));
            TargetThreadWithChild.IsStopping.ShouldBeFalse();
            TargetThreadWithChild.ShouldNotBeNull();
            foreach (var childThread in TargetThreadWithChild.ChildThreads)
            {
                childThread.IsStopping.ShouldBeFalse();
                childThread.ShouldNotBeNull();
            }
        }

        /// <summary>
        /// Tests stopping kernel parameterized thread
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestStopKernelParameterizedThreadWithChildThread()
        {
            Thread.Sleep(500);
            Should.CompleteIn(TargetParameterizedThreadWithChild.Stop, TimeSpan.FromSeconds(5));
            TargetParameterizedThreadWithChild.IsStopping.ShouldBeFalse();
            TargetParameterizedThreadWithChild.ShouldNotBeNull();
            foreach (var childThread in TargetParameterizedThreadWithChild.ChildThreads)
            {
                childThread.IsStopping.ShouldBeFalse();
                childThread.ShouldNotBeNull();
            }
        }

        /// <summary>
        /// Tests getting actual milliseconds
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetActualMilliseconds()
        {
            int actualMilliseconds = ThreadManager.GetActualMilliseconds(1);
            actualMilliseconds.ShouldBeGreaterThanOrEqualTo(1);
        }

        /// <summary>
        /// Tests getting actual milliseconds
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetActualMillisecondsRecursive()
        {
            for (int i = 1; i <= 100; i++)
            {
                int actualMilliseconds = ThreadManager.GetActualMilliseconds(i);
                actualMilliseconds.ShouldBeGreaterThanOrEqualTo(i);
            }
        }

        /// <summary>
        /// Tests getting actual ticks
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetActualTicks()
        {
            long actualTicks = ThreadManager.GetActualTicks(1);
            actualTicks.ShouldBeGreaterThanOrEqualTo(1000);
        }

        /// <summary>
        /// Tests getting actual ticks
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetActualTicksRecursive()
        {
            for (int i = 1; i <= 100; i++)
            {
                long actualTicks = ThreadManager.GetActualTicks(i);
                actualTicks.ShouldBeGreaterThanOrEqualTo(i * 1000);
            }
        }

    }
}
