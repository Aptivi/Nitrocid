
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
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;

namespace KS.Misc.Probers.Motd
{
    public static class MotdParse
    {

        // Variables
        public static string MotdFilePath = Paths.GetKernelPath(KernelPathType.MOTD);

        /// <summary>
        /// Sets the Message of the Day
        /// </summary>
        /// <param name="Message">A message of the day</param>
        public static void SetMotd(string Message)
        {
            try
            {
                System.IO.StreamWriter MOTDStreamW;

                // Get the MOTD and MAL file path
                MotdFilePath = Filesystem.NeutralizePath(MotdFilePath);
                DebugWriter.Wdbg(DebugLevel.I, "Path: {0}", MotdFilePath);

                // Set the message according to message type
                MOTDStreamW = new System.IO.StreamWriter(MotdFilePath) { AutoFlush = true };
                DebugWriter.Wdbg(DebugLevel.I, "Opened stream to MOTD path");
                MOTDStreamW.WriteLine(Message);
                Kernel.Kernel.MOTDMessage = Message;

                // Close the message stream
                MOTDStreamW.Close();
                DebugWriter.Wdbg(DebugLevel.I, "Stream closed");
            }
            catch (Exception ex)
            {
                DebugWriter.WStkTrc(ex);
                throw new MOTDException(Translate.DoTranslation("Error when trying to set MOTD: {0}"), ex.Message);
            }
        }

        /// <summary>
        /// Reads the message of the day
        /// </summary>
        public static void ReadMotd()
        {
            try
            {
                System.IO.StreamReader MOTDStreamR;
                var MOTDBuilder = new System.Text.StringBuilder();

                // Get the MOTD and MAL file path
                MotdFilePath = Filesystem.NeutralizePath(MotdFilePath);
                DebugWriter.Wdbg(DebugLevel.I, "Path: {0}", MotdFilePath);

                // Read the message according to message type
                MOTDStreamR = new System.IO.StreamReader(MotdFilePath);
                DebugWriter.Wdbg(DebugLevel.I, "Opened stream to MOTD path");
                MOTDBuilder.Append(MOTDStreamR.ReadToEnd());
                Kernel.Kernel.MOTDMessage = MOTDBuilder.ToString();
                MOTDStreamR.Close();
                DebugWriter.Wdbg(DebugLevel.I, "Stream closed");
            }
            catch (Exception ex)
            {
                DebugWriter.WStkTrc(ex);
                throw new MOTDException(Translate.DoTranslation("Error when trying to get MOTD: {0}"), ex.Message);
            }
        }

    }
}
