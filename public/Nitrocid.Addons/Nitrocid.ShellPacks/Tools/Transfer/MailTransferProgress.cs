//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using MailKit;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Nitrocid.Misc.Text.Probers.Placeholder;
using Nitrocid.ShellPacks.Shells.Mail;
using Terminaux.Base;
using Terminaux.Base.Extensions;

namespace Nitrocid.ShellPacks.Tools.Transfer
{
    /// <summary>
    /// Mail transfer progress
    /// </summary>
    public class MailTransferProgress : ITransferProgress
    {

        /// <inheritdoc/>
        public void Report(long bytesTransferred, long totalSize)
        {
            if (Config.MainConfig.ShowProgress)
            {
                if (!string.IsNullOrWhiteSpace(MailShellCommon.ProgressStyle))
                {
                    TextWriters.WriteWhere(PlaceParse.ProbePlaces(MailShellCommon.ProgressStyle) + $"{ConsoleClearing.GetClearLineToRightSequence()}", 0, ConsoleWrapper.CursorTop, true, KernelColorType.Progress, bytesTransferred.SizeString(), totalSize.SizeString());
                }
                else
                {
                    TextWriters.WriteWhere("{0}/{1} " + Translate.DoTranslation("of mail transferred...") + $"{ConsoleClearing.GetClearLineToRightSequence()}", 0, ConsoleWrapper.CursorTop, true, KernelColorType.Progress, bytesTransferred.SizeString(), totalSize.SizeString());
                }
            }
        }

        /// <inheritdoc/>
        public void Report(long bytesTransferred)
        {
            if (Config.MainConfig.ShowProgress)
            {
                if (!string.IsNullOrWhiteSpace(MailShellCommon.ProgressStyleSingle))
                {
                    TextWriters.WriteWhere(PlaceParse.ProbePlaces(MailShellCommon.ProgressStyleSingle) + $"{ConsoleClearing.GetClearLineToRightSequence()}", 0, ConsoleWrapper.CursorTop, true, KernelColorType.Progress, bytesTransferred.SizeString());
                }
                else
                {
                    TextWriters.WriteWhere("{0} " + Translate.DoTranslation("of mail transferred...") + $"{ConsoleClearing.GetClearLineToRightSequence()}", 0, ConsoleWrapper.CursorTop, true, KernelColorType.Progress, bytesTransferred.SizeString());
                }
            }
        }

    }
}
