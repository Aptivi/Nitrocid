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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files;
using Nitrocid.Languages;
using Terminaux.Images;
using Terminaux.Images.Interactives;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;

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
                        ArgumentDescription = LanguageTools.GetLocalized("NKS_IMAGES_COMMAND_PREVIEW_ARGUMENT_IMAGEFILE_DESC")
                    }),
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
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

            // Open the image viewer TUI
            var magickImage = ImageProcessor.OpenImage(path);
            ImageViewInteractive.OpenInteractive(magickImage);
            return 0;
        }

    }
}
