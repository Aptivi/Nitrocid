//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Terminaux.Colors.Themes.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Languages;
using System.Linq;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class CheckSettingsEntries : TestFacade
    {
        public override string TestName => LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_CHECKSETTINGSENTRIES_DESC");
        public override TestSection TestSection => TestSection.Kernel;
        public override bool TestInteractive => false;
        public override object TestExpectedValue => false;
        public override void Run()
        {
            var Results = ConfigTools.CheckConfigVariables();
            bool failed = Results.Any((res) => !res);
            if (failed)
                TextWriters.Write(LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_CHECKSETTINGSENTRIES_FAILED"), true, ThemeColorType.Warning);
            TestActualValue = failed;
        }
    }
}
