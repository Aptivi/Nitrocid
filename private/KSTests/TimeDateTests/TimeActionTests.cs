
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using System.Globalization;
using KS.TimeDate;
using NUnit.Framework;
using Shouldly;
using System;

namespace KSTests.TimeDateTests
{

    [TestFixture]
    public class TimeActionTests
    {

        /// <summary>
        /// Tests rendering kernel date
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDate() => TimeDateRenderers.RenderDate().ShouldNotBeNullOrEmpty();

        /// <summary>
        /// Tests rendering kernel date with specific format type
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDateType()
        {
            TimeDateRenderers.RenderDate(TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderDate(TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDateCult()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderDate(TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture and format type
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDateCultType()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderDate(TargetCult, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderDate(TargetCult, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDateUtc() => TimeDateRenderersUtc.RenderDateUtc().ShouldNotBeNullOrEmpty();

        /// <summary>
        /// Tests rendering kernel date with specific format type (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDateUtcType()
        {
            TimeDateRenderersUtc.RenderDateUtc(TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderDateUtc(TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDateUtcCult()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderDateUtc(TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture and format type (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelDateUtcCultType()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderDateUtc(TargetCult, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderDateUtc(TargetCult, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDate()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderers.RenderDate(TargetDate).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDateType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderers.RenderDate(TargetDate, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderDate(TargetDate, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDateCult()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderDate(TargetDate, TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDateCultType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderDate(TargetDate, TargetCult, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderDate(TargetDate, TargetCult, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDateUtc()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderersUtc.RenderDateUtc(TargetDate).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDateUtcType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderersUtc.RenderDateUtc(TargetDate, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderDateUtc(TargetDate, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDateUtcCult()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderDateUtc(TargetDate, TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomDateUtcCultType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderDateUtc(TargetDate, TargetCult, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderDateUtc(TargetDate, TargetCult, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel time
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTime() => TimeDateRenderers.RenderTime().ShouldNotBeNullOrEmpty();

        /// <summary>
        /// Tests rendering kernel time
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTimeType()
        {
            TimeDateRenderers.RenderTime(TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderTime(TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel time with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTimeCult()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderTime(TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel time with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTimeCultType()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderTime(TargetCult, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderTime(TargetCult, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel time (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTimeUtc() => TimeDateRenderersUtc.RenderTimeUtc().ShouldNotBeNullOrEmpty();

        /// <summary>
        /// Tests rendering kernel time with specific format type (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTimeUtcType()
        {
            TimeDateRenderersUtc.RenderTimeUtc(TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderTimeUtc(TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel time with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTimeUtcCult()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderTimeUtc(TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel time with specified culture and format type (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelTimeUtcCultType()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderTimeUtc(TargetCult, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderTimeUtc(TargetCult, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTime()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            TimeDateRenderers.RenderTime(TargetTime).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTimeType()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            TimeDateRenderers.RenderTime(TargetTime, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderTime(TargetTime, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTimeCult()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderTime(TargetTime, TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTimeCultType()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.RenderTime(TargetTime, TargetCult, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.RenderTime(TargetTime, TargetCult, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTimeUtc()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            TimeDateRenderersUtc.RenderTimeUtc(TargetTime).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTimeUtcType()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            TimeDateRenderersUtc.RenderTimeUtc(TargetTime, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderTimeUtc(TargetTime, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTimeUtcCult()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderTimeUtc(TargetTime, TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom time with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomTimeUtcCultType()
        {
            var TargetTime = new DateTime(2018, 2, 22, 5, 40, 37);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderTimeUtc(TargetTime, TargetCult, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderTimeUtc(TargetTime, TargetCult, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernel() => TimeDateRenderers.Render().ShouldNotBeNullOrEmpty();

        /// <summary>
        /// Tests rendering kernel date
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelType()
        {
            TimeDateRenderers.Render(TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.Render(TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelCult()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.Render(TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelCultType()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.Render(TargetCult, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.Render(TargetCult, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelUtc() => TimeDateRenderersUtc.RenderUtc().ShouldNotBeNullOrEmpty();

        /// <summary>
        /// Tests rendering kernel date with specific format type (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelUtcType()
        {
            TimeDateRenderersUtc.RenderUtc(TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderUtc(TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelUtcCult()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderUtc(TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering kernel date with specified culture and format type (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderKernelUtcCultType()
        {
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderUtc(TargetCult, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderUtc(TargetCult, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustom()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderers.Render(TargetDate).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderers.Render(TargetDate, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.Render(TargetDate, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomCult()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.Render(TargetDate, TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with specified culture
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomCultType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderers.Render(TargetDate, TargetCult, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderers.Render(TargetDate, TargetCult, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomUtc()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderersUtc.RenderUtc(TargetDate).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomUtcType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            TimeDateRenderersUtc.RenderUtc(TargetDate, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderUtc(TargetDate, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomUtcCult()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderUtc(TargetDate, TargetCult).ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests rendering custom date with specified culture (UTC)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestRenderCustomUtcCultType()
        {
            var TargetDate = new DateTime(2018, 2, 22);
            var TargetCult = new CultureInfo("es-ES");
            TimeDateRenderersUtc.RenderUtc(TargetDate, TargetCult, TimeDate.FormatType.Long).ShouldNotBeNullOrEmpty();
            TimeDateRenderersUtc.RenderUtc(TargetDate, TargetCult, TimeDate.FormatType.Short).ShouldNotBeNullOrEmpty();
        }

    }
}
