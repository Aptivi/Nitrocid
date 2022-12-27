// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Kernel.Debugging.Trace;
using KS.Kernel.Exceptions;
using KS.Languages;

namespace KS.Kernel.Debugging
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
                var exc = new KernelException(KernelExceptionType.AssertionFailure, "condition is false.");
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Condition is false!");
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Failure at {0} routine in {1}:{2}", trace.RoutineName, trace.RoutineFileName, trace.RoutineLineNumber);
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Message: {0}", message);
                KernelTools.KernelError(KernelErrorLevel.C, false, 0, Translate.DoTranslation("Assertion failure.") + $" {message}", exc);
                throw exc;
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
                var exc = new KernelException(KernelExceptionType.AssertionFailure, "value is null.");
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Value is null!");
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Failure at {0} routine in {1}:{2}", trace.RoutineName, trace.RoutineFileName, trace.RoutineLineNumber);
                DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Message: {0}", message);
                KernelTools.KernelError(KernelErrorLevel.C, false, 0, Translate.DoTranslation("Assertion failure.") + $" {message}", exc);
                throw exc;
            }
        }
    }
}
