using System;
using System.IO;
using Extensification.StringExts;

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

using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Files.Operations
{
    public static class Making
    {

        /// <summary>
        /// Makes a directory
        /// </summary>
        /// <param name="NewDirectory">New directory</param>
        /// <exception cref="IOException"></exception>
        public static void MakeDirectory(string NewDirectory, bool ThrowIfDirectoryExists = true)
        {
            Filesystem.ThrowOnInvalidPath(NewDirectory);
            NewDirectory = Filesystem.NeutralizePath(NewDirectory);
            DebugWriter.Wdbg(DebugLevel.I, "New directory: {0} ({1})", NewDirectory, Checking.FolderExists(NewDirectory));
            if (!Checking.FolderExists(NewDirectory))
            {
                Directory.CreateDirectory(NewDirectory);

                // Raise event
                Kernel.Kernel.KernelEventManager.RaiseDirectoryCreated(NewDirectory);
            }
            else if (ThrowIfDirectoryExists)
            {
                throw new IOException(Translate.DoTranslation("Directory {0} already exists.").FormatString(NewDirectory));
            }
        }

        /// <summary>
        /// Makes a directory
        /// </summary>
        /// <param name="NewDirectory">New directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static bool TryMakeDirectory(string NewDirectory, bool ThrowIfDirectoryExists = true)
        {
            try
            {
                MakeDirectory(NewDirectory, ThrowIfDirectoryExists);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Makes a file
        /// </summary>
        /// <param name="NewFile">New file</param>
        /// <exception cref="IOException"></exception>
        public static void MakeFile(string NewFile, bool ThrowIfFileExists = true)
        {
            Filesystem.ThrowOnInvalidPath(NewFile);
            NewFile = Filesystem.NeutralizePath(NewFile);
            DebugWriter.Wdbg(DebugLevel.I, "File path is {0} and .Exists is {0}", NewFile, Checking.FileExists(NewFile));
            if (!Checking.FileExists(NewFile))
            {
                try
                {
                    var NewFileStream = File.Create(NewFile);
                    DebugWriter.Wdbg(DebugLevel.I, "File created");
                    NewFileStream.Close();
                    DebugWriter.Wdbg(DebugLevel.I, "File closed");

                    // Raise event
                    Kernel.Kernel.KernelEventManager.RaiseFileCreated(NewFile);
                }
                catch (Exception ex)
                {
                    DebugWriter.WStkTrc(ex);
                    throw new IOException(Translate.DoTranslation("Error trying to create a file: {0}").FormatString(ex.Message));
                }
            }
            else if (ThrowIfFileExists)
            {
                throw new IOException(Translate.DoTranslation("File already exists."));
            }
        }

        /// <summary>
        /// Makes a file
        /// </summary>
        /// <param name="NewFile">New file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static bool TryMakeFile(string NewFile, bool ThrowIfFileExists = true)
        {
            try
            {
                MakeFile(NewFile, ThrowIfFileExists);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Makes an empty JSON file
        /// </summary>
        /// <param name="NewFile">New JSON file</param>
        /// <exception cref="IOException"></exception>
        public static void MakeJsonFile(string NewFile, bool ThrowIfFileExists = true)
        {
            Filesystem.ThrowOnInvalidPath(NewFile);
            NewFile = Filesystem.NeutralizePath(NewFile);
            DebugWriter.Wdbg(DebugLevel.I, "File path is {0} and .Exists is {0}", NewFile, Checking.FileExists(NewFile));
            if (!Checking.FileExists(NewFile))
            {
                try
                {
                    var NewFileStream = File.Create(NewFile);
                    DebugWriter.Wdbg(DebugLevel.I, "File created");
                    var NewJsonObject = JObject.Parse("{}");
                    var NewFileWriter = new StreamWriter(NewFileStream);
                    NewFileWriter.WriteLine(JsonConvert.SerializeObject(NewJsonObject));
                    NewFileStream.Close();
                    DebugWriter.Wdbg(DebugLevel.I, "File closed");

                    // Raise event
                    Kernel.Kernel.KernelEventManager.RaiseFileCreated(NewFile);
                }
                catch (Exception ex)
                {
                    DebugWriter.WStkTrc(ex);
                    throw new IOException(Translate.DoTranslation("Error trying to create a file: {0}").FormatString(ex.Message));
                }
            }
            else if (ThrowIfFileExists)
            {
                throw new IOException(Translate.DoTranslation("File already exists."));
            }
        }

        /// <summary>
        /// Makes an empty JSON file
        /// </summary>
        /// <param name="NewFile">New JSON file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static bool TryMakeJsonFile(string NewFile, bool ThrowIfFileExists = true)
        {
            try
            {
                MakeJsonFile(NewFile, ThrowIfFileExists);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}