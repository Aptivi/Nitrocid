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

using Nitrocid.Kernel.Debugging.Trace;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Textify.General;

namespace Nitrocid.Kernel.Debugging
{
    /// <summary>
    /// Debug checking functions, such as assertions
    /// </summary>
    public static class DebugCheck
    {
        /// <summary>
        /// Asserts and checks to see if the condition is satisfied
        /// </summary>
        /// <param name="condition">Condition</param>
        public static void Assert(bool condition) =>
            Assert(condition, "");

        /// <summary>
        /// Asserts and checks to see if the condition is satisfied
        /// </summary>
        /// <param name="condition">Condition</param>
        /// <param name="message">A message to clarify why the assert failed</param>
        public static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                var trace = new DebugStackFrame();
                var exc = new KernelException(KernelExceptionType.AssertionFailure, $"condition is false. {message}");
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Condition is false!");
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Failure at {0} routine in {1}:{2}", trace.RoutineName, trace.RoutineFileName, trace.RoutineLineNumber);
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Message: {0}", message);
                KernelPanic.KernelErrorContinuable(Translate.DoTranslation("Assertion failure.") + $" {message}", exc);
                throw exc;
            }
        }

        /// <summary>
        /// Asserts and checks to see if the condition is satisfied
        /// </summary>
        /// <param name="condition">Condition</param>
        /// <param name="message">A message to clarify why the assert failed</param>
        /// <param name="vars">Variables to format the message with</param>
        public static void Assert(bool condition, string message, params object[] vars)
        {
            if (!condition)
            {
                message = TextTools.FormatString(message, vars);
                Assert(condition, message);
            }
        }

        /// <summary>
        /// Asserts and checks to see if the condition is not satisfied
        /// </summary>
        /// <param name="condition">Condition</param>
        public static void AssertNot(bool condition) =>
            AssertNot(condition, "");

        /// <summary>
        /// Asserts and checks to see if the condition is not satisfied
        /// </summary>
        /// <param name="condition">Condition</param>
        /// <param name="message">A message to clarify why the assert failed</param>
        public static void AssertNot(bool condition, string message)
        {
            if (condition)
            {
                var trace = new DebugStackFrame();
                var exc = new KernelException(KernelExceptionType.AssertionFailure, $"condition is true. {message}");
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Condition is true!");
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Failure at {0} routine in {1}:{2}", trace.RoutineName, trace.RoutineFileName, trace.RoutineLineNumber);
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Message: {0}", message);
                KernelPanic.KernelErrorContinuable(Translate.DoTranslation("Assertion failure.") + $" {message}", exc);
                throw exc;
            }
        }

        /// <summary>
        /// Asserts and checks to see if the condition is not satisfied
        /// </summary>
        /// <param name="condition">Condition</param>
        /// <param name="message">A message to clarify why the assert failed</param>
        /// <param name="vars">Variables to format the message with</param>
        public static void AssertNot(bool condition, string message, params object[] vars)
        {
            if (!condition)
            {
                message = TextTools.FormatString(message, vars);
                AssertNot(condition, message);
            }
        }

        /// <summary>
        /// Asserts and checks to see if the value is null
        /// </summary>
        /// <param name="value">Condition</param>
        public static void AssertNull<T>(T value) =>
            AssertNull(value, "");

        /// <summary>
        /// Asserts and checks to see if the value is null
        /// </summary>
        /// <param name="value">Condition</param>
        /// <param name="message">A message to clarify why the assert failed</param>
        public static void AssertNull<T>(T value, string message)
        {
            if (value is null)
            {
                var trace = new DebugStackFrame();
                var exc = new KernelException(KernelExceptionType.AssertionFailure, $"value is null. {message}");
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Value is null!");
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Failure at {0} routine in {1}:{2}", trace.RoutineName, trace.RoutineFileName, trace.RoutineLineNumber);
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Message: {0}", message);
                KernelPanic.KernelErrorContinuable(Translate.DoTranslation("Assertion failure.") + $" {message}", exc);
                throw exc;
            }
        }

        /// <summary>
        /// Asserts and checks to see if the value is null
        /// </summary>
        /// <param name="value">Condition</param>
        /// <param name="message">A message to clarify why the assert failed</param>
        /// <param name="vars">Variables to format the message with</param>
        public static void AssertNull<T>(T value, string message, params object[] vars)
        {
            if (value is null)
            {
                message = TextTools.FormatString(message, vars);
                AssertNull(value, message);
            }
        }

        /// <summary>
        /// Asserts and checks to see if the value is not null
        /// </summary>
        /// <param name="value">Condition</param>
        public static void AssertNotNull<T>(T value) =>
            AssertNotNull(value, "");

        /// <summary>
        /// Asserts and checks to see if the value is not null
        /// </summary>
        /// <param name="value">Condition</param>
        /// <param name="message">A message to clarify why the assert failed</param>
        public static void AssertNotNull<T>(T value, string message)
        {
            if (value is not null)
            {
                var trace = new DebugStackFrame();
                var exc = new KernelException(KernelExceptionType.AssertionFailure, $"value is not null. {message}");
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Value is not null!");
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Failure at {0} routine in {1}:{2}", trace.RoutineName, trace.RoutineFileName, trace.RoutineLineNumber);
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Message: {0}", message);
                KernelPanic.KernelErrorContinuable(Translate.DoTranslation("Assertion failure.") + $" {message}", exc);
                throw exc;
            }
        }

        /// <summary>
        /// Asserts and checks to see if the value is not null
        /// </summary>
        /// <param name="value">Condition</param>
        /// <param name="message">A message to clarify why the assert failed</param>
        /// <param name="vars">Variables to format the message with</param>
        public static void AssertNotNull<T>(T value, string message, params object[] vars)
        {
            if (value is not null)
            {
                message = TextTools.FormatString(message, vars);
                AssertNotNull(value, message);
            }
        }

        /// <summary>
        /// Triggers assertion failure
        /// </summary>
        /// <param name="message">A message to clarify why the assert failed</param>
        public static void AssertFail(string message)
        {
            var trace = new DebugStackFrame();
            var exc = new KernelException(KernelExceptionType.AssertionFailure, $"undetermined failure. {message}");
            DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Undetermined failure!");
            DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Failure at {0} routine in {1}:{2}", trace.RoutineName, trace.RoutineFileName, trace.RoutineLineNumber);
            DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Message: {0}", message);
            KernelPanic.KernelErrorContinuable(Translate.DoTranslation("Assertion failure.") + $" {message}", exc);
            throw exc;
        }

        /// <summary>
        /// Triggers assertion failure
        /// </summary>
        /// <param name="message">A message to clarify why the assert failed</param>
        /// <param name="vars">Variables to format the message with</param>
        public static void AssertFail(string message, params object[] vars)
        {
            message = TextTools.FormatString(message, vars);
            AssertFail(message);
        }
    }
}
