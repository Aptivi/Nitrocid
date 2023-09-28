
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

using KS.Languages;
using KS.Shell.ShellBase.Commands;
using Newtonsoft.Json.Linq;
using System.Linq;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;

namespace KS.Shell.Shells.Json.Commands
{
    /// <summary>
    /// Gets information about the JSON file and its contents
    /// </summary>
    class JsonShell_JsonInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Base info
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Base JSON token information"), true);
            TextWriterColor.Write(Translate.DoTranslation("Base type") + ": {0}", JsonShellCommon.JsonShell_FileToken.Type);
            TextWriterColor.Write(Translate.DoTranslation("Base has values") + ": {0}", true, false, JsonShellCommon.JsonShell_FileToken.HasValues);
            TextWriterColor.Write(Translate.DoTranslation("Children token count") + ": {0}", JsonShellCommon.JsonShell_FileToken.Count());
            TextWriterColor.Write(Translate.DoTranslation("Base path") + ": {0}", JsonShellCommon.JsonShell_FileToken.Path);
            TextWriterColor.Write();

            // Individual properties
            if (!parameters.SwitchesList.Contains("-simplified"))
            {
                foreach (var token in JsonShellCommon.JsonShell_FileToken)
                {
                    SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Individual JSON token information") + " [{0}]", true, token.Path);
                    TextWriterColor.Write(Translate.DoTranslation("Token type") + ": {0}", token.Type);
                    TextWriterColor.Write(Translate.DoTranslation("Token has values") + ": {0}", true, false, token.HasValues);
                    TextWriterColor.Write(Translate.DoTranslation("Children token count") + ": {0}", token.Count());
                    TextWriterColor.Write(Translate.DoTranslation("Token path") + ": {0}", token.Path);
                    if (parameters.SwitchesList.Contains("-showvals"))
                        TextWriterColor.Write(Translate.DoTranslation("Token value") + ": {0}", token);
                    TextWriterColor.Write();

                    // Check to see if the token is a property
                    if (token.Type == JTokenType.Property)
                    {
                        SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Property information for") + " [{0}]", true, token.Path);
                        TextWriterColor.Write(Translate.DoTranslation("Property type") + ": {0}", ((JProperty)token).Value.Type);
                        TextWriterColor.Write(Translate.DoTranslation("Property count") + ": {0}", ((JProperty)token).Count);
                        TextWriterColor.Write(Translate.DoTranslation("Property name") + ": {0}", ((JProperty)token).Name);
                        TextWriterColor.Write(Translate.DoTranslation("Property path") + ": {0}", ((JProperty)token).Path);
                        if (parameters.SwitchesList.Contains("-showvals"))
                            TextWriterColor.Write(Translate.DoTranslation("Property value") + ": {0}", ((JProperty)token).Value);
                        TextWriterColor.Write();
                    }
                }
            }
            return 0;
        }

    }
}
