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

using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Nitrocid.Extras.ArchiveShell.Archive.Shell;
using Nitrocid.Files;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;

namespace Nitrocid.Extras.ArchiveShell.Archive
{
    /// <summary>
    /// Archive shell tools
    /// </summary>
    public static class ArchiveTools
    {

        /// <summary>
        /// Lists all entries according to the target directory or the current directory
        /// </summary>
        /// <param name="Target">Target directory in an archive</param>
        public static List<IArchiveEntry> ListArchiveEntries(string Target)
        {
            if (string.IsNullOrWhiteSpace(Target))
                Target = ArchiveShellCommon.CurrentArchiveDirectory ?? "";
            var Entries = new List<IArchiveEntry>();
            var archiveEntries = ArchiveShellCommon.Archive?.Entries ??
                throw new KernelException(KernelExceptionType.Archive, Translate.DoTranslation("Can't get archive entries"));
            foreach (IArchiveEntry ArchiveEntry in archiveEntries)
            {
                string key = ArchiveEntry.Key ?? "";
                DebugWriter.WriteDebug(DebugLevel.I, "Parsing entry {0}...", vars: [key]);
                if (Target is not null)
                {
                    if (key.StartsWith(Target))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Entry {0} found in target {1}. Adding...", vars: [key, Target]);
                        Entries.Add(ArchiveEntry);
                    }
                }
                else if (Target is null)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Adding entry {0}...", vars: [key]);
                    Entries.Add(ArchiveEntry);
                }
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Entries: {0}", vars: [Entries.Count]);
            return Entries;
        }

