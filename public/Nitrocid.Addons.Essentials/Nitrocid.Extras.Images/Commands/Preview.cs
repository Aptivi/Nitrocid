//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Base.Files;
using Nitrocid.Base.Languages;
using Terminaux.Images;
using Terminaux.Images.Interactives;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.Images.Commands
{
    class PreviewCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "preview";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_IMAGES_COMMAND_PREVIEW_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "imageFile", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_IMAGES_COMMAND_PREVIEW_ARGUMENT_IMAGEFILE_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string path = parameters.ArgumentsList[0];
            if (string.IsNullOrEmpty(path))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_IMAGES_PATHNEEDED"), ThemeColorType.Error);
                return 39;
            }
            if (!FilesystemTools.FileExists(path))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_IMAGES_IMAGENOTFOUND"), ThemeColorType.Error);
                return 39;
            }

            // Open the image viewer TUI
            var magickImage = ImageProcessor.OpenImage(path);
            ImageViewInteractive.OpenInteractive(magickImage);
            return 0;
        }

    }
}
