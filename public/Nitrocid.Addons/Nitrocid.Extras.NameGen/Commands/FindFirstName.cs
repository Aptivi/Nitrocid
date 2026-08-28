//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Linq;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Writer.ConsoleWriters;
using Textify.Data.NameGen;

namespace Nitrocid.Extras.NameGen.Commands
{
    /// <summary>
    /// First name generator
    /// </summary>
    /// <remarks>
    /// If you're stuck trying to make out your character names (male or female) in your story, or if you just like to generate names, you can use this command. Please note that it requires Internet access.
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-t</term>
    /// <description>Generate nametags (umlauts are currently not supported)</description>
    /// </item>
    /// <item>
    /// <term>-male</term>
    /// <description>Generate names using the male names list</description>
    /// </item>
    /// <item>
    /// <term>-female</term>
    /// <description>Generate names using the female names list</description>
    /// </item>
    /// <item>
    /// <term>-both</term>
    /// <description>Generate names using both male and female names list</description>
    /// </item>
    /// </list>
    /// </remarks>
    class FindFirstNameCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "findfirstname";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_NAMEGEN_COMMAND_FINDFIRSTNAME_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(false, "term", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_NAMEGEN_COMMAND_GENNAME_ARGUMENT_TERM_DESC"
                    }),
                    new CommandArgumentPart(false, "nameprefix", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_NAMEGEN_COMMAND_GENNAME_ARGUMENT_NAMEPREFIX_DESC"
                    }),
                    new CommandArgumentPart(false, "namesuffix", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_NAMEGEN_COMMAND_GENNAME_ARGUMENT_NAMESUFFIX_DESC"
                    }),
                ],
                [
                    new SwitchInfo("t", /* Localizable */ "NKS_NAMEGEN_COMMAND_GENNAME_SWITCH_NAMETAG_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("male", /* Localizable */ "NKS_NAMEGEN_COMMAND_GENNAME_SWITCH_MALE_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["female", "both"],
                        AcceptsValues = false,
                    }),
                    new SwitchInfo("female", /* Localizable */ "NKS_NAMEGEN_COMMAND_GENNAME_SWITCH_FEMALE_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["male", "both"],
                        AcceptsValues = false,
                    }),
                    new SwitchInfo("both", /* Localizable */ "NKS_NAMEGEN_COMMAND_GENNAME_SWITCH_UNIFIED_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["female", "male"],
                        AcceptsValues = false,
                    }),
                ], true)
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string term = "";
            string NamePrefix = "";
            string NameSuffix = "";
            bool nametags = parameters.SwitchesList.Contains("-t");
            NameGenderType genderType = NameGenderType.Unified;
            if (parameters.SwitchesList.Contains("-male"))
                genderType = NameGenderType.Male;
            else if (parameters.SwitchesList.Contains("-female"))
                genderType = NameGenderType.Female;
            string[] NamesList;
            if (parameters.ArgumentsList.Length >= 1)
                term = parameters.ArgumentsList[0];
            if (parameters.ArgumentsList.Length >= 2)
                NamePrefix = parameters.ArgumentsList[1];
            if (parameters.ArgumentsList.Length >= 3)
                NameSuffix = parameters.ArgumentsList[2];

            // Generate n names
            NameGenerator.PopulateNames();
            NamesList = NameGenerator.FindFirstNames(term, NamePrefix, NameSuffix, genderType);

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
