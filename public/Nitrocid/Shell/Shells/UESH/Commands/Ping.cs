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

using System;
using System.Net.NetworkInformation;
using Terminaux.Shell.Commands;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Shell.Switches;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;
using Nitrocid.Network;

namespace Nitrocid.Shell.Shells.UESH.Commands
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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // If the pinged address is actually a number of times
            int PingTimes = 4;
            string projectedTimes = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-times");
            if (!string.IsNullOrEmpty(projectedTimes) && TextTools.IsStringNumeric(projectedTimes))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Projected times {0} is numeric.", vars: [projectedTimes]);
                PingTimes = Convert.ToInt32(projectedTimes);
            }

            // Now, ping the specified addresses
            foreach (string PingedAddress in parameters.ArgumentsList)
            {
                if (!string.IsNullOrEmpty(PingedAddress))
                {
                    SeparatorWriterColor.WriteSeparatorColor(PingedAddress, KernelColorTools.GetColor(KernelColorType.ListTitle));
                    for (int CurrentTime = 1; CurrentTime <= PingTimes; CurrentTime++)
                    {
                        try
                        {
                            var PingReplied = NetworkTools.PingAddress(PingedAddress);
                            if (PingReplied.Status == IPStatus.Success)
                            {
                                TextWriterColor.Write("[{1}] " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_PING_SUCCESS"), PingReplied.RoundtripTime, CurrentTime);
                            }
                            else
                            {
                                TextWriters.Write("[{2}] " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_PING_FAILED"), true, KernelColorType.Error, PingedAddress, PingReplied.Status, CurrentTime);
                            }
                        }
                        catch (Exception ex)
                        {
                            TextWriters.Write("[{2}] " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_PING_FAILED"), true, KernelColorType.Error, PingedAddress, ex.Message, CurrentTime);
                            DebugWriter.WriteDebugStackTrace(ex);
                        }
                    }
                }
                else
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_PING_NEEDSADDR"), true, KernelColorType.Error);
                }
            }
            return 0;
        }

    }
}
