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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Extras.Dates.Tools;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using System;
using Terminaux.Writer.ConsoleWriters;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool implicitExpiry = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-implicit");
            try
            {
                TimeSpan expirySpan = TimeSpan.Zero;

                // Parse the production date
                if (!DateTimeOffset.TryParse(parameters.ArgumentsList[0], out var production))
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_PRODDATEINVALID"), KernelColorType.Error);
                    return 45;
                }

                // Parse the expiry date or time period
                if (implicitExpiry)
                {
                    if (!TimeSpan.TryParse(parameters.ArgumentsList[1], out expirySpan))
                    {
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_EXPTIMEINVALID"), KernelColorType.Error);
                        return 45;
                    }
                }
                else if (DateTimeOffset.TryParse(parameters.ArgumentsList[1], out var expiryDate))
                    expirySpan = expiryDate - production;
                else
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_EXPDATEINVALID"), KernelColorType.Error);
                    return 45;
                }

                // Make the expiry info instance to print info
                var expiryInfo = new ProductExpiryInfo(production, expirySpan);
                var productHealth = expiryInfo.GetProductHealth();
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_PRODDATE"), $"{expiryInfo.ProductionDate}");
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_EXPDATE"), $"{expiryInfo.ExpiryDate}");
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_EXPTIME"), $"{expiryInfo.ExpirySpan}");
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_PRODHEALTH"), $"{productHealth}");

                // Write the status
                string status =
                    productHealth == -1 ? "Pre-production" :
                    productHealth == 0 ?  "Expired" :
                    productHealth <= 25 ? "Poor" :
                    productHealth <= 50 ? "Average" :
                    productHealth <= 75 ? "Good" : "Excellent";
                KernelColorType statusColor =
                    productHealth == -1 ? KernelColorType.ListValue :
                    productHealth == 0 ? KernelColorType.Error :
                    productHealth <= 25 ? KernelColorType.Error :
                    productHealth <= 50 ? KernelColorType.Warning : KernelColorType.Success;
                TextWriterRaw.Write();
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_PRODSTATUS"), status, KernelColorType.ListEntry, statusColor);
                return 0;
            }
            catch (Exception ex)
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_DATES_EXPIRY_NOEXPIRYINFO") + $": {ex.Message}", KernelColorType.Error);
                return 45;
            }
        }
    }
}
