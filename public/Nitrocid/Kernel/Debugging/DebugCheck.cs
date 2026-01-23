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
using Nitrocid.Languages;
using System.IO;
using System.Runtime.CompilerServices;
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
        /// <param name="message">A message to clarify why the assert failed</param>
        /// <param name="memberName">Member name. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberLine">Member line number. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberPath">Member path. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="vars">Variables to format the message with</param>
        public static void Assert(bool condition, string message, [CallerMemberName] string memberName = "", [CallerLineNumber] int memberLine = 0, [CallerFilePath] string memberPath = "", object?[]? vars = null)
        {
            if (!condition)
            {
                if (vars is not null)
                    message = TextTools.FormatString(message, vars);
                AssertFailInternal(message, "Condition is false!", memberName, memberLine, memberPath);
            }
        }

        /// <summary>
        /// Asserts and checks to see if the condition is not satisfied
        /// </summary>
        /// <param name="condition">Condition</param>
        /// <param name="message">A message to clarify why the assert failed</param>
        /// <param name="memberName">Member name. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberLine">Member line number. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberPath">Member path. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="vars">Variables to format the message with</param>
        public static void AssertNot(bool condition, string message, [CallerMemberName] string memberName = "", [CallerLineNumber] int memberLine = 0, [CallerFilePath] string memberPath = "", object?[]? vars = null)
        {
            if (condition)
            {
                if (vars is not null)
                    message = TextTools.FormatString(message, vars);
                AssertFailInternal(message, "Condition is true!", memberName, memberLine, memberPath);
            }
        }

        /// <summary>
        /// Asserts and checks to see if the value is null
        /// </summary>
        /// <param name="value">Condition</param>
        /// <param name="message">A message to clarify why the assert failed</param>
        /// <param name="memberName">Member name. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberLine">Member line number. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberPath">Member path. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="vars">Variables to format the message with</param>
        public static void AssertNull<T>(T value, string message, [CallerMemberName] string memberName = "", [CallerLineNumber] int memberLine = 0, [CallerFilePath] string memberPath = "", object?[]? vars = null)
        {
            if (value is null)
            {
                if (vars is not null)
                    message = TextTools.FormatString(message, vars);
                AssertFailInternal(message, "Value is null!", memberName, memberLine, memberPath);
            }
        }

        /// <summary>
        /// Asserts and checks to see if the value is not null
        /// </summary>
        /// <param name="value">Condition</param>
        /// <param name="message">A message to clarify why the assert failed</param>
        /// <param name="memberName">Member name. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberLine">Member line number. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberPath">Member path. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="vars">Variables to format the message with</param>
        public static void AssertNotNull<T>(T value, string message, [CallerMemberName] string memberName = "", [CallerLineNumber] int memberLine = 0, [CallerFilePath] string memberPath = "", object?[]? vars = null)
        {
            if (value is not null)
            {
                if (vars is not null)
                    message = TextTools.FormatString(message, vars);
                AssertFailInternal(message, "Value is not null!", memberName, memberLine, memberPath);
            }
        }

        /// <summary>
        /// Triggers assertion failure
        /// </summary>
        /// <param name="message">A message to clarify why the assert failed</param>
        /// <param name="memberName">Member name. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberLine">Member line number. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberPath">Member path. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="vars">Variables to format the message with</param>
        public static void AssertFail(string message, [CallerMemberName] string memberName = "", [CallerLineNumber] int memberLine = 0, [CallerFilePath] string memberPath = "", object?[]? vars = null)
        {
            if (vars is not null)
                message = TextTools.FormatString(message, vars);
            AssertFailInternal(message, "Undetermined failure!", memberName, memberLine, memberPath);
        }

        private static void AssertFailInternal(string message, string reason, string memberName = "", int memberLine = 0, string memberPath = "")
        {
            string fileName = Path.GetFileName(memberPath);
            var exc = new KernelException(KernelExceptionType.AssertionFailure, $"{reason} {message}");
            DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! {0}", vars: [reason]);
            DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Failure at {0} routine in {1}:{2}", vars: [memberName, fileName, memberLine]);
            DebugWriter.WriteDebug(DebugLevel.E, "!!! ASSERTION FAILURE !!! Message: {0}", vars: [message]);
            KernelPanic.KernelErrorContinuable(Translate.DoTranslation("Assertion failure.") + $" {message}", exc);
            throw exc;
        }
    }
}
