//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using System;
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can view the information about a pressed key
    /// </summary>
    /// <remarks>
    /// This command lets you view the details about a pressed key on your keyboard, including the pressed key and character, the hexadecimal representation of the letter, the pressed modifiers, and the keyboard shortcut.
    /// </remarks>
    class KeyInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_KEYINFO_PROMPT"));
            var KeyPress = Input.ReadKey();

            // Pressed key
            TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_KEYINFO_KEY") + ": ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write(KeyPress.Key.ToString(), true, ThemeColorType.ListValue);

            // If the pressed key is a control key, don't write the actual key char so as not to corrupt the output
            if (!char.IsControl(KeyPress.KeyChar))
            {
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_KEYINFO_KEYCHAR") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(Convert.ToString(KeyPress.KeyChar), true, ThemeColorType.ListValue);
            }

            // Pressed key character code
            TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_KEYINFO_KEYCHARCODE") + ": ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write($"0x{Convert.ToInt32(KeyPress.KeyChar):X2} [{Convert.ToInt32(KeyPress.KeyChar)}]", true, ThemeColorType.ListValue);

            // Pressed modifiers
            TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_KEYINFO_KEYMODIFIERS") + ": ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write(KeyPress.Modifiers.ToString(), true, ThemeColorType.ListValue);

            // Keyboard shortcut
            TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_KEYINFO_KEYSHORTCUT") + ": ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write($"{string.Join(" + ", KeyPress.Modifiers.ToString().Split([", "], StringSplitOptions.None))} + {KeyPress.Key}", true, ThemeColorType.ListValue);
            return 0;
        }

    }
}
