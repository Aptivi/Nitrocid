
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

using KS.Misc.Games;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Launches the spelling bee game
    /// </summary>
    /// <remarks>
    /// You have played this kind of game before. Is that right? If so, you can use this command to test yourself in the giant list of words, including complicated ones like superspecializations, hypoparathyroidisms, etc.
    /// <br></br>
    /// This game will select a random word, then lets you write, which will obviously show nothing when input. Pressing ENTER will validate your spelling.
    /// </remarks>
    class SpellBeeCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly) => Speller.InitializeWords();

    }
}