//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Linq;

namespace Nitrocid.Shell.ShellBase.Arguments
{
    /// <summary>
    /// Command argument part class
    /// </summary>
    public class CommandArgumentPart
    {

        /// <summary>
        /// Is this argument part required?
        /// </summary>
        public bool ArgumentRequired { get; private set; }
        /// <summary>
        /// Command argument expression
        /// </summary>
        public string ArgumentExpression { get; private set; }
        /// <summary>
        /// Auto completion function delegate
        /// </summary>
        public Func<string[], string[]> AutoCompleter { get; private set; }
        /// <summary>
        /// Command argument expression
        /// </summary>
        public bool IsNumeric { get; private set; }
        /// <summary>
        /// User is required to provide one of the exact wordings
        /// </summary>
        public string[] ExactWording { get; private set; }

        /// <summary>
        /// Installs a new instance of the command argument part class
        /// </summary>
        /// <param name="argumentExpression">Command argument expression</param>
        /// <param name="argumentRequired">Is this argument part required?</param>
        /// <param name="options">Command argument part options</param>
        public CommandArgumentPart(bool argumentRequired, string argumentExpression, CommandArgumentPartOptions options)
        {
            // Get a part instance
            options ??= new CommandArgumentPartOptions();
            var part = new CommandArgumentPart(argumentRequired, argumentExpression, options.AutoCompleter, options.IsNumeric, options.ExactWording);

            // Install some values
            ArgumentRequired = argumentRequired;
            ArgumentExpression = argumentExpression;

            // Install values from the instance
            AutoCompleter = part.AutoCompleter;
            IsNumeric = part.IsNumeric;
            ExactWording = part.ExactWording;
        }

        /// <summary>
        /// Installs a new instance of the command argument part class
        /// </summary>
        /// <param name="argumentExpression">Command argument expression</param>
        /// <param name="argumentRequired">Is this argument part required?</param>
        /// <param name="autoCompleter">Auto completion function</param>
        /// <param name="isNumeric">Specifies whether the argument accepts only numbers (and dots for float values)</param>
        /// <param name="exactWording">User is required to provide this exact wording</param>
        public CommandArgumentPart(bool argumentRequired, string argumentExpression, Func<string[], string[]> autoCompleter = null, bool isNumeric = false, string[] exactWording = null)
        {
            ArgumentRequired = argumentRequired;
            ArgumentExpression = argumentExpression;
            IsNumeric = isNumeric;

            // Check to see if the expression points to a known auto completion function
            if (!string.IsNullOrEmpty(argumentExpression))
            {
                bool done = false;

                // First, check to see if the auto completion is not null
                if (autoCompleter is not null)
                    done = true;

                // Then, the known auto complete lists
                var completion = CommandAutoCompletionList.GetCompletionFunction(argumentExpression);
                if (!done && completion is not null)
                {
                    autoCompleter = completion;
                    done = true;
                }

                // Then, the split by the slash (/) characters
                if (!done && argumentExpression.Contains('/'))
                {
                    string[] expressions = argumentExpression.Split('/');
                    autoCompleter = (_) => expressions.ToArray();
                    done = true;
                }
            }
            AutoCompleter = autoCompleter;
            ExactWording = exactWording ?? [];
        }

    }
}
