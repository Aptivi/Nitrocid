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

using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using System;
using System.Reflection;

namespace Nitrocid.Base.Misc.Reflection
{
    /// <summary>
    /// Method management module
    /// </summary>
    public static class MethodManager
    {

        /// <summary>
        /// Gets a method from method name
        /// </summary>
        /// <param name="Method">Method name. Use operator NameOf to get name.</param>
        /// <returns>Method information</returns>
        public static MethodBase? GetMethod(string Method)
        {
            Type[] PossibleTypes;
            MethodInfo? PossibleMethod;

            // Get types of possible flag locations
            PossibleTypes = ReflectionCommon.KernelTypes;

            // Get fields of flag modules
            foreach (Type PossibleType in PossibleTypes)
            {
                PossibleMethod = PossibleType.GetMethod(Method);
                if (PossibleMethod != null)
                    return PossibleMethod;
            }
            return null;
        }

        /// <summary>
        /// Gets a method from method name
        /// </summary>
        /// <param name="Method">Method name. Use operator NameOf to get name.</param>
        /// <param name="methodType">From which type do we need to get a method from?</param>
        /// <returns>Method information</returns>
        public static MethodBase? GetMethod(string Method, Type methodType) =>
            methodType.GetMethod(Method);

        /// <summary>
        /// Invokes a non-static method with arguments
        /// </summary>
        /// <param name="method">The method to find and execute</param>
        /// <param name="obj">The object on which to invoke the method</param>
        /// <param name="args">Arguments to be specified to the method. Review the method signature for more information.</param>
        /// <returns>The value of the returned object from the method</returns>
        /// <exception cref="KernelException"></exception>
        public static object? InvokeMethod(string method, object obj, params object?[]? args) =>
            InvokeMethod(method, obj, obj.GetType(), args);

        /// <summary>
        /// Invokes a non-static method with arguments
        /// </summary>
        /// <param name="method">The method to find and execute</param>
        /// <param name="obj">The object on which to invoke the method</param>
        /// <param name="methodType">From which type do we need to get a method from?</param>
        /// <param name="args">Arguments to be specified to the method. Review the method signature for more information.</param>
        /// <returns>The value of the returned object from the method</returns>
        /// <exception cref="KernelException"></exception>
        public static object? InvokeMethod(string method, object obj, Type methodType, params object?[]? args) =>
            InvokeMethod(GetMethod(method, methodType), obj, args);

        /// <summary>
        /// Invokes a non-static method with arguments
        /// </summary>
        /// <param name="method">The method to find and execute</param>
        /// <param name="obj">The object on which to invoke the method</param>
        /// <param name="args">Arguments to be specified to the method. Review the method signature for more information.</param>
        /// <returns>The value of the returned object from the method</returns>
        /// <exception cref="KernelException"></exception>
        public static object? InvokeMethod(MethodBase? method, object obj, params object?[]? args)
        {
            if (method is null)
                throw new KernelException(KernelExceptionType.Reflection, LanguageTools.GetLocalized("NKS_MISC_REFLECTION_EXCEPTION_NOSUCHMETHOD"));
            if (method.IsStatic)
                throw new KernelException(KernelExceptionType.Reflection, LanguageTools.GetLocalized("NKS_MISC_REFLECTION_EXCEPTION_METHODSTATIC") + $" {method.Name}");

            // Now, invoke the method.
            if (args is not null && args.Length > 0)
                return method.Invoke(obj, args);
            return method.Invoke(obj, null);
        }

        /// <summary>
        /// Invokes a static method with arguments
        /// </summary>
        /// <param name="method">The method to find and execute</param>
        /// <param name="args">Arguments to be specified to the method. Review the method signature for more information.</param>
        /// <returns>The value of the returned object from the method</returns>
        /// <exception cref="KernelException"></exception>
        public static object? InvokeMethodStatic(string method, params object?[]? args) =>
            InvokeMethodStatic(GetMethod(method), args);

        /// <summary>
        /// Invokes a static method with arguments
        /// </summary>
        /// <param name="method">The method to find and execute</param>
        /// <param name="methodType">From which type do we need to get a method from?</param>
        /// <param name="args">Arguments to be specified to the method. Review the method signature for more information.</param>
        /// <returns>The value of the returned object from the method</returns>
        /// <exception cref="KernelException"></exception>
        public static object? InvokeMethodStatic(string method, Type methodType, params object?[]? args) =>
            InvokeMethodStatic(GetMethod(method, methodType), args);

        /// <summary>
        /// Invokes a static method with arguments
        /// </summary>
        /// <param name="method">The method to find and execute</param>
        /// <param name="args">Arguments to be specified to the method. Review the method signature for more information.</param>
        /// <returns>The value of the returned object from the method</returns>
        /// <exception cref="KernelException"></exception>
        public static object? InvokeMethodStatic(MethodBase? method, params object?[]? args)
        {
            if (method is null)
                throw new KernelException(KernelExceptionType.Reflection, LanguageTools.GetLocalized("NKS_MISC_REFLECTION_EXCEPTION_NOSUCHMETHOD"));
            if (!method.IsStatic)
                throw new KernelException(KernelExceptionType.Reflection, LanguageTools.GetLocalized("NKS_MISC_REFLECTION_EXCEPTION_METHODNONSTATIC") + $" {method.Name}");

            // Now, invoke the method.
            if (args is not null && args.Length > 0)
                return method.Invoke(null, args);
            return method.Invoke(null, null);
        }
    }
}
