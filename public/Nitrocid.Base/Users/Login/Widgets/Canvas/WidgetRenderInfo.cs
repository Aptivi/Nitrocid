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

using System.Collections.Generic;
using Newtonsoft.Json;
using Nitrocid.Base.Users.Login.Widgets.Implementations;
using Terminaux.Base;
using Terminaux.Base.Structures;

namespace Nitrocid.Base.Users.Login.Widgets.Canvas
{
    /// <summary>
    /// Widget render info
    /// </summary>
    public class WidgetRenderInfo
    {
        [JsonProperty(Required = Required.Always)]
        private readonly string widgetName = nameof(UnknownWidget);
        [JsonProperty]
        private readonly bool bordered = false;
        [JsonProperty]
        private readonly bool relative = false;
        [JsonProperty(Required = Required.Always)]
        private readonly int left = 0;
        [JsonProperty(Required = Required.Always)]
        private readonly int top = 0;
        [JsonProperty]
        private readonly int width = ConsoleWrapper.WindowWidth;
        [JsonProperty]
        private readonly int height = ConsoleWrapper.WindowHeight;
        [JsonProperty]
        private readonly int widthLeftMargin = 0;
        [JsonProperty]
        private readonly int widthRightMargin = 0;
        [JsonProperty]
        private readonly int heightTopMargin = 0;
        [JsonProperty]
        private readonly int heightBottomMargin = 0;
        [JsonProperty]
        private readonly Dictionary<string, object> options = [];

        /// <summary>
        /// A widget
        /// </summary>
        [JsonIgnore]
        public string WidgetName =>
            WidgetTools.CheckWidget(widgetName) ? widgetName : nameof(UnknownWidget);

        /// <summary>
        /// Whether to draw a border around the widget
        /// </summary>
        [JsonIgnore]
        public bool Bordered =>
            bordered;

        /// <summary>
        /// Whether the widget has relative sizes (margins subtract from console size) or not
        /// </summary>
        [JsonIgnore]
        public bool Relative =>
            relative;

        /// <summary>
        /// Left and top position of the widget
        /// </summary>
        [JsonIgnore]
        public Coordinate Coordinates =>
            new(Normalize(left, true), Normalize(top, true));

        /// <summary>
        /// Width and height of the widget
        /// </summary>
        [JsonIgnore]
        public Size Size =>
            Relative ? new(Normalize(ConsoleWrapper.WindowWidth), Normalize(ConsoleWrapper.WindowHeight)) : new(Normalize(width), Normalize(height));

        /// <summary>
        /// Margin of width and height of the widget
        /// </summary>
        [JsonIgnore]
        public Margin Margin =>
            new(widthLeftMargin, heightTopMargin, widthRightMargin, heightBottomMargin, Size.Width, Size.Height);

        /// <summary>
        /// Options of the widget
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, object> Options =>
            options;

        private int Normalize(int value, bool increase = false)
        {
            int normalizedValue = increase ? value + (Bordered ? 1 : 0) : value - (Bordered ? 1 : 0);
            return normalizedValue < 0 ? 0 : normalizedValue;
        }

        [JsonConstructor]
        internal WidgetRenderInfo()
        { }
    }
}
