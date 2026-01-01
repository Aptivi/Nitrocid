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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.MailShell.Tools.Transfer;
using Nitrocid.Files;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Textify.General;
using Nitrocid.ConsoleBase.Inputs;

namespace Nitrocid.Extras.MailShell.Mail.Commands
{
    /// <summary>
    /// Sends a mail
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
    /// This command doesn't support encryption. Use sendenc for this functionality.
    /// </remarks>
    class SendCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string Receiver, Subject;
            var Body = new BodyBuilder();

            // Prompt for receiver e-mail address
            TextWriters.Write(Translate.DoTranslation("Enter recipient mail address:") + " ", false, KernelColorType.Input);
            Receiver = InputTools.ReadLine();
            DebugWriter.WriteDebug(DebugLevel.I, "Recipient: {0}", Receiver);

            // Check for mail format
            if (Receiver.Contains('@') & Receiver[Receiver.IndexOf('@')..].Contains('.'))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Mail format satisfied. Contains \"@\" and contains \".\" in the second part after the \"@\" symbol.");

                // Prompt for subject
                TextWriters.Write(Translate.DoTranslation("Enter the subject:") + " ", false, KernelColorType.Input);
                Subject = InputTools.ReadLine();
                DebugWriter.WriteDebug(DebugLevel.I, "Subject: {0} ({1} chars)", Subject, Subject.Length);

                // Prompt for body
                TextWriters.Write(Translate.DoTranslation("Enter your message below. Write \"EOF\" to confirm."), true, KernelColorType.Input);
                string BodyLine = "";
                while (!BodyLine.Equals("EOF", System.StringComparison.OrdinalIgnoreCase))
                {
                    BodyLine = InputTools.ReadLine();
                    if (!BodyLine.Equals("EOF", System.StringComparison.OrdinalIgnoreCase))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Body line: {0} ({1} chars)", BodyLine, BodyLine.Length);
                        Body.TextBody += BodyLine + CharManager.NewLine;
                        DebugWriter.WriteDebug(DebugLevel.I, "Body length: {0} chars", Body.TextBody.Length);
                    }
                }

                TextWriterColor.Write(Translate.DoTranslation("Enter file paths to attachments. Press ENTER on a blank path to confirm."));
                string PathLine = " ";
                while (!string.IsNullOrEmpty(PathLine))
                {
                    TextWriters.Write("> ", false, KernelColorType.Input);
                    PathLine = InputTools.ReadLine();
                    if (!string.IsNullOrEmpty(PathLine))
                    {
                        PathLine = FilesystemTools.NeutralizePath(PathLine);
                        DebugWriter.WriteDebug(DebugLevel.I, "Path line: {0} ({1} chars)", PathLine, PathLine.Length);
                        if (Checking.FileExists(PathLine))
                        {
                            Body.Attachments.Add(PathLine);
                        }
                    }
                }

                // Send the message
                TextWriters.Write(Translate.DoTranslation("Sending message..."), true, KernelColorType.Progress);
                if (MailTransfer.MailSendMessage(Receiver, Subject, Body.ToMessageBody()))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Message sent.");
                    TextWriters.Write(Translate.DoTranslation("Message sent."), true, KernelColorType.Success);
                    return 0;
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "See debug output to find what's wrong.");
                    TextWriters.Write(Translate.DoTranslation("Error sending message."), true, KernelColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Mail);
                }
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Mail format unsatisfied." + Receiver);
                TextWriters.Write(Translate.DoTranslation("Invalid e-mail address. Make sure you've written the address correctly and that it matches the format of the example shown:") + " john.s@example.com", true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Mail);
            }
        }

    }
}
