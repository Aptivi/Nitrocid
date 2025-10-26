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

using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Base.Kernel.Extensions;
using System.Linq;
using Nitrocid.Extras.Images.Commands;
using Nitrocid.Core.Languages;

namespace Nitrocid.Extras.Images
{
    internal class ImagesInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("preview", /* Localizable */ "NKS_IMAGES_COMMAND_PREVIEW_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "imageFile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_IMAGES_COMMAND_PREVIEW_ARGUMENT_IMAGEFILE_DESC"
                        }),
                    ])
                ], new PreviewCommand()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasImages);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasImages);

        public void FinalizeAddon()
        { }

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Images.Resources.Languages.Output.Localizations", typeof(ImagesInit).Assembly));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
        }
    }
}
