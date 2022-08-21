using System.Collections.Generic;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

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

using static Namer.NameGenerator;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Name generator
    /// </summary>
    /// <remarks>
    /// If you're stuck trying to make out your character names (male or female) in your story, or if you just like to generate names, you can use this command. Please note that it requires Internet access.
    /// </remarks>
    class GenNameCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            int NamesCount = 10;
            string NamePrefix = "";
            string NameSuffix = "";
            string SurnamePrefix = "";
            string SurnameSuffix = "";
            List<string> NamesList;
            if (ListArgsOnly.Length >= 1)
                NamesCount = int.Parse(ListArgsOnly[0]);
            if (ListArgsOnly.Length >= 2)
                NamePrefix = ListArgsOnly[1];
            if (ListArgsOnly.Length >= 3)
                NameSuffix = ListArgsOnly[2];
            if (ListArgsOnly.Length >= 4)
                SurnamePrefix = ListArgsOnly[3];
            if (ListArgsOnly.Length >= 5)
                SurnameSuffix = ListArgsOnly[4];

            // Generate n names
            PopulateNames();
            NamesList = GenerateNames(NamesCount, NamePrefix, NameSuffix, SurnamePrefix, SurnameSuffix);
            ListWriterColor.WriteList(NamesList);
        }

    }
}