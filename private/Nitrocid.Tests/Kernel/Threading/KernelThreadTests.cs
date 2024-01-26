//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.Kernel.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Threading;

namespace Nitrocid.Tests.Kernel.Threading
{

    [TestClass]
    public class KernelThreadTests
    {

        internal static KernelThread TargetThread;
        internal static KernelThread TargetParameterizedThread;
        internal static KernelThread TargetThreadWithChild;
        internal static KernelThread TargetParameterizedThreadWithChild;
        internal static KernelThread TargetThreadWithAppendingChild;
        internal static KernelThread TargetParameterizedThreadWithAppendingChild;

        /// <summary>
        /// Tests initializing kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeKernelThread() =>
            TargetThread = new KernelThread("Unit test thread #1", true, KernelThreadTestHelper.WriteHello);

        /// <summary>
        /// Tests initializing kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeKernelParameterizedThread() =>
            TargetParameterizedThread = new KernelThread("Unit test thread #2", true, (_) => KernelThreadTestHelper.WriteHelloWithArgument("Hello"));

        /// <summary>
        /// Tests initializing kernel thread
        /// </summary>
        [TestMethod]
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
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeKernelParameterizedThreadWithChildThread()
        {
            TargetParameterizedThreadWithChild = new KernelThread("Unit test thread #4", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentWithChild("Hello"));
            TargetParameterizedThreadWithChild.AddChild("Unit test child thread #1 for parent thread #4", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChild("Hello"));
            TargetParameterizedThreadWithChild.AddChild("Unit test child thread #2 for parent thread #4", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChild("Hello"));
            TargetParameterizedThreadWithChild.AddChild("Unit test child thread #3 for parent thread #4", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChild("Hello"));
        }

        /// <summary>
        /// Tests initializing kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeKernelThreadWithAppendingChildThread()
        {
            TargetThreadWithAppendingChild = new KernelThread("Unit test thread #5", true, KernelThreadTestHelper.WriteHelloWithAppendingChild);
            TargetThreadWithAppendingChild.AddChild("Unit test child thread #1 for parent thread #5", true, KernelThreadTestHelper.WriteHelloFromAppendingChild);
            TargetThreadWithAppendingChild.AddChild("Unit test child thread #2 for parent thread #5", true, KernelThreadTestHelper.WriteHelloFromAppendingChild);
            TargetThreadWithAppendingChild.AddChild("Unit test child thread #3 for parent thread #5", true, KernelThreadTestHelper.WriteHelloFromAppendingChild);
        }

        /// <summary>
        /// Tests initializing kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeKernelParameterizedThreadWithAppendingChildThread()
        {
            TargetParameterizedThreadWithAppendingChild = new KernelThread("Unit test thread #6", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentWithAppendingChild("Hello"));
            TargetParameterizedThreadWithAppendingChild.AddChild("Unit test child thread #1 for parent thread #6", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChild("Hello"));
            TargetParameterizedThreadWithAppendingChild.AddChild("Unit test child thread #2 for parent thread #6", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChild("Hello"));
            TargetParameterizedThreadWithAppendingChild.AddChild("Unit test child thread #3 for parent thread #6", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChild("Hello"));
        }

        /// <summary>
        /// Tests starting kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelThread() =>
            TargetThread.Start();

        /// <summary>
        /// Tests starting kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelParameterizedThread() =>
            TargetParameterizedThread.Start("Agustin");

        /// <summary>
        /// Tests starting kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelThreadWithChildThread() =>
            TargetThreadWithChild.Start();

        /// <summary>
        /// Tests starting kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelParameterizedThreadWithChildThread() =>
            TargetParameterizedThreadWithChild.Start("Agustin");

        /// <summary>
        /// Tests starting kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelThreadWithAppendingChildThread()
        {
            TargetThreadWithAppendingChild.Start();
            Thread.Sleep(1000);
            TargetThreadWithAppendingChild.AddChild("Unit test additional child thread #4 for parent thread #5", true, KernelThreadTestHelper.WriteHelloFromAppendingChild);
            TargetThreadWithAppendingChild.AddChild("Unit test additional child thread #5 for parent thread #5", true, KernelThreadTestHelper.WriteHelloFromAppendingChild);
            TargetThreadWithAppendingChild.AddChild("Unit test additional child thread #6 for parent thread #5", true, KernelThreadTestHelper.WriteHelloFromAppendingChild);
        }

