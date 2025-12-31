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

using Terminaux.Shell.Commands;
using Nitrocid.Base.Languages;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using BassBoom.Basolia.Radio;

namespace Nitrocid.Extras.BassBoom.Commands
{
    class NetFmInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
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
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_FULLURL") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{internetFm.ServerHostFull}", true, ThemeColorType.ListValue);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_STATIONTYPE") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{internetFm.ServerType}", true, ThemeColorType.ListValue);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_CURRENTLISTENERS") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{internetFm.CurrentListeners}", true, ThemeColorType.ListValue);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_PEAKLISTENERS") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{internetFm.PeakListeners}", true, ThemeColorType.ListValue);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_STREAMS") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{internetFm.TotalStreams}", true, ThemeColorType.ListValue);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_ACTIVESTREAMS") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{internetFm.ActiveStreams}\n", true, ThemeColorType.ListValue);

                // Now, the stream info
                foreach (var stream in internetFm.Streams)
                {
                    SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_STREAMINFO") + $" {stream.StreamId}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_TITLE") + ": ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{stream.StreamTitle}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_PATH") + ": ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{stream.StreamPath}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_CURRENTLYPLAYING") + ": ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{stream.SongTitle}", true, ThemeColorType.ListValue);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_UPTIME") + ": ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{stream.StreamUptimeSpan}", true, ThemeColorType.ListValue);
                }
            }
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_CANTGETINFO"), ThemeColorType.Error);
            return 0;
        }

    }
}
