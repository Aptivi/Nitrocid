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

using System;
using System.Data;
using System.Linq;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using UnitsNet;

namespace Nitrocid.Extras.UnitConv.Commands
{
    /// <summary>
    /// Lists all units
    /// </summary>
    /// <remarks>
    /// If you don't know what units are there, you can use this command. If you don't know what unit types are there, use its help entry.
    /// </remarks>
    class ListUnitsCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "listunits";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_UNITCONV_COMMAND_LISTUNITS_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (_) => [.. Quantity.Infos.Select((src) => src.Name)],
                        ArgumentDescription = /* Localizable */ "NKS_UNITCONV_COMMAND_ARGUMENT_UNITTYPE_DESC"
                    }),
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var abbreviations = UnitsNetSetup.Default.UnitAbbreviations;
            string type = parameters.ArgumentsList[0];
            var quantities = Quantity.Infos.Where(x => x.Name == type);
            if (quantities.Any())
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_UNITCONV_LISTUNITS_AVAILABLETYPESUNITS"));
                foreach (QuantityInfo QuantityInfo in quantities)
                {
                    SeparatorWriterColor.WriteSeparator(QuantityInfo.Name, true);
                    foreach (Enum UnitValues in QuantityInfo.UnitInfos.Select(x => x.Value))
                    {
                        string abbreviationsStr = string.Join(", ", abbreviations.GetDefaultAbbreviation(UnitValues.GetType(), (int)(object)UnitValues));
                        ListEntryWriterColor.WriteListEntry(abbreviationsStr, UnitValues.ToString());
                    }
                }
                return 0;
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_UNITCONV_LISTUNITS_NOUNITTYPE") + " {0}", true, ThemeColorType.Error, type);
                return 3;
            }
        }

        public override void HelpHelper(IShell? shell)
        {
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_UNITCONV_LISTUNITS_AVAILABLETYPES"));
            foreach (QuantityInfo QuantityInfo in Quantity.Infos)
                TextWriterColor.Write("- {0}", true, ThemeColorType.ListEntry, QuantityInfo.Name);
        }

    }
}
