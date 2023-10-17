
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Writers.MiscWriters;
using KS.Kernel.Configuration;
using KS.Kernel.Extensions;
using Nitrocid.Extras.Tips.Settings;
using System;

namespace Nitrocid.Extras.Tips
{
    internal class TipsInit : IAddon
    {
        string IAddon.AddonName => "Extras - Kernel Tips";

        AddonType IAddon.AddonType => AddonType.Optional;

        internal static TipsConfig TipsConfig =>
            (TipsConfig)Config.baseConfigurations[nameof(TipsConfig)];

        void IAddon.FinalizeAddon() =>
            WelcomeMessage.tips = TipsList.tips;

        void IAddon.StartAddon()
        {
            var config = new TipsConfig();
            ConfigTools.RegisterBaseSetting(config);
            KernelFlags.ShowTip = TipsConfig.ShowTip;
        }

        void IAddon.StopAddon()
        {
            WelcomeMessage.tips = Array.Empty<string>();
            ConfigTools.UnregisterBaseSetting(nameof(TipsConfig));
        }
    }
}
