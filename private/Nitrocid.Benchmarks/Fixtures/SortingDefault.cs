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

using BenchmarkDotNet.Attributes;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Drivers.Sorting;

namespace Nitrocid.Benchmarks.Fixtures
{
    public class SortingDefault : BenchFixture
    {
        private readonly ISortingDriver sort = DriverHandler.GetDriver<ISortingDriver>("Default");
        private readonly IRandomDriver random = DriverHandler.GetDriver<IRandomDriver>("Standard");

        [Benchmark]
        public override void Run()
        {
            int[] array = new int[1000000];
            for (int i = 0; i < 1000000; i++)
            {
                int num = random.Random();
                array[i] = num;
            }
            sort.SortNumbers(array);
        }

        [Benchmark]
        public void RunHuge()
        {
            int[] array = new int[int.MaxValue];
            for (int i = 0; i < int.MaxValue; i++)
            {
                int num = random.Random();
                array[i] = num;
            }
            sort.SortNumbers(array);
        }
    }
}
