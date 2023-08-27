
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

using System;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Builders;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace Nitrocid.Tests
{
    /// <inheritdoc/>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TestGenericAttribute : TestAttribute, ISimpleTestBuilder
    {
        public TestGenericAttribute() :
            base()
        { }

        public Type[] TypeArguments { get; set; }

        TestMethod ISimpleTestBuilder.BuildFrom(IMethodInfo method, Test suite)
        {
            if (!method.IsGenericMethodDefinition)
                return BuildFrom(method, suite);

            if (TypeArguments == null || TypeArguments.Length != method.GetGenericArguments().Length)
            {
                var parms = new TestCaseParameters { RunState = RunState.NotRunnable };
                parms.Properties.Set(PropertyNames.SkipReason, $"{nameof(TypeArguments)} should have {method.GetGenericArguments().Length} elements");
                return new NUnitTestCaseBuilder().BuildTestMethod(method, suite, parms);
            }

            var genMethod = method.MakeGenericMethod(TypeArguments);
            return BuildFrom(genMethod, suite);
        }
    }

    /// <inheritdoc/>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TestAttribute<T> : TestGenericAttribute
    {
        public TestAttribute() :
            base() => TypeArguments = new[] { typeof(T) };
    }

    /// <inheritdoc/>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TestCaseGenericAttribute : TestCaseAttribute, ITestBuilder
    {
        public TestCaseGenericAttribute(params object[] arguments) :
            base(arguments)
        { }

        public Type[] TypeArguments { get; set; }

        IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, Test suite)
        {
            if (!method.IsGenericMethodDefinition)
                return BuildFrom(method, suite);

            if (TypeArguments == null || TypeArguments.Length != method.GetGenericArguments().Length)
            {
                var parms = new TestCaseParameters { RunState = RunState.NotRunnable };
                parms.Properties.Set(PropertyNames.SkipReason, $"{nameof(TypeArguments)} should have {method.GetGenericArguments().Length} elements");
                return new[] { new NUnitTestCaseBuilder().BuildTestMethod(method, suite, parms) };
            }

            var genMethod = method.MakeGenericMethod(TypeArguments);
            return BuildFrom(genMethod, suite);
        }
    }

    /// <inheritdoc/>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TestCaseAttribute<T> : TestCaseGenericAttribute
    {
        public TestCaseAttribute(params object[] arguments) :
            base(arguments) => TypeArguments = new[] { typeof(T) };
    }

    /// <inheritdoc/>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TestCaseSourceGenericAttribute : TestCaseSourceAttribute, ITestBuilder
    {
        public TestCaseSourceGenericAttribute(string sourceName) :
            base(sourceName)
        { }

        public Type[] TypeArguments { get; set; }

        IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, Test suite)
        {
            if (!method.IsGenericMethodDefinition)
                return BuildFrom(method, suite);

            if (TypeArguments == null || TypeArguments.Length != method.GetGenericArguments().Length)
            {
                var parms = new TestCaseParameters { RunState = RunState.NotRunnable };
                parms.Properties.Set(PropertyNames.SkipReason, $"{nameof(TypeArguments)} should have {method.GetGenericArguments().Length} elements");
                return new[] { new NUnitTestCaseBuilder().BuildTestMethod(method, suite, parms) };
            }

            var genMethod = method.MakeGenericMethod(TypeArguments);
            return BuildFrom(genMethod, suite);
        }
    }

    /// <inheritdoc/>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TestCaseSourceAttribute<T> : TestCaseSourceGenericAttribute
    {
        public TestCaseSourceAttribute(string sourceName) :
            base(sourceName) => TypeArguments = new[] { typeof(T) };
    }
}
