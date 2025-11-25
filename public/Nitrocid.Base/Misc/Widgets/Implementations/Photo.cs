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

using System.IO;
using Nitrocid.Base.Files;
using Nitrocid.Base.Misc.Reflection.Internal;
using Terminaux.Images;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.Base.Misc.Widgets.Implementations
{
    internal class Photo : BaseWidget, IWidget
    {
        public override string Cleanup(int left, int top, int width, int height) =>
            "";

        public override string Initialize(int left, int top, int width, int height) =>
            "";

        public override string Render(int left, int top, int width, int height)
        {
            string photoPath = Options.TryGetValue("photoPath", out object? photoPathValue) ? (string)photoPathValue : "";
            if (string.IsNullOrEmpty(photoPath) || !FilesystemTools.FileExists(photoPath))
            {
                // Get the Nitrocid logo
                var resourceLogo = ResourcesManager.GetData("Misc.Widgets.Resources.Photo.Placeholder", ResourcesType.Misc);
                if (resourceLogo is Stream resourceStream)
                    return ImageProcessor.RenderImage(resourceStream, width, height, left, top);

                // Else, return the Aptivi logo
                return ImageProcessor.RenderImage(width, height, left, top);
            }
            else
                return ImageProcessor.RenderImage(photoPath, width, height, left, top);
        }
    }
}
