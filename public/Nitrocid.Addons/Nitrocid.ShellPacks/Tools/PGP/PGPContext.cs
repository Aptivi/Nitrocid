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
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.ConsoleBase.Inputs;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
using Textify.Tools.Placeholder;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Nitrocid.ShellPacks.Tools.PGP
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
            if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.MailGPGPromptStyle))
                TextWriterColor.Write(PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.MailGPGPromptStyle), false, ThemeColorType.Input, key.KeyId);
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_PGP_KEYPASSWORD") + ": ", false, ThemeColorType.Input, key.KeyId);
            string Password = InputTools.ReadLineNoInput();
            return Password;
        }
    }
}
