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
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Kernel.Starting;

namespace Nitrocid.Extras.Tips
{
    internal class TipsInit : IAddon
    {
        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasTips);

        internal static TipsConfig TipsConfig =>
            (TipsConfig)Config.baseConfigurations[nameof(TipsConfig)];

        void IAddon.FinalizeAddon() =>
            WelcomeMessage.tips = TipsList.tips;

        void IAddon.StartAddon()
        {
            var config = new TipsConfig();
            ConfigTools.RegisterBaseSetting(config);
            WelcomeMessage.ShowTip = TipsConfig.ShowTip;
        }

        void IAddon.StopAddon()
        {
            WelcomeMessage.tips = [];
            ConfigTools.UnregisterBaseSetting(nameof(TipsConfig));
        }
    }
}
