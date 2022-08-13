
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Imports UnameNET

Namespace Misc.Platform
    Public Module KernelPlatform

        ''' <summary>
        ''' Is this system a Windows system?
        ''' </summary>
        Public Function IsOnWindows() As Boolean
            Return Environment.OSVersion.Platform = PlatformID.Win32NT
        End Function

        ''' <summary>
        ''' Is this system a Unix system? True for macOS, too!
        ''' </summary>
        Public Function IsOnUnix() As Boolean
            Return Environment.OSVersion.Platform = PlatformID.Unix
        End Function

        ''' <summary>
        ''' Is this system a macOS system?
        ''' </summary>
        <CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification:="We already have IsOnUnix() to do the job, so it returns false when Windows is detected.")>
        <CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification:=".NET Framework doesn't have CA1416.")>
        Public Function IsOnMacOS() As Boolean
            If IsOnUnix() Then
                Dim System As String = UnameManager.GetUname(UnameTypes.KernelName)
                Return System.Contains("Darwin")
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Are we running KS on Mono?
        ''' </summary>
        Public Function IsOnMonoRuntime() As Boolean
            Return Type.GetType("Mono.Runtime") IsNot Nothing
        End Function

        ''' <summary>
        ''' Is Kernel Simulator running from GRILO?
        ''' </summary>
        Public Function IsRunningFromGrilo() As Boolean
            Return System.Reflection.Assembly.GetEntryAssembly().GetName().Name.StartsWith("GRILO")
        End Function

    End Module
End Namespace
