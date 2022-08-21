
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
using System.IO;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Files.Folders;
using KS.Kernel;
using KS.Languages;
using KS.Login;
using KS.Misc.Text;
using KS.Misc.Writers.DebugWriters;
using KS.Network.Mail;
using KS.Scripting;
using KS.Shell.Shells.FTP;
using KS.Shell.Shells.Mail;
using KS.Shell.Shells.SFTP;
using KS.TimeDate;

namespace KS.Misc.Probers
{
    public static class PlaceParse
    {

        /// <summary>
        /// Probes the placeholders found in string
        /// </summary>
        /// <param name="text">Specified string</param>
        /// <param name="ThrowIfFailure">Throw if the placeholder parsing fails</param>
        /// <returns>A string that has the parsed placeholders</returns>
        public static string ProbePlaces(string text, bool ThrowIfFailure = false)
        {
            Kernel.Kernel.KernelEventManager.RaisePlaceholderParsing(text);
            try
            {
                // Parse the text for the following placeholders:
                DebugWriter.Wdbg(DebugLevel.I, "Parsing text for placeholders...");

                // -> User placeholder
                if (text.Contains("<user>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Username placeholder found.");
                    text = text.Replace("<user>", Login.Login.CurrentUser.Username);
                }

                // -> FTP user placeholder
                if (text.Contains("<ftpuser>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "FTP username placeholder found.");
                    text = text.Replace("<ftpuser>", FTPShellCommon.FtpUser);
                }

                // -> FTP address placeholder
                if (text.Contains("<ftpaddr>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "FTP address placeholder found.");
                    text = text.Replace("<ftpaddr>", FTPShellCommon.FtpSite);
                }

                // -> Current FTP directory placeholder
                if (text.Contains("<currentftpdirectory>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "FTP directory placeholder found.");
                    text = text.Replace("<currentftpdirectory>", FTPShellCommon.FtpCurrentRemoteDir);
                }

                // -> Current FTP local directory placeholder
                if (text.Contains("<currentftplocaldirectory>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "FTP local directory placeholder found.");
                    text = text.Replace("<currentftplocaldirectory>", FTPShellCommon.FtpCurrentDirectory);
                }

                // -> Current FTP local directory name placeholder
                if (text.Contains("<currentftplocaldirectoryname>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "FTP local directory name placeholder found.");
                    text = text.Replace("<currentftplocaldirectoryname>", new DirectoryInfo(FTPShellCommon.FtpCurrentDirectory).Name);
                }

                // -> SFTP user placeholder
                if (text.Contains("<sftpuser>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "SFTP username placeholder found.");
                    text = text.Replace("<sftpuser>", SFTPShellCommon.SFTPUser);
                }

                // -> SFTP address placeholder
                if (text.Contains("<sftpaddr>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "SFTP address placeholder found.");
                    text = text.Replace("<sftpaddr>", SFTPShellCommon.SFTPSite);
                }

                // -> Current SFTP directory placeholder
                if (text.Contains("<currentsftpdirectory>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "SFTP directory placeholder found.");
                    text = text.Replace("<currentsftpdirectory>", SFTPShellCommon.SFTPCurrentRemoteDir);
                }

                // -> Current SFTP local directory placeholder
                if (text.Contains("<currentsftplocaldirectory>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "SFTP local directory placeholder found.");
                    text = text.Replace("<currentsftplocaldirectory>", SFTPShellCommon.SFTPCurrDirect);
                }

                // -> Current SFTP local directory name placeholder
                if (text.Contains("<currentsftplocaldirectoryname>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "SFTP local directory name placeholder found.");
                    text = text.Replace("<currentsftplocaldirectoryname>", new DirectoryInfo(SFTPShellCommon.SFTPCurrDirect).Name);
                }

                // -> Mail user placeholder
                if (text.Contains("<mailuser>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Mail username placeholder found.");
                    text = text.Replace("<mailuser>", MailLogin.Mail_Authentication.UserName);
                }

                // -> Mail address placeholder
                if (text.Contains("<mailaddr>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Mail address placeholder found.");
                    text = text.Replace("<mailaddr>", MailLogin.Mail_Authentication.Domain);
                }

                // -> Current mail directory placeholder
                if (text.Contains("<currentmaildirectory>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Mail directory placeholder found.");
                    text = text.Replace("<currentmaildirectory>", MailShellCommon.IMAP_CurrentDirectory);
                }

                // -> Hostname placeholder
                if (text.Contains("<host>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Hostname placeholder found.");
                    text = text.Replace("<host>", Kernel.Kernel.HostName);
                }

                // -> Current directory placeholder
                if (text.Contains("<currentdirectory>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Current directory placeholder found.");
                    text = text.Replace("<currentdirectory>", CurrentDirectory.CurrentDir);
                }

                // -> Current directory name placeholder
                if (text.Contains("<currentdirectoryname>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Current directory name placeholder found.");
                    text = text.Replace("<currentdirectoryname>", new DirectoryInfo(CurrentDirectory.CurrentDir).Name);
                }

                // -> Short date placeholder
                if (text.Contains("<shortdate>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Short Date placeholder found.");
                    text = text.Replace("<shortdate>", TimeDate.TimeDate.KernelDateTime.ToShortDateString());
                }

                // -> Long date placeholder
                if (text.Contains("<longdate>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Long Date placeholder found.");
                    text = text.Replace("<longdate>", TimeDate.TimeDate.KernelDateTime.ToLongDateString());
                }

                // -> Short time placeholder
                if (text.Contains("<shorttime>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Short Time placeholder found.");
                    text = text.Replace("<shorttime>", TimeDate.TimeDate.KernelDateTime.ToShortTimeString());
                }

                // -> Long time placeholder
                if (text.Contains("<longtime>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Long Time placeholder found.");
                    text = text.Replace("<longtime>", TimeDate.TimeDate.KernelDateTime.ToShortDateString());
                }

                // -> Date placeholder
                if (text.Contains("<date>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Rendered Date placeholder found.");
                    text = text.Replace("<date>", TimeDateRenderers.RenderDate());
                }

                // -> Time placeholder
                if (text.Contains("<time>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Rendered Time placeholder found.");
                    text = text.Replace("<time>", TimeDateRenderers.RenderTime());
                }

                // -> Timezone placeholder
                if (text.Contains("<timezone>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Standard Time Zone placeholder found.");
                    text = text.Replace("<timezone>", TimeZoneInfo.Local.StandardName);
                }

                // -> Summer timezone placeholder
                if (text.Contains("<summertimezone>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Summer Time Zone placeholder found.");
                    text = text.Replace("<summertimezone>", TimeZoneInfo.Local.DaylightName);
                }

                // -> System placeholder
                if (text.Contains("<system>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "System placeholder found.");
                    text = text.Replace("<system>", Environment.OSVersion.ToString());
                }

                // -> Newline placeholder
                if (text.Contains("<newline>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Newline placeholder found.");
                    text = text.Replace("<newline>", Kernel.Kernel.NewLine);
                }

                // -> User dollar sign placeholder
                if (text.Contains("<dollar>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Dollar placeholder found.");
                    text = text.Replace("<dollar>", UserManagement.GetUserDollarSign());
                }

                // -> Foreground color reset placeholder
                if (text.Contains("<f:reset>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Foreground color reset placeholder found.");
                    text = text.Replace("<f:reset>", ColorTools.NeutralTextColor.VTSequenceForeground);
                }

                // -> Background color reset placeholder
                if (text.Contains("<b:reset>"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Background color reset placeholder found.");
                    text = text.Replace("<b:reset>", Flags.SetBackground ? ColorTools.BackgroundColor.VTSequenceBackground : Convert.ToString(CharManager.GetEsc()) + $"[49m");
                }

                // -> Foreground color placeholder
                if (text.Contains("<f:"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Foreground color placeholder found.");
                    while (text.Contains("<f:"))
                    {
                        int StartForegroundIndex = text.IndexOf("<f:");
                        int EndForegroundIndex = text.Substring(text.IndexOf("<f:")).IndexOf(">");
                        string SequenceSubstring = text.Substring(text.IndexOf("<f:"), length: EndForegroundIndex + 1);
                        string PlainSequence = SequenceSubstring.Substring(3, SequenceSubstring.Length - 1 - 3);
                        string VTSequence = new Color(PlainSequence).VTSequenceForeground;
                        text = text.Replace(SequenceSubstring, VTSequence);
                    }
                }

                // -> Background color placeholder
                if (text.Contains("<b:"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Background color placeholder found.");
                    while (text.Contains("<b:"))
                    {
                        int StartBackgroundIndex = text.IndexOf("<b:");
                        int EndBackgroundIndex = text.Substring(text.IndexOf("<b:")).IndexOf(">");
                        string SequenceSubstring = text.Substring(text.IndexOf("<b:"), length: EndBackgroundIndex + 1);
                        string PlainSequence = SequenceSubstring.Substring(3, SequenceSubstring.Length - 1 - 3);
                        string VTSequence = Flags.SetBackground ? new Color(PlainSequence).VTSequenceBackground : Convert.ToString(CharManager.GetEsc()) + $"[49m";
                        text = text.Replace(SequenceSubstring, VTSequence);
                    }
                }

                // -> UESH variable placeholder
                if (text.Contains("<$"))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "UESH variable placeholder found.");
                    while (text.Contains("<$"))
                    {
                        int StartShellVariableIndex = text.IndexOf("<$");
                        int EndShellVariableIndex = text.Substring(text.IndexOf("<$")).IndexOf(">");
                        string ShellVariableSubstring = text.Substring(text.IndexOf("<$"), length: EndShellVariableIndex + 1);
                        string PlainShellVariable = ShellVariableSubstring.Substring(1, ShellVariableSubstring.Length - 1 - 1);
                        text = text.Replace(ShellVariableSubstring, UESHVariables.GetVariable(PlainShellVariable));
                    }
                }

                // If successful, raise the parsed event
                Kernel.Kernel.KernelEventManager.RaisePlaceholderParsed(text);
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed to parse placeholder {0}: {1}", text, ex.Message);
                DebugWriter.WStkTrc(ex);
                Kernel.Kernel.KernelEventManager.RaisePlaceholderParseError(text, ex);
                if (ThrowIfFailure)
                    throw new Kernel.Exceptions.InvalidPlaceholderException(Translate.DoTranslation("Error trying to parse placeholders. {0}"), ex.Message);
            }
            return text;
        }

    }
}