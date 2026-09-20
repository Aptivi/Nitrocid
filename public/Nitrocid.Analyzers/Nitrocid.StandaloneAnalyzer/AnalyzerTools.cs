//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Microsoft.CodeAnalysis;
using System;
using Colorimetry.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Nitrocid.StandaloneAnalyzer
{
    internal static class AnalyzerTools
    {
        internal static void PrintFromLocation(Location? location, Document document, Type targetType, string message) =>
            PrintFromLocation(location, document.FilePath ?? "", targetType.Name, message);

        internal static void PrintFromLocation(Location? location, string filePath, string targetType, string message)
        {
            if (location is null)
                TextWriterColor.WriteColor($"{targetType}: {(!string.IsNullOrEmpty(filePath) ? filePath : "<<unknown path>>")}: {message}", true, ConsoleColors.Yellow);
            else
            {
                var lineSpan = location.GetLineSpan();
                TextWriterColor.WriteColor($"{targetType}: {(!string.IsNullOrEmpty(filePath) ? filePath : "<<unknown path>>")} ({lineSpan.StartLinePosition} -> {lineSpan.EndLinePosition}): {message}", true, ConsoleColors.Yellow);
                if (!string.IsNullOrEmpty(filePath))
                {
                    var lineHandle = new LineHandle(filePath)
                    {
                        Ranged = true,
                        Position = lineSpan.StartLinePosition.Line + 1,
                        SourcePosition = lineSpan.StartLinePosition.Character + 1,
                        TargetPosition = lineSpan.EndLinePosition.Character,
                        Color = ConsoleColors.Olive,
                    };
                    TextWriterRaw.WriteRaw(lineHandle.Render());
                }
            }
        }
    }
}
