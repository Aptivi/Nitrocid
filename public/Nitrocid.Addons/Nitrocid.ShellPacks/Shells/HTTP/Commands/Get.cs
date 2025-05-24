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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ShellPacks.Tools;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.ShellPacks.Shells.HTTP.Commands
{
    /// <summary>
    /// Gets a response from the HTTP server
    /// </summary>
    /// <remarks>
    /// If you want to test a GET function of the REST API, you can do so using this command.
    /// </remarks>
    class GetCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Print a message
            TextWriters.Write(Translate.DoTranslation("Getting {0}..."), true, KernelColorType.Progress, parameters.ArgumentsList[0]);

            try
            {
                var ResponseTask = HttpTools.HttpGet(parameters.ArgumentsList[0]);
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
                TextWriters.Write(aex.Message + ":", true, KernelColorType.Error);
                foreach (Exception InnerException in aex.InnerExceptions)
                {
                    TextWriters.Write("- " + InnerException.Message, true, KernelColorType.Error);
                    if (InnerException.InnerException is not null)
                    {
                        TextWriters.Write("- " + InnerException.InnerException.Message, true, KernelColorType.Error);
                    }
                }
                return aex.GetHashCode();
            }
            catch (Exception ex)
            {
                TextWriters.Write(ex.Message, true, KernelColorType.Error);
                return ex.GetHashCode();
            }
        }

    }
}