        /// <summary>
        /// Tests starting kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelParameterizedThreadWithAppendingChildThread()
        {
            TargetParameterizedThreadWithAppendingChild.Start("Agustin");
            Thread.Sleep(1000);
            TargetParameterizedThreadWithAppendingChild.AddChild("Unit test child thread #4 for parent thread #6", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChild("Hello"));
            TargetParameterizedThreadWithAppendingChild.AddChild("Unit test child thread #5 for parent thread #6", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChild("Hello"));
            TargetParameterizedThreadWithAppendingChild.AddChild("Unit test child thread #6 for parent thread #6", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChild("Hello"));
        }

        /// <summary>
        /// Tests stopping kernel thread
        /// </summary>
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
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
        /// Tests stopping kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStopKernelThreadWithAppendingChildThread()
        {
            Thread.Sleep(500);
            Should.CompleteIn(TargetThreadWithAppendingChild.Stop, TimeSpan.FromSeconds(5));
            TargetThreadWithAppendingChild.IsStopping.ShouldBeFalse();
            TargetThreadWithAppendingChild.ShouldNotBeNull();
            foreach (var childThread in TargetThreadWithAppendingChild.ChildThreads)
            {
                childThread.IsStopping.ShouldBeFalse();
                childThread.ShouldNotBeNull();
            }
        }

        /// <summary>
        /// Tests stopping kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStopKernelParameterizedThreadWithAppendingChildThread()
        {
            Thread.Sleep(500);
            Should.CompleteIn(TargetParameterizedThreadWithAppendingChild.Stop, TimeSpan.FromSeconds(5));
            TargetParameterizedThreadWithAppendingChild.IsStopping.ShouldBeFalse();
            TargetParameterizedThreadWithAppendingChild.ShouldNotBeNull();
            foreach (var childThread in TargetParameterizedThreadWithAppendingChild.ChildThreads)
            {
                childThread.IsStopping.ShouldBeFalse();
                childThread.ShouldNotBeNull();
            }
        }

        /// <summary>
        /// Tests getting actual milliseconds
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetActualMilliseconds()
        {
            int actualMilliseconds = ThreadManager.GetActualMilliseconds(1);
            actualMilliseconds.ShouldBeGreaterThanOrEqualTo(1);
        }

        /// <summary>
        /// Tests getting actual milliseconds
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetActualMillisecondsRecursive()
        {
            for (int i = 1; i <= 20; i++)
            {
                int actualMilliseconds = ThreadManager.GetActualMilliseconds(i);
                actualMilliseconds.ShouldBeGreaterThanOrEqualTo(i);
            }
        }

        /// <summary>
        /// Tests getting actual ticks
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetActualTicks()
        {
            long actualTicks = ThreadManager.GetActualTicks(1);
            actualTicks.ShouldBeGreaterThanOrEqualTo(1000);
        }

        /// <summary>
        /// Tests getting actual ticks
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetActualTicksRecursive()
        {
            for (int i = 1; i <= 20; i++)
            {
                long actualTicks = ThreadManager.GetActualTicks(i);
                actualTicks.ShouldBeGreaterThanOrEqualTo(i * 1000);
            }
        }

        /// <summary>
        /// Tests getting actual milliseconds
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetActualTimeSpan()
        {
            var actualSpan = ThreadManager.GetActualTimeSpan(1);
            actualSpan.Milliseconds.ShouldBeGreaterThanOrEqualTo(1);
            actualSpan.TotalMilliseconds.ShouldBeGreaterThanOrEqualTo(1);
            actualSpan.Ticks.ShouldBeGreaterThanOrEqualTo(1000);
        }

        /// <summary>
        /// Tests getting actual milliseconds
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetActualTimeSpanRecursive()
        {
            for (int i = 1; i <= 20; i++)
            {
                var actualSpan = ThreadManager.GetActualTimeSpan(i);
                actualSpan.Milliseconds.ShouldBeGreaterThanOrEqualTo(i);
                actualSpan.TotalMilliseconds.ShouldBeGreaterThanOrEqualTo(i);
                actualSpan.Ticks.ShouldBeGreaterThanOrEqualTo(i * 1000);
            }
        }

    }
}
