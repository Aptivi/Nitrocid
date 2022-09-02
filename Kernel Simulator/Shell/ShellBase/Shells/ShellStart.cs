
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
using System.Collections.Generic;
using KS.Languages;
using KS.Misc.Threading;
using KS.Shell.ShellBase.Commands;
using KS.Shell.Shells.Archive;
using KS.Shell.Shells.FTP;
using KS.Shell.Shells.Hex;
using KS.Shell.Shells.HTTP;
using KS.Shell.Shells.Json;
using KS.Shell.Shells.Mail;
using KS.Shell.Shells.Rar;
using KS.Shell.Shells.RSS;
using KS.Shell.Shells.SFTP;
using KS.Shell.Shells.Text;
using KS.Shell.Shells.UESH;
using KS.Shell.Shells.Zip;
using ReadLineReboot;

namespace KS.Shell.ShellBase.Shells
{
    public static class ShellStart
    {

        internal static List<ShellInfo> ShellStack = new();

        /// <summary>
        /// Starts the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellArgs">Arguments to pass to shell</param>
        public static void StartShell(ShellType ShellType, params object[] ShellArgs)
        {
            if (ShellStack.Count >= 1)
            {
                // The shell stack has a mother shell. Start another shell.
                StartShellForced(ShellType, ShellArgs);
            }
        }

        /// <summary>
        /// Force starts the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellArgs">Arguments to pass to shell</param>
        public static void StartShellForced(ShellType ShellType, params object[] ShellArgs)
        {
            // Make a shell executor based on shell type to select a specific executor (if the shell type is not UESH, and if the new shell isn't a mother shell)
            // Please note that the remote debug shell is not supported because it works on its own space, so it can't be interfaced using the standard IShell.
            var ShellExecute = GetShellExecutor(ShellType);

            // Make a new instance of shell information
            var ShellCommandThread = new KernelThread($"{ShellType} Command Thread", false, (cmdThreadParams) => GetCommand.ExecuteCommand((GetCommand.ExecuteCommandParameters)cmdThreadParams));
            var ShellInfo = new ShellInfo(ShellType, ShellExecute, ShellCommandThread);

            // Now, initialize the command autocomplete handler. This will not be invoked if we have auto completion disabled.
            ReadLine.AutoCompletionHandler = new CommandAutoComplete(ShellType);

            // Add a new shell to the shell stack to indicate that we have a new shell (a visitor)!
            ShellStack.Add(ShellInfo);
            ShellExecute.InitializeShell(ShellArgs);
        }

        /// <summary>
        /// Kills the last running shell
        /// </summary>
        public static void KillShell()
        {
            // We must have at least two shells to kill the last shell. Else, we will have zero shells running, making us look like we've logged out!
            if (ShellStack.Count >= 2)
            {
                ShellStack[ShellStack.Count - 1].ShellExecutor.Bail = true;
                PurgeShells();
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("Can not kill the mother shell!"));
            }
        }

        /// <summary>
        /// Force kills the last running shell
        /// </summary>
        public static void KillShellForced()
        {
            if (ShellStack.Count >= 1)
            {
                ShellStack[ShellStack.Count - 1].ShellExecutor.Bail = true;
                PurgeShells();
            }
        }

        /// <summary>
        /// Cleans up the shell stack
        /// </summary>
        public static void PurgeShells()
        {
            // Remove these shells from the stack
            ShellStack.RemoveAll(x => x.ShellExecutor.Bail == true);
        }

        /// <summary>
        /// Gets the shell executor based on the shell type
        /// </summary>
        /// <param name="ShellType">The requested shell type</param>
        public static ShellExecutor GetShellExecutor(ShellType ShellType)
        {
            switch (ShellType)
            {
                case ShellType.Shell:
                    {
                        return new UESHShell();
                    }
                case ShellType.FTPShell:
                    {
                        return new FTPShell();
                    }
                case ShellType.MailShell:
                    {
                        return new MailShell();
                    }
                case ShellType.SFTPShell:
                    {
                        return new SFTPShell();
                    }
                case ShellType.TextShell:
                    {
                        return new TextShell();
                    }
                case ShellType.TestShell:
                    {
                        return new KS.Shell.Shells.Test.TestShell();
                    }
                case ShellType.ZIPShell:
                    {
                        return new ZipShell();
                    }
                case ShellType.RSSShell:
                    {
                        return new RSSShell();
                    }
                case ShellType.JsonShell:
                    {
                        return new JsonShell();
                    }
                case ShellType.HTTPShell:
                    {
                        return new HTTPShell();
                    }
                case ShellType.HexShell:
                    {
                        return new HexShell();
                    }
                case ShellType.RARShell:
                    {
                        return new RarShell();
                    }
                case ShellType.ArchiveShell:
                    {
                        return new ArchiveShell();
                    }

                default:
                    {
                        return new UESHShell();
                    }
            }
        }

    }
}
