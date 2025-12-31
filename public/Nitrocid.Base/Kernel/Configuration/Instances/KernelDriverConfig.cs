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

using Newtonsoft.Json;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Drivers.Encoding;
using Nitrocid.Base.Drivers.Network;
using Nitrocid.Base.Drivers.HardwareProber;
using Nitrocid.Base.Misc.Reflection.Internal;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Drivers.Filesystem;
using Nitrocid.Base.Drivers.Sorting;
using Nitrocid.Base.Drivers.DebugLogger;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Drivers.Regexp;
using Nitrocid.Base.Drivers.EncodingAsymmetric;
using Nitrocid.Base.Drivers.Encryption;

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Driver kernel configuration instance
    /// </summary>
    public class KernelDriverConfig : BaseKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override string Name =>
            LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_INSTANCES_DRIVERSETTINGS");

        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries
        {
            get
            {
                var dataStream = ResourcesManager.GetData("DriverSettingsEntries.json", ResourcesType.Settings) ??
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_ENTRIESFAILED_DRIVER"));
                string dataString = ResourcesManager.ConvertToString(dataStream);
                return ConfigTools.GetSettingsEntries(dataString);
            }
        }

        #region Drivers
        /// <summary>
        /// Current random number generator driver
        /// </summary>
        public string CurrentRandomDriver
        {
            get => DriverHandler.GetDriverName<IRandomDriver>(DriverHandler.CurrentRandomDriver);
            set => RandomDriverTools.SetRandomDriver(value);
        }
        /// <summary>
        /// Current network driver
        /// </summary>
        public string CurrentNetworkDriver
        {
            get => DriverHandler.GetDriverName<INetworkDriver>(DriverHandler.CurrentNetworkDriver);
            set => NetworkDriverTools.SetNetworkDriver(value);
        }
        /// <summary>
        /// Current filesystem driver
        /// </summary>
        public string CurrentFilesystemDriver
        {
            get => DriverHandler.GetDriverName<IFilesystemDriver>(DriverHandler.CurrentFilesystemDriver);
            set => FilesystemDriverTools.SetFilesystemDriver(value);
        }
        /// <summary>
        /// Current encryption driver
        /// </summary>
        public string CurrentEncryptionDriver
        {
            get => DriverHandler.GetDriverName<IEncryptionDriver>(DriverHandler.CurrentEncryptionDriver);
            set => EncryptionDriverTools.SetEncryptionDriver(value);
        }
        /// <summary>
        /// Current regular expression driver
        /// </summary>
        public string CurrentRegexpDriver
        {
            get => DriverHandler.GetDriverName<IRegexpDriver>(DriverHandler.CurrentRegexpDriver);
            set => RegexpDriverTools.SetRegexpDriver(value);
        }
        /// <summary>
        /// Current regular expression driver
        /// </summary>
        public string CurrentDebugLoggerDriver
        {
            get => DriverHandler.GetDriverName<IDebugLoggerDriver>(DriverHandler.CurrentDebugLoggerDriver);
            set => DebugLoggerDriverTools.SetDebugLoggerDriver(value);
        }
        /// <summary>
        /// Current encoding driver
        /// </summary>
        public string CurrentEncodingDriver
        {
            get => DriverHandler.GetDriverName<IEncodingDriver>(DriverHandler.CurrentEncodingDriver);
            set => EncodingDriverTools.SetEncodingDriver(value);
        }
        /// <summary>
        /// Current hardware prober driver
        /// </summary>
        public string CurrentHardwareProberDriver
        {
            get => DriverHandler.GetDriverName<IHardwareProberDriver>(DriverHandler.CurrentHardwareProberDriver);
            set => HardwareProberDriverTools.SetHardwareProberDriver(value);
        }
        /// <summary>
        /// Current sorting driver
        /// </summary>
        public string CurrentSortingDriver
        {
            get => DriverHandler.GetDriverName<ISortingDriver>(DriverHandler.CurrentSortingDriver);
            set => SortingDriverTools.SetSortingDriver(value);
        }
        /// <summary>
        /// Current asymmetric encoding driver
        /// </summary>
        public string CurrentEncodingAsymmetricDriver
        {
            get => DriverHandler.GetDriverName<IEncodingAsymmetricDriver>(DriverHandler.CurrentEncodingAsymmetricDriver);
            set => EncodingAsymmetricDriverTools.SetEncodingAsymmetricDriver(value);
        }
        #endregion
    }
}
