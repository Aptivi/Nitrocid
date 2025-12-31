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

using BenchmarkDotNet.Attributes;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Base.Drivers.RNG;

namespace Nitrocid.Benchmarks.Fixtures
{
    public class EncryptionSha256 : BenchFixture
    {
        private readonly IEncryptionDriver encryption = DriverHandler.GetDriver<IEncryptionDriver>("SHA256");
        private readonly IRandomDriver random = DriverHandler.GetDriver<IRandomDriver>("Default");

        [Benchmark]
        public override void Run()
        {
            for (int i = 0; i < 1000000; i++)
            {
                int num = random.Random();
                encryption.GetEncryptedString($"#{num}");
            }
        }
    }
}
