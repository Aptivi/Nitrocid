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

using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#if !NOTERMINAUX
using System;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Simple;
#endif

namespace Nitrocid.Analyzers.Common
{
    internal static class AnalyzerTools
    {
#if !NOTERMINAUX
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
#endif

        internal static Location? GenerateLocation(JToken? token, string str, string path, bool enclose = true)
        {
            if (token is null)
                return null;
            if (enclose)
                str = $"\"{str}\"";
            var lineInfo = (IJsonLineInfo)token;
            var location = lineInfo.HasLineInfo() ? Location.Create(path, new(lineInfo.LinePosition - str.Length, str.Length), new(new(lineInfo.LineNumber - 1, lineInfo.LinePosition - str.Length), new(lineInfo.LineNumber - 1, lineInfo.LinePosition))) : null;
            return location;
        }
    }
}
