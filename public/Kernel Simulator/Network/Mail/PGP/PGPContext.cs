
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Misc.Probers;
using KS.Misc.Writers.ConsoleWriters;
using MimeKit.Cryptography;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace KS.Network.Mail.PGP
{
    /// <summary>
    /// KS PGP context
    /// </summary>
    public class PGPContext : GnuPGContext
    {

        /// <summary>
        /// Gets password for secret key.
        /// </summary>
        /// <param name="key">Target key</param>
        /// <returns>Entered Password</returns>
        protected override string GetPasswordForKey(PgpSecretKey key)
        {
            if (!string.IsNullOrWhiteSpace(MailLogin.Mail_GPGPromptStyle))
            {
                TextWriterColor.Write(PlaceParse.ProbePlaces(MailLogin.Mail_GPGPromptStyle), false, KernelColorType.Input, key.KeyId);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Write password for key ID {0}") + ": ", false, KernelColorType.Input, key.KeyId);
            }
            string Password = Input.ReadLineNoInput();
            return Password;
        }
    }
}
