
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Changes system language
    /// </summary>
    /// <remarks>
    /// The system language can be changed either by manually editing configuration files or by using this command. Restart is not required, since printing text, viewing user manual, and updating help list relies on the current language field.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-alwaystransliterated</term>
    /// <description>Always use the transliterated version of the language. Must be transliterable.</description>
    /// </item>
    /// <item>
    /// <term>-alwaystranslated</term>
    /// <description>Always use the translated version of the language. Must be transliterable.</description>
    /// </item>
    /// <item>
    /// <term>-force</term>
    /// <description>Forces the language to be set.</description>
    /// </item>
    /// <item>
    /// <term>-list</term>
    /// <description>Lists the installed languages.</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class ChLangCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListSwitchesOnly.Contains("-list"))
            {
                TextWriterColor.Write(Translate.DoTranslation("Available languages:"), true, KernelColorType.ListTitle);
                foreach (string Language in LanguageManager.Languages.Keys)
                {
                    TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, Language);
                    TextWriterColor.Write(LanguageManager.Languages[Language].FullLanguageName, true, KernelColorType.ListValue);
                }
            }
            else
            {
                bool AlwaysTransliterated = default, AlwaysTranslated = default, Force = default;
                if (ListSwitchesOnly.Contains("-alwaystransliterated"))
                    AlwaysTransliterated = true;
                if (ListSwitchesOnly.Contains("-alwaystranslated"))
                    AlwaysTranslated = true; // -alwaystransliterated has higher priority.
                if (ListSwitchesOnly.Contains("-force"))
                    Force = true;
                LanguageManager.PromptForSetLang(ListArgsOnly[0], Force, AlwaysTransliterated, AlwaysTranslated);
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"));
            TextWriterColor.Write("  -alwaystransliterated: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Always use the transliterated version"), true, KernelColorType.ListValue);
            TextWriterColor.Write("  -alwaystranslated: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Always use the translated version"), true, KernelColorType.ListValue);
            TextWriterColor.Write("  -force: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Force switching language"), true, KernelColorType.ListValue);
            TextWriterColor.Write("  -list: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Lists available languages"), true, KernelColorType.ListValue);
        }

    }
}