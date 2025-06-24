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

using Nitrocid.Base.Languages;
using System;

namespace Nitrocid.Base.Files.Extensions
{
    /// <summary>
    /// Extension handler class
    /// </summary>
    public class ExtensionHandler
    {
        private readonly string extension;
        private readonly string implementer;
        private readonly string mimeType;
        private readonly Action<string> handler;
        private readonly Func<string, string> infoHandler;

        /// <summary>
        /// Supported extension
        /// </summary>
        public string Extension =>
            extension;
        /// <summary>
        /// Extension handler implementer name
        /// </summary>
        public string Implementer =>
            implementer;
        /// <summary>
        /// MIME type for extension
        /// </summary>
        public string MimeType =>
            mimeType;
        /// <summary>
        /// Handler for opening files that have the same extension
        /// </summary>
        public Action<string> Handler =>
            handler;
        /// <summary>
        /// Handler for getting information about files that have the same extension
        /// </summary>
        public Func<string, string> InfoHandler =>
            infoHandler;

        internal ExtensionHandler(string extension, string implementer, Action<string> handler, Func<string, string> infoHandler)
        {
            // First, get the MIME type
            mimeType = MimeTypes.GetMimeType(extension);

            // Then, install the below values
            this.extension = extension;
            this.implementer = implementer;
            this.handler = handler;
            this.infoHandler = infoHandler ?? ((_) => LanguageTools.GetLocalized("NKS_FILES_EXTENSIONS_NOEXTRAINFO"));
        }
    }
}
