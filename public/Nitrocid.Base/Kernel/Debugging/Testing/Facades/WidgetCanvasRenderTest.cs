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

using Nitrocid.Base.Languages;
using Nitrocid.Base.Users.Login.Widgets.Canvas;
using Terminaux.Base;
using Terminaux.Inputs;
using Terminaux.Shell.Arguments;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Kernel.Debugging.Testing.Facades
{
    internal class WidgetCanvasRenderTest : TestFacade
    {
        // TODO: NKS_KERNEL_DEBUGGING_TESTFACADES_WIDGETCANVASRENDERTEST_DESC -> Tests the widget canvas renderer
        public override string TestName => LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_WIDGETCANVASRENDERTEST_DESC");
        public override TestSection TestSection => TestSection.Misc;
        public override void Run()
        {
            string widgetList = /*lang=json*/
                """
                [
                    {
                        "widgetName": "AnalogClock",
                        "bordered": true,
                        "left": 3,
                        "top": 1,
                        "width": 30,
                        "height": 15
                    },
                    {
                        "widgetName": "TextWidget",
                        "left": 3,
                        "top": 1,
                        "options": {
                            "text": "✅"
                        }
                    },
                    {
                        "widgetName": "TextWidget",
                        "left": 32,
                        "top": 1,
                        "options": {
                            "text": "✅"
                        }
                    },
                    {
                        "widgetName": "TextWidget",
                        "left": 3,
                        "top": 16,
                        "options": {
                            "text": "✅"
                        }
                    },
                    {
                        "widgetName": "TextWidget",
                        "left": 32,
                        "top": 16,
                        "options": {
                            "text": "✅"
                        }
                    }
                ]
                """;
            var widgetRenderInfos = WidgetCanvasTools.GetRenderInfos(widgetList);
            string renderedWidgets = WidgetCanvasTools.RenderFromInfos(widgetRenderInfos);
            ConsoleWrapper.Clear();
            TextWriterRaw.WriteRaw(renderedWidgets);
            Input.ReadKey();
        }
    }
}
