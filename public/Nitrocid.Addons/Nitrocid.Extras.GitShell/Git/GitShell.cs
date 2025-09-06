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

using System;
using System.Threading;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

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
                RepoPath = Convert.ToString(ShellArgs[0]) ?? "";
                if (string.IsNullOrEmpty(RepoPath))
                {
                    TextWriters.Write(Translate.DoTranslation("Repository not specified. Exiting shell..."), true, KernelColorType.Error);
                    Bail = true;
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Repository not specified. Exiting shell..."), true, KernelColorType.Error);
                Bail = true;
            }

            // Open repo
            if (GitShellCommon.repo is null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Repo not open yet. Trying to open {0}...", vars: [RepoPath]);
                if (!GitShellCommon.OpenRepository(RepoPath))
                {
                    TextWriters.Write(Translate.DoTranslation("Failed to open repository. Exiting shell..."), true, KernelColorType.Error);
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
                    TextWriters.Write(Translate.DoTranslation("There was an error in the shell.") + " {0}", true, KernelColorType.Error, ex.Message);
                    DebugWriter.WriteDebug(DebugLevel.E, "Shell will have to exit: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    InputTools.DetectKeypress();
                    Bail = true;
                }
            }

            // Close repo
            GitShellCommon.CloseRepository();
        }

    }
}
