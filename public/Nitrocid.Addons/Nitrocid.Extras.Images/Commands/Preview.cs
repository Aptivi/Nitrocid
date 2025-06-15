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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using System.Linq;
using System.Text;
using Terminaux.Images;
using Terminaux.Inputs.Styles.Editor;
using Textify.General;

namespace Nitrocid.Extras.Images.Commands
{
    class PreviewCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string path = parameters.ArgumentsList[0];
            if (string.IsNullOrEmpty(path))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_IMAGES_PATHNEEDED"), KernelColorType.Error);
                return 39;
            }
            if (!FilesystemTools.FileExists(path))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_IMAGES_IMAGENOTFOUND"), KernelColorType.Error);
                return 39;
            }

            // Currently, Terminaux.Images doesn't provide a way to get the image's accurate dimensions, so we need to
            // use the colors array to get the dimension so that we can preview the image 100%.
            var colors = ImageProcessor.GetColorsFromImage(path);
            (int width, int height) = (colors.GetLength(0), colors.GetLength(1));

            // Unfortunately, Terminaux.Images assumes that we need to render in the console somewhere by specifying the
            // x and y positions, but this is out of scope because we need an interactive text viewer, and it doesn't
            // deal with such things. Use the colors array to formulate a sequence that describes the whole image in
            // scanlines.
            StringBuilder builder = new();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var color = colors[x, y];
                    builder.Append(color.VTSequenceBackgroundTrueColor + "  ");
                }
                builder.AppendLine();
            }

            // Use the interactive text viewer to describe the image.
            var lines = builder.ToString().SplitNewLines().ToList();
            TextEditInteractive.OpenInteractive(ref lines, edit: false);
            return 0;
        }

    }
}
