
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Linq;
using System.Net.NetworkInformation;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Network.Base;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Pings an address
    /// </summary>
    /// <remarks>
    /// This command was implemented when the basic network support was released in 0.0.2 using the old way of pinging. Eventually, it was removed in 0.0.7. It came back in 0.0.12 under a different implementation.
    /// <br></br>
    /// If you want to ping an address to see if it's offline or online, or if you want to see if you're online or offline, use this command.
    /// </remarks>
    class PingCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            // If the pinged address is actually a number of times
            int PingTimes = 4;
            bool hasTimes = SwitchManager.ContainsSwitch(ListSwitchesOnly, "-times");
            string projectedTimes = SwitchManager.GetSwitchValue(ListSwitchesOnly, "-times");
            if (TextTools.IsStringNumeric(projectedTimes))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Projected times {0} is numeric.", projectedTimes);
                PingTimes = Convert.ToInt32(projectedTimes);
            }

            // Now, ping the specified addresses
            foreach (string PingedAddress in ListArgsOnly)
            {
                if (!string.IsNullOrEmpty(PingedAddress))
                {
                    SeparatorWriterColor.WriteSeparator(PingedAddress, true);
                    for (int CurrentTime = 1; CurrentTime <= PingTimes; CurrentTime++)
                    {
                        try
                        {
                            var PingReplied = NetworkTools.PingAddress(PingedAddress);
                            if (PingReplied.Status == IPStatus.Success)
                            {
                                TextWriterColor.Write("[{1}] " + Translate.DoTranslation("Ping succeeded in {0} ms."), PingReplied.RoundtripTime, CurrentTime);
                            }
                            else
                            {
                                TextWriterColor.Write("[{2}] " + Translate.DoTranslation("Failed to ping {0}: {1}"), true, KernelColorType.Error, PingedAddress, PingReplied.Status, CurrentTime);
                            }
                        }
                        catch (Exception ex)
                        {
                            TextWriterColor.Write("[{2}] " + Translate.DoTranslation("Failed to ping {0}: {1}"), true, KernelColorType.Error, PingedAddress, ex.Message, CurrentTime);
                            DebugWriter.WriteDebugStackTrace(ex);
                        }
                    }
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Address may not be empty."), true, KernelColorType.Error);
                }
            }
        }

    }
}
