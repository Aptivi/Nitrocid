//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
        /// Argument options
        /// </summary>
        public CommandArgumentPartOptions Options { get; private set; }

        /// <summary>
        /// Installs a new instance of the command argument part class
        /// </summary>
        /// <param name="argumentExpression">Command argument expression</param>
        /// <param name="argumentRequired">Is this argument part required?</param>
        /// <param name="options">Command argument part options</param>
        public CommandArgumentPart(bool argumentRequired, string argumentExpression, CommandArgumentPartOptions options)
        {
            // Install some values
            ArgumentRequired = argumentRequired;
            ArgumentExpression = argumentExpression;
            Options = options ?? new CommandArgumentPartOptions();
        }

        /// <summary>
        /// Installs a new instance of the command argument part class
        /// </summary>
        /// <param name="argumentExpression">Command argument expression</param>
        /// <param name="argumentRequired">Is this argument part required?</param>
        /// <param name="autoCompleter">Auto completion function</param>
        /// <param name="isNumeric">Specifies whether the argument accepts only numbers (and dots for float values)</param>
        /// <param name="exactWording">User is required to provide this exact wording</param>
        /// <param name="argumentDesc">Argument description (unlocalized)</param>
        public CommandArgumentPart(bool argumentRequired, string argumentExpression, Func<string[], string[]>? autoCompleter = null, bool isNumeric = false, string[]? exactWording = null, string argumentDesc = "")
        {
            ArgumentRequired = argumentRequired;
            ArgumentExpression = argumentExpression;

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
                    autoCompleter = (_) => [.. expressions];
                    done = true;
                }
            }

            // Populate options
            Options = new CommandArgumentPartOptions()
            {
                ArgumentDescription = argumentDesc,
                AutoCompleter = autoCompleter,
                ExactWording = exactWording ?? [],
                IsNumeric = isNumeric,
            };
        }

    }
}
