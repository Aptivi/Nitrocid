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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files.Operations;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace Nitrocid.Extras.Pastebin.Commands
{
    /// <summary>
    /// The pastebin command
    /// </summary>
    /// <remarks>
    /// If you want to paste the contents of a file or a string, you'll need to use this command in order to be able to upload text to one of the text hosting providers, such as Pastebin or Termbin.
    /// </remarks>
    class PastebinCommand : BaseCommand, ICommand
    {
        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check the contents
            string contents = parameters.ArgumentsList[0];
            if (string.IsNullOrWhiteSpace(contents))
            {
                TextWriters.Write(Translate.DoTranslation("Specify either a file name or text to upload."), KernelColorType.Error);
                return 35;
            }

            // Check the provider and the type
            string provider = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-provider") ? SwitchManager.GetSwitchValue(parameters.SwitchesList, "-provider") : "pastebin.com";
            int port = 443;
            string type = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-type") ? SwitchManager.GetSwitchValue(parameters.SwitchesList, "-type") : "https";
            if (provider.Contains(':'))
            {
                string portStr = provider.Substring(provider.IndexOf(":") + 1);
                port = int.Parse(portStr);
                provider = provider.Substring(0, provider.IndexOf(':'));
            }
            if (type != "raw" && type != "http" && type != "https")
            {
                TextWriters.Write(Translate.DoTranslation("Pastebin provider type is not supported."), KernelColorType.Error);
                return 36;
            }

            // Get the post page, field, format
            string page = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-postpage") ? SwitchManager.GetSwitchValue(parameters.SwitchesList, "-postpage") : "api/api_post.php";
            string format = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-postformat") ? SwitchManager.GetSwitchValue(parameters.SwitchesList, "-postformat") : "text";
            string field = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-postfield") ? SwitchManager.GetSwitchValue(parameters.SwitchesList, "-postfield") : "api_paste_code";

            // Get the contents
            if (Checking.FileExists(contents))
                contents = Reading.ReadAllTextNoBlock(contents);

            // Now, form a URI and choose how to send the request
            if (type == "raw")
            {
                try
                {
                    // Make a socket and connect
                    using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(provider, port);

                    // Now, send the contents to the remote raw pastebin provider
                    var contentsBytes = Encoding.UTF8.GetBytes(contents);
                    socket.Send(contentsBytes);

                    // Wait for the response
                    SpinWait.SpinUntil(() => socket.Available > 0);
                    var buffer = new byte[socket.Available];
                    socket.Receive(buffer);

                    // Decode the response
                    string response = Encoding.UTF8.GetString(buffer);
                    TextWriters.Write(Translate.DoTranslation("Successfully pasted to the provider!"), KernelColorType.Success);
                    TextWriters.Write(response, KernelColorType.NeutralText);
                }
                catch (Exception ex)
                {
                    TextWriters.Write(Translate.DoTranslation("Unable to paste to the provider."), KernelColorType.Error);
                    TextWriters.Write(ex.Message, KernelColorType.NeutralText);
                    return 37;
                }
            }
            else
            {
                // Just for validation
                string encoded = HttpUtility.UrlEncode(contents);
                string url = $"{type}://{provider}:{port}/{page}";
                var uri = new Uri(url);

                // Open the HTTP client and choose how to post
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", $"Nitrocid v{KernelMain.Version}");

                // Now, post and return the response.
                var response = client.PostAsync(uri, new StringContent($"{field}={encoded}{(parameters.ArgumentsList.Length > 1 ? $"&{parameters.ArgumentsList[1]}" : "")}", Encoding.UTF8, format == "json" ? "text/json" : "application/x-www-form-urlencoded")).Result;
                string reply = response.Content.ReadAsStringAsync().Result;
                if (!response.IsSuccessStatusCode)
                {
                    TextWriters.Write(Translate.DoTranslation("Unable to paste to the provider.") + $" [{(int)response.StatusCode}] {response.StatusCode}", KernelColorType.Error);
                    TextWriters.Write(reply, KernelColorType.NeutralText);
                    return 37;
                }
                TextWriters.Write(Translate.DoTranslation("Successfully pasted to the provider!"), KernelColorType.Success);
                TextWriters.Write(reply, KernelColorType.NeutralText);
            }
            return 0;
        }
    }
}
