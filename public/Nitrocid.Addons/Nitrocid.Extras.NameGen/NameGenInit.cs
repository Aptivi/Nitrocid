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
using Terminaux.Shell.Switches;
using Nitrocid.Extras.NameGen.Localized;
using Nitrocid.Extras.NameGen.Screensavers;
using Nitrocid.Extras.NameGen.Settings;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Base.Misc.Screensaver;
using System.Linq;
using Nitrocid.Base.Languages;

namespace Nitrocid.Extras.NameGen
{
    internal class NameGenInit : IAddon
    {
        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasNameGen);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasNameGen);

        internal static NameGenSaversConfig SaversConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(NameGenSaversConfig)) ? (NameGenSaversConfig)Config.baseConfigurations[nameof(NameGenSaversConfig)] : Config.GetFallbackKernelConfig<NameGenSaversConfig>();

        public void StartAddon()
        {
            // Initialize everything
            LanguageTools.AddCustomAction(AddonName, new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            ScreensaverManager.AddonSavers.Add("personlookup", new PersonLookupDisplay());

            // Then, initialize configuration in a way that no mod can play with them
            var saversConfig = new NameGenSaversConfig();
            ConfigTools.RegisterBaseSetting(saversConfig);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            ScreensaverManager.AddonSavers.Remove("personlookup");
            ConfigTools.UnregisterBaseSetting(nameof(NameGenSaversConfig));
        }

        public void FinalizeAddon()
        { }
    }
}
