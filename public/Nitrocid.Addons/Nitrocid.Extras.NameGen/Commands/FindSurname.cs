
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

using System.Linq;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using Namer;
using static Namer.NameGenerator;

namespace Nitrocid.Extras.NameGen.Commands
{
    /// <summary>
    /// Surname generator
    /// </summary>
    /// <remarks>
    /// If you're stuck trying to make out your character names (male or female) in your story, or if you just like to generate names, you can use this command. Please note that it requires Internet access.
    /// </remarks>
    class FindSurnameCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string SurnamePrefix = "";
            string SurnameSuffix = "";
            bool nametags = parameters.SwitchesList.Contains("-t");
            NameGenderType genderType = NameGenderType.Unified;
            string[] NamesList;
            if (parameters.ArgumentsList.Length >= 1)
                SurnamePrefix = parameters.ArgumentsList[0];
            if (parameters.ArgumentsList.Length >= 2)
                SurnameSuffix = parameters.ArgumentsList[1];

            // Generate n names
            // TODO: Namer needs GenerateSurnames and FindSurnames in order for this to be more accurate
            PopulateNames();
            NamesList = GenerateNames(10, "", "", SurnamePrefix, SurnameSuffix, genderType);
            for (int i = 0; i < NamesList.Length; i++)
            {
                if (NamesList[i].Contains(' '))
                    NamesList[i] = NamesList[i][(NamesList[i].LastIndexOf(' ') + 1)..];
            }

            // Check to see if we need to modify the list to have nametags
            if (nametags)
                for (int i = 0; i < NamesList.Length; i++)
                    NamesList[i] = "@" + NamesList[i].ToLower().Replace(" ", ".");
            foreach (string name in NamesList)
                TextWriterColor.Write(name);
            variableValue = string.Join('\n', NamesList);
            return 0;
        }

    }
}
