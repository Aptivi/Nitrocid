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

using Nitrocid.Extras.Tips.Settings;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Base.Kernel.Starting;
using Nitrocid.Core.Languages;

namespace Nitrocid.Extras.Tips
{
    internal class TipsInit : IAddon
    {
        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasTips);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasTips);

        internal static TipsConfig TipsConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(TipsConfig)) ? (TipsConfig)Config.baseConfigurations[nameof(TipsConfig)] : Config.GetFallbackKernelConfig<TipsConfig>();

        public void FinalizeAddon() =>
            WelcomeMessage.tips = TipsList.tips;

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Tips.Resources.Languages.Output.Localizations", typeof(TipsInit).Assembly));
            var config = new TipsConfig();
            ConfigTools.RegisterBaseSetting(config);
            WelcomeMessage.ShowTip = TipsConfig.ShowTip;
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            WelcomeMessage.tips = [];
            ConfigTools.UnregisterBaseSetting(nameof(TipsConfig));
        }
    }
}
