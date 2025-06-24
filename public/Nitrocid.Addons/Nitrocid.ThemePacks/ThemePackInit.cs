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

using Terminaux.Colors.Themes;
using Newtonsoft.Json.Linq;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Base.Misc.Reflection.Internal;
using Textify.General;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.ThemePacks.Localized;

namespace Nitrocid.ThemePacks
{
    internal class ThemePackInit : IAddon
    {
        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.AddonThemePacks);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.AddonThemePacks);

        public ModLoadPriority AddonType => ModLoadPriority.Optional;

        public void StartAddon()
        {
            // Add them all!
            LanguageTools.AddCustomAction(AddonName, new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            string[] themeResNames = ResourcesManager.GetResourceNames(typeof(ThemePackInit).Assembly);
            foreach (string resource in themeResNames)
            {
                string key = resource.RemovePrefix("Themes.");
                string themeName = key.RemoveSuffix(".json");
                string data = ResourcesManager.ConvertToString(ResourcesManager.GetData(key, ResourcesType.Themes, typeof(ThemePackInit).Assembly) ??
                    throw new KernelException(KernelExceptionType.Reflection, LanguageTools.GetLocalized("NKS_THEMEPACKS_EXCEPTION_NODATA")));
                var themeToken = JToken.Parse(data);
                ThemeTools.RegisterTheme(themeName, new ThemeInfo(themeToken));
                DebugWriter.WriteDebug(DebugLevel.I, "Added {0}", vars: [themeName]);
            }
        }

        public void StopAddon()
        {
            // Remove them all!
            LanguageTools.RemoveCustomAction(AddonName);
            string[] themeResNames = ResourcesManager.GetResourceNames(typeof(ThemePackInit).Assembly);
            foreach (string resource in themeResNames)
            {
                string key = resource.RemovePrefix("Themes.");
                string themeName = key.RemoveSuffix(".json");
                ThemeTools.UnregisterTheme(themeName);
                DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}", vars: [themeName]);
            }
        }

        public void FinalizeAddon()
        { }
    }
}
