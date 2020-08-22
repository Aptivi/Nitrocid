
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

Public Module HardwareVars

    ''' <summary>
    ''' [Windows] Hard drive
    ''' </summary>
    Public Class HDD
        Public Model As String
        Public Manufacturer As String
        Public InterfaceType As String
        Public Cylinders As ULong
        Public Heads As UInteger
        Public Sectors As ULong
        Public Size As ULong
        Public ID As String
        Public Parts As New List(Of Part)
    End Class

    ''' <summary>
    ''' [Linux] Hard drive
    ''' </summary>
    Public Class HDD_Linux
        Inherits HDD
        Public Size_LNX As String
        Public Model_LNX As String
        Public Vendor_LNX As String
        Public Parts_LNX As New List(Of Part_Linux)
    End Class

    ''' <summary>
    ''' [Windows] Partition
    ''' </summary>
    Public Class Part
        Public Bootable, Boot, Primary As Boolean
        Public Size As ULong
        Public Type As String
    End Class

    ''' <summary>
    ''' [Linux] Partition
    ''' </summary>
    Public Class Part_Linux
        Inherits Part
        Public Part, FileSystem, SizeMEAS, Used As String
    End Class

    ''' <summary>
    ''' [Windows] Logical partition
    ''' </summary>
    Public Class Logical
        Public Compressed As Boolean
        Public Size, Free As ULong
        Public FileSystem, Name As String
    End Class

    ''' <summary>
    ''' [Windows] CPU
    ''' </summary>
    Public Class CPU
        Public Name As String
        Public ClockSpeed As ULong
    End Class

    ''' <summary>
    ''' [Linux] CPU
    ''' </summary>
    Public Class CPU_Linux
        Inherits CPU
        Public CPUName As String
        Public Clock As String
        Public SSE2 As Boolean
    End Class

    ''' <summary>
    ''' [Windows] RAM
    ''' </summary>
    Public Class RAM
        Public ChipCapacity As ULong
        Public SlotNumber As Integer
        Public SlotName As String
    End Class

    ''' <summary>
    ''' [Linux] RAM
    ''' </summary>
    Public Class RAM_Linux
        Inherits RAM
        Public Capacity As String
    End Class

    'Hardware Lists
    Public HDDList As New List(Of HDD)
    Public LogList As New List(Of Logical)
    Public CPUList As New List(Of CPU)
    Public RAMList As New List(Of RAM)

    'Hardware Variables (important)
    Public slotsUsedName As String
    Public slotsUsedNum As Integer
    Public Capacities() As String
    Public totalSlots As Integer

    'These are used to check to see if probing specific hardware is done.
    Public CPUDone As Boolean = False
    Public ParDone As Boolean = True
    Public RAMDone As Boolean = False
    Public HDDDone As Boolean = False

End Module
