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

namespace Nitrocid.Languages
{
    /// <summary>
    /// Localized string
    /// </summary>
    public readonly struct LocalizedString
    {
        private readonly string original;

        /// <summary>
        /// Original, untranslated string
        /// </summary>
        public readonly string Original =>
            original;

        /// <summary>
        /// Translated string
        /// </summary>
        public readonly string Localized =>
            Translate.DoTranslation(original);

        /// <summary>
        /// Translated string
        /// </summary>
        public override string ToString() =>
            Localized;

        /// <summary>
        /// Creates a localized string instance implicitly from a string
        /// </summary>
        /// <param name="text">Text to localize (there is no need for <see cref="Translate.DoTranslation(string)"/> here)</param>
        public static implicit operator LocalizedString(string text) =>
            new(text);

        /// <summary>
        /// Creates a localized string instance implicitly from a string
        /// </summary>
        /// <param name="text">Text to localize (there is no need for <see cref="Translate.DoTranslation(string)"/> here)</param>
        public static implicit operator string(LocalizedString text) =>
            text.Localized;

        /// <summary>
        /// Localized string structure
        /// </summary>
        /// <param name="text">Text to localize (there is no need for <see cref="Translate.DoTranslation(string)"/> here)</param>
        public LocalizedString(string text)
        {
            original = text;
        }
    }
}
