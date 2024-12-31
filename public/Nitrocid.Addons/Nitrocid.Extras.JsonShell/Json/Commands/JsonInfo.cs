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
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Terminaux.Writer.CyclicWriters;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Extras.JsonShell.Json.Commands
{
    /// <summary>
    /// Gets information about the JSON file and its contents
    /// </summary>
    class JsonInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Base info
            SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Base JSON token information"), KernelColorTools.GetColor(KernelColorType.Separator));
            WriteEntry(Translate.DoTranslation("Base type"), $"{JsonShellCommon.FileToken.Type}");
            WriteEntry(Translate.DoTranslation("Base has values"), $"{JsonShellCommon.FileToken.HasValues}");
            WriteEntry(Translate.DoTranslation("Children token count"), $"{JsonShellCommon.FileToken.Count()}");
            WriteEntry(Translate.DoTranslation("Base path"), JsonShellCommon.FileToken.Path);
            TextWriterRaw.Write();

            // Individual properties
            if (!parameters.SwitchesList.Contains("-simplified"))
            {
                foreach (var token in JsonShellCommon.FileToken)
                {
                    SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Individual JSON token information") + " [{0}]", KernelColorTools.GetColor(KernelColorType.Separator), true, token.Path);
                    WriteEntry(Translate.DoTranslation("Token type"), $"{token.Type}");
                    WriteEntry(Translate.DoTranslation("Token has values"), $"{token.HasValues}");
                    WriteEntry(Translate.DoTranslation("Children token count"), $"{token.Count()}");
                    WriteEntry(Translate.DoTranslation("Token path"), token.Path);
                    if (parameters.SwitchesList.Contains("-showvals"))
                        WriteEntry(Translate.DoTranslation("Token value"), $"{token}");
                    TextWriterRaw.Write();

                    // Check to see if the token is a property
                    if (token.Type == JTokenType.Property)
                    {
                        var prop = (JProperty)token;
                        SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Property information for") + " [{0}]", KernelColorTools.GetColor(KernelColorType.Separator), true, token.Path);
                        WriteEntry(Translate.DoTranslation("Property type"), $"{prop.Value.Type}");
                        WriteEntry(Translate.DoTranslation("Property count"), $"{prop.Count}");
                        WriteEntry(Translate.DoTranslation("Property name"), prop.Name);
                        WriteEntry(Translate.DoTranslation("Property path"), prop.Path);
                        if (parameters.SwitchesList.Contains("-showvals"))
                            WriteEntry(Translate.DoTranslation("Property value"), $"{prop.Value}");
                        TextWriterRaw.Write();
                    }
                }
            }
            return 0;
        }

        private static void WriteEntry(string entry, string value)
        {
            var listEntry = new ListEntry()
            {
                Entry = entry,
                Value = value,
                KeyColor = KernelColorTools.GetColor(KernelColorType.ListEntry),
                ValueColor = KernelColorTools.GetColor(KernelColorType.ListValue),
            };
            TextWriterRaw.WritePlain(listEntry.Render());
        }
    }
}
