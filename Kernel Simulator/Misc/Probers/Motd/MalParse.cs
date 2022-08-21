
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
using KS.Files;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Probers.Motd
{
    public static class MalParse
    {

        // Variables
        public static string MalFilePath = Paths.GetKernelPath(KernelPathType.MAL);

        /// <summary>
        /// Sets the MAL
        /// </summary>
        /// <param name="Message">A message of the day after login</param>
        public static void SetMal(string Message)
        {
            try
            {
                System.IO.StreamWriter MALStreamW;

                // Get the MOTD and MAL file path
                MalFilePath = Filesystem.NeutralizePath(MalFilePath);
                DebugWriter.Wdbg(DebugLevel.I, "Path: {0}", MalFilePath);

                // Set the message according to message type
                MALStreamW = new System.IO.StreamWriter(MalFilePath) { AutoFlush = true };
                DebugWriter.Wdbg(DebugLevel.I, "Opened stream to MAL path");
                MALStreamW.Write(Message);
                Kernel.Kernel.MAL = Message;

                // Close the message stream
                MALStreamW.Close();
                DebugWriter.Wdbg(DebugLevel.I, "Stream closed");
            }
            catch (Exception ex)
            {
                throw new MOTDException(Translate.DoTranslation("Error when trying to set MAL: {0}"), ex.Message);
                DebugWriter.WStkTrc(ex);
            }
        }

        /// <summary>
        /// Reads the message of the day before/after login
        /// </summary>
        public static void ReadMal()
        {
            try
            {
                System.IO.StreamReader MALStreamR;
                var MALBuilder = new System.Text.StringBuilder();

                // Get the MOTD and MAL file path
                MalFilePath = Filesystem.NeutralizePath(MalFilePath);
                DebugWriter.Wdbg(DebugLevel.I, "Path: {0}", MalFilePath);

                // Read the message according to message type
                MALStreamR = new System.IO.StreamReader(MalFilePath);
                DebugWriter.Wdbg(DebugLevel.I, "Opened stream to MAL path");
                MALBuilder.Append(MALStreamR.ReadToEnd());
                Kernel.Kernel.MAL = MALBuilder.ToString();
                MALStreamR.Close();
                DebugWriter.Wdbg(DebugLevel.I, "Stream closed");
            }
            catch (Exception ex)
            {
                throw new MOTDException(Translate.DoTranslation("Error when trying to get MAL: {0}"), ex.Message);
                DebugWriter.WStkTrc(ex);
            }
        }

    }
}