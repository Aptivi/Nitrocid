//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Switches;
using ShoutStats.Core;

namespace Nitrocid.Extras.InternetRadioInfo.Commands
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
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("The port number for your online radio is invalid."), KernelColorType.Error);
                return 25;
            }

            // Now, get the server info
            var internetFm = new ShoutcastServer(internetFmUrl, internetFmPortInt, https);
            internetFm.Refresh();
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Internet Radio (FM) info for") + $" {internetFmUrl}");
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Full URL") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{internetFm.ServerHostFull}", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Version") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{internetFm.ServerVersion}", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Current listeners") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{internetFm.CurrentListeners}", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Peak listeners") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{internetFm.PeakListeners}", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Max listeners") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{internetFm.MaxListeners}", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Unique listeners") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{internetFm.UniqueListeners}", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Streams") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{internetFm.TotalStreams}", true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Active streams") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor($"{internetFm.ActiveStreams}\n", true, KernelColorType.ListValue);

            // Now, the stream info
            foreach (var stream in internetFm.Streams)
            {
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Stream info for ID") + $" {stream.StreamId}");
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Title") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{stream.StreamTitle}", true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Path") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{stream.StreamPath}", true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Currently playing") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{stream.SongTitle}", true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Uptime") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{stream.StreamUptimeSpan}", true, KernelColorType.ListValue);
            }

            return 0;
        }

    }
}
