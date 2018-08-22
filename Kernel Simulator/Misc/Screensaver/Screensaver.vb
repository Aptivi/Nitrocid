
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

Imports Microsoft.VisualBasic
Imports System.CodeDom.Compiler
Imports System.Reflection
Imports System.ComponentModel

Public Module Screensaver

    'Variables and Interface
    Public LockMode As Boolean = False
    Public defSaverName As String = "matrix" 'Permanent screensavers will be added in 0.0.6. Currently, it has been set to matrix
    Public ScrnSvrdb As New Dictionary(Of String, Boolean) From {{"colorMix", False}, {"matrix", False}, {"disco", False}}
    Public WithEvents colorMix As New BackgroundWorker
    Public WithEvents matrix As New BackgroundWorker
    Public WithEvents disco As New BackgroundWorker
    Public WithEvents custom As New BackgroundWorker
    Public colors() As ConsoleColor = CType(ConsoleColor.GetValues(GetType(ConsoleColor)), ConsoleColor())  'Console Colors
    Private execCustomSaver As CompilerResults
    Private finalSaver As ICustomSaver
    Private DoneFlag As Boolean = False
    Public Interface ICustomSaver
        Sub initSaver()
        Sub preDisplay()
        Sub scrnSaver()
        Property initialized As Boolean
    End Interface

    Sub custom_DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs) Handles custom.DoWork

        'To Screensaver Developers: ONLY put the effect code in your scrnSaver() sub.
        '                           Set colors, write welcome message, etc. with the exception of infinite loop and the effect code in preDisplay() sub
        '                           Recommended: Turn off console cursor, and clear the screen in preDisplay() sub.
        '                           Substitute: TextWriterColor.W() or TextWriterColor.Wln() with System.Console.WriteLine() or System.Console.Write().
        finalSaver.preDisplay()
        Do While True
            Sleep(1)
            If (custom.CancellationPending = True) Then
                e.Cancel = True
                Console.Clear()
                System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                System.Console.BackgroundColor = CType(backgroundColor, ConsoleColor)
                LoadBackground.Load()
                Console.CursorVisible = True
                Exit Do
            Else
                finalSaver.scrnSaver()
            End If
        Loop

    End Sub

    Sub colorMix_DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs) Handles colorMix.DoWork

        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        Console.CursorVisible = False
        Dim colorrand As New Random()
        Do While True
            Sleep(1)
            If (colorMix.CancellationPending = True) Then
                e.Cancel = True
                Console.Clear()
                System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                System.Console.BackgroundColor = CType(backgroundColor, ConsoleColor)
                LoadBackground.Load()
                Console.CursorVisible = True
                Exit Do
            Else
                System.Console.BackgroundColor = CType(colorrand.Next(1, 16), ConsoleColor) : Console.Write(" ")
            End If
        Loop

    End Sub

    Sub matrix_DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs) Handles matrix.DoWork

        'At this time, the matrix is basic and only prints zeroes and ones. The advanced matrix will be on 0.0.6.
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.Green
        Console.Clear()
        Console.CursorVisible = False
        Dim random As New Random()
        Do While True
            If (matrix.CancellationPending = True) Then
                e.Cancel = True
                Console.Clear()
                System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                System.Console.BackgroundColor = CType(backgroundColor, ConsoleColor)
                LoadBackground.Load()
                Console.CursorVisible = True
                Exit Do
            Else
                Sleep(1)
                Console.Write(CStr(random.Next(2)))
            End If
        Loop

    End Sub

    Sub disco_DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs) Handles disco.DoWork

        Console.CursorVisible = False
        Do While True
            For Each color In colors
                Sleep(100)
                If (disco.CancellationPending = True) Then
                    e.Cancel = True
                    Console.Clear()
                    System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                    System.Console.BackgroundColor = CType(backgroundColor, ConsoleColor)
                    LoadBackground.Load()
                    Console.CursorVisible = True
                    Exit Do
                Else
                    Console.BackgroundColor = color
                    Console.Clear()
                End If
            Next
        Loop

    End Sub

    Sub ShowSavers(ByVal saver As String)

        If (saver = "colorMix") Then
            colorMix.WorkerSupportsCancellation = True
            colorMix.RunWorkerAsync()
            Console.ReadKey()
            colorMix.CancelAsync()
            Sleep(50)
        ElseIf (saver = "matrix") Then
            matrix.WorkerSupportsCancellation = True
            matrix.RunWorkerAsync()
            Console.ReadKey()
            matrix.CancelAsync()
            Sleep(50)
        ElseIf (saver = "disco") Then
            disco.WorkerSupportsCancellation = True
            disco.RunWorkerAsync()
            Console.ReadKey()
            disco.CancelAsync()
            Sleep(150)
        ElseIf (ScrnSvrdb.ContainsKey(saver)) Then
            'Only one custom screensaver can be used.
            custom.WorkerSupportsCancellation = True
            custom.RunWorkerAsync()
            Console.ReadKey()
            custom.CancelAsync()
            Sleep(50)
        Else
            Wln("The requested screensaver {0} is not found.", "neutralText", saver)
        End If

    End Sub

    'TODO: In the final release of 0.0.5, change so it probes everything on boot
    Sub compileCustom(ByVal file As String)

        Dim modPath As String = Environ("USERPROFILE") + "\KSMods\"
        If (FileIO.FileSystem.FileExists(modPath + file)) Then
            For Each modFile As String In FileIO.FileSystem.GetFiles(modPath)
                modFile = modFile.Replace(modPath, "")
                If (modFile = file) Then
                    If Not modFile.EndsWith("SS.m") Then
                        Wdbg("{0} is not a screensaver. A screensaver code should have ""SS.m"" at the end.", True, modFile)
                    Else
                        finalSaver = GenSaver(IO.File.ReadAllText(modPath + modFile))
                        If (DoneFlag = True) Then
                            finalSaver.initSaver()
                            If (finalSaver.initialized = True) Then
                                If Not (ScrnSvrdb.ContainsKey(modFile)) Then
                                    Wln("{0} has been initialized properly.", "neutralText", modFile)
                                    ScrnSvrdb.Add(modFile, False)
                                Else
                                    Wln("{0} already exists.", "neutralText", modFile)
                                End If
                            Else
                                Wln("{0} did not initialize. The screensaver code might have experienced an error while initializing.", "neutralText", modFile)
                            End If
                        End If
                    End If
                End If
            Next
        Else
            Wln("Screensaver {0} does not exist.", "neutralText", file)
        End If

    End Sub

    Sub setDefaultScreensaver(ByVal saver As String, Optional ByVal setDef As Boolean = True)

        If (ScrnSvrdb.ContainsKey(saver)) Then

            ScrnSvrdb(defSaverName) = False
            defSaverName = saver
            ScrnSvrdb(saver) = setDef
            Wln("{0} is set to default screensaver.", "neutralText", saver)

        Else

            Wln("Screensaver {0} not found in database. Initialize with ""loadsaver {0}"".", "neutralText", saver)

        End If

    End Sub

    Function GenSaver(ByVal code As String) As ICustomSaver
        Using provider As New VBCodeProvider()
            Dim prm As New CompilerParameters()
            prm.GenerateExecutable = False
            prm.GenerateInMemory = True
            prm.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly.Location)
            prm.ReferencedAssemblies.Add("System.dll")
            prm.ReferencedAssemblies.Add("System.Core.dll")
            prm.ReferencedAssemblies.Add("System.Data.dll")
            prm.ReferencedAssemblies.Add("System.DirectoryServices.dll")
            prm.ReferencedAssemblies.Add("System.Xml.dll")
            prm.ReferencedAssemblies.Add("System.Xml.Linq.dll")
            Dim namespc As String = GetType(ICustomSaver).Namespace
            Dim modCode() As String = New String() {"Imports " & namespc & vbNewLine & code}
            execCustomSaver = provider.CompileAssemblyFromSource(prm, modCode)
            If (execCustomSaver.Errors.HasErrors) And (Quiet = False) Then
                Wln("Screensaver can't be loaded because of the following: ", "neutralText")
                For Each errorName In execCustomSaver.Errors
                    Wln(errorName.ToString, "neutralText") : Wdbg(errorName.ToString, True)
                Next
                Exit Function
            Else
                DoneFlag = True
            End If
            For Each t As Type In execCustomSaver.CompiledAssembly.GetTypes()
                If t.GetInterface(GetType(ICustomSaver).Name) IsNot Nothing Then Return CType(execCustomSaver.CompiledAssembly.CreateInstance(t.Name), ICustomSaver)
            Next
        End Using
    End Function

End Module
