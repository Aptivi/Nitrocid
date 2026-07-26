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

using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Core.Languages;
using Nitrocid.Extras.Images.Icons.Tools;

namespace Nitrocid.Extras.Images.Icons
{
    internal class IconsInit : IAddon
    {
        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasImagesIcons);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasImagesIcons);

        public void StartAddon()
        {
            // Verify that all icons load successfully
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Images.Icons.Resources.Languages.Output.Localizations", typeof(IconsInit).Assembly));
            var iconNames = IconsTools.GetIconNames();
            DebugWriter.WriteDebug(DebugLevel.I, $"Icons are {iconNames.Length} [{string.Join(", ", iconNames)}]");
            if (iconNames.Length == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Icons don't exist in distribution, icon tools will fail");
                throw new KernelException(KernelExceptionType.AssertionFailure, LanguageTools.GetLocalized("NKS_IMAGES_ICONS_EXCEPTION_NOICONS"));
            }
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
        }
    }
}
