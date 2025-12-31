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

using MimeKit;
using Nitrocid.ShellPacks.Tools.Transfer;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Files;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Terminaux.Colors.Themes.Colors;
using Textify.General;
using Nitrocid.Base.ConsoleBase.Inputs;

namespace Nitrocid.ShellPacks.Shells.Mail.Commands
{
    /// <summary>
    /// Sends an encrypted mail.
    /// </summary>
    /// <remarks>
    /// This command opens to a prompt which will tell you to provide the following details:
    /// <br></br>
    /// <list type="bullet">
    /// <item>
    /// <term>Recipient mail address</term>
    /// <description>The account who will receive the mail.</description>
    /// </item>
    /// <item>
    /// <term>Subject</term>
    /// <description>The title of the message.</description>
    /// </item>
    /// <item>
    /// <term>Body</term>
    /// <description>The body of the message. This is where most information lies.</description>
    /// </item>
    /// <item>
    /// <term>Attachments</term>
    /// <description>If you want to provide attachments, enter the file name. It supports relative and absolute directories.</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// This command supports encryption, assuming you have a private key.
    /// </remarks>
    class SendEncCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string Receiver, Subject;
            var Body = new BodyBuilder();

            // Prompt for receiver e-mail address
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_SEND_TARGETPROMPT") + " ", false, ThemeColorType.Input);
            Receiver = InputTools.ReadLine();
            DebugWriter.WriteDebug(DebugLevel.I, "Recipient: {0}", vars: [Receiver]);

            // Check for mail format
            if (Receiver.Contains('@') & Receiver[Receiver.IndexOf('@')..].Contains('.'))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Mail format satisfied. Contains \"@\" and contains \".\" in the second part after the \"@\" symbol.");

                // Prompt for subject
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_SEND_SUBJECTPROMPT") + " ", false, ThemeColorType.Input);
                Subject = InputTools.ReadLine();
                DebugWriter.WriteDebug(DebugLevel.I, "Subject: {0} ({1} chars)", vars: [Subject, Subject.Length]);

                // Prompt for body
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_SEND_BODYPROMPT"), true, ThemeColorType.Input);
                string BodyLine = "";
                while (!BodyLine.Equals("EOF", System.StringComparison.OrdinalIgnoreCase))
                {
                    BodyLine = InputTools.ReadLine();
                    if (!BodyLine.Equals("EOF", System.StringComparison.OrdinalIgnoreCase))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Body line: {0} ({1} chars)", vars: [BodyLine, BodyLine.Length]);
                        Body.TextBody += BodyLine + CharManager.NewLine;
                        DebugWriter.WriteDebug(DebugLevel.I, "Body length: {0} chars", vars: [Body.TextBody.Length]);
                    }
                }

                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_SEND_PATHSFORATTACHMENT"));
                string PathLine = " ";
                while (!string.IsNullOrEmpty(PathLine))
                {
                    TextWriterColor.Write("> ", false, ThemeColorType.Input);
                    PathLine = InputTools.ReadLine();
                    if (!string.IsNullOrEmpty(PathLine))
                    {
                        PathLine = FilesystemTools.NeutralizePath(PathLine);
                        DebugWriter.WriteDebug(DebugLevel.I, "Path line: {0} ({1} chars)", vars: [PathLine, PathLine.Length]);
                        if (FilesystemTools.FileExists(PathLine))
                        {
                            Body.Attachments.Add(PathLine);
                        }
                    }
                }

                // Send the message
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_SEND_SENDING"), true, ThemeColorType.Progress);
                if (MailTransfer.MailSendEncryptedMessage(Receiver, Subject, Body.ToMessageBody()))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Message sent.");
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_SEND_SENT"), true, ThemeColorType.Success);
                    return 0;
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "See debug output to find what's wrong.");
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_SEND_ERRORSENDING"), true, ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Mail);
                }
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Mail format unsatisfied." + Receiver);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_SEND_INVALIDMAIL") + " john.s@example.com", true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Mail);
            }
        }

    }
}
