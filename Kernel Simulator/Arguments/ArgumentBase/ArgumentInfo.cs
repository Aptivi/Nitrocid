using System;
using KS.Languages;

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

namespace KS.Arguments.ArgumentBase
{
    public class ArgumentInfo
    {

        /// <summary>
        /// The argument
        /// </summary>
        public string Argument { get; private set; }
        /// <summary>
        /// The type of argument
        /// </summary>
        public ArgumentType Type { get; private set; }
        /// <summary>
        /// The untranslated help definition of argument. Translated by <see cref="GetTranslatedHelpEntry()"/>
        /// </summary>
        public string HelpDefinition { get; set; }
        /// <summary>
        /// The help usage of command.
        /// </summary>
        public string HelpUsage { get; private set; }
        /// <summary>
        /// Does the argument require arguments?
        /// </summary>
        public bool ArgumentsRequired { get; private set; }
        /// <summary>
        /// User must specify at least this number of arguments
        /// </summary>
        public int MinimumArguments { get; private set; }
        /// <summary>
        /// Kernel argument base for execution
        /// </summary>
        public ArgumentExecutor ArgumentBase { get; private set; }
        /// <summary>
        /// Is the argument obsolete?
        /// </summary>
        public bool Obsolete { get; private set; }
        /// <summary>
        /// An extra help action intended to show extra information
        /// </summary>
        public Action AdditionalHelpAction { get; private set; }

        /// <summary>
        /// Installs a new instance of argument info class
        /// </summary>
        /// <param name="Argument">Argument</param>
        /// <param name="Type">Argument type</param>
        /// <param name="HelpDefinition">Argument help definition</param>
        /// <param name="HelpUsage">Command help usage</param>
        /// <param name="ArgumentsRequired">Does the argument require arguments?</param>
        /// <param name="MinimumArguments">User must specify at least this number of arguments</param>
        /// <param name="ArgumentBase">Kernel argument base for execution</param>
        /// <param name="Obsolete">Is the command obsolete?</param>
        /// <param name="AdditionalHelpAction">An extra help action intended to show extra information</param>
        public ArgumentInfo(string Argument, ArgumentType Type, string HelpDefinition, string HelpUsage, bool ArgumentsRequired, int MinimumArguments, ArgumentExecutor ArgumentBase, bool Obsolete = false, Action AdditionalHelpAction = null)
        {
            this.Argument = Argument;
            this.Type = Type;
            this.HelpDefinition = HelpDefinition;
            this.HelpUsage = HelpUsage;
            this.ArgumentsRequired = ArgumentsRequired;
            this.MinimumArguments = MinimumArguments;
            this.ArgumentBase = ArgumentBase;
            this.Obsolete = Obsolete;
            this.AdditionalHelpAction = AdditionalHelpAction;
        }

        /// <summary>
        /// Gets the translated version of help entry (KS built-in arguments only)
        /// </summary>
        /// <returns></returns>
        public string GetTranslatedHelpEntry()
        {
            return Translate.DoTranslation(HelpDefinition);
        }

    }
}