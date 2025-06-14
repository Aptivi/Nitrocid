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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.Misc.Progress;
using System.Threading;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class TestProgressHandler : TestFacade
    {
        public override string TestName => LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_TESTPROGRESSHANDLER_DESC");
        public override void Run()
        {
            int progress = 0;
            var handler = new ProgressHandler((num, text) =>
                TextWriters.Write($"{num}% {text}", KernelColorType.Progress)
            , "General");
            var handler2 = new ProgressHandler((num, text) =>
                TextWriters.Write($"{num}% {text}", KernelColorType.NeutralText)
            , "Nongeneral");
            ProgressManager.RegisterProgressHandler(handler);
            ProgressManager.RegisterProgressHandler(handler2);
            while (progress != 100)
            {
                Thread.Sleep(100);
                progress += 1;
                ProgressManager.ReportProgress(progress, "General", LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_TESTPROGRESSHANDLER_TEST"));
                ProgressManager.ReportProgress(progress, "Nongeneral", LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_TESTPROGRESSHANDLER_TEST"));
            }
            ProgressManager.UnregisterProgressHandler(handler);
            ProgressManager.UnregisterProgressHandler(handler2);
        }
    }
}
