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

using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Languages;
using System;

namespace Nitrocid.Base.Kernel.Time.Alarm
{
    /// <summary>
    /// Alarm entry class
    /// </summary>
    public class AlarmEntry
    {
        private string alarmName;
        private string alarmDesc;
        private DateTime alarmSpan;
        
        /// <summary>
        /// The fully qualified name of the alarm
        /// </summary>
        public string Name =>
            alarmName;
        
        /// <summary>
        /// The description of the alarm
        /// </summary>
        public string Description =>
            alarmDesc;

        /// <summary>
        /// The length of the alarm
        /// </summary>
        public DateTime Length =>
            alarmSpan;

        internal AlarmEntry(string alarmName, string alarmDesc, DateTime alarmSpan)
        {
            if (alarmSpan < TimeDateTools.KernelDateTime)
                throw new KernelException(KernelExceptionType.Alarm, LanguageTools.GetLocalized("NKS_KERNEL_TIME_ALARM_EXCEPTION_LESSTHANPRESENT"));
            this.alarmName = alarmName ?? "";
            this.alarmDesc = alarmDesc ?? "";
            this.alarmSpan = alarmSpan;
        }
    }
}
