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

using Nitrocid.Base.Kernel.Configuration.Instances;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Kernel.Debugging;
using System;

namespace Nitrocid.Base.Kernel.Configuration.Settings.KeyInputs
{
    internal class BooleanSettingsKeyInput : ISettingsKeyInput
    {
        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, BaseKernelConfig configType, out bool bail)
        {
            bail = true;
            var defaultValue = (bool?)KeyDefaultValue ?? false;
            return Convert.ToInt32(!defaultValue);
        }

        public object? TranslateStringValue(SettingsKey key, string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            if (bool.TryParse(value, out bool valueBool))
                return valueBool;
            return false;
        }

        public object? TranslateStringValueWithDefault(SettingsKey key, string value, object? KeyDefaultValue)
        {
            if (KeyDefaultValue is not bool defaultBool)
                return false;
            if (string.IsNullOrEmpty(value))
                return defaultBool;
            if (bool.TryParse(value, out bool valueBool))
                return valueBool;
            return defaultBool;
        }

        public void SetValue(SettingsKey key, object? value, BaseKernelConfig configType)
        {
            // We're dealing with boolean
            DebugWriter.WriteDebug(DebugLevel.I, "Key is of the Boolean type.");
            var FinalBool = true;

            // Set boolean
            switch (value)
            {
                case 0:
                case false:
                    // False is set
                    DebugWriter.WriteDebug(DebugLevel.I, "Setting to False...");
                    FinalBool = false;
                    break;
                case 1:
                case true:
                    // True is set
                    DebugWriter.WriteDebug(DebugLevel.I, "Setting to True...");
                    FinalBool = true;
                    break;
                default:
                    // Unknown...
                    DebugWriter.WriteDebug(DebugLevel.W, "Trying to set unknown value ({0}), setting to true anyways...", value?.ToString() ?? "<null>");
                    break;
            }

            // Now, set the value
            SettingsAppTools.SetPropertyValue(key.Variable, FinalBool, configType);
        }

    }
}
