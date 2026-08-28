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

using BassBoom.Basolia.Media.Radio;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.BassBoom.Commands
{
    class NetFmInfoCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "netfminfo";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_BASSBOOM_COMMAND_NETFMINFO_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "hostname", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_BASSBOOM_COMMAND_NETFMINFO_ARGUMENT_HOSTNAME_DESC"
                    }),
                    new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_BASSBOOM_COMMAND_NETFMINFO_ARGUMENT_PORT_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Get the variables
            bool https = parameters.ContainsSwitch("-secure");
            string internetFmUrl = $"{(https ? "https" : "http")}://" + parameters.ArgumentsList[0];
            string internetFmPort = parameters.ArgumentsList[1];

            // Check for the port integrity
            if (!int.TryParse(internetFmPort, out int internetFmPortInt))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_PORTINVALID"), ThemeColorType.Error);
                return 25;
            }

            // Now, get the server info
            var internetFm = RadioTools.GetRadioInfo($"{internetFmUrl}:{internetFmPortInt}");
            if (internetFm is not null)
            {
                internetFm.Refresh();
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_HEADER") + $" {internetFmUrl}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_FULLURL"), internetFm.ServerHostFull);
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_STATIONTYPE"), $"{internetFm.ServerType}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_CURRENTLISTENERS"), $"{internetFm.CurrentListeners}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_PEAKLISTENERS"), $"{internetFm.PeakListeners}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_STREAMS"), $"{internetFm.TotalStreams}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_ACTIVESTREAMS"), $"{internetFm.ActiveStreams}");
                TextWriterRaw.Write();

                // Now, the stream info
                for (int i = 0; i < internetFm.Streams.Length; i++)
                {
                    StreamInfo stream = internetFm.Streams[i];
                    SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_STREAMINFO") + $" {stream.StreamId}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_TITLE"), stream.StreamTitle);
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_PATH"), stream.StreamPath);
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_CURRENTLYPLAYING"), stream.SongTitle);
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_UPTIME"), $"{stream.StreamUptimeSpan}");
                    if (i < internetFm.Streams.Length - 1)
                        TextWriterRaw.Write();
                }
            }
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_CANTGETINFO"), ThemeColorType.Error);
            return 0;
        }

    }
}
