
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

Module HardwareVars

    'Hardware Classes. We start with HDD class
    Public Class HDD
        Public Model As String
        Public Manufacturer As String
        Public InterfaceType As String
        Public Status As String
        Public Cylinders As UInt64
        Public Heads As UInt32
        Public Sectors As UInt64
        Public Size As UInt64
    End Class

    'then CPU
    Public Class CPU
        Public Name As String
        Public ClockSpeed As UInt64
    End Class

    'then GPU
    Public Class GPU
        Public Name As String
        Public Memory As UInt64
    End Class

    'then RAM
    Public Class RAM
        Public ChipCapacity As UInt64
        Public Status As String
        Public SlotNumber As Integer
        Public SlotName As String
    End Class

    'then BIOS
    Public Class BIOS
        Public Name As String
        Public Manufacturer As String
        Public SMBIOSVersion As String
        Public Version As String
    End Class

    'Hardware Lists
    Public HDDList As New List(Of HDD)
    Public CPUList As New List(Of CPU)
    Public GPUList As New List(Of GPU)
    Public RAMList As New List(Of RAM)
    Public BIOSList As New List(Of BIOS)

    'Hardware Variables (important)
    Public BIOSCaption As String
    Public BIOSMan As String
    Public BIOSSMBIOSVersion As String
    Public BIOSVersion As String
    Public slotsUsedName As String
    Public slotsUsedNum As Integer
    Public Capacities() As String
    Public totalSlots As Integer

    'These are used to check to see if probing specific hardware is done.
    Public CPUDone As Boolean = False
    Public RAMDone As Boolean = False
    Public HDDDone As Boolean = False
    Public GPUDone As Boolean = False
    Public BIOSDone As Boolean = False

End Module
