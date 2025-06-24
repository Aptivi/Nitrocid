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
using Nitrocid.Extras.Stocks.Settings;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Extras.Stocks.Commands;
using Nitrocid.Extras.Stocks.Localized;
using Nitrocid.Base.Users.Login.Widgets;
using Nitrocid.Extras.Stocks.Widgets;
using Nitrocid.Base.Languages;

namespace Nitrocid.Extras.Stocks
{
    internal class StocksInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("stock", /* Localizable */ "NKS_STOCKS_COMMAND_STOCK_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "company", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_STOCKS_COMMAND_STOCK_ARGUMENT_COMPANY_DESC"
                        }),
                    ])
                ], new StockCommand()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasStocks);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasStocks);

        public ModLoadPriority AddonType => ModLoadPriority.Optional;

        internal static StocksConfig StocksConfig =>
            (StocksConfig)Config.baseConfigurations[nameof(StocksConfig)];

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            var config = new StocksConfig();
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
            ConfigTools.RegisterBaseSetting(config);
            WidgetTools.AddBaseWidget(new StocksWidget());
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(StocksConfig));
            WidgetTools.RemoveBaseWidget(nameof(StocksWidget));
        }

        public void FinalizeAddon()
        { }
    }
}
