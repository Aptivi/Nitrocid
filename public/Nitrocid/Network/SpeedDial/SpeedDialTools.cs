
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

using KS.Files.Operations;
using KS.Files;
using KS.Languages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel.Debugging;
using System.Linq;
using KS.Kernel.Exceptions;
using KS.Misc.Text;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;

namespace KS.Network.SpeedDial
{
    /// <summary>
    /// Speed dial management tools
    /// </summary>
    public static class SpeedDialTools
    {

        /// <summary>
        /// Gets the token from the speed dial
        /// </summary>
        public static JObject GetTokenFromSpeedDial()
        {
            // Make the speed dial file if not found, then read the contents
            Making.MakeFile(Paths.GetKernelPath(KernelPathType.SpeedDial), false);
            string SpeedDialJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.SpeedDial));

            // Now, parse the contents
            return JObject.Parse(!string.IsNullOrEmpty(SpeedDialJsonContent) ? SpeedDialJsonContent : "{}");
        }

        /// <summary>
        /// Adds an entry to speed dial
        /// </summary>
        /// <param name="Address">A speed dial address</param>
        /// <param name="Port">A speed dial port</param>
        /// <param name="SpeedDialType">Speed dial type</param>
        /// <param name="ThrowException">Optionally throw exception</param>
        /// <param name="arguments">List of arguments to pass to the entry</param>
        public static void AddEntryToSpeedDial(string Address, int Port, SpeedDialType SpeedDialType, bool ThrowException = true, params object[] arguments)
        {
            // Parse the token
            var SpeedDialToken = GetTokenFromSpeedDial();
            if (SpeedDialToken[Address] is null)
            {
                // The entry doesn't exist. Go ahead and create it.
                var NewSpeedDial = new JObject(
                    new JProperty("Address", Address),
                    new JProperty("Port", Port),
                    new JProperty("Type", SpeedDialType),
                    new JProperty("Options", new JArray(arguments))
                );

                // Add the entry and write it to the file
                SpeedDialToken.Add(Address, NewSpeedDial);
                File.WriteAllText(Paths.GetKernelPath(KernelPathType.SpeedDial), JsonConvert.SerializeObject(SpeedDialToken, Formatting.Indented));
            }
            else if (ThrowException)
            {
                // Entry already exists! Throw an exception if needed.
                throw new KernelException(KernelExceptionType.Network, Translate.DoTranslation("Entry already exists."));
            }
        }

        /// <summary>
        /// Adds an entry to speed dial
        /// </summary>
        /// <param name="Address">A speed dial address</param>
        /// <param name="Port">A speed dial port</param>
        /// <param name="SpeedDialType">Speed dial type</param>
        /// <param name="ThrowException">Optionally throw exception</param>
        /// <param name="arguments">List of arguments to pass to the entry</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryAddEntryToSpeedDial(string Address, int Port, SpeedDialType SpeedDialType, bool ThrowException = true, params object[] arguments)
        {
            try
            {
                AddEntryToSpeedDial(Address, Port, SpeedDialType, ThrowException, arguments);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Lists all speed dial entries
        /// </summary>
        /// <returns>A list</returns>
        public static Dictionary<string, JToken> ListSpeedDialEntries()
        {
            // Parse the token
            var SpeedDialToken = GetTokenFromSpeedDial();
            var SpeedDialEntries = new Dictionary<string, JToken>();
            foreach (var SpeedDialAddress in SpeedDialToken.Properties())
                SpeedDialEntries.Add(SpeedDialAddress.Name, SpeedDialAddress.Value);
            return SpeedDialEntries;
        }

        /// <summary>
        /// Opens speed dial prompt
        /// </summary>
        public static JToken GetQuickConnectInfo()
        {
            // Get the speed dial entries
            var SpeedDialEntries = ListSpeedDialEntries();
            DebugWriter.WriteDebug(DebugLevel.I, "Speed dial length: {0}", SpeedDialEntries.Count);

            // Get the headers ready for prompt
            string Answer;
            var SpeedDialHeaders = new[] { 
                "#",
                Translate.DoTranslation("Host Name"),
                Translate.DoTranslation("Host Port"),
                Translate.DoTranslation("Parameters")
            };
            var SpeedDialData = new string[SpeedDialEntries.Count, 4];
            if (!(SpeedDialEntries.Count == 0))
            {
                TextWriterColor.Write(Translate.DoTranslation("Select an address to connect to:"));
                for (int i = 0; i <= SpeedDialEntries.Count - 1; i++)
                {
                    string SpeedDialAddress = SpeedDialEntries.Keys.ElementAtOrDefault(i);
                    var SpeedDialOptions = SpeedDialEntries[SpeedDialAddress]["Options"];
                    DebugWriter.WriteDebug(DebugLevel.I, "Speed dial address: {0}", SpeedDialAddress);
                    SpeedDialData[i, 0] = (i + 1).ToString();
                    SpeedDialData[i, 1] = SpeedDialAddress;
                    SpeedDialData[i, 2] = (string)SpeedDialEntries[SpeedDialAddress]["Port"];
                    SpeedDialData[i, 3] = SpeedDialOptions is not null ? string.Join(", ", SpeedDialEntries[SpeedDialAddress]["Options"]) : "";
                }
                TableColor.WriteTable(SpeedDialHeaders, SpeedDialData, 2);
                TextWriterColor.Write();
                while (true)
                {
                    TextWriterColor.Write(">> ", false, KernelColorType.Input);
                    Answer = Input.ReadLine();
                    DebugWriter.WriteDebug(DebugLevel.I, "Response: {0}", Answer);
                    if (TextTools.IsStringNumeric(Answer))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Response is numeric. IsStringNumeric(Answer) returned true. Checking to see if in-bounds...");
                        int AnswerInt = Convert.ToInt32(Answer);
                        if (AnswerInt <= SpeedDialEntries.Count)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Response is in-bounds. Connecting...");
                            return SpeedDialEntries.Values.ElementAtOrDefault(AnswerInt - 1);
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Response is out-of-bounds. Retrying...");
                            TextWriterColor.Write(Translate.DoTranslation("The selection is out of range. Select between 1-{0}. Try again."), true, KernelColorType.Error, SpeedDialEntries.Count);
                        }
                    }
                    else {
                        DebugWriter.WriteDebug(DebugLevel.W, "Response isn't numeric. IsStringNumeric(Answer) returned false.");
                        TextWriterColor.Write(Translate.DoTranslation("The selection is not a number. Try again."), true, KernelColorType.Error);
                    }
                }
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Speed dial is empty. Lines count is 0.");
                TextWriterColor.Write(Translate.DoTranslation("Speed dial is empty. Connect to a server to add an address to it."), true, KernelColorType.Error);
            }
            return null;
        }
    }
}
