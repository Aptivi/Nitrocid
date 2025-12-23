//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Network.Transfer;
using System.Text;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Generates a modeline for XFree86 3.3.6
    /// </summary>
    /// <remarks>
    /// If you want to generate a modeline for XFree86 3.3.6 with your monitor parameters, such as frequency and resolution, you can use this command.
    /// </remarks>
    class ModelineCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we need Modeline for monitor or generate a whole Mode block
            bool modelineOneLine = parameters.ContainsSwitch("-oneline");

            // Inspired by http://zaph.com/Modeline/Code.py
            // Get arguments for vertical
            int verticalPixels = int.Parse(parameters.ArgumentsList[1]);
            double verticalSync = parameters.ArgumentsList.Length > 3 ? double.Parse(parameters.ArgumentsList[3]) : 0;
            double verticalBlanking = parameters.ArgumentsList.Length > 4 ? double.Parse(parameters.ArgumentsList[4]) : 500;
            double verticalFreq = double.Parse(parameters.ArgumentsList[2]);

            // Get arguments for horizontal
            int horizontalPixels = int.Parse(parameters.ArgumentsList[0]);
            double horizontalSync = parameters.ArgumentsList.Length > 5 ? double.Parse(parameters.ArgumentsList[5]) : 1;
            double horizontalBlanking = parameters.ArgumentsList.Length > 6 ? double.Parse(parameters.ArgumentsList[6]) : 3;

            // Get blank periods in microseconds
            double verticalSyncUs = verticalSync / 1000000;
            double verticalBlankUs = verticalBlanking / 1000000;

            // Get vertical porch syncs
            double verticalFrontPorchRatio = parameters.ArgumentsList.Length > 7 ? double.Parse(parameters.ArgumentsList[7]) : 1;
            double verticalSyncRatio = parameters.ArgumentsList.Length > 8 ? double.Parse(parameters.ArgumentsList[8]) : 1;
            double verticalBackPorchRatio = parameters.ArgumentsList.Length > 9 ? double.Parse(parameters.ArgumentsList[9]) : 10;

            // Check for vertical sync and blanking
            double totalVerticalRatio = verticalFrontPorchRatio + verticalSyncRatio + verticalBackPorchRatio;
            if (verticalSyncUs > 0 && verticalBlankUs > 0)
            {
                verticalSyncRatio = (verticalFrontPorchRatio + verticalBackPorchRatio) * (verticalSyncUs / (verticalBlankUs - verticalSyncUs));
                totalVerticalRatio = verticalFrontPorchRatio + verticalSyncRatio + verticalBackPorchRatio;
            }
            else if (verticalSyncUs > 0 && verticalBlankUs <= 0)
                verticalBlankUs = verticalSyncUs * (totalVerticalRatio / verticalSyncRatio);
            else
                verticalSyncUs = verticalBlankUs * (verticalSyncRatio / totalVerticalRatio);

            // Get the vertical base frequency
            double verticalBase = 1d / verticalFreq;
            verticalBase = verticalPixels / (verticalBase - verticalBlankUs) * verticalBase;
            verticalBase = (verticalBase - verticalPixels) / totalVerticalRatio;

            // Get the vertical timings
            double verticalTiming1 = verticalPixels + (int)((verticalBase * verticalFrontPorchRatio) + 1);
            double verticalTiming2 = verticalTiming1 + (int)((verticalBase * verticalSyncRatio) + 1);
            double verticalTiming3 = verticalTiming2 + (int)((verticalBase * verticalBackPorchRatio) + 1);

            // Get horizontal frequency
            double horizontalFreq = verticalTiming3 * verticalFreq;

            // Get blank periods in microseconds
            double horizontalSyncUs = horizontalSync / 1000000;
            double horizontalBlankUs = horizontalBlanking / 1000000;

            // Get horizontal porch syncs
            double horizontalFrontPorchRatio = parameters.ArgumentsList.Length > 10 ? double.Parse(parameters.ArgumentsList[10]) : 1;
            double horizontalSyncRatio = parameters.ArgumentsList.Length > 11 ? double.Parse(parameters.ArgumentsList[11]) : 4;
            double horizontalBackPorchRatio = parameters.ArgumentsList.Length > 12 ? double.Parse(parameters.ArgumentsList[12]) : 7;

            // Modify the horizontal pixels, as appropriate
            horizontalPixels = (horizontalPixels + 7) / 8 * 8;
            double totalHorizontalRatio = horizontalFrontPorchRatio + horizontalSyncRatio + horizontalBackPorchRatio;
            if (horizontalSyncUs > 0 && horizontalBlankUs > 0)
            {
                horizontalSyncRatio = (horizontalFrontPorchRatio + horizontalBackPorchRatio) * (horizontalSyncUs / (horizontalBlankUs - horizontalSyncUs));
                totalHorizontalRatio = horizontalFrontPorchRatio + horizontalSyncRatio + horizontalBackPorchRatio;
            }
            else if (horizontalSyncUs > 0 && horizontalBlankUs <= 0)
                horizontalBlankUs = horizontalSyncUs * (totalHorizontalRatio / horizontalSyncRatio);
            else
                horizontalSyncUs = horizontalBlankUs * (horizontalSyncRatio / totalHorizontalRatio);

            // Get the horizontal base frequency
            double horizontalBase = 1d / horizontalFreq;
            horizontalBase = horizontalPixels / (horizontalBase - horizontalBlankUs) * horizontalBase;
            horizontalBase = (horizontalBase - horizontalPixels) / totalHorizontalRatio;

            // Get the horizontal timings
            double horizontalTiming1 = horizontalPixels + (int)(((horizontalBase * horizontalFrontPorchRatio) + 8) / 8) * 8;
            double horizontalTiming2 = horizontalTiming1 + (int)(((horizontalBase * horizontalSyncRatio) + 8) / 8) * 8;
            double horizontalTiming3 = horizontalTiming2 + (int)(((horizontalBase * horizontalBackPorchRatio) + 8) / 8) * 8;

            // Get the dot clock in microseconds
            double dotClock = horizontalTiming3 * verticalTiming3 * verticalFreq / 1000000;
            double horizontalFreqKilohertz = horizontalFreq / 1000;

            // Print the results
            var modelineBuilder = new StringBuilder();
            if (modelineOneLine)
            {
                modelineBuilder.Append(
                    $"Modeline  \"1280x1024\"  {dotClock} {horizontalPixels} {horizontalTiming1} {horizontalTiming2} {horizontalTiming3} {verticalPixels} {verticalTiming1} {verticalTiming2} {verticalTiming3} +hsync +vsync");
            }
            else
            {
                modelineBuilder.Append(
                    $"""
                    # {horizontalPixels}x{verticalPixels} @ {verticalFreq}Hz, {horizontalFreqKilohertz} kHz hsync
                    #
                    # Horiz. sync and blank: {horizontalSyncUs} microseconds, {horizontalBlankUs} microseconds
                    # Vert. sync and blank: {verticalSyncUs} microseconds, {verticalBlankUs} microseconds
                    #
                    # Porch ratios
                    #   - Horiz.: F {horizontalFrontPorchRatio}, S {horizontalSyncRatio}, B {horizontalBackPorchRatio}
                    #   - Vert.: F {verticalFrontPorchRatio}, S {verticalSyncRatio}, B {verticalBackPorchRatio}
                    #
                    # Bases: H {horizontalBase}, V {verticalBase}

                    Mode "{horizontalPixels}x{verticalPixels}"
                        DotClock  {dotClock}
                        HTimings  {horizontalPixels} {horizontalTiming1} {horizontalTiming2} {horizontalTiming3}
                        VTimings  {verticalPixels} {verticalTiming1} {verticalTiming2} {verticalTiming3}
                    EndMode
                    """);
            }
            TextWriterColor.Write(modelineBuilder.ToString());
            return 0;
        }
    }
}
