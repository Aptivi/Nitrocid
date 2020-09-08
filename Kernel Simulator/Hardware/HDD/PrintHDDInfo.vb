
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Module PrintHDDInfo

    ''' <summary>
    ''' Prints information about probed drives
    ''' </summary>
    Sub PrintDrives()
        Dim DriveNum As Integer = 1
        If EnvironmentOSType.Contains("Windows") Then
            For Each HD As HDD In HDDList
                W("==========================================", True, ColTypes.Neutral)
                W(DoTranslation("Drive Number: {0}", currentLang), True, ColTypes.Neutral, DriveNum)
                W("ID: {0}", True, ColTypes.Neutral, HD.ID)
                W(DoTranslation("Manufacturer: {0}", currentLang), True, ColTypes.Neutral, HD.Manufacturer)
                W(DoTranslation("Model: {0}", currentLang), True, ColTypes.Neutral, HD.Model)
                W(DoTranslation("Capacity: {0} GB ({1}, {2}, {3})", currentLang), True, ColTypes.Neutral, FormatNumber(HD.Size / 1024 / 1024 / 1024, 2), HD.Cylinders, HD.Heads, HD.Sectors)
                W(DoTranslation("Interface Type: {0}", currentLang), True, ColTypes.Neutral, HD.InterfaceType)
                DriveNum += 1
            Next
            W("==========================================", True, ColTypes.Neutral)
        ElseIf EnvironmentOSType.Contains("Unix") Then
            For Each HD As HDD_Linux In HDDList
                W("==========================================", True, ColTypes.Neutral)
                W(DoTranslation("Drive Number: {0}", currentLang), True, ColTypes.Neutral, DriveNum)
                W(DoTranslation("Manufacturer: {0}", currentLang), True, ColTypes.Neutral, HD.Vendor_LNX)
                W(DoTranslation("Model: {0}", currentLang), True, ColTypes.Neutral, HD.Model_LNX)
                W(DoTranslation("Capacity: {0}", currentLang), True, ColTypes.Neutral, HD.Size_LNX)
                W(DoTranslation("Partition Count: {0}", currentLang), True, ColTypes.Neutral, HD.Parts_LNX.Count)
                DriveNum += 1
            Next
            W("==========================================", True, ColTypes.Neutral)
        End If
    End Sub

    ''' <summary>
    ''' Prints information about partitions
    ''' </summary>
    ''' <param name="Drive">Hard drive number</param>
    Sub PrintPartitions(ByVal Drive As Integer)
        Dim PartNum As Integer = 1
        Dim DriveIndex As Integer = Drive - 1
        If EnvironmentOSType.Contains("Windows") Then
            For Each P As Part In HDDList(DriveIndex).Parts
                W("==========================================", True, ColTypes.Neutral)
                W(DoTranslation("Physical partition: {0}", currentLang), True, ColTypes.Neutral, PartNum)
                W(DoTranslation("Boot flag: {0} ({1})", currentLang), True, ColTypes.Neutral, P.Boot, P.Bootable)
                W(DoTranslation("Primary flag: {0}", currentLang), True, ColTypes.Neutral, P.Primary)
                W(DoTranslation("Size: {0} GB", currentLang), True, ColTypes.Neutral, FormatNumber(P.Size / 1024 / 1024 / 1024, 2))
                PartNum += 1
            Next
            PartNum = 1
            W(DoTranslation("Listing all logical partitions on all drives.", currentLang), True, ColTypes.Neutral)
            For Each LP As Logical In LogList
                W("==========================================", True, ColTypes.Neutral)
                W(DoTranslation("Logical partition: {0}", currentLang), True, ColTypes.Neutral, PartNum)
                W(DoTranslation("Compressed: {0}", currentLang), True, ColTypes.Neutral, LP.Compressed)
                W(DoTranslation("Name: {0}", currentLang), True, ColTypes.Neutral, LP.Name)
                W(DoTranslation("File system: {0}", currentLang), True, ColTypes.Neutral, LP.FileSystem)
                W(DoTranslation("Capacity: {0} GB free of {1} GB", currentLang), True, ColTypes.Neutral, FormatNumber(LP.Free / 1024 / 1024 / 1024, 2), FormatNumber(LP.Size / 1024 / 1024 / 1024, 2))
                PartNum += 1
            Next
        ElseIf EnvironmentOSType.Contains("Unix") Then
            For Each P As Part_Linux In HDDList(DriveIndex).Parts
                W("==========================================", True, ColTypes.Neutral)
                W(DoTranslation("Physical partition: {0}", currentLang), True, ColTypes.Neutral, PartNum)
                W(DoTranslation("File system: {0}", currentLang), True, ColTypes.Neutral, P.FileSystem)
                W(DoTranslation("Size: {0}", currentLang), True, ColTypes.Neutral, P.SizeMEAS)
                W(DoTranslation("Used: {0}", currentLang), True, ColTypes.Neutral, P.Used)
                PartNum += 1
            Next
        End If
        W("==========================================", True, ColTypes.Neutral)
    End Sub

End Module
