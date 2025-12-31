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
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Kernel.Configuration.Settings.KeyInputs
{
    internal class UnknownSettingsKeyInput : ISettingsKeyInput
    {
        public object? PromptForSet(SettingsKey key, object? KeyDefaultValue, BaseKernelConfig configType, out bool bail)
        {
            bail = true;
            throw new KernelException(KernelExceptionType.NotImplementedYet);
        }

        public object? TranslateStringValue(SettingsKey key, string value) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public object? TranslateStringValueWithDefault(SettingsKey key, string value, object? KeyDefaultValue) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

        public void SetValue(SettingsKey key, object? value, BaseKernelConfig configType) =>
            throw new KernelException(KernelExceptionType.NotImplementedYet);

    }
}
