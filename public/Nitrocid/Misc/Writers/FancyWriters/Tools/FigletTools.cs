
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

using System.Collections.Generic;
using Extensification.StringExts;
using Figgle;
using KS.Misc.Reflection;

namespace KS.Misc.Writers.FancyWriters.Tools
{
    /// <summary>
    /// Figlet tools
    /// </summary>
    public static class FigletTools
    {

        private readonly static Dictionary<string, string> cachedFiglets = new();

        /// <summary>
        /// The figlet fonts dictionary. It lists all the Figlet fonts supported by the Figgle library.
        /// </summary>
        public readonly static Dictionary<string, object> FigletFonts = PropertyManager.GetProperties(typeof(FiggleFonts));

        /// <summary>
        /// Gets the figlet lines
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="FigletFont">Target figlet font</param>
        public static string[] GetFigletLines(string Text, FiggleFont FigletFont)
        {
            Text = FigletFont.Render(Text);
            var TextLines = Text.SplitNewLines();
            List<string> lines = new(TextLines);

            // Try to trim from the top
            for (int line = 0; line < lines.Count; line++)
            {
                if (!string.IsNullOrWhiteSpace(lines[line]))
                    break;
                lines.RemoveAt(line);
            }

            // Try to trim from the bottom
            for (int line = lines.Count - 1; line > 0; line--)
            {
                if (!string.IsNullOrWhiteSpace(lines[line]))
                    break;
                lines.RemoveAt(line);
            }

            return lines.ToArray();
        }

        /// <summary>
        /// Gets the figlet text height
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="FigletFont">Target figlet font</param>
        public static int GetFigletHeight(string Text, FiggleFont FigletFont) => 
            GetFigletLines(Text, FigletFont).Length;

        /// <summary>
        /// Gets the figlet text width
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="FigletFont">Target figlet font</param>
        public static int GetFigletWidth(string Text, FiggleFont FigletFont) =>
            GetFigletLines(Text, FigletFont)[0].Length;

        /// <summary>
        /// Gets the figlet font from font name
        /// </summary>
        /// <param name="FontName">Font name that is supported by the Figgle library. Consult <see cref="FigletFonts"/> for more info.</param>
        /// <returns>Figlet font instance of your font, or Small if not found</returns>
        public static FiggleFont GetFigletFont(string FontName)
        {
            if (FigletFonts.ContainsKey(FontName))
            {
                return (FiggleFont)FigletFonts[FontName];
            }
            else
            {
                return FiggleFonts.Small;
            }
        }

        /// <summary>
        /// Gets the figlet font name from font
        /// </summary>
        /// <param name="Font">Font instance that is supported by the Figgle library. Consult <see cref="FigletFonts"/> for more info.</param>
        /// <returns>Figlet font name of your font, or an empty string if not found</returns>
        /// <remarks>
        /// Since the Figgle library doesn't have a meaningful way of checking if the provided FiggleFont exists, the function looks for the specific instance of the font
        /// that the function provided to check to see if the FigletFonts contains that specific font instance.
        /// </remarks>
        public static string GetFigletFontName(FiggleFont Font)
        {
            // Since the Figgle library doesn't have a meaningful way of checking if the provided FiggleFont exists, we have no option other than using the
            // FigletFonts variable and scouring through it to look for this specific copy.
            string figletFontName = "";
            foreach (string FigletFontToCompare in FigletFonts.Keys)
            {
                if (GetFigletFont(FigletFontToCompare) == Font)
                {
                    figletFontName = FigletFontToCompare;
                    break;
                }
            }

            // If we don't have the font in the supported fonts dictionary, return an empty string
            if (string.IsNullOrEmpty(figletFontName))
                return "";

            // Otherwise, return the name
            return figletFontName;
        }

        /// <summary>
        /// Renders the figlet font
        /// </summary>
        /// <param name="Text">Text to render</param>
        /// <param name="figletFontName">Figlet font name to render. Consult <see cref="FigletFonts"/> for more info.</param>
        /// <param name="Vars">Variables to use when formatting the string</param>
        public static string RenderFiglet(string Text, string figletFontName, params object[] Vars)
        {
            var FigletFont = GetFigletFont(figletFontName);
            return RenderFiglet(Text, FigletFont, Vars);
        }

        /// <summary>
        /// Renders the figlet font
        /// </summary>
        /// <param name="Text">Text to render</param>
        /// <param name="FigletFont">Figlet font instance to render. Consult <see cref="FigletFonts"/> for more info.</param>
        /// <param name="Vars">Variables to use when formatting the string</param>
        public static string RenderFiglet(string Text, FiggleFont FigletFont, params object[] Vars)
        {
            // Look at the Remarks section of GetFigletFontName to see why we're doing this.
            string figletFontName = GetFigletFontName(FigletFont);
            if (string.IsNullOrEmpty(figletFontName))
                return "";

            // Now, render the figlet and add to the cache
            string cachedFigletKey = $"[{cachedFiglets.Count} - {figletFontName}] {Text}";
            string cachedFigletKeyToAdd = $"[{cachedFiglets.Count + 1} - {figletFontName}] {Text}";
            if (cachedFiglets.ContainsKey(cachedFigletKey))
                return cachedFiglets[cachedFigletKey];
            else
            {
                // Format string as needed
                if (!(Vars.Length == 0))
                    Text = StringManipulate.FormatString(Text, Vars);

                // Write the font
                Text = string.Join("\n", GetFigletLines(Text, FigletFont));
                cachedFiglets.Add(cachedFigletKeyToAdd, Text);
                return Text;
            }
        }

    }
}
