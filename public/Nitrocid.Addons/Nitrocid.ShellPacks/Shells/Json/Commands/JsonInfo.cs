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
using Nitrocid.Shell.ShellBase.Commands;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;

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
            SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_TITLE", "Nitrocid.ShellPacks"), KernelColorTools.GetColor(KernelColorType.Separator));
            TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_TYPE", "Nitrocid.ShellPacks"), $"{JsonShellCommon.FileToken.Type}");
            TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_VALUES", "Nitrocid.ShellPacks"), $"{JsonShellCommon.FileToken.HasValues}");
            TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_CHILDRENTOKENS", "Nitrocid.ShellPacks"), $"{JsonShellCommon.FileToken.Count()}");
            TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_BASEPATH", "Nitrocid.ShellPacks"), JsonShellCommon.FileToken.Path);
            TextWriterRaw.Write();

            // Individual properties
            if (!parameters.SwitchesList.Contains("-simplified"))
            {
                foreach (var token in JsonShellCommon.FileToken)
                {
                    SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_INDIVIDUAL_TITLE", "Nitrocid.ShellPacks") + " [{0}]", KernelColorTools.GetColor(KernelColorType.Separator), true, token.Path);
                    TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_INDIVIDUAL_TYPE", "Nitrocid.ShellPacks"), $"{token.Type}");
                    TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_INDIVIDUAL_VALUES", "Nitrocid.ShellPacks"), $"{token.HasValues}");
                    TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_CHILDRENTOKENS", "Nitrocid.ShellPacks"), $"{token.Count()}");
                    TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_INDIVIDUAL_PATH", "Nitrocid.ShellPacks"), token.Path);
                    if (parameters.SwitchesList.Contains("-showvals"))
                        TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_INDIVIDUAL_VALUE", "Nitrocid.ShellPacks"), $"{token}");
                    TextWriterRaw.Write();

                    // Check to see if the token is a property
                    if (token.Type == JTokenType.Property)
                    {
                        var prop = (JProperty)token;
                        SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_PROPERTY_TITLE", "Nitrocid.ShellPacks") + " [{0}]", KernelColorTools.GetColor(KernelColorType.Separator), true, token.Path);
                        TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_PROPERTY_TYPE", "Nitrocid.ShellPacks"), $"{prop.Value.Type}");
                        TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_PROPERTY_COUNT", "Nitrocid.ShellPacks"), $"{prop.Count}");
                        TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_PROPERTY_NAME", "Nitrocid.ShellPacks"), prop.Name);
                        TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_PROPERTY_PATH", "Nitrocid.ShellPacks"), prop.Path);
                        if (parameters.SwitchesList.Contains("-showvals"))
                            TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_JSONINFO_PROPERTY_VALUE", "Nitrocid.ShellPacks"), $"{prop.Value}");
                        TextWriterRaw.Write();
                    }
                }
            }
            return 0;
        }
    }
}
