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
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Drivers;
using Nitrocid.Drivers.RNG;
using Nitrocid.Drivers.Filesystem;
using Nitrocid.Drivers.Encoding;
using Nitrocid.Drivers.HardwareProber;
using Nitrocid.Drivers.Network;
using Nitrocid.Drivers.Sorting;
using Nitrocid.Drivers.DebugLogger;
using Nitrocid.Drivers.Encryption;
using Nitrocid.Drivers.Regexp;
using Nitrocid.Drivers.Console;
using Nitrocid.Drivers.Input;
using Nitrocid.Misc.Reflection.Internal;
using Nitrocid.Drivers.EncodingAsymmetric;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;

namespace Nitrocid.Kernel.Configuration.Instances
{
    /// <summary>
    /// Driver kernel configuration instance
    /// </summary>
    public class KernelDriverConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries =>
            ConfigTools.GetSettingsEntries(ResourcesManager.GetData("DriverSettingsEntries.json", ResourcesType.Settings) ??
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Failed to obtain driver settings entries.")));

        #region Drivers
        /// <summary>
        /// Current console driver
        /// </summary>
        public string CurrentConsoleDriver
        {
            get => DriverHandler.GetDriverName<IConsoleDriver>(DriverHandler.CurrentConsoleDriver);
            set => ConsoleDriverTools.SetConsoleDriver(value);
        }
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
        /// Current input driver
        /// </summary>
        public string CurrentInputDriver
        {
            get => DriverHandler.GetDriverName<IInputDriver>(DriverHandler.CurrentInputDriver);
            set => InputDriverTools.SetInputDriver(value);
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
