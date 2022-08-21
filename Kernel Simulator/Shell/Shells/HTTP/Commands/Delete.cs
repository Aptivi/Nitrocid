using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.HTTP;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.HTTP.Commands
{
    /// <summary>
    /// Removes content from the HTTP server
    /// </summary>
    /// <remarks>
    /// If you want to test a DELETE function of the REST API, you can do so using this command.
    /// </remarks>
    class HTTP_DeleteCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (HTTPShellCommon.HTTPConnected == true)
            {
                // Print a message
                TextWriterColor.Write(Translate.DoTranslation("Deleting {0}..."), true, ColorTools.ColTypes.Progress, ListArgsOnly[0]);

                // Make a confirmation message so user will not accidentally delete a file or folder
                TextWriterColor.Write(Translate.DoTranslation("Are you sure you want to delete {0} <y/n>?") + " ", false, ColorTools.ColTypes.Input, ListArgsOnly[0]);
                string answer = Convert.ToString(Console.ReadKey().KeyChar);
                Console.WriteLine();

                try
                {
                    var DeleteTask = HTTPTools.HttpDelete(ListArgsOnly[0]);
                    DeleteTask.Wait();
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
                TextWriterColor.Write(Translate.DoTranslation("You must connect to server with administrative privileges before performing the deletion."), true, ColorTools.ColTypes.Error);
            }
        }

    }
}