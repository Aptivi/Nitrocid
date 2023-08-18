
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

using KS.Languages;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// Command information class
    /// </summary>
    public class CommandInfo
    {

        /// <summary>
        /// The command
        /// </summary>
        public string Command { get; private set; }
        /// <summary>
        /// The type of command
        /// </summary>
        public string Type { get; private set; }
        /// <summary>
        /// The untranslated help definition of command. Translated by <see cref="GetTranslatedHelpEntry()"/>
        /// </summary>
        public string HelpDefinition { get; set; }
        /// <summary>
        /// Command argument info
        /// </summary>
        public CommandArgumentInfo[] CommandArgumentInfo { get; private set; }
        /// <summary>
        /// Command base for execution
        /// </summary>
        public BaseCommand CommandBase { get; private set; }
        /// <summary>
        /// Command properties
        /// </summary>
        public CommandFlags Flags { get; private set; }

        /// <summary>
        /// Installs a new instance of command info class
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="Type">Shell command type</param>
        /// <param name="HelpDefinition">Command help definition</param>
        /// <param name="CommandArgumentInfo">Command argument info</param>
        /// <param name="CommandBase">Command base for execution</param>
        /// <param name="Flags">Command flags</param>
        public CommandInfo(string Command, ShellType Type, string HelpDefinition, CommandArgumentInfo[] CommandArgumentInfo, BaseCommand CommandBase, CommandFlags Flags = CommandFlags.None) :
            this(Command, ShellManager.GetShellTypeName(Type), HelpDefinition, CommandArgumentInfo, CommandBase, Flags)
        { }

        /// <summary>
        /// Installs a new instance of command info class
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="Type">Shell command type</param>
        /// <param name="HelpDefinition">Command help definition</param>
        /// <param name="CommandArgumentInfo">Command argument info</param>
        /// <param name="CommandBase">Command base for execution</param>
        /// <param name="Flags">Command flags</param>
        public CommandInfo(string Command, string Type, string HelpDefinition, CommandArgumentInfo[] CommandArgumentInfo, BaseCommand CommandBase, CommandFlags Flags = CommandFlags.None)
        {
            this.Command = Command;
            this.Type = Type;
            this.HelpDefinition = HelpDefinition;
            this.CommandArgumentInfo = CommandArgumentInfo;
            this.CommandBase = CommandBase;
            this.Flags = Flags;
        }

        /// <summary>
        /// Gets the translated version of help entry (KS built-in commands only)
        /// </summary>
        public string GetTranslatedHelpEntry() => Translate.DoTranslation(HelpDefinition);

    }
}
