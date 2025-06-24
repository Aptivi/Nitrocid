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

namespace Nitrocid.Base.Network.Transfer
{
    /// <summary>
    /// Network transfer information
    /// </summary>
    public class NetworkTransferInfo
    {

        private bool _MessageSuppressed;

        /// <summary>
        /// How many bytes downloaded/uploaded
        /// </summary>
        public long DoneSize { get; private set; }
        /// <summary>
        /// File size
        /// </summary>
        public long FileSize { get; private set; }
        /// <summary>
        /// The transfer type
        /// </summary>
        public NetworkTransferType TransferType { get; private set; }
        /// <summary>
        /// Whether the message is suppressed. Once set, it can't be unset.
        /// </summary>
        public bool MessageSuppressed
        {
            get
            {
                return _MessageSuppressed;
            }
            set
            {
                if (!_MessageSuppressed)
                    _MessageSuppressed = value;
            }
        }

        /// <summary>
        /// Makes a new instance of the <see cref="NetworkTransferInfo"/> class
        /// </summary>
        /// <param name="DoneSize">How many bytes are transferred?</param>
        /// <param name="FileSize">File size</param>
        /// <param name="TransferType">Transfer type</param>
        protected internal NetworkTransferInfo(long DoneSize, long FileSize, NetworkTransferType TransferType)
        {
            this.DoneSize = DoneSize;
            this.FileSize = FileSize;
            this.TransferType = TransferType;
        }

    }
}
