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

using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.ConsoleBase.Colors;
using BassBoom.Basolia.Radio;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.BassBoom.Commands
{
    class NetFmInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Get the variables
            bool https = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-secure");
            string internetFmUrl = $"{(https ? "https" : "http")}://" + parameters.ArgumentsList[0];
            string internetFmPort = parameters.ArgumentsList[1];

            // Check for the port integrity
            if (!int.TryParse(internetFmPort, out int internetFmPortInt))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_PORTINVALID"), KernelColorType.Error);
                return 25;
            }

            // Now, get the server info
            var internetFm = RadioTools.GetRadioInfo($"{internetFmUrl}:{internetFmPortInt}");
            if (internetFm is not null)
            {
                internetFm.Refresh();
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_HEADER") + $" {internetFmUrl}", KernelColorTools.GetColor(KernelColorType.ListTitle));
                TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_FULLURL") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{internetFm.ServerHostFull}", true, KernelColorType.ListValue);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_STATIONTYPE") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{internetFm.ServerType}", true, KernelColorType.ListValue);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_CURRENTLISTENERS") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{internetFm.CurrentListeners}", true, KernelColorType.ListValue);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_PEAKLISTENERS") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{internetFm.PeakListeners}", true, KernelColorType.ListValue);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_STREAMS") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{internetFm.TotalStreams}", true, KernelColorType.ListValue);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_ACTIVESTREAMS") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{internetFm.ActiveStreams}\n", true, KernelColorType.ListValue);

                // Now, the stream info
                foreach (var stream in internetFm.Streams)
                {
                    SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_STREAMINFO") + $" {stream.StreamId}", KernelColorTools.GetColor(KernelColorType.ListTitle));
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_TITLE") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{stream.StreamTitle}", true, KernelColorType.ListValue);
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_PATH") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{stream.StreamPath}", true, KernelColorType.ListValue);
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_CURRENTLYPLAYING") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{stream.SongTitle}", true, KernelColorType.ListValue);
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_UPTIME") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{stream.StreamUptimeSpan}", true, KernelColorType.ListValue);
                }
            }
            else
                TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_RADIO_CANTGETINFO"), KernelColorType.Error);
            return 0;
        }

    }
}
