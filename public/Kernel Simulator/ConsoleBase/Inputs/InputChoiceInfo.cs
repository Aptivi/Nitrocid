
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

namespace KS.ConsoleBase.Inputs
{
    /// <summary>
    /// Choice information for input
    /// </summary>
    public class InputChoiceInfo
    {
        /// <summary>
        /// Choice name
        /// </summary>
        public string ChoiceName { get; }

        /// <summary>
        /// Choice title
        /// </summary>
        public string ChoiceTitle { get; }

        /// <summary>
        /// Choice description
        /// </summary>
        public string ChoiceDescription { get; }

        /// <summary>
        /// Makes a new instance of choice information
        /// </summary>
        /// <param name="choiceName">Choice name</param>
        /// <param name="choiceTitle">Choice title</param>
        public InputChoiceInfo(string choiceName, string choiceTitle)
            : this(choiceName, choiceTitle, "") { }

        /// <summary>
        /// Makes a new instance of choice information
        /// </summary>
        /// <param name="choiceName">Choice name</param>
        /// <param name="choiceTitle">Choice title</param>
        /// <param name="choiceDescription">Choice description</param>
        public InputChoiceInfo(string choiceName, string choiceTitle, string choiceDescription)
        {
            ChoiceName = choiceName;
            ChoiceTitle = choiceTitle;
            ChoiceDescription = choiceDescription;
        }
    }
}
