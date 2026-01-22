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

using System.Collections.Generic;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Misc.Reflection;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Textify.General;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Mazer
    /// </summary>
    public class MazerDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Mazer";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleColoring.LoadBackDry(new Color(ConsoleColors.Black));
            ConsoleWrapper.CursorVisible = false;

            // Use Kruskal's algorithm to generate a maze
            GenerateMaze();

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.MazerNewMazeDelay);
        }

        private static void GenerateMaze()
        {
            // Necessary variables for maze generation
            int width = (ConsoleWrapper.WindowWidth / 2) - 3;
            int height = ConsoleWrapper.WindowHeight - 3;
            double delay = ScreensaverPackInit.SaversConfig.MazerGenerationSpeed;

            // Now, the directions and their opposites
            Dictionary<Direction, int> opposites = new()
            {
                { Direction.East, (int)Direction.West },
                { Direction.West, (int)Direction.East },
                { Direction.North, (int)Direction.South },
                { Direction.South, (int)Direction.North },
            };
            Dictionary<Direction, int> directionX = new()
            {
                { Direction.East, 1 },
                { Direction.West, -1 },
                { Direction.North, 0 },
                { Direction.South, 0 },
            };
            Dictionary<Direction, int> directionY = new()
            {
                { Direction.East, 0 },
                { Direction.West, 0 },
                { Direction.North, -1 },
                { Direction.South, 1 },
            };

            // Populate the grid and the sets
            var grid = new int[height, width];
            var sets = new MazeTree[height, width];
            int yLength = grid.GetLength(0);
            int xLength = grid.GetLength(1);
            for (int y = 0; y < yLength; y++)
                for (int x = 0; x < xLength; x++)
                    sets[y, x] = new();

            // Build the list of edges
            List<(int x, int y, Direction direction)> plainEdges = [];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y > 0)
                        plainEdges.Add((x, y, Direction.North));
                    if (x > 0)
                        plainEdges.Add((x, y, Direction.West));
                }
            }
            var edgesArray = plainEdges.ToArray();
            var edges =
                ScreensaverPackInit.SaversConfig.MazerUseSchwartzian ?
                edgesArray.RandomizeArray() :
                edgesArray.RandomizeArraySystem();
            if (edges is null)
                return;

            // Iterate through all the edges to display the maze in a spectacular way
            foreach (var (x, y, direction) in edges)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Get necessary values for connections
                int nx = x + directionX[direction];
                int ny = y + directionY[direction];
                var set1 = sets[y, x];
                var set2 = sets[ny, nx];

                // Now, connect the grids
                while (!set1.IsConnected(set2))
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;

                    // Display the maze
                    DisplayMazeFromGrid(grid);
                    ScreensaverManager.Delay((int)delay);

                    // Now, do the connection
                    set1.Connect(set2);
                    grid[y, x] |= (int)direction;
                    grid[ny, nx] |= opposites[direction];
                }
            }
        }

        private static void DisplayMazeFromGrid(int[,] grid)
        {
            // Build the edge
            int yLength = grid.GetLength(0);
            int xLength = grid.GetLength(1);
            var mazeBuilder = new StringBuilder();
            mazeBuilder.Append(" " + new string('_', (xLength * 2) - 1));
            mazeBuilder.AppendLine();

            // Build the whole maze from the grid
            for (int y = 0; y < yLength; y++)
            {
                // The left edge
                mazeBuilder.Append('|');

                // Now, get the row and print the cells
                for (int x = 0; x < xLength; x++)
                {
                    // Get the cell
                    int cell = grid[y, x];
                    if (cell == 0 && ScreensaverPackInit.SaversConfig.MazerHighlightUncovered)
                        mazeBuilder.Append($"{CharManager.GetEsc()}[47m");
                    mazeBuilder.Append(IsDirection(cell, Direction.South) ? " " : "_");

                    if (x + 1 < xLength && IsDirection(cell, Direction.East))
                    {
                        int finalCell = cell | grid[y, x + 1];
                        mazeBuilder.Append(IsDirection(finalCell, Direction.South) ? " " : "_");
                    }
                    else
                        mazeBuilder.Append('|');
                    if (cell == 0 && ScreensaverPackInit.SaversConfig.MazerHighlightUncovered)
                        mazeBuilder.Append($"{CharManager.GetEsc()}[m");
                }

                // New line for the new row
                mazeBuilder.AppendLine();
            }

            // Print the maze
            TextWriterWhereColor.WriteWhere(mazeBuilder.ToString(), 2, 1);
        }

        private static bool IsDirection(int cell, Direction direction) =>
            (cell & (int)direction) != 0;

    }

    internal class MazeTree
    {
        internal MazeTree? parent = null;

        internal MazeTree Root =>
            parent is not null ? parent.Root : this;

        internal bool IsConnected(MazeTree tree) =>
            Root == tree.Root;

        internal void Connect(MazeTree tree) =>
            tree.Root.parent = this;
    }

    internal enum Direction
    {
        North = 1,
        South = 2,
        East = 4,
        West = 8,
    }
}
