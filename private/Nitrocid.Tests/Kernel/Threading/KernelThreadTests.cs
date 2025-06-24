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

using Nitrocid.Base.Kernel.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Threading;

namespace Nitrocid.Tests.Kernel.Threading
{

    [TestClass]
    public class KernelThreadTests
    {

        internal static KernelThread? TargetThread;
        internal static KernelThread? TargetParameterizedThread;
        internal static KernelThread? TargetThreadWithChild;
        internal static KernelThread? TargetParameterizedThreadWithChild;
        internal static KernelThread? TargetThreadWithAppendingChild;
        internal static KernelThread? TargetParameterizedThreadWithAppendingChild;
        internal static KernelThread? TargetThreadWithChildrenOfChildren;
        internal static KernelThread? TargetParameterizedThreadWithChildrenOfChildren;
        internal static KernelThread? TargetThreadWithAppendingChildrenOfChildren;
        internal static KernelThread? TargetParameterizedThreadWithAppendingChildrenOfChildren;

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
        /// Tests initializing kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeKernelThreadWithChildrenOfChildrenThread()
        {
            TargetThreadWithChildrenOfChildren = new KernelThread("Unit test thread #7", true, KernelThreadTestHelper.WriteHelloWithChildrenOfChildren);
            TargetThreadWithChildrenOfChildren.AddChild("Unit test child thread #1 for parent thread #7", true, KernelThreadTestHelper.WriteHelloFromChildrenOfChildren);
            TargetThreadWithChildrenOfChildren.AddChild("Unit test child thread #2 for parent thread #7", true, KernelThreadTestHelper.WriteHelloFromChildrenOfChildren);
            TargetThreadWithChildrenOfChildren.AddChild("Unit test child thread #3 for parent thread #7", true, KernelThreadTestHelper.WriteHelloFromChildrenOfChildren);
            var child1 = TargetThreadWithChildrenOfChildren.GetChild(0);
            var child2 = TargetThreadWithChildrenOfChildren.GetChild(1);
            var child3 = TargetThreadWithChildrenOfChildren.GetChild(2);
            child1.AddChild("Unit test child thread #1 of child thread #1 for parent thread #7", true, KernelThreadTestHelper.WriteHelloFromChildrenOfChildren);
            child1.AddChild("Unit test child thread #2 of child thread #1 for parent thread #7", true, KernelThreadTestHelper.WriteHelloFromChildrenOfChildren);
            child1.AddChild("Unit test child thread #3 of child thread #1 for parent thread #7", true, KernelThreadTestHelper.WriteHelloFromChildrenOfChildren);
            child2.AddChild("Unit test child thread #1 of child thread #2 for parent thread #7", true, KernelThreadTestHelper.WriteHelloFromChildrenOfChildren);
            child2.AddChild("Unit test child thread #2 of child thread #2 for parent thread #7", true, KernelThreadTestHelper.WriteHelloFromChildrenOfChildren);
            child2.AddChild("Unit test child thread #3 of child thread #2 for parent thread #7", true, KernelThreadTestHelper.WriteHelloFromChildrenOfChildren);
            child3.AddChild("Unit test child thread #1 of child thread #3 for parent thread #7", true, KernelThreadTestHelper.WriteHelloFromChildrenOfChildren);
            child3.AddChild("Unit test child thread #2 of child thread #3 for parent thread #7", true, KernelThreadTestHelper.WriteHelloFromChildrenOfChildren);
            child3.AddChild("Unit test child thread #3 of child thread #3 for parent thread #7", true, KernelThreadTestHelper.WriteHelloFromChildrenOfChildren);
        }

