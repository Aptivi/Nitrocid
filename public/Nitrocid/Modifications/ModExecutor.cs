
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

using KS.Kernel.Debugging;
using KS.Shell.ShellBase.Commands;
using KS.Kernel.Events;
using KS.Misc.Text;

namespace KS.Modifications
{
    static class ModExecutor
    {

        /// <summary>
        /// Executes the command provided by a mod
        /// </summary>
        /// <param name="cmd">A mod command with arguments</param>
        public static void ExecuteModCommand(string cmd)
        {
            EventsManager.FireEvent(EventType.PreExecuteModCommand, cmd);

            // Variables
            var parts = cmd.SplitEncloseDoubleQuotes();
            string actualCmd = parts[0];
            DebugWriter.WriteDebug(DebugLevel.I, "Command = {0}", actualCmd);

            // Check to see if the command written needs normalization
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (ModPartInfo PartInfo in ModPart.ModParts.Values)
                {
                    var script = PartInfo.PartScript;
                    if (script.Commands is not null)
                    {
                        if (script.Commands.ContainsKey(actualCmd) & !string.IsNullOrEmpty(script.Name) & actualCmd != script.Name)
                        {
                            // The commands in the script has the actual command, the mod name is not null, and the command doesn't equal the mod name.
                            // In this case, make the actual command executed the script name.
                            actualCmd = script.Name;
                            DebugWriter.WriteDebug(DebugLevel.I, "Actual command = {0}", actualCmd);
                        }
                    }
                }
            }

            // Try to execute the command.
            foreach (var mod in ModManager.Mods.Values)
            {
                foreach (var modPart in mod.ModParts.Values)
                {
                    var Script = modPart.PartScript;
                    if (Script.Commands is not null)
                    {
                        // Found commands dictionary! Now, check it for the command
                        if (Script.Commands.ContainsKey(parts[0]))
                        {
                            var cmdInfo = Script.Commands[parts[0]];
                            var Params = new CommandExecutor.ExecuteCommandParameters(cmd, cmdInfo.Type)
                            {
                                CustomCommand = true,
                                ModCommands = Script.Commands
                            };
                            CommandExecutor.StartCommandThread(Params);
                        }
                    }
                }
            }

            // Raise event
            EventsManager.FireEvent(EventType.PostExecuteModCommand, cmd);
            DebugWriter.WriteDebug(DebugLevel.I, "Command executed successfully.");
        }

    }
}
