
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
using KS.Languages;
using KS.Shell.ShellBase.Arguments;

namespace KS.Arguments.ArgumentBase
{
    /// <summary>
    /// Argument information class
    /// </summary>
    public class ArgumentInfo
    {

        /// <summary>
        /// The argument
        /// </summary>
        public string Argument { get; private set; }
        /// <summary>
        /// The untranslated help definition of argument. Translated by <see cref="GetTranslatedHelpEntry()"/>
        /// </summary>
        public string HelpDefinition { get; set; }
        /// <summary>
        /// Argument info
        /// </summary>
        public CommandArgumentInfo[] ArgArgumentInfo { get; private set; }
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
        /// <param name="HelpDefinition">Argument help definition</param>
        /// <param name="ArgArgumentInfo">Argument info</param>
        /// <param name="ArgumentBase">Kernel argument base for execution</param>
        /// <param name="Obsolete">Is the command obsolete?</param>
        /// <param name="AdditionalHelpAction">An extra help action intended to show extra information</param>
        public ArgumentInfo(string Argument, string HelpDefinition, CommandArgumentInfo[] ArgArgumentInfo, ArgumentExecutor ArgumentBase, bool Obsolete = false, Action AdditionalHelpAction = null)
        {
            this.Argument = Argument;
            this.HelpDefinition = HelpDefinition;
            this.ArgArgumentInfo = ArgArgumentInfo;
            this.ArgumentBase = ArgumentBase;
            this.Obsolete = Obsolete;
            this.AdditionalHelpAction = AdditionalHelpAction;
        }

        /// <summary>
        /// Gets the translated version of help entry (KS built-in arguments only)
        /// </summary>
        public string GetTranslatedHelpEntry() => Translate.DoTranslation(HelpDefinition);

    }
}