        /// <summary>
        /// Tests initializing kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeKernelParameterizedThreadWithChildrenOfChildrenThread()
        {
            TargetParameterizedThreadWithChildrenOfChildren = new KernelThread("Unit test thread #8", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentWithChildrenOfChildren("Hello"));
            TargetParameterizedThreadWithChildrenOfChildren.AddChild("Unit test child thread #1 for parent thread #8", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChildrenOfChildren("Hello"));
            TargetParameterizedThreadWithChildrenOfChildren.AddChild("Unit test child thread #2 for parent thread #8", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChildrenOfChildren("Hello"));
            TargetParameterizedThreadWithChildrenOfChildren.AddChild("Unit test child thread #3 for parent thread #8", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChildrenOfChildren("Hello"));
            var child1 = TargetParameterizedThreadWithChildrenOfChildren.GetChild(0);
            var child2 = TargetParameterizedThreadWithChildrenOfChildren.GetChild(1);
            var child3 = TargetParameterizedThreadWithChildrenOfChildren.GetChild(2);
            child1.AddChild("Unit test child thread #1 of child thread #1 for parent thread #8", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChildrenOfChildren("Hello"));
            child1.AddChild("Unit test child thread #2 of child thread #1 for parent thread #8", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChildrenOfChildren("Hello"));
            child1.AddChild("Unit test child thread #3 of child thread #1 for parent thread #8", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChildrenOfChildren("Hello"));
            child2.AddChild("Unit test child thread #1 of child thread #2 for parent thread #8", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChildrenOfChildren("Hello"));
            child2.AddChild("Unit test child thread #2 of child thread #2 for parent thread #8", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChildrenOfChildren("Hello"));
            child2.AddChild("Unit test child thread #3 of child thread #2 for parent thread #8", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChildrenOfChildren("Hello"));
            child3.AddChild("Unit test child thread #1 of child thread #3 for parent thread #8", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChildrenOfChildren("Hello"));
            child3.AddChild("Unit test child thread #2 of child thread #3 for parent thread #8", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChildrenOfChildren("Hello"));
            child3.AddChild("Unit test child thread #3 of child thread #3 for parent thread #8", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromChildrenOfChildren("Hello"));
        }

