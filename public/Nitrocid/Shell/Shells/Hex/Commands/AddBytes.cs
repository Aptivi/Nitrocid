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

using System.Collections.Generic;
using Nitrocid.Files.Editors.HexEdit;
using Nitrocid.Languages;
using Terminaux.Reader;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Adds new bytes at the end of the file
    /// </summary>
    /// <remarks>
    /// You can use this command to add new bytes at the end of the file.
    /// </remarks>
    class AddBytesCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "addbytes";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_COMMAND_ADDBYTES_DESC");

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var FinalBytes = new List<byte>();
            string FinalByte = "";

            // Keep prompting for bytes until the user finishes
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_ADDBYTES_PROMPT"));
            while (FinalByte != "EOF")
            {
                TextWriterColor.Write(">> ", false, ThemeColorType.Input);
                FinalByte = TermReader.Read();
                if (FinalByte != "EOF")
                {
                    if (byte.TryParse(FinalByte, System.Globalization.NumberStyles.HexNumber, null, out byte ByteContent))
                        FinalBytes.Add(ByteContent);
                    else
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_ADDBYTES_INVALIDBYTE"), true, ThemeColorType.Error);
                }
            }

            // Add the new bytes
            HexEditTools.AddNewBytes([.. FinalBytes]);
            return 0;
        }

    }
}
