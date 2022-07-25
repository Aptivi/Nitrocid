
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports KS.Arguments.PreBootCommandLineArguments

Namespace Arguments.ArgumentBase
    Public Module PreBootCommandLineArgsParse

        Public ReadOnly AvailablePreBootCMDLineArgs As New Dictionary(Of String, ArgumentInfo) From {
            {"reset", New ArgumentInfo("reset", ArgumentType.PreBootCommandLineArgs, "Resets the kernel to the factory settings", "", False, 0, New PreBootCommandLine_ResetArgument)},
            {"bypasssizedetection", New ArgumentInfo("bypasssizedetection", ArgumentType.PreBootCommandLineArgs, "Bypasses the console size detection", "", False, 0, New PreBootCommandLine_BypassSizeDetectionArgument)}
        }

    End Module
End Namespace