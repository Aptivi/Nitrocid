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

using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.Base.Users.Login.Widgets.Implementations
{
    internal class EmojiChar : BaseWidget, IWidget
    {
        public override string Cleanup(int left, int top, int width, int height) =>
            "";

        public override string Initialize(int left, int top, int width, int height) =>
            "";

        public override string Render(int left, int top, int width, int height)
        {
            // TODO: Let this widget choose random emoji instead of a happy face, as this is a test
            var emojiAlignedText = new BoundedText()
            {
                Left = left,
                Top = top,
                Width = width,
                Height = height,
                Text = "😊",
            };
            return emojiAlignedText.Render();
        }
    }
}
