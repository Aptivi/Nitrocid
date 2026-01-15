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
using Nitrocid.Base.Languages;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Base.Buffered;
using System.Text;
using Terminaux.Writer.CyclicWriters.Graphical.Rulers;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.Data.Figlet;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Base.Structures;
using Terminaux.Base.Extensions;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    internal static class ScoreSim
    {

        internal static void InitializeScoreSim(int mode, string firstTeamName, string secondTeamName)
        {
            int scoreFirstTeam = 0;
            int scoreSecondTeam = 0;
            int yellowCardsFirstTeam = 0;
            int redCardsFirstTeam = 0;
            int yellowCardsSecondTeam = 0;
            int redCardsSecondTeam = 0;
            int teamNumber = 1;
            bool supportsCards = mode == 1;
            bool done = false;
            var scoreSimScreen = new Screen();
            var scoreSimScreenPart = new ScreenPart();
            firstTeamName = string.IsNullOrEmpty(firstTeamName) ? LanguageTools.GetLocalized("NKS_AMUSEMENTS_SCORESIM_FIRSTTEAM") : firstTeamName;
            secondTeamName = string.IsNullOrEmpty(secondTeamName) ? LanguageTools.GetLocalized("NKS_AMUSEMENTS_SCORESIM_SECONDTEAM") : secondTeamName;

            // Make the screen part for the game
            scoreSimScreenPart.AddDynamicText(() =>
            {
                // Make a panel for the score sim game
                int width = ConsoleWrapper.WindowWidth - 4;
                int height = ConsoleWrapper.WindowHeight - 6;
                int panelPosX = 2;
                int panelPosY = 1;
                var boxFrame = new BoxFrame()
                {
                    Left = panelPosX,
                    Top = panelPosY,
                    Width = width,
                    Height = height,
                    Rulers =
                    [
                        new(width / 2, RulerOrientation.Vertical),
                    ]
                };
                var gameBuffer = new StringBuilder();
                gameBuffer.Append(boxFrame.Render());

                // Print the information about two teams here
                int infoPosY = ConsoleWrapper.WindowHeight - 2;
                var firstTeamInfo = new AlignedText()
                {
                    Left = 0,
                    Top = infoPosY,
                    Width = ConsoleWrapper.WindowWidth / 2,
                    Height = 1,
                    OneLine = true,

                    Text =
                        (supportsCards ? $"{ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Red, true)} {ConsoleColoring.RenderResetBackground()} x{redCardsFirstTeam} | " : "") +
                        (supportsCards ? $"{ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Yellow, true)} {ConsoleColoring.RenderResetBackground()} x{yellowCardsFirstTeam} | " : "") +
                        firstTeamName,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    },
                    ForegroundColor = teamNumber == 1 ? ConsoleColors.Lime : ConsoleColors.White
                };
                gameBuffer.Append(firstTeamInfo.Render());
                var secondTeamInfo = new AlignedText()
                {
                    Left = ConsoleWrapper.WindowWidth / 2,
                    Top = infoPosY,
                    Width = ConsoleWrapper.WindowWidth / 2,
                    Height = 1,
                    OneLine = true,

                    Text =
                        (supportsCards ? $"{ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Red, true)} {ConsoleColoring.RenderResetBackground()} x{redCardsSecondTeam} | " : "") +
                        (supportsCards ? $"{ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Yellow, true)} {ConsoleColoring.RenderResetBackground()} x{yellowCardsSecondTeam} | " : "") +
                        secondTeamName,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    },
                    ForegroundColor = teamNumber == 2 ? ConsoleColors.Lime : ConsoleColors.White
                };
                gameBuffer.Append(secondTeamInfo.Render());

                // Erase the panels
                var eraserFirst = new Eraser()
                {
                    Left = panelPosX + 1,
                    Top = panelPosY + 1,
                    Width = width / 2,
                    Height = height,
                };
                gameBuffer.Append(eraserFirst.Render());
                var eraserSecond = new Eraser()
                {
                    Left = panelPosX + width / 2 + 2,
                    Top = panelPosY + 1,
                    Width = width / 2 - 1,
                    Height = height,
                };
                gameBuffer.Append(eraserSecond.Render());

                // Print scores using figlet
                var font = FigletTools.GetFigletFont("banner3");
                int figletWidthFirst = FigletTools.GetFigletWidth($"{scoreFirstTeam}", font);
                int figletWidthSecond = FigletTools.GetFigletWidth($"{scoreSecondTeam}", font);
                int figletHeightFirst = FigletTools.GetFigletHeight($"{scoreFirstTeam}", font);
                int figletHeightSecond = FigletTools.GetFigletHeight($"{scoreSecondTeam}", font);
                var firstTeamScore = new FigletText(font)
                {
                    Text = $"{scoreFirstTeam}",
                };
                gameBuffer.Append(RendererTools.RenderRenderable(firstTeamScore, new Coordinate(panelPosX + 1 + width / 4 - figletWidthFirst / 2, ConsoleWrapper.WindowHeight / 2 - figletHeightSecond / 2 - 1)));
                var secondTeamScore = new FigletText(font)
                {
                    Text = $"{scoreSecondTeam}",
                };
                gameBuffer.Append(RendererTools.RenderRenderable(secondTeamScore, new Coordinate(panelPosX + width * 3 / 4 + 2 - figletWidthSecond / 2, ConsoleWrapper.WindowHeight / 2 - figletHeightSecond / 2 - 1)));
                return gameBuffer.ToString();
            });
            scoreSimScreen.AddBufferedPart("Score Sim", scoreSimScreenPart);
            ScreenTools.SetCurrent(scoreSimScreen);

            // Main loop
            while (!done)
            {
                ScreenTools.Render();

                // Let the user decide the position
                var pressedChar = Input.ReadKey();
                switch (pressedChar.Key)
                {
                    case ConsoleKey.Escape:
                        // User decided to escape
                        done = true;
                        break;
                    case ConsoleKey.Tab:
                        // Switch teams
                        if (++teamNumber > 2)
                            teamNumber = 1;
                        break;
                    case ConsoleKey.R:
                        // Red card
                        if (!supportsCards)
                            break;
                        if (teamNumber == 1)
                            redCardsFirstTeam++;
                        else if (teamNumber == 2)
                            redCardsSecondTeam++;
                        break;
                    case ConsoleKey.Y:
                        // Yellow card
                        if (!supportsCards)
                            break;
                        if (teamNumber == 1)
                            yellowCardsFirstTeam++;
                        else if (teamNumber == 2)
                            yellowCardsSecondTeam++;
                        break;
                    case ConsoleKey.I:
                        // Increment one or two, depending on mode where:
                        // - Standard:      1 or 2
                        // - Soccer:        1 or 1
                        // - Basketball:    2 or 3
                        int incrementFirstLevel = mode == 2 ? 2 : 1;
                        int incrementSecondLevel = mode == 0 ? 2 : mode == 2 ? 3 : 1;
                        bool isSecondLevel = pressedChar.Modifiers.HasFlag(ConsoleModifiers.Shift);
                        if (teamNumber == 1)
                            scoreFirstTeam += isSecondLevel ? incrementSecondLevel : incrementFirstLevel;
                        else if (teamNumber == 2)
                            scoreSecondTeam += isSecondLevel ? incrementSecondLevel : incrementFirstLevel;
                        break;
                }
            }

            // Clean up
            ScreenTools.UnsetCurrent(scoreSimScreen);
            ConsoleWrapper.Clear();
        }
    }
}
