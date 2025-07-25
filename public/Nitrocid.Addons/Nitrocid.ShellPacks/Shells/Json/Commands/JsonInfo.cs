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

using Newtonsoft.Json.Linq;
using System.Linq;
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
using Terminaux.Colors.Themes.Colors;

namespace Nitrocid.ShellPacks.Shells.Json.Commands
{
    /// <summary>
    /// Gets information about the JSON file and its contents
    /// </summary>
    class JsonInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Base info
            SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_TITLE"), ThemeColorsTools.GetColor(ThemeColorType.Separator));
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_TYPE"), $"{JsonShellCommon.FileToken.Type}");
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_VALUES"), $"{JsonShellCommon.FileToken.HasValues}");
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_CHILDRENTOKENS"), $"{JsonShellCommon.FileToken.Count()}");
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_BASEPATH"), JsonShellCommon.FileToken.Path);
            TextWriterRaw.Write();

            // Individual properties
            if (!parameters.SwitchesList.Contains("-simplified"))
            {
                foreach (var token in JsonShellCommon.FileToken)
                {
                    SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_INDIVIDUAL_TITLE") + " [{0}]", ThemeColorsTools.GetColor(ThemeColorType.Separator), true, token.Path);
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_INDIVIDUAL_TYPE"), $"{token.Type}");
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_INDIVIDUAL_VALUES"), $"{token.HasValues}");
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_CHILDRENTOKENS"), $"{token.Count()}");
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_INDIVIDUAL_PATH"), token.Path);
                    if (parameters.SwitchesList.Contains("-showvals"))
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_INDIVIDUAL_VALUE"), $"{token}");
                    TextWriterRaw.Write();

                    // Check to see if the token is a property
                    if (token.Type == JTokenType.Property)
                    {
                        var prop = (JProperty)token;
                        SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_PROPERTY_TITLE") + " [{0}]", ThemeColorsTools.GetColor(ThemeColorType.Separator), true, token.Path);
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_PROPERTY_TYPE"), $"{prop.Value.Type}");
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_PROPERTY_COUNT"), $"{prop.Count}");
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_PROPERTY_NAME"), prop.Name);
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_PROPERTY_PATH"), prop.Path);
                        if (parameters.SwitchesList.Contains("-showvals"))
                            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_PROPERTY_VALUE"), $"{prop.Value}");
                        TextWriterRaw.Write();
                    }
                }
            }
            return 0;
        }
    }
}
