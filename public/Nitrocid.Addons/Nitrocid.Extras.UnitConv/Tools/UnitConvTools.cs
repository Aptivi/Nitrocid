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

using Nitrocid.Extras.UnitConv.Interactives;
using Nitrocid.Base.Languages;
using System;
using Terminaux.Inputs.Interactive;

namespace Nitrocid.Extras.UnitConv.Tools
{
    internal static class UnitConvTools
    {
        internal static void OpenUnitConvTui()
        {
            var tui = new UnitConverterCli();
            tui.Bindings.Add(new InteractiveTuiBinding<object>(LanguageTools.GetLocalized("NKS_UNITCONV_CLI_KEYBINDING_CONVERT"), ConsoleKey.F1, (_, _, _, _) => tui.OpenConvert()));
            InteractiveTuiTools.OpenInteractiveTui(tui);
        }
    }
}
