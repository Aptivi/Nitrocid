
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Mail.Transfer;
using KS.Shell.ShellBase.Commands;
using MimeKit;
using KS.Kernel.Debugging;

namespace KS.Shell.Shells.Mail.Commands
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
    class Mail_SendEncCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string Receiver, Subject;
            var Body = new BodyBuilder();

            // Prompt for receiver e-mail address
            TextWriterColor.Write(Translate.DoTranslation("Enter recipient mail address:") + " ", false, KernelColorType.Input);
            Receiver = Input.ReadLine();
            DebugWriter.WriteDebug(DebugLevel.I, "Recipient: {0}", Receiver);

            // Check for mail format
            if (Receiver.Contains("@") & Receiver.Substring(Receiver.IndexOf("@")).Contains("."))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Mail format satisfied. Contains \"@\" and contains \".\" in the second part after the \"@\" symbol.");

                // Prompt for subject
                TextWriterColor.Write(Translate.DoTranslation("Enter the subject:") + " ", false, KernelColorType.Input);
                Subject = Input.ReadLine(false);
                DebugWriter.WriteDebug(DebugLevel.I, "Subject: {0} ({1} chars)", Subject, Subject.Length);

                // Prompt for body
                TextWriterColor.Write(Translate.DoTranslation("Enter your message below. Write \"EOF\" to confirm."), true, KernelColorType.Input);
                string BodyLine = "";
                while (!(BodyLine.ToUpper() == "EOF"))
                {
                    BodyLine = Input.ReadLine();
                    if (!(BodyLine.ToUpper() == "EOF"))
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
                    TextWriterColor.Write("> ", false, KernelColorType.Input);
                    PathLine = Input.ReadLine(false);
                    if (!string.IsNullOrEmpty(PathLine))
                    {
                        PathLine = Filesystem.NeutralizePath(PathLine);
                        DebugWriter.WriteDebug(DebugLevel.I, "Path line: {0} ({1} chars)", PathLine, PathLine.Length);
                        if (Checking.FileExists(PathLine))
                        {
                            Body.Attachments.Add(PathLine);
                        }
                    }
                }

                // Send the message
                TextWriterColor.Write(Translate.DoTranslation("Sending message..."), true, KernelColorType.Progress);
                if (MailTransfer.MailSendEncryptedMessage(Receiver, Subject, Body.ToMessageBody()))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Message sent.");
                    TextWriterColor.Write(Translate.DoTranslation("Message sent."), true, KernelColorType.Success);
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "See debug output to find what's wrong.");
                    TextWriterColor.Write(Translate.DoTranslation("Error sending message."), true, KernelColorType.Error);
                }
            }
            else {
                DebugWriter.WriteDebug(DebugLevel.E, "Mail format unsatisfied." + Receiver);
                TextWriterColor.Write(Translate.DoTranslation("Invalid e-mail address. Make sure you've written the address correctly and that it matches the format of the example shown:") + " john.s@example.com", true, KernelColorType.Error);
            }
        }

    }
}
