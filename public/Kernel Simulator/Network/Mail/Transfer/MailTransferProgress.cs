
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using FluentFTP.Helpers;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Probers;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.Shells.Mail;
using MailKit;

namespace KS.Network.Mail.Transfer
{
    /// <summary>
    /// Mail transfer progress
    /// </summary>
    public class MailTransferProgress : ITransferProgress
    {

        /// <inheritdoc/>
        public void Report(long bytesTransferred, long totalSize)
        {
            if (MailShellCommon.Mail_ShowProgress)
            {
                if (!string.IsNullOrWhiteSpace(MailShellCommon.Mail_ProgressStyle))
                {
                    TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(MailShellCommon.Mail_ProgressStyle) + Convert.ToString(CharManager.GetEsc()) + "[0K", 0, ConsoleBase.ConsoleWrapper.CursorTop, true, KernelColorType.Progress, bytesTransferred.FileSizeToString(), totalSize.FileSizeToString());
                }
                else
                {
                    TextWriterWhereColor.WriteWhere("{0}/{1} " + Translate.DoTranslation("of mail transferred...") + Convert.ToString(CharManager.GetEsc()) + "[0K", 0, ConsoleBase.ConsoleWrapper.CursorTop, true, KernelColorType.Progress, bytesTransferred.FileSizeToString(), totalSize.FileSizeToString());
                }
            }
        }

        /// <inheritdoc/>
        public void Report(long bytesTransferred)
        {
            if (MailShellCommon.Mail_ShowProgress)
            {
                if (!string.IsNullOrWhiteSpace(MailShellCommon.Mail_ProgressStyleSingle))
                {
                    TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(MailShellCommon.Mail_ProgressStyleSingle) + Convert.ToString(CharManager.GetEsc()) + "[0K", 0, ConsoleBase.ConsoleWrapper.CursorTop, true, KernelColorType.Progress, bytesTransferred.FileSizeToString());
                }
                else
                {
                    TextWriterWhereColor.WriteWhere("{0} " + Translate.DoTranslation("of mail transferred...") + Convert.ToString(CharManager.GetEsc()) + "[0K", 0, ConsoleBase.ConsoleWrapper.CursorTop, true, KernelColorType.Progress, bytesTransferred.FileSizeToString());
                }
            }
        }

    }
}
