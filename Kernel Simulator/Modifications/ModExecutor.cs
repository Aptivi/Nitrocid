using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Login;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

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
            Kernel.Kernel.KernelEventManager.RaisePreExecuteModCommand(cmd);

            // Variables
            var parts = cmd.SplitEncloseDoubleQuotes(" ");
            string args = "";
            string actualCmd = parts[0];
            DebugWriter.Wdbg(DebugLevel.I, "Command = {0}", actualCmd);

            // Check to see if the command written needs normalization
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    var script = PartInfo.PartScript;
                    if (script.Commands is not null)
                    {
                        if (script.Commands.ContainsKey(actualCmd) & !string.IsNullOrEmpty(script.Name) & (actualCmd ?? "") != (script.Name ?? ""))
                        {
                            // The commands in the script has the actual command, the mod name is not null, and the command doesn't equal the mod name.
                            // In this case, make the actual command executed the script name.
                            actualCmd = script.Name;
                            DebugWriter.Wdbg(DebugLevel.I, "Actual command = {0}", actualCmd);
                        }
                    }
                }
            }

            // Prepare the argument string.
            if (cmd.StartsWith(parts[0] + " ") | cmd.StartsWith("\"" + parts[0] + "\" "))
            {
                // These below will be executed if there are arguments
                args = cmd.Replace($"{parts[0]} ", "").Replace($"\"{parts[0]}\" ", "");
                DebugWriter.Wdbg(DebugLevel.I, "Command {0} will be run with arguments: {1}", actualCmd, args);
            }

            // Try to execute the command.
            var ScriptCommandExecutable = default(bool);
            foreach (string ModPart in ModManager.Mods[actualCmd].ModParts.Keys)
            {
                var Script = ModManager.Mods[actualCmd].ModParts[ModPart].PartScript;
                if (Script.Commands is not null)
                {
                    // Found commands dictionary! Now, check it for the command
                    if (Script.Commands.ContainsKey(parts[0]))
                    {
                        // Populate the arguments info and command base variables
                        var ScriptCommandBase = Script.Commands[parts[0]].CommandBase;
                        var ScriptCommandArgsInfo = new ProvidedCommandArgumentsInfo(cmd, Script.Commands[parts[0]].Type);
                        var ScriptCommandArgs = ScriptCommandArgsInfo.ArgumentsList;
                        var ScriptCommandSwitches = ScriptCommandArgsInfo.SwitchesList;

                        // Check to see if we're in the shell type command.
                        if (Script.Commands[parts[0]].Type == ShellType.Shell)
                        {
                            // Command type is of shell. Check the user privileges for restricted commands.
                            if (Script.Commands[parts[0]].Flags.HasFlag(CommandFlags.Strict) & PermissionManagement.HasPermission(Login.Login.CurrentUser.Username, PermissionManagement.PermissionType.Administrator) | !Script.Commands[parts[0]].Flags.HasFlag(CommandFlags.Strict))
                            {
                                // User is authorized to use the command, or the command isn't strict
                                ph0b6dd4784e9d4a609316b142b38bf53e = true;
                            }
                            else
                            {
                                // User wasn't authorized.
                                DebugWriter.Wdbg(DebugLevel.E, "User {0} doesn't have permission to use {1} from {2}!", Login.Login.CurrentUser.Username, parts[0], ModPart);
                                TextWriterColor.Write(Translate.DoTranslation("You don't have permission to use {0}"), true, ColorTools.ColTypes.Error, parts[0]);
                            }
                        }
                        else
                        {
                            // Command type is not of shell. Execute anyway.
                            ph0b6dd4784e9d4a609316b142b38bf53e = true;
                        }

                        // If the command check went all well without any hiccups, execute the command.
                        if (ph0b6dd4784e9d4a609316b142b38bf53e)
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Using command {0} from {1} to be executed...", parts[0], ModPart);
                            if (ScriptCommandBase is not null)
                            {
                                // Use the modern CommandBase.Execute() command
                                ScriptCommandBase.Execute(args, ScriptCommandArgs, ScriptCommandSwitches);
                            }
                        }
                    }
                }
            }

            // Raise event
            Kernel.Kernel.KernelEventManager.RaisePostExecuteModCommand(cmd);
            DebugWriter.Wdbg(DebugLevel.I, "Command executed successfully.");
        }

    }
}