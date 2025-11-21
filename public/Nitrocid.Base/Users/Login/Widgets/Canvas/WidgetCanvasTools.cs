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

extern alias TextifyDep;

using System.Text;
using Newtonsoft.Json;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Base.Structures;
using Terminaux.Writer.CyclicWriters.Graphical;
using TextifyDep::System.Diagnostics.CodeAnalysis;

namespace Nitrocid.Base.Users.Login.Widgets.Canvas
{
    /// <summary>
    /// Widget canvas tools
    /// </summary>
    public static class WidgetCanvasTools
    {
        /// <summary>
        /// Gets the render information instances from a JSON file
        /// </summary>
        /// <param name="fileName">File name that contains a JSON representation of a list of widgets</param>
        /// <returns>A list of <see cref="WidgetRenderInfo"/></returns>
        public static WidgetRenderInfo[] GetRenderInfosFromFile(string fileName)
        {
            // Load the file
            string fileContent = FilesystemTools.ReadAllTextNoBlock(fileName);
            return GetRenderInfos(fileContent);
        }

        /// <summary>
        /// Gets the render information instances from a JSON representation
        /// </summary>
        /// <param name="renderInfoJson">A JSON representation of a list of widgets</param>
        /// <returns>A list of <see cref="WidgetRenderInfo"/></returns>
        /// <exception cref="KernelException"></exception>
        public static WidgetRenderInfo[] GetRenderInfos([StringSyntax("json")] string renderInfoJson)
        {
            // Load the JSON representation
            // TODO: NKS_USERS_LOGIN_WIDGETS_CANVAS_EXCEPTION_DESERIALIZE -> "Failed to deserialize the widget canvas info"
            WidgetRenderInfo[] renderInfos = JsonConvert.DeserializeObject<WidgetRenderInfo[]?>(renderInfoJson) ??
                throw new KernelException(KernelExceptionType.Widget, LanguageTools.GetLocalized("NKS_USERS_LOGIN_WIDGETS_CANVAS_EXCEPTION_DESERIALIZE"));

            // Return the result
            return renderInfos;
        }

        /// <summary>
        /// Exports the render information instances to a JSON representation
        /// </summary>
        /// <param name="renderInfos">A list of <see cref="WidgetRenderInfo"/></param>
        /// <returns>A JSON representation of a list of widgets</returns>
        public static string ExportRenderInfos(WidgetRenderInfo[] renderInfos)
        {
            string jsonRepresentation = JsonConvert.SerializeObject(renderInfos);
            return jsonRepresentation;
        }

        /// <summary>
        /// Saves the render information instances to a JSON file
        /// </summary>
        /// <param name="renderInfos">A list of <see cref="WidgetRenderInfo"/></param>
        /// <param name="jsonFilePath">Target file path to save to</param>
        /// <returns>A JSON representation of a list of widgets</returns>
        public static void SaveRenderInfos(WidgetRenderInfo[] renderInfos, string jsonFilePath)
        {
            string jsonRepresentation = ExportRenderInfos(renderInfos);
            FilesystemTools.WriteAllTextNoBlock(jsonFilePath, jsonRepresentation);
        }

        /// <summary>
        /// Renders the widgets to a string
        /// </summary>
        /// <param name="renderInfos">A list of <see cref="WidgetRenderInfo"/></param>
        /// <returns>Rendered widgets in a string to print to the console</returns>
        public static string RenderFromInfos(WidgetRenderInfo[] renderInfos)
        {
            var renderBuilder = new StringBuilder();
            foreach (var renderInfo in renderInfos)
            {
                // Get the total width and height
                Coordinate pos = renderInfo.Coordinates;
                Margin margin = renderInfo.Margin;

                // First, determine whether to print the border or not (decreases the widget width and height)
                if (renderInfo.Bordered)
                {
                    // Get the border position
                    int borderLeft = pos.X - 1;
                    int borderTop = pos.Y - 1;

                    // Render the border using the internal size
                    var border = new BoxFrame()
                    {
                        Left = borderLeft,
                        Top = borderTop,
                        Width = margin.Width,
                        Height = margin.Height,
                    };
                    renderBuilder.Append(border.Render());
                }

                // Now, render the widget
                var widget = WidgetTools.GetWidget(renderInfo.WidgetName);
                widget.Options = renderInfo.Options;
                renderBuilder.Append(widget.Initialize(pos.X, pos.Y, margin.Width, margin.Height));
                renderBuilder.Append(widget.Render(pos.X, pos.Y, margin.Width, margin.Height));
            }
            return renderBuilder.ToString();
        }
    }
}
