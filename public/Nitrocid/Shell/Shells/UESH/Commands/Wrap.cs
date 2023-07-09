
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

using System.Collections;
using System.IO;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Wraps a command
    /// </summary>
    /// <remarks>
    /// You can wrap a command so it stops outputting until you press a key if the console has printed lines that exceed the console window height. Only the commands that are explicitly set to be wrappable can be used with this command.
    /// </remarks>
    class WrapCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string CommandToBeWrapped = ListArgsOnly[0].Split(' ')[0];
            if (Shell.GetShellInfo(ShellType.Shell).Commands.ContainsKey(CommandToBeWrapped))
            {
                if (Shell.GetShellInfo(ShellType.Shell).Commands[CommandToBeWrapped].Flags.HasFlag(CommandFlags.Wrappable))
                {
                    string WrapOutputPath = Paths.TempPath + "/wrapoutput.txt";
                    if (!Checking.FileExists(WrapOutputPath))
                        Making.MakeFile(WrapOutputPath);
                    var AltThreads = ShellStart.ShellStack[^1].AltCommandThreads;
                    if (AltThreads.Count == 0 || AltThreads[^1].IsAlive)
                    {
                        var WrappedCommand = new KernelThread($"Wrapped Shell Command Thread", false, (cmdThreadParams) => CommandExecutor.ExecuteCommand((CommandExecutor.ExecuteCommandParameters)cmdThreadParams));
                        ShellStart.ShellStack[^1].AltCommandThreads.Add(WrappedCommand);
                    }
                    Shell.GetLine(StringArgs, WrapOutputPath);
                    var WrapOutputStream = new StreamReader(WrapOutputPath);
                    string WrapOutput = WrapOutputStream.ReadToEnd();
                    TextWriterWrappedColor.WriteWrapped(WrapOutput, false, KernelColorType.NeutralText);
                    if (!WrapOutput.EndsWith(CharManager.NewLine))
                        TextWriterColor.Write();
                    WrapOutputStream.Close();
                    File.Delete(WrapOutputPath);
                }
                else
                {
                    var WrappableCmds = new ArrayList();
                    foreach (CommandInfo CommandInfo in Shell.GetShellInfo(ShellType.Shell).Commands.Values)
                    {
                        if (CommandInfo.Flags.HasFlag(CommandFlags.Wrappable))
                            WrappableCmds.Add(CommandInfo.Command);
                    }
                    TextWriterColor.Write(Translate.DoTranslation("The command is not wrappable. These commands are wrappable:") + " {0}", true, KernelColorType.Error, string.Join(", ", WrappableCmds.ToArray()));
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("The wrappable command is not found."), true, KernelColorType.Error);
            }
        }

        public override void HelpHelper()
        {
            // Get wrappable commands
            var WrappableCmds = new ArrayList();
            foreach (CommandInfo CommandInfo in Shell.GetShellInfo(ShellType.Shell).Commands.Values)
            {
                if (CommandInfo.Flags.HasFlag(CommandFlags.Wrappable))
                    WrappableCmds.Add(CommandInfo.Command);
            }

            // Print them along with help description
            TextWriterColor.Write(Translate.DoTranslation("Wrappable commands:") + " {0}", string.Join(", ", WrappableCmds.ToArray()));
        }

    }
}
