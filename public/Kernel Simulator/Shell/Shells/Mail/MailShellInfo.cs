
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

using System.Collections.Generic;
using KS.Shell.Prompts.Presets.Mail;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Mail.Commands;

namespace KS.Shell.Shells.Mail
{
    /// <summary>
    /// Common mail shell class
    /// </summary>
    internal class MailShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// Mail commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "cd", new CommandInfo("cd", ShellType, /* Localizable */ "Changes current mail directory", new CommandArgumentInfo(new[] { "<folder>" }, true, 1), new Mail_CdCommand()) },
            { "lsdirs", new CommandInfo("lsdirs", ShellType, /* Localizable */ "Lists directories in your mail address", new CommandArgumentInfo(), new Mail_LsDirsCommand()) },
            { "list", new CommandInfo("list", ShellType, /* Localizable */ "Downloads messages and lists them", new CommandArgumentInfo(new[] { "[pagenum]" }, false, 0), new Mail_ListCommand()) },
            { "mkdir", new CommandInfo("mkdir", ShellType, /* Localizable */ "Makes a directory in the current working directory", new CommandArgumentInfo(new[] { "<foldername>" }, true, 1), new Mail_MkdirCommand()) },
            { "mv", new CommandInfo("mv", ShellType, /* Localizable */ "Moves a message", new CommandArgumentInfo(new[] { "<mailid> <targetfolder>" }, true, 2), new Mail_MvCommand()) },
            { "mvall", new CommandInfo("mvall", ShellType, /* Localizable */ "Moves all messages from recipient", new CommandArgumentInfo(new[] { "<sendername> <targetfolder>" }, true, 2), new Mail_MvAllCommand()) },
            { "read", new CommandInfo("read", ShellType, /* Localizable */ "Opens a message", new CommandArgumentInfo(new[] { "<mailid>" }, true, 1), new Mail_ReadCommand()) },
            { "readenc", new CommandInfo("readenc", ShellType, /* Localizable */ "Opens an encrypted message", new CommandArgumentInfo(new[] { "<mailid>" }, true, 1), new Mail_ReadEncCommand()) },
            { "ren", new CommandInfo("ren", ShellType, /* Localizable */ "Renames a folder", new CommandArgumentInfo(new[] { "<oldfoldername> <newfoldername>" }, true, 2), new Mail_RenCommand()) },
            { "rm", new CommandInfo("rm", ShellType, /* Localizable */ "Removes a message", new CommandArgumentInfo(new[] { "<mailid>" }, true, 1), new Mail_RmCommand()) },
            { "rmall", new CommandInfo("rmall", ShellType, /* Localizable */ "Removes all messages from recipient", new CommandArgumentInfo(new[] { "<sendername>" }, true, 1), new Mail_RmAllCommand()) },
            { "rmdir", new CommandInfo("rmdir", ShellType, /* Localizable */ "Removes a directory from the current working directory", new CommandArgumentInfo(new[] { "<foldername>" }, true, 1), new Mail_RmdirCommand()) },
            { "send", new CommandInfo("send", ShellType, /* Localizable */ "Sends a message to an address", new CommandArgumentInfo(), new Mail_SendCommand()) },
            { "sendenc", new CommandInfo("sendenc", ShellType, /* Localizable */ "Sends an encrypted message to an address", new CommandArgumentInfo(), new Mail_SendEncCommand()) }
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new MailDefaultPreset() },
            { "PowerLine1", new MailPowerLine1Preset() },
            { "PowerLine2", new MailPowerLine2Preset() },
            { "PowerLine3", new MailPowerLine3Preset() },
            { "PowerLineBG1", new MailPowerLineBG1Preset() },
            { "PowerLineBG2", new MailPowerLineBG2Preset() },
            { "PowerLineBG3", new MailPowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new MailShell();

        public override PromptPresetBase CurrentPreset => PromptPresetManager.CurrentPresets["MailShell"];

    }
}
