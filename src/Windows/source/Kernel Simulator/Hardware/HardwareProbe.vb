
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Module HardwareProbe

    Sub ProbeHW(Optional ByVal QuietHWProbe As Boolean = False, Optional ByVal KernelUser As Char = "U")

        If (QuietHWProbe = False) Then
            If (KernelUser = "K") Then
                If (ProbeFlag = True) Then
                    System.Console.WriteLine("hwprobe: Your hardware will be probed. Please wait...")
                    Cpuinfo()
                    System.Console.WriteLine("hwprobe: CPU: {0} {1}MHz", Cpuname, Cpuspeed)
                    SysMemory()
                    System.Console.WriteLine("hwprobe: RAM: {0}", SysMem)
                    Hddinfo()
                    System.Console.WriteLine("hwprobe: HDD: {0} {1}GB", Hddmodel, FormatNumber(Hddsize, 2))
                Else
                    System.Console.WriteLine("hwprobe: Hardware is not probed. Probe using 'hwprobe'")
                End If
            ElseIf (KernelUser = "U") Then
                If (ProbeFlag = False) Then
                    System.Console.WriteLine("hwprobe: Your hardware will be probed. Please wait...")
                    Cpuinfo()
                    System.Console.WriteLine("hwprobe: CPU: {0} {1}MHz", Cpuname, Cpuspeed)
                    SysMemory()
                    System.Console.WriteLine("hwprobe: RAM: {0}", SysMem)
                    Hddinfo()
                    System.Console.WriteLine("hwprobe: HDD: {0} {1}GB", Hddmodel, FormatNumber(Hddsize, 2))
                    ProbeFlag = True
                Else
                    System.Console.WriteLine("hwprobe: Hardware already probed.")
                End If
            End If
        ElseIf (QuietHWProbe = True) Then
            If (ProbeFlag = True) Then
                Cpuinfo()
                SysMemory()
                Hddinfo()
            End If
        End If

    End Sub

    Sub ProbeBIOS(Optional ByVal QuietBIOS As Boolean = False)

        If (QuietBIOS = False) Then
            System.Console.WriteLine("hwprobe: Your BIOS will be probed. Please wait...")
            BiosInformation()
            System.Console.WriteLine("hwprobe: BIOS: {0} {1}", BIOSMan, BIOSCaption)
        ElseIf (QuietBIOS = True) Then
            BiosInformation()
        End If

    End Sub

End Module
