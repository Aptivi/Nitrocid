
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

using System;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace Nitrocid.Extras.GitShell.Git
{
    /// <summary>
    /// The Git editor shell
    /// </summary>
    public class GitShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "GitShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Get repo path
            string RepoPath = "";
            if (ShellArgs.Length > 0)
            {
                RepoPath = Convert.ToString(ShellArgs[0]);
            }
            else
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Repository not specified. Exiting shell..."), true, KernelColorType.Error);
                Bail = true;
            }

            // Open repo
            if (GitShellCommon.repo is null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Repo not open yet. Trying to open {0}...", RepoPath);
                if (!GitShellCommon.OpenRepository(RepoPath))
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Failed to open repository. Exiting shell..."), true, KernelColorType.Error);
                    Bail = true;
                }
            }

            // Actual shell logic
            while (!Bail)
            {
                try
                {
                    // Prompt for the command
                    ShellManager.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    CancellationHandlers.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("There was an error in the shell.") + CharManager.NewLine + "Error {0}: {1}", true, KernelColorType.Error, ex.GetType().FullName, ex.Message);
                    continue;
                }
            }

            // Close repo
            GitShellCommon.CloseRepository();
        }

    }
}
