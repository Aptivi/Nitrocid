
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

using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Languages;
using System;
using System.Reflection;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class TestExecuteAssembly : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests assembly entry point execution");
        public override void Run()
        {
            string Text = Input.ReadLine(Translate.DoTranslation("Write a path to assembly file:") + " ", "");
            Text = Filesystem.NeutralizePath(Text);
            Assembly.LoadFrom(Text).EntryPoint.Invoke("", Array.Empty<object>());
        }
    }
}