        /// <summary>
        /// Tests initializing kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeKernelThreadWithAppendingChildrenOfChildrenThread()
        {
            TargetThreadWithAppendingChildrenOfChildren = new KernelThread("Unit test thread #9", true, KernelThreadTestHelper.WriteHelloWithAppendingChildrenOfChildren);
            TargetThreadWithAppendingChildrenOfChildren.AddChild("Unit test child thread #1 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            TargetThreadWithAppendingChildrenOfChildren.AddChild("Unit test child thread #2 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            TargetThreadWithAppendingChildrenOfChildren.AddChild("Unit test child thread #3 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            var child1 = TargetThreadWithAppendingChildrenOfChildren.GetChild(0);
            var child2 = TargetThreadWithAppendingChildrenOfChildren.GetChild(1);
            var child3 = TargetThreadWithAppendingChildrenOfChildren.GetChild(2);
            child1.AddChild("Unit test child thread #1 of child thread #1 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child1.AddChild("Unit test child thread #2 of child thread #1 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child1.AddChild("Unit test child thread #3 of child thread #1 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child2.AddChild("Unit test child thread #1 of child thread #2 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child2.AddChild("Unit test child thread #2 of child thread #2 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child2.AddChild("Unit test child thread #3 of child thread #2 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child3.AddChild("Unit test child thread #1 of child thread #3 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child3.AddChild("Unit test child thread #2 of child thread #3 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child3.AddChild("Unit test child thread #3 of child thread #3 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
        }

        /// <summary>
        /// Tests initializing kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeKernelParameterizedThreadWithAppendingChildrenOfChildrenThread()
        {
            TargetParameterizedThreadWithAppendingChildrenOfChildren = new KernelThread("Unit test thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentWithAppendingChildrenOfChildren("Hello"));
            TargetParameterizedThreadWithAppendingChildrenOfChildren.AddChild("Unit test child thread #1 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            TargetParameterizedThreadWithAppendingChildrenOfChildren.AddChild("Unit test child thread #2 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            TargetParameterizedThreadWithAppendingChildrenOfChildren.AddChild("Unit test child thread #3 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            var child1 = TargetParameterizedThreadWithAppendingChildrenOfChildren.GetChild(0);
            var child2 = TargetParameterizedThreadWithAppendingChildrenOfChildren.GetChild(1);
            var child3 = TargetParameterizedThreadWithAppendingChildrenOfChildren.GetChild(2);
            child1.AddChild("Unit test child thread #1 of child thread #1 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child1.AddChild("Unit test child thread #2 of child thread #1 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child1.AddChild("Unit test child thread #3 of child thread #1 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child2.AddChild("Unit test child thread #1 of child thread #2 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child2.AddChild("Unit test child thread #2 of child thread #2 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child2.AddChild("Unit test child thread #3 of child thread #2 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child3.AddChild("Unit test child thread #1 of child thread #3 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child3.AddChild("Unit test child thread #2 of child thread #3 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child3.AddChild("Unit test child thread #3 of child thread #3 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
        }

        /// <summary>
        /// Tests starting kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelThread() =>
            TargetThread?.Start();

        /// <summary>
        /// Tests starting kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelParameterizedThread() =>
            TargetParameterizedThread?.Start("Agustin");

        /// <summary>
        /// Tests starting kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelThreadWithChildThread() =>
            TargetThreadWithChild?.Start();

        /// <summary>
        /// Tests starting kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelParameterizedThreadWithChildThread() =>
            TargetParameterizedThreadWithChild?.Start("Agustin");

        /// <summary>
        /// Tests starting kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelThreadWithAppendingChildThread()
        {
            TargetThreadWithAppendingChild?.Start();
            Thread.Sleep(1000);
            TargetThreadWithAppendingChild?.AddChild("Unit test additional child thread #4 for parent thread #5", true, KernelThreadTestHelper.WriteHelloFromAppendingChild);
            TargetThreadWithAppendingChild?.AddChild("Unit test additional child thread #5 for parent thread #5", true, KernelThreadTestHelper.WriteHelloFromAppendingChild);
            TargetThreadWithAppendingChild?.AddChild("Unit test additional child thread #6 for parent thread #5", true, KernelThreadTestHelper.WriteHelloFromAppendingChild);
        }

        /// <summary>
        /// Tests starting kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelParameterizedThreadWithAppendingChildThread()
        {
            TargetParameterizedThreadWithAppendingChild?.Start("Agustin");
            Thread.Sleep(1000);
            TargetParameterizedThreadWithAppendingChild?.AddChild("Unit test child thread #4 for parent thread #6", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChild("Hello"));
            TargetParameterizedThreadWithAppendingChild?.AddChild("Unit test child thread #5 for parent thread #6", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChild("Hello"));
            TargetParameterizedThreadWithAppendingChild?.AddChild("Unit test child thread #6 for parent thread #6", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChild("Hello"));
        }

        /// <summary>
        /// Tests starting kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelThreadWithChildrenOfChildrenThread() =>
            TargetThreadWithChildrenOfChildren?.Start();

        /// <summary>
        /// Tests starting kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelParameterizedThreadWithChildrenOfChildrenThread() =>
            TargetParameterizedThreadWithChildrenOfChildren?.Start("Agustin");

        /// <summary>
        /// Tests starting kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelThreadWithAppendingChildrenOfChildrenThread()
        {
            TargetThreadWithAppendingChildrenOfChildren?.Start();
            Thread.Sleep(1000);
            TargetThreadWithAppendingChildrenOfChildren?.AddChild("Unit test additional child thread #4 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            TargetThreadWithAppendingChildrenOfChildren?.AddChild("Unit test additional child thread #5 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            TargetThreadWithAppendingChildrenOfChildren?.AddChild("Unit test additional child thread #6 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            var child4 = TargetThreadWithAppendingChildrenOfChildren?.GetChild(3);
            var child5 = TargetThreadWithAppendingChildrenOfChildren?.GetChild(4);
            var child6 = TargetThreadWithAppendingChildrenOfChildren?.GetChild(5);
            child4?.AddChild("Unit test child thread #1 of child thread #4 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child4?.AddChild("Unit test child thread #2 of child thread #4 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child4?.AddChild("Unit test child thread #3 of child thread #4 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child5?.AddChild("Unit test child thread #1 of child thread #5 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child5?.AddChild("Unit test child thread #2 of child thread #5 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child5?.AddChild("Unit test child thread #3 of child thread #5 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child6?.AddChild("Unit test child thread #1 of child thread #6 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child6?.AddChild("Unit test child thread #2 of child thread #6 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
            child6?.AddChild("Unit test child thread #3 of child thread #6 for parent thread #9", true, KernelThreadTestHelper.WriteHelloFromAppendingChildrenOfChildren);
        }

        /// <summary>
        /// Tests starting kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStartKernelParameterizedThreadWithAppendingChildrenOfChildrenThread()
        {
            TargetParameterizedThreadWithAppendingChildrenOfChildren?.Start("Agustin");
            Thread.Sleep(1000);
            TargetParameterizedThreadWithAppendingChildrenOfChildren?.AddChild("Unit test child thread #4 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            TargetParameterizedThreadWithAppendingChildrenOfChildren?.AddChild("Unit test child thread #5 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            TargetParameterizedThreadWithAppendingChildrenOfChildren?.AddChild("Unit test child thread #6 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            var child4 = TargetParameterizedThreadWithAppendingChildrenOfChildren?.GetChild(3);
            var child5 = TargetParameterizedThreadWithAppendingChildrenOfChildren?.GetChild(4);
            var child6 = TargetParameterizedThreadWithAppendingChildrenOfChildren?.GetChild(5);
            child4?.AddChild("Unit test child thread #1 of child thread #4 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child4?.AddChild("Unit test child thread #2 of child thread #4 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child4?.AddChild("Unit test child thread #3 of child thread #4 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child5?.AddChild("Unit test child thread #1 of child thread #5 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child5?.AddChild("Unit test child thread #2 of child thread #5 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child5?.AddChild("Unit test child thread #3 of child thread #5 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child6?.AddChild("Unit test child thread #1 of child thread #6 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child6?.AddChild("Unit test child thread #2 of child thread #6 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
            child6?.AddChild("Unit test child thread #3 of child thread #6 for parent thread #10", true, (_) => KernelThreadTestHelper.WriteHelloWithArgumentFromAppendingChildrenOfChildren("Hello"));
        }

        /// <summary>
        /// Tests stopping kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStopKernelThread()
        {
            Thread.Sleep(300);
            TargetThread?.Stop();
            TargetThread?.IsStopping.ShouldBeFalse();
            TargetThread?.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests stopping kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStopKernelParameterizedThread()
        {
            Thread.Sleep(300);
            TargetParameterizedThread?.Stop();
            TargetParameterizedThread?.IsStopping.ShouldBeFalse();
            TargetParameterizedThread?.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests stopping kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStopKernelThreadWithChildThread()
        {
            if (TargetThreadWithChild is null)
                throw new Exception($"{nameof(TargetThreadWithChild)} is null");
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
            if (TargetParameterizedThreadWithChild is null)
                throw new Exception($"{nameof(TargetParameterizedThreadWithChild)} is null");
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
            if (TargetThreadWithAppendingChild is null)
                throw new Exception($"{nameof(TargetThreadWithAppendingChild)} is null");
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
            if (TargetParameterizedThreadWithAppendingChild is null)
                throw new Exception($"{nameof(TargetParameterizedThreadWithAppendingChild)} is null");
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
        /// Tests stopping kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStopKernelThreadWithChildrenOfChildrenThread()
        {
            if (TargetThreadWithChildrenOfChildren is null)
                throw new Exception($"{nameof(TargetThreadWithChildrenOfChildren)} is null");
            Thread.Sleep(500);
            Should.CompleteIn(TargetThreadWithChildrenOfChildren.Stop, TimeSpan.FromSeconds(5));
            TargetThreadWithChildrenOfChildren.IsStopping.ShouldBeFalse();
            TargetThreadWithChildrenOfChildren.ShouldNotBeNull();
            foreach (var childThread in TargetThreadWithChildrenOfChildren.ChildThreads)
            {
                childThread.IsStopping.ShouldBeFalse();
                childThread.ShouldNotBeNull();
                foreach (var childChildThread in childThread.ChildThreads)
                {
                    childChildThread.IsStopping.ShouldBeFalse();
                    childChildThread.ShouldNotBeNull();
                }
            }
        }

        /// <summary>
        /// Tests stopping kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStopKernelParameterizedThreadWithChildrenOfChildrenThread()
        {
            if (TargetParameterizedThreadWithChildrenOfChildren is null)
                throw new Exception($"{nameof(TargetParameterizedThreadWithChildrenOfChildren)} is null");
            Thread.Sleep(500);
            Should.CompleteIn(TargetParameterizedThreadWithChildrenOfChildren.Stop, TimeSpan.FromSeconds(5));
            TargetParameterizedThreadWithChildrenOfChildren.IsStopping.ShouldBeFalse();
            TargetParameterizedThreadWithChildrenOfChildren.ShouldNotBeNull();
            foreach (var childThread in TargetParameterizedThreadWithChildrenOfChildren.ChildThreads)
            {
                childThread.IsStopping.ShouldBeFalse();
                childThread.ShouldNotBeNull();
                foreach (var childChildThread in childThread.ChildThreads)
                {
                    childChildThread.IsStopping.ShouldBeFalse();
                    childChildThread.ShouldNotBeNull();
                }
            }
        }

        /// <summary>
        /// Tests stopping kernel thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStopKernelThreadWithAppendingChildrenOfChildrenThread()
        {
            if (TargetThreadWithAppendingChildrenOfChildren is null)
                throw new Exception($"{nameof(TargetThreadWithAppendingChildrenOfChildren)} is null");
            Thread.Sleep(500);
            Should.CompleteIn(TargetThreadWithAppendingChildrenOfChildren.Stop, TimeSpan.FromSeconds(5));
            TargetThreadWithAppendingChildrenOfChildren.IsStopping.ShouldBeFalse();
            TargetThreadWithAppendingChildrenOfChildren.ShouldNotBeNull();
            foreach (var childThread in TargetThreadWithAppendingChildrenOfChildren.ChildThreads)
            {
                childThread.IsStopping.ShouldBeFalse();
                childThread.ShouldNotBeNull();
                foreach (var childChildThread in childThread.ChildThreads)
                {
                    childChildThread.IsStopping.ShouldBeFalse();
                    childChildThread.ShouldNotBeNull();
                }
            }
        }

        /// <summary>
        /// Tests stopping kernel parameterized thread
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestStopKernelParameterizedThreadWithAppendingChildrenOfChildrenThread()
        {
            if (TargetParameterizedThreadWithAppendingChildrenOfChildren is null)
                throw new Exception($"{nameof(TargetParameterizedThreadWithAppendingChildrenOfChildren)} is null");
            Thread.Sleep(500);
            Should.CompleteIn(TargetParameterizedThreadWithAppendingChildrenOfChildren.Stop, TimeSpan.FromSeconds(5));
            TargetParameterizedThreadWithAppendingChildrenOfChildren.IsStopping.ShouldBeFalse();
            TargetParameterizedThreadWithAppendingChildrenOfChildren.ShouldNotBeNull();
            foreach (var childThread in TargetParameterizedThreadWithAppendingChildrenOfChildren.ChildThreads)
            {
                childThread.IsStopping.ShouldBeFalse();
                childThread.ShouldNotBeNull();
                foreach (var childChildThread in childThread.ChildThreads)
                {
                    childChildThread.IsStopping.ShouldBeFalse();
                    childChildThread.ShouldNotBeNull();
                }
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
