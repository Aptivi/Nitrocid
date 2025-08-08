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
using System.Threading;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Nitrocid.Base.Kernel.Debugging;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
using Terminaux.Colors.Themes.Colors;
using Textify.General;
using Nitrocid.Base.ConsoleBase.Inputs;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.Kernel.Exceptions;
using OpenAI.Chat;

namespace Nitrocid.Extras.ChatbotAI.Shell
{
    /// <summary>
    /// The chatbot shell
    /// </summary>
    public class ChatbotShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "ChatbotShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        internal bool detaching = false;

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Show disclaimer
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CHATBOTAI_SHELL_DISCLAIMER"), ThemeColorType.Warning);

            // Finalize current connection
            NetworkConnection chatbotConnection = (NetworkConnection)ShellArgs[0];
            ChatbotShellCommon.clientConnection = chatbotConnection;

            // Actual shell logic
            while (!Bail)
            {
                try
                {
                    // Prompt for the command
                    ShellManager.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    CancellationHandlers.DismissRequest();
                    Bail = true;
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CHATBOTAI_COMMON_SHELL_ERROR") + " {0}", true, ThemeColorType.Error, ex.Message);
                    DebugWriter.WriteDebug(DebugLevel.E, "Shell will have to exit: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    InputTools.DetectKeypress();
                    Bail = true;
                }

                // Check if the shell is going to exit
                if (Bail)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Exiting shell...");
                    if (!detaching)
                    {
                        int connectionIndex = NetworkConnectionTools.GetConnectionIndex(ChatbotShellCommon.clientConnection);
                        NetworkConnectionTools.CloseConnection(connectionIndex);
                        ChatbotShellCommon.clientConnection = null;
                    }
                    detaching = false;
                }
            }
        }

    }
}
