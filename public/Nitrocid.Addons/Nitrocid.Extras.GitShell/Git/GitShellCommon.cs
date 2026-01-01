//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using LibGit2Sharp;
using Nitrocid.Files.Operations.Querying;
using System.IO;

namespace Nitrocid.Extras.GitShell.Git
{
    /// <summary>
    /// Common Git editor shell module
    /// </summary>
    public static class GitShellCommon
    {

        internal static string branchName = "";
        internal static string repoPath = "";
        internal static Repository? repo = null;
        internal static string name = "";
        internal static string email = "";
        internal static bool isIdentified = false;

        /// <summary>
        /// Branch name for Git
        /// </summary>
        public static string BranchName =>
            branchName;

        /// <summary>
        /// Repository path for Git
        /// </summary>
        public static string RepoPath =>
            repoPath;

        /// <summary>
        /// Repository instance for Git
        /// </summary>
        public static Repository? Repository =>
            repo;

        /// <summary>
        /// Repository name for Git
        /// </summary>
        public static string RepoName =>
            Path.GetFileName(RepoPath);

        /// <summary>
        /// Opens a repository
        /// </summary>
        /// <param name="path">Path to a Git repository folder</param>
        /// <returns>True if successful; false otherwise.</returns>
        public static bool OpenRepository(string path)
        {
            // Check to see if the specified path exists
            if (!Checking.FolderExists(path))
                return false;

            // Now, check to see if we have the .git folder
            string repoGitPath = path + "/.git";
            if (!Checking.FolderExists(repoGitPath))
                return false;

            // Initialize the repository
            var repository = new Repository(repoGitPath);
            repo = repository;
            branchName = repo.Head.CanonicalName;
            return true;
        }

        /// <summary>
        /// Closes the repository
        /// </summary>
        /// <returns>True if successful; false if unsuccessful or already closed.</returns>
        public static bool CloseRepository()
        {
            // Check to see if we're open
            if (repo is null)
                return false;

            // Close it.
            repo.Dispose();
            repo = null;
            repoPath = "";
            return true;
        }

    }
}
