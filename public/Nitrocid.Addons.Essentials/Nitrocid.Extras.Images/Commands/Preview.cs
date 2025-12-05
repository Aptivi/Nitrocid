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

using System.Linq;
using System.Text;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files;
using Nitrocid.Languages;
using Terminaux.Images;
using Terminaux.Images.Interactives;
using Terminaux.Inputs.Styles.Editor;
using Terminaux.Shell.Commands;
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
                TextWriters.Write(Translate.DoTranslation("Path to the image is not provided."), KernelColorType.Error);
                return 39;
            }
            if (!FilesystemTools.FileExists(path))
            {
                TextWriters.Write(Translate.DoTranslation("Image file doesn't exist."), KernelColorType.Error);
                return 39;
            }

            // Open the image viewer TUI
            var magickImage = ImageProcessor.OpenImage(path);
            ImageViewInteractive.OpenInteractive(magickImage);
            return 0;
        }

    }
}
