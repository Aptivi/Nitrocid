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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Graphical.Rulers;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.Data.Figlet;
using Textify.Data.NameGen;
using Textify.Data.Words;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for MarkScreen
    /// </summary>
    public class MarkScreenDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_MARKSCREEN_SETTINGS_DESC -> Shows an old-style terminal screen that features student names and grades for all subjects, similar to an old university grade book
        private int gradeEntries = 0;
        private readonly List<(string, double[])> students = [];
        private readonly (double, string)[] averages =
        [
            // Fail is below 60%
            (00.00d, "F"),
            (60.00d, "E-"),
            (65.00d, "E"),
            (67.50d, "E+"),
            (70.00d, "D-"),
            (72.50d, "D"),
            (75.00d, "D+"),
            (77.50d, "C-"),
            (80.00d, "C"),
            (82.50d, "C+"),
            (85.00d, "B-"),
            (87.50d, "B"),
            (90.00d, "B+"),
            (92.50d, "A-"),
            (95.00d, "A"),
            (97.50d, "A+"),
        ];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "MarkScreen";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            int width = ConsoleWrapper.WindowWidth - 8;
            students.Clear();
            gradeEntries = (width - 19 - 7) / 7;
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            var builder = new StringBuilder();

            // Write the box frame of an old grade book simulation in rows
            int height = ConsoleWrapper.WindowHeight - 4;
            int width = ConsoleWrapper.WindowWidth - 8;
            int posX = ConsoleWrapper.WindowWidth / 2 - width / 2 - 1;
            int posY = ConsoleWrapper.WindowHeight / 2 - height / 2 - 1;
            var border = new Border()
            {
                Left = posX,
                Top = posY,
                Width = width,
                Height = height,
                UseColors = true,
                Color = ConsoleColors.Lime,
                Rulers = [
                    // Split name (truncated to 16 chars in width) and grade pane
                    new(19, RulerOrientation.Vertical),

                    // Split grade pane (grades shown per width) and average grade pane
                    new(width - 10, RulerOrientation.Vertical),

                    // Split average grade pane (grade number + category) and grade category (A+, A, A-, ...)
                    new(width - 3, RulerOrientation.Vertical)
                ]
            };
            builder.Append(border.Render());

            // Add a new student for each frame generation
            List<double> studentGrades = [];
            string studentName = NameGenerator.GenerateNames(1)[0];
            int gradeMin = ScreensaverPackInit.SaversConfig.MarkScreenMinimumGrade;
            int gradeMax = ScreensaverPackInit.SaversConfig.MarkScreenMaximumGrade;
            IntegerTools.SwapIfSourceLarger(ref gradeMin, ref gradeMax);
            for (int i = 0; i < gradeEntries; i++)
                studentGrades.Add(RandomDriver.Random(gradeMin, gradeMax) / 100);
            students.Add((studentName, [.. studentGrades]));
            if (students.Count > height)
                students.RemoveAt(0);

            // Process the students and their grades
            int namePosX = posX + 1;
            int gradesPosX = posX + 21;
            int averagePosX = posX + width - 8;
            int averageRepPosX = posX + width - 1;
            for (int i = 0; i < students.Count; i++)
            {
                int studentPosY = posY + 1 + i;
                (string finalName, double[] finalGrades) = students[i];

                // Print the student name in the students pane
                string enumStudentName = finalName.ToUpper().Truncate(19);
                builder.Append(TextWriterWhereColor.RenderWhereColor(enumStudentName, namePosX, studentPosY, ConsoleColors.Lime));

                // Print the grades as they fit
                for (int g = 0; g < finalGrades.Length; g++)
                {
                    int finalGradesPosX = gradesPosX + (g * 7);
                    double finalGrade = finalGrades[g];
                    builder.Append(TextWriterWhereColor.RenderWhereColor($"{finalGrade:##0.00}", finalGradesPosX, studentPosY, ConsoleColors.White));
                }

                // Get the average and the grade representation
                double studentAverage = finalGrades.Average();
                string studentAverageStr = averages.MaxBy((tuple) => tuple.Item1 >= studentAverage).Item2;

                // Print the average
                builder.Append(TextWriterWhereColor.RenderWhereColor($"{studentAverage:##0.00}", averagePosX, studentPosY, ConsoleColors.Lime));
                builder.Append(TextWriterWhereColor.RenderWhereColor(studentAverageStr, averageRepPosX, studentPosY, ConsoleColors.Lime));
            }

            // Delay
            TextWriterRaw.WriteRaw(builder.ToString());
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.MarkScreenDelay);
        }

    }
}
