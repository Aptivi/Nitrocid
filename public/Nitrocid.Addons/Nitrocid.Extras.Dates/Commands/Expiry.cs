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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.Dates.Tools;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using System;
using Terminaux.Shell.Shells;

namespace Nitrocid.Extras.Dates.Commands
{
    /// <summary>
    /// Product expiry info
    /// </summary>
    /// <remarks>
    /// If you want to know whether your product is expired or not, you can do so using this command.
    /// </remarks>
    class ExpiryCommand : BaseCommand, ICommand
    {

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            bool implicitExpiry = parameters.ContainsSwitch("-implicit");
            string productionDateStr = parameters.ArgumentsList[0];
            string expiryDateStr = parameters.ArgumentsList[1];
            try
            {
                TimeSpan expirySpan = TimeSpan.Zero;

                // Parse the production date
                if (!DateTimeOffset.TryParse(productionDateStr, out var production))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_PRODDATEINVALID"), ThemeColorType.Error);
                    return 45;
                }

                // Parse the expiry date or time period
                if (implicitExpiry)
                {
                    if (!TimeSpan.TryParse(expiryDateStr, out expirySpan))
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_EXPTIMEINVALID"), ThemeColorType.Error);
                        return 45;
                    }
                }
                else if (DateTimeOffset.TryParse(expiryDateStr, out var expiryDate))
                    expirySpan = expiryDate - production;
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_EXPDATEINVALID"), ThemeColorType.Error);
                    return 45;
                }

                // Make the expiry info instance to print info
                var expiryInfo = new ProductExpiryInfo(production, expirySpan);
                var productHealth = expiryInfo.GetProductHealth();
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_PRODDATE"), $"{expiryInfo.ProductionDate}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_EXPDATE"), $"{expiryInfo.ExpiryDate}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_EXPTIME"), $"{expiryInfo.ExpirySpan}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_PRODHEALTH"), $"{productHealth}");

                // Write the status
                // TODO: NKS_DATES_EXPIRY_STATUS_PREPRODUCTION -> Pre-production
                // TODO: NKS_DATES_EXPIRY_STATUS_EXPIRED -> Expired
                // TODO: NKS_DATES_EXPIRY_STATUS_POOR -> Poor
                // TODO: NKS_DATES_EXPIRY_STATUS_AVERAGE -> Average
                // TODO: NKS_DATES_EXPIRY_STATUS_GOOD -> Good
                // TODO: NKS_DATES_EXPIRY_STATUS_EXCELLENT -> Excellent
                string status =
                    productHealth == -1 ? LanguageTools.GetLocalized("NKS_DATES_EXPIRY_STATUS_PREPRODUCTION") :
                    productHealth == 0 ? LanguageTools.GetLocalized("NKS_DATES_EXPIRY_STATUS_EXPIRED") :
                    productHealth <= 25 ? LanguageTools.GetLocalized("NKS_DATES_EXPIRY_STATUS_POOR") :
                    productHealth <= 50 ? LanguageTools.GetLocalized("NKS_DATES_EXPIRY_STATUS_AVERAGE") :
                    productHealth <= 75 ? LanguageTools.GetLocalized("NKS_DATES_EXPIRY_STATUS_GOOD") :
                    LanguageTools.GetLocalized("NKS_DATES_EXPIRY_STATUS_EXCELLENT");
                ThemeColorType statusColor =
                    productHealth == -1 ? ThemeColorType.ListValue :
                    productHealth == 0 ? ThemeColorType.Error :
                    productHealth <= 25 ? ThemeColorType.Error :
                    productHealth <= 50 ? ThemeColorType.Warning : ThemeColorType.Success;
                TextWriterRaw.Write();
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_PRODSTATUS"), status, ThemeColorType.ListEntry, statusColor);
                return 0;
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_NOEXPIRYINFO") + $": {ex.Message}", ThemeColorType.Error);
                return 45;
            }
        }
    }
}
