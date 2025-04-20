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

using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Users.Login.Widgets.Implementations;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Users.Login.Widgets
{
    /// <summary>
    /// Widget tools class
    /// </summary>
    public static class WidgetTools
    {
        private static readonly List<BaseWidget> baseWidgets =
        [
            new AnalogClock(),
            new DigitalClock(),
            new Emoji(),
        ];
        private static readonly List<BaseWidget> customWidgets = [];

        /// <summary>
        /// Adds a widget
        /// </summary>
        /// <param name="widget">Widget instance to add to the custom widget list</param>
        /// <exception cref="KernelException"></exception>
        public static void AddWidget(BaseWidget widget)
        {
            if (widget is null)
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget is not provided."));
            string widgetName = GetWidgetName(widget);
            if (CheckWidget(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget already exists."));
            customWidgets.Add(widget);
        }

        /// <summary>
        /// Removes a widget
        /// </summary>
        /// <param name="widgetName">Widget name to remove</param>
        /// <exception cref="KernelException"></exception>
        public static void RemoveWidget(string widgetName)
        {
            if (string.IsNullOrWhiteSpace(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget name is not provided."));
            if (!CheckWidget(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget doesn't exist."));
            if (IsWidgetBuiltin(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget can't be removed."));
            var widget = GetWidget(widgetName);
            customWidgets.Remove(widget);
        }

        /// <summary>
        /// Gets a widget instance
        /// </summary>
        /// <param name="widgetName">Widget name to query</param>
        /// <returns>Base widget instance</returns>
        /// <exception cref="KernelException"></exception>
        public static BaseWidget GetWidget(string widgetName)
        {
            if (string.IsNullOrWhiteSpace(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget name is not provided."));
            if (!CheckWidget(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget doesn't exist."));
            if (IsWidgetBuiltin(widgetName))
                return baseWidgets.Single((w) => GetWidgetName(w) == widgetName);
            else
                return customWidgets.Single((w) => GetWidgetName(w) == widgetName);
        }

        /// <summary>
        /// Checks to see if a widget is registered or not
        /// </summary>
        /// <param name="widgetName">Widget name to check</param>
        /// <returns>True if the widget is found in all the widget lists; false otherwise.</returns>
        /// <exception cref="KernelException"></exception>
        public static bool CheckWidget(string widgetName)
        {
            if (string.IsNullOrWhiteSpace(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget name is not provided."));
            return
                IsWidgetBuiltin(widgetName) || customWidgets.Any((w) => GetWidgetName(w) == widgetName);
        }

        /// <summary>
        /// Checks to see if a widget is built-in or not
        /// </summary>
        /// <param name="widgetName">Widget name to check</param>
        /// <returns>True if the widget is found in the base widget list defined by Nitrocid; false otherwise, even if it exists in the custom widget list.</returns>
        /// <exception cref="KernelException"></exception>
        public static bool IsWidgetBuiltin(string widgetName)
        {
            if (string.IsNullOrWhiteSpace(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget name is not provided."));
            return baseWidgets.Any((w) => GetWidgetName(w) == widgetName);
        }

        /// <summary>
        /// Gets the widget class name
        /// </summary>
        /// <param name="widget">A widget instance to query</param>
        /// <returns>A widget class name related to this widget instance</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetWidgetName(BaseWidget widget)
        {
            if (widget is null)
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget is not provided."));
            return widget.GetType().Name;
        }

        /// <summary>
        /// Renders a widget
        /// </summary>
        /// <param name="widgetName">A widget instance to render</param>
        /// <exception cref="KernelException"></exception>
        public static void RenderWidget(string widgetName)
        {
            if (string.IsNullOrWhiteSpace(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget name is not provided."));
            if (!CheckWidget(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget doesn't exist."));
            var widget = GetWidget(widgetName);
            string widgetSeq = widget.Render();
            TextWriterRaw.WriteRaw(widgetSeq);
        }

        /// <summary>
        /// Renders a widget
        /// </summary>
        /// <param name="widgetName">A widget instance to render</param>
        /// <param name="left">Left position of the widget</param>
        /// <param name="top">Top position of the widget</param>
        /// <param name="width">Width of a widget</param>
        /// <param name="height">Height of a widget</param>
        /// <exception cref="KernelException"></exception>
        public static void RenderWidget(string widgetName, int left, int top, int width, int height)
        {
            if (string.IsNullOrWhiteSpace(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget name is not provided."));
            if (!CheckWidget(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget doesn't exist."));
            var widget = GetWidget(widgetName);
            string widgetSeq = widget.Render(left, top, width, height);
            TextWriterRaw.WriteRaw(widgetSeq);
        }

        /// <summary>
        /// Initializes a widget
        /// </summary>
        /// <param name="widgetName">A widget instance to initialize</param>
        /// <exception cref="KernelException"></exception>
        public static void InitializeWidget(string widgetName)
        {
            if (string.IsNullOrWhiteSpace(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget name is not provided."));
            if (!CheckWidget(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget doesn't exist."));
            var widget = GetWidget(widgetName);
            string widgetSeq = widget.Initialize();
            TextWriterRaw.WriteRaw(widgetSeq);
        }

        /// <summary>
        /// Initializes a widget
        /// </summary>
        /// <param name="widgetName">A widget instance to initialize</param>
        /// <param name="left">Left position of the widget</param>
        /// <param name="top">Top position of the widget</param>
        /// <param name="width">Width of a widget</param>
        /// <param name="height">Height of a widget</param>
        /// <exception cref="KernelException"></exception>
        public static void InitializeWidget(string widgetName, int left, int top, int width, int height)
        {
            if (string.IsNullOrWhiteSpace(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget name is not provided."));
            if (!CheckWidget(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget doesn't exist."));
            var widget = GetWidget(widgetName);
            string widgetSeq = widget.Initialize(left, top, width, height);
            TextWriterRaw.WriteRaw(widgetSeq);
        }

        /// <summary>
        /// Cleans a widget up
        /// </summary>
        /// <param name="widgetName">A widget instance to cleanup</param>
        /// <exception cref="KernelException"></exception>
        public static void CleanupWidget(string widgetName)
        {
            if (string.IsNullOrWhiteSpace(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget name is not provided."));
            if (!CheckWidget(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget doesn't exist."));
            var widget = GetWidget(widgetName);
            string widgetSeq = widget.Cleanup();
            TextWriterRaw.WriteRaw(widgetSeq);
        }

        /// <summary>
        /// Cleans a widget up
        /// </summary>
        /// <param name="widgetName">A widget instance to cleanup</param>
        /// <param name="left">Left position of the widget</param>
        /// <param name="top">Top position of the widget</param>
        /// <param name="width">Width of a widget</param>
        /// <param name="height">Height of a widget</param>
        /// <exception cref="KernelException"></exception>
        public static void CleanupWidget(string widgetName, int left, int top, int width, int height)
        {
            if (string.IsNullOrWhiteSpace(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget name is not provided."));
            if (!CheckWidget(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget doesn't exist."));
            var widget = GetWidget(widgetName);
            string widgetSeq = widget.Cleanup(left, top, width, height);
            TextWriterRaw.WriteRaw(widgetSeq);
        }

        /// <summary>
        /// Gets widget names
        /// </summary>
        /// <returns>An array containing base and custom widget class names</returns>
        public static string[] GetWidgetNames()
        {
            var baseNames = baseWidgets.Select(GetWidgetName).ToArray();
            var customNames = customWidgets.Select(GetWidgetName).ToArray();
            return [.. baseNames, .. customNames];
        }

        internal static void AddBaseWidget(BaseWidget widget)
        {
            if (widget is null)
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget is not provided."));
            string widgetName = GetWidgetName(widget);
            if (CheckWidget(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget already exists."));
            baseWidgets.Add(widget);
        }

        internal static void RemoveBaseWidget(string widgetName)
        {
            if (string.IsNullOrWhiteSpace(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget name is not provided."));
            if (!CheckWidget(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget doesn't exist."));
            if (!IsWidgetBuiltin(widgetName))
                throw new KernelException(KernelExceptionType.Widget, Translate.DoTranslation("Widget is not a built-in widget."));
            var widget = GetWidget(widgetName);
            baseWidgets.Remove(widget);
        }
    }
}
