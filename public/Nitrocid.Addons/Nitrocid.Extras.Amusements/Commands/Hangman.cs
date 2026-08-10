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

using Nitrocid.Base.Languages;
using Nitrocid.Extras.Amusements.Amusements.Games;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;

namespace Nitrocid.Extras.Amusements.Commands
{
    class HangmanCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "hangman";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_HANGMAN_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo([
                    new SwitchInfo("hardcore", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_HANGMAN_SWITCH_HARDCORE_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["practice"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("practice", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_HANGMAN_SWITCH_PRACTICE_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["hardcore"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("common", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SWITCH_COMMON_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["uncommon"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("uncommon", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SWITCH_UNCOMMON_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["common"],
                        AcceptsValues = false
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var difficulty =
                parameters.ContainsSwitch("-hardcore") ? HangmanDifficulty.Hardcore :
                parameters.ContainsSwitch("-practice") ? HangmanDifficulty.Practice :
                HangmanDifficulty.None;
            var wordDifficulty =
                parameters.ContainsSwitch("-uncommon") ? HangmanWordDifficulty.Uncommon :
                HangmanWordDifficulty.Common;
            Hangman.InitializeHangman(difficulty, wordDifficulty);
            return 0;
        }
    }
}
