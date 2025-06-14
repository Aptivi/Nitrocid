using Nitrocid.Kernel.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace KSMod
{
    public class ModClass : IMod
    {
        public string Name => "KSMod";

        public string Version => "1.0.0";

        public Version MinimumSupportedApiVersion => new(4, 0, 28, 10);

        public ModLoadPriority LoadPriority => ModLoadPriority.Optional;

        public void StartMod()
        {

        }

        public void StopMod()
        {

        }
    }
}

// Refer to https://aptivi.github.io/Nitrocid for up-to-date API documentation for mod developers.
