
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
using System.Data;
using System.IO;
using System.Linq;
using Extensification.StringExts;
using KS.Files;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.Shells.Rar;
using SharpCompress.Archives.Rar;
using SharpCompress.Readers;

namespace KS.Misc.RarFile
{
    public static class RarTools
    {

        /// <summary>
        /// Lists all RAR entries according to the target directory or the current directory
        /// </summary>
        /// <param name="Target">Target directory in an archive</param>
        public static List<RarArchiveEntry> ListRarEntries(string Target)
        {
            if (string.IsNullOrWhiteSpace(Target))
                Target = RarShellCommon.RarShell_CurrentArchiveDirectory;
            var Entries = new List<RarArchiveEntry>();
            foreach (RarArchiveEntry ArchiveEntry in RarShellCommon.RarShell_RarArchive?.Entries)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Parsing entry {0}...", ArchiveEntry.Key);
                if (Target is not null)
                {
                    if (ArchiveEntry.Key.StartsWith(Target))
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Entry {0} found in target {1}. Adding...", ArchiveEntry.Key, Target);
                        Entries.Add(ArchiveEntry);
                    }
                }
                else if (Target is null)
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Adding entry {0}...", ArchiveEntry.Key);
                    Entries.Add(ArchiveEntry);
                }
            }
            DebugWriter.Wdbg(DebugLevel.I, "Entries: {0}", Entries.Count);
            return Entries;
        }

        /// <summary>
        /// Extracts a RAR entry to a target directory
        /// </summary>
        /// <param name="Target">Target file in an archive</param>
        /// <param name="Where">Where in the local filesystem to extract?</param>
        public static bool ExtractRarFileEntry(string Target, string Where, bool FullTargetPath = false)
        {
            if (string.IsNullOrWhiteSpace(Target))
                throw new ArgumentException(Translate.DoTranslation("Can't extract nothing."));
            if (string.IsNullOrWhiteSpace(Where))
                Where = RarShellCommon.RarShell_CurrentDirectory;

            // Define absolute target
            string AbsoluteTarget = RarShellCommon.RarShell_CurrentArchiveDirectory + "/" + Target;
            if (AbsoluteTarget.StartsWith("/"))
                AbsoluteTarget = AbsoluteTarget.RemoveLetter(0);
            DebugWriter.Wdbg(DebugLevel.I, "Target: {0}, AbsoluteTarget: {1}", Target, AbsoluteTarget);

            // Define local destination while getting an entry from target
            string LocalDestination = Where + "/";
            var RarEntry = RarShellCommon.RarShell_RarArchive.Entries.Where(x => (x.Key ?? "") == (AbsoluteTarget ?? "")).ToArray()[0];
            if (FullTargetPath)
            {
                LocalDestination += RarEntry.Key;
            }
            DebugWriter.Wdbg(DebugLevel.I, "Where: {0}", LocalDestination);

            // Try to extract file
            Directory.CreateDirectory(LocalDestination);
            Making.MakeFile(LocalDestination + RarEntry.Key);
            RarShellCommon.RarShell_FileStream.Seek(0L, SeekOrigin.Begin);
            var RarReader = ReaderFactory.Open(RarShellCommon.RarShell_FileStream);
            while (RarReader.MoveToNextEntry())
            {
                if ((RarReader.Entry.Key ?? "") == (RarEntry.Key ?? "") & !RarReader.Entry.IsDirectory)
                {
                    RarReader.WriteEntryToFile(LocalDestination + RarEntry.Key);
                }
            }
            return true;
        }

        /// <summary>
        /// Changes the working archive directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        public static bool ChangeWorkingArchiveDirectory(string Target)
        {
            if (string.IsNullOrWhiteSpace(Target))
                Target = RarShellCommon.RarShell_CurrentArchiveDirectory;

            // Check to see if we're going back
            if (Target.Contains(".."))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Target contains going back. Counting...");
                var CADSplit = RarShellCommon.RarShell_CurrentArchiveDirectory.Split('/').ToList();
                var TargetSplit = Target.Split('/').ToList();
                var CADBackSteps = default(int);

                // Add back steps if target is ".."
                DebugWriter.Wdbg(DebugLevel.I, "Target length: {0}", TargetSplit.Count);
                for (int i = 0, loopTo = TargetSplit.Count - 1; i <= loopTo; i++)
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Target part {0}: {1}", i, TargetSplit[i]);
                    if (TargetSplit[i] == "..")
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Target is going back. Adding step...");
                        CADBackSteps += 1;
                        TargetSplit[i] = "";
                        DebugWriter.Wdbg(DebugLevel.I, "Steps: {0}", CADBackSteps);
                    }
                }

                // Remove empty strings
                TargetSplit.RemoveAll(x => string.IsNullOrEmpty(x));
                DebugWriter.Wdbg(DebugLevel.I, "Target length: {0}", TargetSplit.Count);

                // Remove every last entry that goes back
                DebugWriter.Wdbg(DebugLevel.I, "Old CADSplit length: {0}", CADSplit.Count);
                for (int Steps = CADBackSteps; Steps >= 1; Steps -= 1)
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Current step: {0}", Steps);
                    DebugWriter.Wdbg(DebugLevel.I, "Removing index {0} from CADSplit...", CADSplit.Count - Steps);
                    CADSplit.RemoveAt(CADSplit.Count - Steps);
                    DebugWriter.Wdbg(DebugLevel.I, "New CADSplit length: {0}", CADSplit.Count);
                }

                // Set current archive directory and target
                RarShellCommon.RarShell_CurrentArchiveDirectory = string.Join("/", CADSplit);
                DebugWriter.Wdbg(DebugLevel.I, "Setting CAD to {0}...", RarShellCommon.RarShell_CurrentArchiveDirectory);
                Target = string.Join("/", TargetSplit);
                DebugWriter.Wdbg(DebugLevel.I, "Setting target to {0}...", Target);
            }

            // Prepare the target
            Target = RarShellCommon.RarShell_CurrentArchiveDirectory + "/" + Target;
            if (Target.StartsWith("/"))
                Target = Target.RemoveLetter(0);
            DebugWriter.Wdbg(DebugLevel.I, "Setting target to {0}...", Target);

            // Enumerate entries
            foreach (RarArchiveEntry Entry in ListRarEntries(Target))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Entry: {0}", Entry.Key);
                if (Entry.Key.StartsWith(Target))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "{0} found ({1}). Changing...", Target, Entry.Key);
                    RarShellCommon.RarShell_CurrentArchiveDirectory = Entry.Key.RemoveLetter(Entry.Key.Length - 1);
                    DebugWriter.Wdbg(DebugLevel.I, "Setting CAD to {0}...", RarShellCommon.RarShell_CurrentArchiveDirectory);
                    return true;
                }
            }

            // Assume that we didn't find anything.
            DebugWriter.Wdbg(DebugLevel.E, "{0} not found.", Target);
            return false;
        }

        /// <summary>
        /// Changes the working local directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        public static bool ChangeWorkingRarLocalDirectory(string Target)
        {
            if (string.IsNullOrWhiteSpace(Target))
                Target = RarShellCommon.RarShell_CurrentDirectory;
            if (Checking.FolderExists(Filesystem.NeutralizePath(Target, RarShellCommon.RarShell_CurrentDirectory)))
            {
                DebugWriter.Wdbg(DebugLevel.I, "{0} found. Changing...", Target);
                RarShellCommon.RarShell_CurrentDirectory = Target;
                return true;
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.E, "{0} not found.", Target);
                return false;
            }
        }

    }
}