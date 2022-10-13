
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
        /// Gets the figlet text height
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="FigletFont">Target figlet font</param>
        public static int GetFigletHeight(string Text, FiggleFont FigletFont)
        {
            Text = FigletFont.Render(Text);
            var TextLines = Text.SplitNewLines();
            return TextLines.Length;
        }

        /// <summary>
        /// Gets the figlet text width
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="FigletFont">Target figlet font</param>
        public static int GetFigletWidth(string Text, FiggleFont FigletFont)
        {
            Text = FigletFont.Render(Text);
            var TextLines = Text.SplitNewLines();
            return TextLines[0].Length;
        }

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

        public static string RenderFiglet(string Text, string figletFontName, params object[] Vars)
        {
            var FigletFont = FigletTools.GetFigletFont(figletFontName);
            return RenderFiglet(Text, FigletFont, Vars);
        }

        public static string RenderFiglet(string Text, FiggleFont FigletFont, params object[] Vars)
        {
            // Since Figgle library doesn't have a meaningful way of checking if the provided FiggleFont exists, so we have no option other than using the
            // FigletFonts variable and scouring through it to look for this specific copy.
            string figletFontName = "";
            foreach (string FigletFontToCompare in FigletFonts.Keys)
            {
                if (GetFigletFont(FigletFontToCompare) == FigletFont)
                    figletFontName = FigletFontToCompare;
                if (!string.IsNullOrEmpty(figletFontName))
                    break;
            }
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
                Text = FigletFont.Render(Text);
                cachedFiglets.Add(cachedFigletKeyToAdd, Text);
                return Text;
            }
        }

    }
}
