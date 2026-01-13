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

using Nettify.MailAddress;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Commands
{
    /// <summary>
    /// Gets an ISP info
    /// </summary>
    /// <remarks>
    /// Gets the internet service provider mail information for the specified e-mail address or the host
    /// </remarks>
    class IspInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Get mail or host
            string mailOrHost = parameters.ArgumentsList[0];
            bool hostMode = parameters.ContainsSwitch("-host");

            // Determine how to get ISP info
            var ispInfo = hostMode ?
                IspTools.GetIspConfigFromHost(mailOrHost) :
                IspTools.GetIspConfig(mailOrHost);

            // Print ISP info
            SeparatorWriterColor.WriteSeparator(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMON_ISPINFO_HEADER") + $" {mailOrHost}", true);
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMON_ISPINFO_DISPLAYNAME"), ispInfo.EmailProvider?.DisplayName ?? "");
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMON_ISPINFO_DISPLAYSHORTNAME"), ispInfo.EmailProvider?.DisplayShortName ?? "");
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMON_ISPINFO_DOMAIN"), string.Join(", ", ispInfo.EmailProvider?.Domain ?? []));
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMON_ISPINFO_DOMINATINGDOMAIN"), ispInfo.EmailProvider?.DominatingDomain ?? "");
            TextWriterRaw.Write();

            // Print server info
            SeparatorWriterColor.WriteSeparator(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMON_ISPINFO_SERVERS_HEADER") + $" {mailOrHost}", true);
            foreach (var server in ispInfo.EmailProvider?.IncomingServer ?? [])
            {
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMON_ISPINFO_INCOMING_HOSTNAME"), server.Hostname);
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMON_ISPINFO_INCOMING_PORT"), $"{server.Port}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMON_ISPINFO_INCOMING_TYPE"), server.Type);
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMON_ISPINFO_INCOMING_SOCKETTYPE"), server.SocketType);
            }
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMON_ISPINFO_OUTGOING_HOSTNAME"), ispInfo.EmailProvider?.OutgoingServer?.Hostname ?? "");
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMON_ISPINFO_OUTGOING_PORT"), $"{ispInfo.EmailProvider?.OutgoingServer?.Port}");
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMON_ISPINFO_OUTGOING_TYPE"), ispInfo.EmailProvider?.OutgoingServer?.Type ?? "");
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMON_ISPINFO_OUTGOING_SOCKETTYPE"), ispInfo.EmailProvider?.OutgoingServer?.SocketType ?? "");
            return 0;
        }

    }
}
