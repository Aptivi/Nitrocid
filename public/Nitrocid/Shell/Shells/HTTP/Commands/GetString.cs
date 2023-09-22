
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

using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Network.HTTP;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.HTTP.Commands
{
    /// <summary>
    /// Gets a string response from the HTTP server
    /// </summary>
    /// <remarks>
    /// If you want to test a GET function of the REST API, you can do so using this command. It returns responses using strings.
    /// </remarks>
    class HTTP_GetStringCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            // Print a message
            TextWriterColor.Write(Translate.DoTranslation("Getting {0}..."), true, KernelColorType.Progress, ListArgsOnly[0]);

            try
            {
                var ResponseTask = HTTPTools.HttpGetString(ListArgsOnly[0]);
                ResponseTask.Wait();
                string Response = ResponseTask.Result;
                TextWriterColor.Write(Response);
                return 0;
            }
            catch (AggregateException aex)
            {
                TextWriterColor.Write(aex.Message + ":", true, KernelColorType.Error);
                foreach (Exception InnerException in aex.InnerExceptions)
                {
                    TextWriterColor.Write("- " + InnerException.Message, true, KernelColorType.Error);
                    if (InnerException.InnerException is not null)
                    {
                        TextWriterColor.Write("- " + InnerException.InnerException.Message, true, KernelColorType.Error);
                    }
                }
                return aex.GetHashCode();
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(ex.Message, true, KernelColorType.Error);
                return ex.GetHashCode();
            }
        }

    }
}
