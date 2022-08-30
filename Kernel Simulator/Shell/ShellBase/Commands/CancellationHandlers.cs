
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

using System;
using KS.Kernel;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.WriterBase;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.ShellBase.Commands
{
    static class CancellationHandlers
    {

        internal static object CancelSync = new(), EditorCancelSync = new(), FTPCancelSync = new(), HTTPCancelSync = new(), JsonShellCancelSync = new(), MailCancelSync = new(), RssShellCancelSync = new(), SFTPCancelSync = new(), TestCancelSync = new(), ZipShellCancelSync = new(), HexEditorCancelSync = new(), RarShellCancelSync = new();

        public static void CancelCommand(object sender, ConsoleCancelEventArgs e)
        {
            lock (GetCancelSyncLock(ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].ShellType))
            {
                if (e.SpecialKey == ConsoleSpecialKey.ControlC)
                {
                    Flags.CancelRequested = true;
                    TextWriterColor.Write();
                    WriterPlainManager.ChangePlain("Null");
                    e.Cancel = true;
                    var StartCommandThread = ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].ShellCommandThread;
                    StartCommandThread.Stop();
                    Shell.ProcessStartCommandThread.Stop();
                    WriterPlainManager.ChangePlain("Console");
                }
            }
        }

        public static object GetCancelSyncLock(ShellType ShellType)
        {
            switch (ShellType)
            {
                case ShellType.Shell:
                    {
                        return CancelSync;
                    }
                case ShellType.FTPShell:
                    {
                        return FTPCancelSync;
                    }
                case ShellType.MailShell:
                    {
                        return MailCancelSync;
                    }
                case ShellType.SFTPShell:
                    {
                        return SFTPCancelSync;
                    }
                case ShellType.TextShell:
                    {
                        return EditorCancelSync;
                    }
                case ShellType.TestShell:
                    {
                        return TestCancelSync;
                    }
                case ShellType.ZIPShell:
                    {
                        return ZipShellCancelSync;
                    }
                case ShellType.RSSShell:
                    {
                        return RssShellCancelSync;
                    }
                case ShellType.JsonShell:
                    {
                        return JsonShellCancelSync;
                    }
                case ShellType.HTTPShell:
                    {
                        return HTTPCancelSync;
                    }
                case ShellType.HexShell:
                    {
                        return HexEditorCancelSync;
                    }
                case ShellType.RARShell:
                    {
                        return RarShellCancelSync;
                    }

                default:
                    {
                        return CancelSync;
                    }
            }
        }

    }
}
