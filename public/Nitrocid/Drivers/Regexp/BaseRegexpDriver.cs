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

extern alias TextifyDep;

using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using System.Text.RegularExpressions;
using Textify.General;
using TextifyDep::System.Diagnostics.CodeAnalysis;

namespace Nitrocid.Drivers.Regexp
{
    /// <summary>
    /// Base regexp driver
    /// </summary>
    public abstract class BaseRegexpDriver : IRegexpDriver
    {
        /// <inheritdoc/>
        public virtual string DriverName => "Default";

        /// <inheritdoc/>
        public virtual DriverTypes DriverType => DriverTypes.Regexp;

        /// <inheritdoc/>
        public virtual bool DriverInternal => false;

        /// <inheritdoc/>
        public bool IsValidRegex([StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
        {
            try
            {
                Regex.Match(string.Empty, pattern);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool IsMatch(string text, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
        {
            if (!IsValidRegex(pattern))
                return false;

            return new Regex(pattern).IsMatch(text);
        }

        /// <inheritdoc/>
        public Match Match(string text, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
        {
            if (!IsValidRegex(pattern))
                throw new KernelException(KernelExceptionType.RegularExpression, Translate.DoTranslation("Invalid regular expression syntax."));

            return new Regex(pattern).Match(text);
        }

        /// <inheritdoc/>
        public MatchCollection Matches(string text, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
        {
            if (!IsValidRegex(pattern))
                throw new KernelException(KernelExceptionType.RegularExpression, Translate.DoTranslation("Invalid regular expression syntax."));

            return new Regex(pattern).Matches(text);
        }

        /// <inheritdoc/>
        public string Filter(string text, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
        {
            if (!IsValidRegex(pattern))
                throw new KernelException(KernelExceptionType.RegularExpression, Translate.DoTranslation("Invalid regular expression syntax."));

            return new Regex(pattern).Replace(text, "");
        }

        /// <inheritdoc/>
        public string Filter(string text, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern, string replaceWith)
        {
            if (!IsValidRegex(pattern))
                throw new KernelException(KernelExceptionType.RegularExpression, Translate.DoTranslation("Invalid regular expression syntax."));

            return new Regex(pattern).Replace(text, replaceWith);
        }

        /// <inheritdoc/>
        public string[] Split(string text, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
        {
            if (!IsValidRegex(pattern))
                throw new KernelException(KernelExceptionType.RegularExpression, Translate.DoTranslation("Invalid regular expression syntax."));

            return new Regex(pattern).Split(text);
        }

        /// <inheritdoc/>
        public string Escape(string text) =>
            text.Escape();

        /// <inheritdoc/>
        public string Unescape(string text) =>
            text.Unescape();
    }
}
