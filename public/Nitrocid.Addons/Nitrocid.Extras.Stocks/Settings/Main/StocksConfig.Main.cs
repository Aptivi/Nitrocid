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

using Nitrocid.Base.Kernel.Configuration.Instances;

namespace Nitrocid.Extras.Stocks.Settings
{
    /// <summary>
    /// Configuration instance for stocks
    /// </summary>
    public partial class StocksConfig : BaseKernelConfig
    {
        /// <summary>
        /// Key for the AlphaVantage Stocks API
        /// </summary>
        public string StocksApiKey { get; set; } = "";
        /// <summary>
        /// Stocks company
        /// </summary>
        public string StocksCompany { get; set; } = "MSFT";
    }
}
