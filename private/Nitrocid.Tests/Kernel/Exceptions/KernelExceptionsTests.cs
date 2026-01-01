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

using Nitrocid.Kernel.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Nitrocid.Tests.Kernel.Exceptions
{

    [TestClass]
    public class KernelExceptionsTests
    {

        /// <summary>
        /// Tests raising the kernel error exception
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestRaiseKernelErrorException()
        {
            var exc = new KernelErrorException();
            exc.ShouldNotBeNull();
            Should.Throw(() => throw exc, typeof(KernelErrorException));
        }

        /// <summary>
        /// Tests raising the kernel exception
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestRaiseKernelException()
        {
            var exc = new KernelException(KernelExceptionType.Unknown);
            exc.ShouldNotBeNull();
            exc.ExceptionType.ShouldBe(KernelExceptionType.Unknown);
            Should.Throw(() => throw exc, typeof(KernelException));
        }

        /// <summary>
        /// Tests getting the kernel exception message
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestGetFinalExceptionMessage()
        {
            var exceptionTypes = Enum.GetNames(typeof(KernelExceptionType));
            foreach (var type in exceptionTypes)
            {
                var exceptionType = (KernelExceptionType)Enum.Parse(typeof(KernelExceptionType), type);
                string message = KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, "Hello world!", null);
                string initialMessage = KernelExceptionMessages.Messages[exceptionType];
                message.ShouldContain(initialMessage);
                message.ShouldContain("Hello world!");
                message.ShouldContain("The module didn't provide the exception information, so it's usually an indicator that something is wrong.");
            }
        }

        /// <summary>
        /// Tests getting the kernel exception message
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestGetFinalExceptionMessageWithNoExtraMessage()
        {
            var exceptionTypes = Enum.GetNames(typeof(KernelExceptionType));
            foreach (var type in exceptionTypes)
            {
                var exceptionType = (KernelExceptionType)Enum.Parse(typeof(KernelExceptionType), type);
                string message = KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, "", null);
                string initialMessage = KernelExceptionMessages.Messages[exceptionType];
                message.ShouldContain(initialMessage);
                message.ShouldContain("The module that caused the fault didn't provide additional information.");
                message.ShouldContain("The module didn't provide the exception information, so it's usually an indicator that something is wrong.");
            }
        }

        /// <summary>
        /// Tests getting the kernel exception message
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestGetFinalExceptionMessageWithException()
        {
            var exceptionTypes = Enum.GetNames(typeof(KernelExceptionType));
            var exc = new KernelErrorException("Testing...");
            foreach (var type in exceptionTypes)
            {
                var exceptionType = (KernelExceptionType)Enum.Parse(typeof(KernelExceptionType), type);
                string message = KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, "Hello world!", exc);
                string initialMessage = KernelExceptionMessages.Messages[exceptionType];
                message.ShouldContain(initialMessage);
                message.ShouldContain("Hello world!");
                message.ShouldContain("If the additional info above doesn't help you pinpoint the problem, this may help you pinpoint it.");
                message.ShouldContain($"{exc.GetType().Name}: {exc.Message}");
            }
        }

        /// <summary>
        /// Tests getting the kernel exception message
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestGetFinalExceptionMessageWithExceptionWithNoExtraMessage()
        {
            var exceptionTypes = Enum.GetNames(typeof(KernelExceptionType));
            var exc = new KernelErrorException("Testing...");
            foreach (var type in exceptionTypes)
            {
                var exceptionType = (KernelExceptionType)Enum.Parse(typeof(KernelExceptionType), type);
                string message = KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, "", exc);
                string initialMessage = KernelExceptionMessages.Messages[exceptionType];
                message.ShouldContain(initialMessage);
                message.ShouldContain("The module that caused the fault didn't provide additional information.");
                message.ShouldContain("If the additional info above doesn't help you pinpoint the problem, this may help you pinpoint it.");
                message.ShouldContain($"{exc.GetType().Name}: {exc.Message}");
            }
        }

        /// <summary>
        /// Tests getting the kernel exception message
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestGetFinalExceptionMessageWithUnknownType()
        {
            var exceptionType = (KernelExceptionType)(-1);
            string message = KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, "", null);
            message.ShouldContain("Unfortunately, an invalid message type was given, so it's possible that something is messed up. Try turning on the debugger and reproducing the problem.");
            message.ShouldContain("The module didn't provide the exception information, so it's usually an indicator that something is wrong.");
        }

        /// <summary>
        /// Tests getting the kernel exception error codes
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestGetErrorCodes()
        {
            var exceptionTypes = Enum.GetNames(typeof(KernelExceptionType));
            foreach (var type in exceptionTypes)
            {
                var exceptionType = (KernelExceptionType)Enum.Parse(typeof(KernelExceptionType), type);
                int code = KernelExceptionTools.GetErrorCode(exceptionType);
                code.ShouldBe(10000 + (int)exceptionType);
            }
        }

    }
}
