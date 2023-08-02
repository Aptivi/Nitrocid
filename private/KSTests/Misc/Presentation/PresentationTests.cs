
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

using KS.Misc.Presentation;
using KS.Misc.Presentation.Elements;
using KSTests.Misc.Interactive.Interactives;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSTests.Misc.Presentation
{

    [TestFixture]
    public class PresentationTests
    {

        /// <summary>
        /// Tests building the presentation
        /// </summary>
        [Test]
        [Description("Management")]
        public static void BuildPresentation()
        {
            // Presentation...
            var presentation = new KS.Misc.Presentation.Presentation("MyPresentation", new()
            {
                new PresentationPage("Page one", new()
                {
                    new TextElement()
                    {
                        Arguments = new object[] { "Hello, world!" },
                        InvokeAction = () => Debug.WriteLine("Invoke action")
                    }
                }),
                new PresentationPage("Page two", new()
                {
                    new InputElement()
                    {
                        Arguments = new object[] { "Hello, world!" },
                        InvokeActionInput = (obj) => Debug.WriteLine(obj.ToString())
                    }
                }),
            });

            // General info
            presentation.ShouldNotBeNull();
            presentation.Name.ShouldBe("MyPresentation");
            presentation.Pages.Count.ShouldBe(2);

            // Individual pages
            var pageOne = presentation.Pages[0];
            var pageTwo = presentation.Pages[1];

            // Checking their correctness...
            pageOne.ShouldNotBeNull();
            pageTwo.ShouldNotBeNull();
            pageOne.Name.ShouldBe("Page one");
            pageTwo.Name.ShouldBe("Page two");
            pageOne.Elements.ShouldNotBeNull();
            pageTwo.Elements.ShouldNotBeNull();
            pageOne.Elements.ShouldNotBeEmpty();
            pageTwo.Elements.ShouldNotBeEmpty();

            // Individual elements
            var elementForPageOne = pageOne.Elements[0];
            var elementForPageTwo = pageTwo.Elements[0];

            // Checking their correctness...
            elementForPageOne.ShouldNotBeNull();
            elementForPageTwo.ShouldNotBeNull();
            elementForPageOne.IsInput.ShouldBeFalse();
            elementForPageTwo.IsInput.ShouldBeTrue();
            elementForPageOne.Arguments.ShouldNotBeNull();
            elementForPageTwo.Arguments.ShouldNotBeNull();
            elementForPageOne.Arguments.ShouldNotBeEmpty();
            elementForPageTwo.Arguments.ShouldNotBeEmpty();
            elementForPageOne.InvokeAction.ShouldNotBeNull();
            elementForPageTwo.InvokeActionInput.ShouldNotBeNull();

            // Verification...
            PresentationTools.PresentationContainsInput(presentation).ShouldBeTrue();
        }

    }

}
