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

using System.Threading;
using System;
using Shouldly;

namespace Nitrocid.Tests.Kernel.Threading
{

    public static class KernelThreadTestHelper
    {

        /// <summary>
        /// [Kernel thread test] Write hello to console
        /// </summary>
        public static void WriteHello()
        {
            try
            {
                Console.WriteLine("Hello world!");
                Console.WriteLine("- Writing from thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                while (!KernelThreadTests.TargetThread.IsStopping)
                    Thread.Sleep(1000);
            }
            catch
            {
                Console.WriteLine("- Goodbye from thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                KernelThreadTests.TargetThread.IsStopping.ShouldBeTrue();
            }
        }

        /// <summary>
        /// [Kernel thread test] Write hello to console with argument
        /// </summary>
        public static void WriteHelloWithArgument(string Name)
        {
            try
            {
                Console.WriteLine("Hello, {0}!", Name);
                Console.WriteLine("- Writing from thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                while (!KernelThreadTests.TargetParameterizedThread.IsStopping)
                    Thread.Sleep(1000);
            }
            catch
            {
                Console.WriteLine("- Goodbye from thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                KernelThreadTests.TargetParameterizedThread.IsStopping.ShouldBeTrue();
            }
        }

        /// <summary>
        /// [Kernel thread test] Write hello to console
        /// </summary>
        public static void WriteHelloWithChild()
        {
            try
            {
                Console.WriteLine("Hello world!");
                Console.WriteLine("- Writing from parent thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                while (!KernelThreadTests.TargetThreadWithChild.IsStopping)
                    Thread.Sleep(1000);
            }
            catch
            {
                Console.WriteLine("- Goodbye from parent thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                KernelThreadTests.TargetThreadWithChild.IsStopping.ShouldBeTrue();
            }
        }

        /// <summary>
        /// [Kernel thread test] Write hello to console with argument
        /// </summary>
        public static void WriteHelloWithArgumentWithChild(string Name)
        {
            try
            {
                Console.WriteLine("Hello, {0}!", Name);
                Console.WriteLine("- Writing from parent thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                while (!KernelThreadTests.TargetParameterizedThreadWithChild.IsStopping)
                    Thread.Sleep(1000);
            }
            catch
            {
                Console.WriteLine("- Goodbye from parent thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                KernelThreadTests.TargetParameterizedThreadWithChild.IsStopping.ShouldBeTrue();
            }
        }

        /// <summary>
        /// [Kernel thread test] Write hello to console
        /// </summary>
        public static void WriteHelloFromChild()
        {
            try
            {
                Console.WriteLine("- Hello world!");
                Console.WriteLine("  - Writing from thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                while (!KernelThreadTests.TargetThreadWithChild.IsStopping)
                    Thread.Sleep(1000);
            }
            catch
            {
                Console.WriteLine("  - Goodbye from thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                KernelThreadTests.TargetThreadWithChild.IsStopping.ShouldBeTrue();
            }
        }

        /// <summary>
        /// [Kernel thread test] Write hello to console with argument
        /// </summary>
        public static void WriteHelloWithArgumentFromChild(string Name)
        {
            try
            {
                Console.WriteLine("- Hello, {0}!", Name);
                Console.WriteLine("  - Writing from thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                while (!KernelThreadTests.TargetParameterizedThreadWithChild.IsStopping)
                    Thread.Sleep(1000);
            }
            catch
            {
                Console.WriteLine("  - Goodbye from thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                KernelThreadTests.TargetParameterizedThreadWithChild.IsStopping.ShouldBeTrue();
            }
        }

        /// <summary>
        /// [Kernel thread test] Write hello to console
        /// </summary>
        public static void WriteHelloWithAppendingChild()
        {
            try
            {
                Console.WriteLine("Hello world!");
                Console.WriteLine("- Writing from parent thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                while (!KernelThreadTests.TargetThreadWithAppendingChild.IsStopping)
                    Thread.Sleep(1000);
            }
            catch
            {
                Console.WriteLine("- Goodbye from parent thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                KernelThreadTests.TargetThreadWithAppendingChild.IsStopping.ShouldBeTrue();
            }
        }

        /// <summary>
        /// [Kernel thread test] Write hello to console with argument
        /// </summary>
        public static void WriteHelloWithArgumentWithAppendingChild(string Name)
        {
            try
            {
                Console.WriteLine("Hello, {0}!", Name);
                Console.WriteLine("- Writing from parent thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                while (!KernelThreadTests.TargetParameterizedThreadWithAppendingChild.IsStopping)
                    Thread.Sleep(1000);
            }
            catch
            {
                Console.WriteLine("- Goodbye from parent thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                KernelThreadTests.TargetParameterizedThreadWithAppendingChild.IsStopping.ShouldBeTrue();
            }
        }

        /// <summary>
        /// [Kernel thread test] Write hello to console
        /// </summary>
        public static void WriteHelloFromAppendingChild()
        {
            try
            {
                Console.WriteLine("- Hello world!");
                Console.WriteLine("  - Writing from thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                while (!KernelThreadTests.TargetThreadWithAppendingChild.IsStopping)
                    Thread.Sleep(1000);
            }
            catch
            {
                Console.WriteLine("  - Goodbye from thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                KernelThreadTests.TargetThreadWithAppendingChild.IsStopping.ShouldBeTrue();
            }
        }

        /// <summary>
        /// [Kernel thread test] Write hello to console with argument
        /// </summary>
        public static void WriteHelloWithArgumentFromAppendingChild(string Name)
        {
            try
            {
                Console.WriteLine("- Hello, {0}!", Name);
                Console.WriteLine("  - Writing from thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                while (!KernelThreadTests.TargetParameterizedThreadWithAppendingChild.IsStopping)
                    Thread.Sleep(1000);
            }
            catch
            {
                Console.WriteLine("  - Goodbye from thread: {0} [{1}]", Thread.CurrentThread.Name, Environment.CurrentManagedThreadId);
                KernelThreadTests.TargetParameterizedThreadWithAppendingChild.IsStopping.ShouldBeTrue();
            }
        }

    }
}
