
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Text.Probers.Placeholder;
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
                    TextWriterWhereColor.WriteWhereKernelColor(PlaceParse.ProbePlaces(MailShellCommon.Mail_ProgressStyle) + $"{ConsoleExtensions.GetClearLineToRightSequence()}", 0, ConsoleWrapper.CursorTop, true, KernelColorType.Progress, bytesTransferred.SizeString(), totalSize.SizeString());
                }
                else
                {
                    TextWriterWhereColor.WriteWhereKernelColor("{0}/{1} " + Translate.DoTranslation("of mail transferred...") + $"{ConsoleExtensions.GetClearLineToRightSequence()}", 0, ConsoleWrapper.CursorTop, true, KernelColorType.Progress, bytesTransferred.SizeString(), totalSize.SizeString());
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
                    TextWriterWhereColor.WriteWhereKernelColor(PlaceParse.ProbePlaces(MailShellCommon.Mail_ProgressStyleSingle) + $"{ConsoleExtensions.GetClearLineToRightSequence()}", 0, ConsoleWrapper.CursorTop, true, KernelColorType.Progress, bytesTransferred.SizeString());
                }
                else
                {
                    TextWriterWhereColor.WriteWhereKernelColor("{0} " + Translate.DoTranslation("of mail transferred...") + $"{ConsoleExtensions.GetClearLineToRightSequence()}", 0, ConsoleWrapper.CursorTop, true, KernelColorType.Progress, bytesTransferred.SizeString());
                }
            }
        }

    }
}
