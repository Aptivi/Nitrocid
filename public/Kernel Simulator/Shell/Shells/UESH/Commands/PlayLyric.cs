
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using System.Threading;
using KS.ConsoleBase;
using KS.Misc.Animations.Lyrics;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Plays a lyric file
    /// </summary>
    /// <remarks>
    /// This command allows you to play a lyric file by showing you the basic lyrics visualizer.
    /// </remarks>
    class PlayLyricCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string pathToLyrics = ListArgsOnly[0];

            // Visualize it!
            Lyrics.VisualizeLyric(pathToLyrics);
            Thread.Sleep(10000);
            ConsoleWrapper.CursorVisible = true;
        }

    }
}
