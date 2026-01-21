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

using System.Text;
using QRCoder;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters;

namespace Nitrocid.Base.Users.TwoFactorAuth
{
    internal class MicroQrCode : SimpleCyclicWriter
    {
        private Color foregroundColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
        private bool useColors = true;

        /// <summary>
        /// Foreground color
        /// </summary>
        public Color ForegroundColor
        {
            get => foregroundColor;
            set => foregroundColor = value;
        }

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Text to encode to Micro QR code
        /// </summary>
        public string Text { get; set; } = "Terminaux";

        /// <summary>
        /// Specifies the Micro QR code error correction level
        /// </summary>
        public QRCodeGenerator.ECCLevel ErrorCorrection { get; set; } = QRCodeGenerator.ECCLevel.L;

        /// <summary>
        /// Renders a Micro QR code
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            if (string.IsNullOrEmpty(Text))
                return "";

            var qrData = QRCodeGenerator.GenerateMicroQrCode(Text, ErrorCorrection);
            var qrMatrix = qrData.ModuleMatrix;

            // Now, we need to use this data to print QR code to the console
            // TODO: In Terminaux 8.2, move this simple writer, MicroQrCode, from Nitrocid.
            var qrBuilder = new StringBuilder();
            if (UseColors)
            {
                qrBuilder.Append(ConsoleColoring.RenderSetConsoleColor(ForegroundColor));
                qrBuilder.Append(ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true));
            }
            for (int y = 4; y < qrMatrix.Count - 4; y += 2)
            {
                for (int x = 4; x < qrMatrix[y].Count - 4; x++)
                {
                    // Get the upper and the lower matrix of QR code to determine how to print the code
                    bool upperMatrix = qrMatrix[y][x];
                    bool lowerMatrix = y + 1 < qrMatrix.Count && qrMatrix[y + 1][x];

                    // Now, use the block characters to print the QR code
                    if (upperMatrix && lowerMatrix)
                        qrBuilder.Append('█');
                    else if (upperMatrix && !lowerMatrix)
                        qrBuilder.Append('▀');
                    else if (!upperMatrix && lowerMatrix)
                        qrBuilder.Append('▄');
                    else
                        qrBuilder.Append(' ');
                }
                qrBuilder.AppendLine();
            }
            if (UseColors)
                qrBuilder.Append(ConsoleColoring.RenderResetColors());
            return qrBuilder.ToString();
        }

        /// <summary>
        /// Makes a new instance of the Micro QR code renderer
        /// </summary>
        public MicroQrCode()
        { }
    }
}
