using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
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
    class HTTP_GetStringCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (HTTPShellCommon.HTTPConnected == true)
            {
                // Print a message
                TextWriterColor.Write(Translate.DoTranslation("Getting {0}..."), true, ColorTools.ColTypes.Progress, ListArgsOnly[0]);

                try
                {
                    var ResponseTask = HTTPTools.HttpGetString(ListArgsOnly[0]);
                    ResponseTask.Wait();
                    string Response = ResponseTask.Result;
                    TextWriterColor.Write(Response, true, ColorTools.ColTypes.Neutral);
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