        /// <summary>
        /// Extracts an entry to a target directory
        /// </summary>
        /// <param name="Target">Target file in an archive</param>
        /// <param name="Where">Where in the local filesystem to extract?</param>
        /// <param name="FullTargetPath">Whether to use the full target path</param>
        public static bool ExtractFileEntry(string Target, string Where, bool FullTargetPath = false)
        {
            if (ArchiveShellCommon.Archive is null)
                throw new KernelException(KernelExceptionType.Archive, Translate.DoTranslation("Archive is not loaded yet."));
            if (ArchiveShellCommon.FileStream is null)
                throw new KernelException(KernelExceptionType.Archive, Translate.DoTranslation("Archive is not loaded yet."));
            if (string.IsNullOrWhiteSpace(Target))
                throw new KernelException(KernelExceptionType.Archive, Translate.DoTranslation("Can't extract nothing."));
            if (string.IsNullOrWhiteSpace(Where))
                Where = ArchiveShellCommon.CurrentDirectory ?? "";

            // Define absolute target
            string AbsoluteTarget = ArchiveShellCommon.CurrentArchiveDirectory + "/" + Target;
            if (AbsoluteTarget.StartsWith("/"))
                AbsoluteTarget = AbsoluteTarget[1..];
            DebugWriter.WriteDebug(DebugLevel.I, "Target: {0}, AbsoluteTarget: {1}", vars: [Target, AbsoluteTarget]);

            // Define local destination while getting an entry from target
            string LocalDestination = Where + "/";
            var ArchiveEntry = ArchiveShellCommon.Archive.Entries.Where(x => x.Key == AbsoluteTarget).ToArray()[0];
            string localDirDestination = Path.GetDirectoryName(ArchiveEntry.Key) ?? "";
            if (FullTargetPath)
                LocalDestination += ArchiveEntry.Key;
            DebugWriter.WriteDebug(DebugLevel.I, "Where: {0}", vars: [LocalDestination]);

            // Try to extract file
            Directory.CreateDirectory(LocalDestination);
            if (!FilesystemTools.FolderExists(LocalDestination + "/" + localDirDestination))
                Directory.CreateDirectory(LocalDestination + "/" + localDirDestination);
            FilesystemTools.MakeFile(LocalDestination + ArchiveEntry.Key);
            ArchiveShellCommon.FileStream.Seek(0L, SeekOrigin.Begin);
            var ArchiveReader = ReaderFactory.Open(ArchiveShellCommon.FileStream);
            while (ArchiveReader.MoveToNextEntry())
            {
                if (ArchiveReader.Entry.Key == ArchiveEntry.Key & !ArchiveReader.Entry.IsDirectory)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Extract started. {0}...", vars: [LocalDestination + ArchiveEntry.Key]);
                    ArchiveReader.WriteEntryToFile(LocalDestination + ArchiveEntry.Key);
                }
            }
            return true;
        }

        /// <summary>
        /// Packs a local file to the archive
        /// </summary>
        /// <param name="Target">Target local file</param>
        /// <param name="Where">Where in the archive to extract?</param>
        public static bool PackFile(string Target, string Where)
        {
            if (ArchiveShellCommon.Archive is null)
                throw new KernelException(KernelExceptionType.Archive, Translate.DoTranslation("Archive is not loaded yet."));
            if (ArchiveShellCommon.FileStream is null)
                throw new KernelException(KernelExceptionType.Archive, Translate.DoTranslation("Archive is not loaded yet."));
            if (string.IsNullOrWhiteSpace(Target))
                throw new KernelException(KernelExceptionType.Archive, Translate.DoTranslation("Can't pack nothing."));
            if (string.IsNullOrWhiteSpace(Where))
                Where = ArchiveShellCommon.CurrentDirectory ?? "";
            if (ArchiveShellCommon.Archive is not IWritableArchive)
                throw new KernelException(KernelExceptionType.Archive, Translate.DoTranslation("Archive is not writable because type is") + " {0}.", ArchiveShellCommon.Archive.Type);

            // Define absolute archive target
            string ArchiveTarget = ArchiveShellCommon.CurrentArchiveDirectory + "/" + Target;
            if (ArchiveTarget.StartsWith("/"))
                ArchiveTarget = ArchiveTarget[1..];
            DebugWriter.WriteDebug(DebugLevel.I, "Where: {0}, ArchiveTarget: {1}", vars: [Where, ArchiveTarget]);

            // Select compression type
            CompressionType compression = CompressionType.None;
            switch (ArchiveShellCommon.Archive.Type)
            {
                case ArchiveType.Zip:
                    compression = CompressionType.Deflate;
                    break;
            }

            // Define local destination while getting an entry from target
            Target = FilesystemTools.NeutralizePath(Target, Where);
            DebugWriter.WriteDebug(DebugLevel.I, "Where: {0}", vars: [Target]);
            ((IWritableArchive)ArchiveShellCommon.Archive).AddEntry(ArchiveTarget, Target);
            ((IWritableArchive)ArchiveShellCommon.Archive).SaveTo(ArchiveShellCommon.FileStream, new WriterOptions(compression));
            return true;
        }

        /// <summary>
        /// Changes the working archive directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        public static bool ChangeWorkingArchiveDirectory(string Target)
        {
            if (string.IsNullOrWhiteSpace(Target))
                Target = ArchiveShellCommon.CurrentArchiveDirectory ??
                    throw new KernelException(KernelExceptionType.Archive, Translate.DoTranslation("Can't determine current archive directory."));
            string archiveDir = ArchiveShellCommon.CurrentArchiveDirectory ?? "";

            // Check to see if we're going back
            if (Target.Contains(".."))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Target contains going back. Counting...");
                var CADSplit = archiveDir.Split('/').ToList();
                var TargetSplit = Target.Split('/').ToList();
                var CADBackSteps = 0;

                // Add back steps if target is ".."
                DebugWriter.WriteDebug(DebugLevel.I, "Target length: {0}", vars: [TargetSplit.Count]);
                for (int i = 0; i <= TargetSplit.Count - 1; i++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Target part {0}: {1}", vars: [i, TargetSplit[i]]);
                    if (TargetSplit[i] == "..")
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Target is going back. Adding step...");
                        CADBackSteps += 1;
                        TargetSplit[i] = "";
                        DebugWriter.WriteDebug(DebugLevel.I, "Steps: {0}", vars: [CADBackSteps]);
                    }
                }

                // Remove empty strings
                TargetSplit.RemoveAll(string.IsNullOrEmpty);
                DebugWriter.WriteDebug(DebugLevel.I, "Target length: {0}", vars: [TargetSplit.Count]);

                // Remove every last entry that goes back
                DebugWriter.WriteDebug(DebugLevel.I, "Old CADSplit length: {0}", vars: [CADSplit.Count]);
                for (int Steps = CADBackSteps; Steps >= 1; Steps -= 1)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Current step: {0}", vars: [Steps]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Removing index {0} from CADSplit...", vars: [CADSplit.Count - Steps]);
                    CADSplit.RemoveAt(CADSplit.Count - Steps);
                    DebugWriter.WriteDebug(DebugLevel.I, "New CADSplit length: {0}", vars: [CADSplit.Count]);
                }

                // Set current archive directory and target
                ArchiveShellCommon.CurrentArchiveDirectory = string.Join("/", CADSplit);
                DebugWriter.WriteDebug(DebugLevel.I, "Setting CAD to {0}...", vars: [ArchiveShellCommon.CurrentArchiveDirectory ?? ""]);
                Target = string.Join("/", TargetSplit);
                DebugWriter.WriteDebug(DebugLevel.I, "Setting target to {0}...", vars: [Target]);
            }

            // Prepare the target
            Target = ArchiveShellCommon.CurrentArchiveDirectory + "/" + Target;
            if (Target.StartsWith("/"))
                Target = Target[1..];
            DebugWriter.WriteDebug(DebugLevel.I, "Setting target to {0}...", vars: [Target]);

            // Enumerate entries
            foreach (IArchiveEntry Entry in ListArchiveEntries(Target))
            {
                string key = Entry.Key ?? "";
                DebugWriter.WriteDebug(DebugLevel.I, "Entry: {0}", vars: [key]);
                if (key.StartsWith(Target))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "{0} found ({1}). Changing...", vars: [Target, key]);
                    ArchiveShellCommon.CurrentArchiveDirectory = key[..^1];
                    DebugWriter.WriteDebug(DebugLevel.I, "Setting CAD to {0}...", vars: [ArchiveShellCommon.CurrentArchiveDirectory ?? ""]);
                    return true;
                }
            }

            // Assume that we didn't find anything.
            DebugWriter.WriteDebug(DebugLevel.E, "{0} not found.", vars: [Target]);
            return false;
        }

        /// <summary>
        /// Changes the working local directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        public static bool ChangeWorkingArchiveLocalDirectory(string Target)
        {
            if (string.IsNullOrWhiteSpace(Target))
                Target = ArchiveShellCommon.CurrentArchiveDirectory ??
                    throw new KernelException(KernelExceptionType.Archive, Translate.DoTranslation("Can't determine current archive directory."));
            if (FilesystemTools.FolderExists(FilesystemTools.NeutralizePath(Target, ArchiveShellCommon.CurrentDirectory)))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} found. Changing...", vars: [Target]);
                ArchiveShellCommon.CurrentDirectory = Target;
                return true;
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "{0} not found.", vars: [Target]);
                return false;
            }
        }

    }
}
