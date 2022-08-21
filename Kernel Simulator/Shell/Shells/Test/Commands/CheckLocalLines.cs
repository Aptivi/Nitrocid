
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

using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using Newtonsoft.Json.Linq;
using System;

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// Checks the localization files to see if the line numbers in them are all equal.
    /// </summary>
    class Test_CheckLocalLinesCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var EnglishJson = JToken.Parse(Properties.Resources.Resources.eng);
            JToken LanguageJson;
            foreach (string LanguageName in LanguageManager.Languages.Keys)
            {
                LanguageJson = JToken.Parse(Properties.Resources.Resources.ResourceManager.GetString(LanguageName.Replace("-", "_")));
                if (LanguageJson.Count() != EnglishJson.Count())
                {
                    TextWriterColor.Write(Translate.DoTranslation("Line mismatch in") + " {0}: {1} <> {2}", true, ColorTools.ColTypes.Warning, LanguageName, LanguageJson.Count(), EnglishJson.Count());
                }
            }
        }

    }
}
