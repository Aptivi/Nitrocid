
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

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Time.Renderers;
using KS.Languages;
using KS.Misc.Calendar;
using System;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class ShowDateDiffCalendar : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests printing date using different calendars");
        public override TestSection TestSection => TestSection.Kernel;
        public override void Run()
        {
            TextWriterColor.Write(Translate.DoTranslation("Make sure your terminal supports Right-to-Left rendering!") + "\n");
            var types = Enum.GetValues(typeof(CalendarTypes));
            foreach (var type in types)
                TextWriterColor.Write($"{(CalendarTypes)type}: {TimeDateRenderers.RenderDate(CalendarTools.GetCultureFromCalendar((CalendarTypes)type))}");
        }
    }
}
