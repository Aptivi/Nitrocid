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

using System;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ShellPacks.Tools;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;

namespace Nitrocid.ShellPacks.Shells.HTTP.Commands
{
    class PostCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Print a message
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_POST_POSTING_FILE"), true, ThemeColorType.Progress, parameters.ArgumentsList[1]);

            try
            {
                var ResponseTask = HttpTools.HttpPostFile(parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
                ResponseTask.Wait();
                var Response = ResponseTask.Result;
                string ResponseContent = Response.Content.ReadAsStringAsync().Result;
                TextWriterColor.Write("[{0}] {1}", (int)Response.StatusCode, Response.StatusCode.ToString());
                TextWriterColor.Write(ResponseContent);
                TextWriterColor.Write(Response.ReasonPhrase ?? "");
                return 0;
            }
            catch (AggregateException aex)
            {
                TextWriterColor.Write(aex.Message + ":", true, ThemeColorType.Error);
                foreach (Exception InnerException in aex.InnerExceptions)
                {
                    TextWriterColor.Write("- " + InnerException.Message, true, ThemeColorType.Error);
                    if (InnerException.InnerException is not null)
                    {
                        TextWriterColor.Write("- " + InnerException.InnerException.Message, true, ThemeColorType.Error);
                    }
                }
                return aex.GetHashCode();
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(ex.Message, true, ThemeColorType.Error);
                return ex.GetHashCode();
            }
        }

    }
}
