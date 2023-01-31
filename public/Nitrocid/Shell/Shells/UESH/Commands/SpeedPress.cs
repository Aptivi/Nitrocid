
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

using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Games;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Launches the speed press game
    /// </summary>
    /// <remarks>
    /// This game will test your keystroke speed. It will only give you very little time to press a key before moving to the next one.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-e</term>
    /// <description>Easy</description>
    /// </item>
    /// <item>
    /// <term>-m</term>
    /// <description>Medium</description>
    /// </item>
    /// <item>
    /// <term>-h</term>
    /// <description>Hard</description>
    /// </item>
    /// <item>
    /// <term>-v</term>
    /// <description>Very Hard</description>
    /// </item>
    /// <item>
    /// <term>-c</term>
    /// <description>Custom. The timeout should be specified in milliseconds.</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class SpeedPressCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var Difficulty = SpeedPress.SpeedPressDifficulty.Medium;
            int CustomTimeout = SpeedPress.SpeedPressTimeout;
            if (ListSwitchesOnly.Contains("-e"))
                Difficulty = SpeedPress.SpeedPressDifficulty.Easy;
            if (ListSwitchesOnly.Contains("-m"))
                Difficulty = SpeedPress.SpeedPressDifficulty.Medium;
            if (ListSwitchesOnly.Contains("-h"))
                Difficulty = SpeedPress.SpeedPressDifficulty.Hard;
            if (ListSwitchesOnly.Contains("-v"))
                Difficulty = SpeedPress.SpeedPressDifficulty.VeryHard;
            if (ListSwitchesOnly.Contains("-c") & ListArgsOnly.Length > 0 && StringQuery.IsStringNumeric(ListArgsOnly[0]))
            {
                Difficulty = SpeedPress.SpeedPressDifficulty.Custom;
                CustomTimeout = Convert.ToInt32(ListArgsOnly[0]);
            }
            SpeedPress.InitializeSpeedPress(Difficulty, CustomTimeout);
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"));
            TextWriterColor.Write("  -e: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Starts the game in easy difficulty"), true, KernelColorType.ListValue);
            TextWriterColor.Write("  -m: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Starts the game in medium difficulty"), true, KernelColorType.ListValue);
            TextWriterColor.Write("  -h: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Starts the game in hard difficulty"), true, KernelColorType.ListValue);
            TextWriterColor.Write("  -v: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Starts the game in very hard difficulty"), true, KernelColorType.ListValue);
            TextWriterColor.Write("  -c: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Starts the game in custom difficulty. Please note that the custom timeout in milliseconds should be written as argument."), true, KernelColorType.ListValue);
        }

    }
}