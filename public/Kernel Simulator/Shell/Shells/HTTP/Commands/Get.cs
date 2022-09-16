
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

using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.HTTP;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.HTTP.Commands
{
    /// <summary>
    /// Gets a response from the HTTP server
    /// </summary>
    /// <remarks>
    /// If you want to test a GET function of the REST API, you can do so using this command.
    /// </remarks>
    class HTTP_GetCommand : CommandExecutor, ICommand
    {

        public override async void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (HTTPShellCommon.HTTPConnected == true)
            {
                // Print a message
                TextWriterColor.Write(Translate.DoTranslation("Getting {0}..."), true, ColorTools.ColTypes.Progress, ListArgsOnly[0]);

                try
                {
                    var ResponseTask = HTTPTools.HttpGet(ListArgsOnly[0]);
                    ResponseTask.Wait();
                    var Response = ResponseTask.Result;
                    string ResponseContent = await Response.Content.ReadAsStringAsync();
                    TextWriterColor.Write("[{0}] {1}", true, ColorTools.ColTypes.NeutralText, (int)Response.StatusCode, Response.StatusCode.ToString());
                    TextWriterColor.Write(ResponseContent, true, ColorTools.ColTypes.NeutralText);
                    TextWriterColor.Write(Response.ReasonPhrase, true, ColorTools.ColTypes.NeutralText);
                }
                catch (AggregateException aex)
                {
                    TextWriterColor.Write(aex.Message + ":", true, ColorTools.ColTypes.Error);
                    foreach (Exception InnerException in aex.InnerExceptions)
                    {
                        TextWriterColor.Write("- " + InnerException.Message, true, ColorTools.ColTypes.Error);
                        if (InnerException.InnerException is not null)
                        {
                            TextWriterColor.Write("- " + InnerException.InnerException.Message, true, ColorTools.ColTypes.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(ex.Message, true, ColorTools.ColTypes.Error);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("You must connect to server before performing transmission."), true, ColorTools.ColTypes.Error);
            }
        }

    }
}