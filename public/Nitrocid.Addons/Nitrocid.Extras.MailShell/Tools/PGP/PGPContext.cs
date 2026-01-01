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

using MimeKit.Cryptography;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.Misc.Text.Probers.Placeholder;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Nitrocid.Extras.MailShell.Tools.PGP
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
            if (!string.IsNullOrWhiteSpace(MailLogin.GPGPromptStyle))
            {
                TextWriters.Write(PlaceParse.ProbePlaces(MailLogin.GPGPromptStyle), false, KernelColorType.Input, key.KeyId);
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Write password for key ID {0}") + ": ", false, KernelColorType.Input, key.KeyId);
            }
            string Password = InputTools.ReadLineNoInput();
            return Password;
        }
    }
}
