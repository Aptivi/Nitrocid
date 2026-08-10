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
    class ScoreSimCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "scoresim";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SCORESIM_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo([
                    new SwitchInfo("soccer", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SCORESIM_SWITCH_SOCCER_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["basketball"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("basketball", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SCORESIM_SWITCH_BASKETBALL_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["soccer"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("firstTeamName", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SCORESIM_SWITCH_FIRSTTEAMNAME_DESC"),
                    new SwitchInfo("secondTeamName", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SCORESIM_SWITCH_SECONDTEAMNAME_DESC"),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            bool isSoccer = parameters.ContainsSwitch("-soccer");
            bool isBasketball = parameters.ContainsSwitch("-basketball");
            string firstTeamName = parameters.GetSwitchValue("-firstTeamName");
            string secondTeamName = parameters.GetSwitchValue("-secondTeamName");
            int mode = isSoccer ? 1 : isBasketball ? 2 : 0;
            ScoreSim.InitializeScoreSim(mode, firstTeamName, secondTeamName);
            return 0;
        }
    }
}
