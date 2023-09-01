using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS.Kernel.Power
{
    internal enum PowerSignals
    {
        SIGTERM = -4,
        SIGINT = -3,
        SIGSEGV = 11,
        SIGUSR1 = 10,
        SIGUSR2 = 12,
    }
}
