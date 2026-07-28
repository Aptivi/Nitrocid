//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using FluentFTP;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base.Extensions;

namespace Nitrocid.ShellPacks.Shells.FTP.Tools.Transfer
{
    /// <summary>
    /// FTP transfer progress class
    /// </summary>
    public static class FTPTransferProgress
    {

        /// <summary>
        /// Action of file progress. You can make your own handler by mods
        /// </summary>
        public static Action<FtpProgress> FileProgress { get; set; } = new(FileProgressHandler);
        /// <summary>
        /// Action of folder/multiple file progress. You can make your own handler by mods
        /// </summary>
        public static Action<FtpProgress> MultipleProgress { get; set; } = new(MultipleProgressHandler);

        /// <summary>
        /// Handles the individual file download/upload progress
        /// </summary>
        private static void FileProgressHandler(FtpProgress Percentage)
        {
            if (Percentage.Progress >= 0d)
            {
                TextWriterColor.Write("\r {0}% (ETA: {1}d {2}:{3}:{4} @ {5})", false, ThemeColorType.Progress, Percentage.Progress.ToString("N2"), Percentage.ETA.Days, Percentage.ETA.Hours, Percentage.ETA.Minutes, Percentage.ETA.Seconds, Percentage.TransferSpeedToString());
                ConsoleClearing.ClearLineToRight();
            }
        }

        /// <summary>
        /// Handles the multiple files/folder download/upload progress
        /// </summary>
        private static void MultipleProgressHandler(FtpProgress Percentage)
        {
            if (Percentage.Progress >= 0d)
            {
                TextWriterColor.Write("\r - [{0}/{1}] {2}: ", false, ThemeColorType.ListEntry, Percentage.FileIndex + 1, Percentage.FileCount, Percentage.RemotePath);
                TextWriterColor.Write("{0}% (ETA: {1}d {2}:{3}:{4} @ {5})", false, ThemeColorType.Progress, Percentage.Progress.ToString("N2"), Percentage.ETA.Days, Percentage.ETA.Hours, Percentage.ETA.Minutes, Percentage.ETA.Seconds, Percentage.TransferSpeedToString());
                ConsoleClearing.ClearLineToRight();
            }
        }

    }
}
