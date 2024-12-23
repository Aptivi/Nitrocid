//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Microsoft.CodeAnalysis;
using System;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;

namespace Nitrocid.Analyzers.Common
{
    internal static class AnalyzerTools
    {
        internal static void PrintFromLocation(Location location, Document document, Type targetType, string message)
        {
            var lineSpan = location.GetLineSpan();
            TextWriterColor.WriteColor($"{targetType.Name}: {document.FilePath} ({lineSpan.StartLinePosition} -> {lineSpan.EndLinePosition}): {message}", true, ConsoleColors.Yellow);
            if (!string.IsNullOrEmpty(document.FilePath))
            {
                var lineHandle = new LineHandle(document.FilePath)
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
