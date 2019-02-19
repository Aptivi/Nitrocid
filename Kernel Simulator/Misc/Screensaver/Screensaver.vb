
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Imports System.CodeDom.Compiler
Imports System.Reflection
Imports System.ComponentModel
Imports System.Threading

Public Module Screensaver

    'TODO: Screensavers can have their own variables, and the only thing required is the CancelPending variable.
    'Variables and Interface
    Public LockMode As Boolean = False
    Public defSaverName As String = "matrix" 'TODO: Permanent screensavers will be added in 0.0.6. Currently, it has been set to matrix
    Public ScrnSvrdb As New Dictionary(Of String, Boolean) From {{"colorMix", False}, {"matrix", False}, {"disco", False}}
    Public WithEvents ColorMix As New BackgroundWorker
    Public WithEvents Matrix As New BackgroundWorker
    Public WithEvents Disco As New BackgroundWorker
    Public WithEvents Custom As New BackgroundWorker
    Public colors() As ConsoleColor = CType([Enum].GetValues(GetType(ConsoleColor)), ConsoleColor())  'Console Colors
    Private execCustomSaver As CompilerResults
    Private finalSaver As ICustomSaver
    Private DoneFlag As Boolean = False
    Public Interface ICustomSaver
        Sub InitSaver()
        Sub PreDisplay()
        Sub ScrnSaver()
        Property Initialized As Boolean
    End Interface

    Sub Custom_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Custom.DoWork

        'To Screensaver Developers: ONLY put the effect code in your scrnSaver() sub.
        '                           Set colors, write welcome message, etc. with the exception of infinite loop and the effect code in preDisplay() sub
        '                           Recommended: Turn off console cursor, and clear the screen in preDisplay() sub.
        '                           Substitute: TextWriterColor.W() or TextWriterColor.Wln() with System.Console.WriteLine() or System.Console.Write().
        finalSaver.PreDisplay()
        Do While True
            Thread.Sleep(1)
            If (Custom.CancellationPending = True) Then
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = CType(inputColor, ConsoleColor)
                Console.BackgroundColor = CType(backgroundColor, ConsoleColor)
                LoadBackground.Load()
                Console.CursorVisible = True
                Exit Do
            Else
                finalSaver.ScrnSaver()
            End If
        Loop

    End Sub

    Sub ColorMix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles ColorMix.DoWork

        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        Console.CursorVisible = False
        Dim colorrand As New Random()
        Do While True
            Thread.Sleep(1)
            If (ColorMix.CancellationPending = True) Then
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = CType(inputColor, ConsoleColor)
                Console.BackgroundColor = CType(backgroundColor, ConsoleColor)
                LoadBackground.Load()
                Console.CursorVisible = True
                Exit Do
            Else
                Console.BackgroundColor = CType(colorrand.Next(1, 16), ConsoleColor) : Console.Write(" ")
            End If
        Loop

    End Sub

    Sub Matrix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Matrix.DoWork

        'TODO: At this time, the matrix is basic and only prints zeroes and ones. The advanced matrix will be on 0.0.6.
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.Green
        Console.Clear()
        Console.CursorVisible = False
        Dim random As New Random()
        Do While True
            If (Matrix.CancellationPending = True) Then
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = CType(inputColor, ConsoleColor)
                Console.BackgroundColor = CType(backgroundColor, ConsoleColor)
                LoadBackground.Load()
                Console.CursorVisible = True
                Exit Do
            Else
                Thread.Sleep(1)
                Console.Write(CStr(random.Next(2)))
            End If
        Loop

    End Sub

    Sub Disco_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Disco.DoWork

        Console.CursorVisible = False
        Do While True
            For Each color In colors
                Thread.Sleep(100)
                If (Disco.CancellationPending = True) Then
                    e.Cancel = True
                    Console.Clear()
                    Console.ForegroundColor = CType(inputColor, ConsoleColor)
                    Console.BackgroundColor = CType(backgroundColor, ConsoleColor)
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

        EventManager.RaisePreShowScreensaver()
        If (saver = "colorMix") Then
            ColorMix.WorkerSupportsCancellation = True
            ColorMix.RunWorkerAsync()
            Console.ReadKey()
            ColorMix.CancelAsync()
            Thread.Sleep(50)
        ElseIf (saver = "matrix") Then
            Matrix.WorkerSupportsCancellation = True
            Matrix.RunWorkerAsync()
            Console.ReadKey()
            Matrix.CancelAsync()
            Thread.Sleep(50)
        ElseIf (saver = "disco") Then
            Disco.WorkerSupportsCancellation = True
            Disco.RunWorkerAsync()
            Console.ReadKey()
            Disco.CancelAsync()
            Thread.Sleep(150)
        ElseIf (ScrnSvrdb.ContainsKey(saver)) Then
            'Only one custom screensaver can be used.
            Custom.WorkerSupportsCancellation = True
            Custom.RunWorkerAsync()
            Console.ReadKey()
            Custom.CancelAsync()
            Thread.Sleep(50)
        Else
            Wln(DoTranslation("The requested screensaver {0} is not found.", currentLang), "neutralText", saver)
        End If
        EventManager.RaisePostShowScreensaver()

    End Sub

    Sub CompileCustom(ByVal file As String)

        Dim modPath As String
        If (EnvironmentOSType.Contains("Unix")) Then 'TODO: Remove instances of this block, making path variables for each path appropriate for every platform
            modPath = Environ("HOME") + "/KSMods/"
        Else
            modPath = Environ("USERPROFILE") + "\KSMods\"
        End If
        If (FileIO.FileSystem.FileExists(modPath + file)) Then
            For Each modFile As String In FileIO.FileSystem.GetFiles(modPath)
                modFile = modFile.Replace(modPath, "")
                If (modFile = file) Then
                    If Not modFile.EndsWith("SS.m") Then
                        Wdbg("{0} is not a screensaver. A screensaver code should have ""SS.m"" at the end.", modFile)
                    Else
                        finalSaver = GenSaver(IO.File.ReadAllText(modPath + modFile))
                        If (DoneFlag = True) Then
                            finalSaver.InitSaver()
                            If (finalSaver.Initialized = True) Then
                                If Not (ScrnSvrdb.ContainsKey(modFile)) Then
                                    Wln(DoTranslation("{0} has been initialized properly.", currentLang), "neutralText", modFile)
                                    ScrnSvrdb.Add(modFile, False)
                                Else
                                    ScrnSvrdb.Remove(modFile)
                                    CompileCustom(file)
                                    Exit Sub
                                End If
                            Else
                                Wln(DoTranslation("{0} did not initialize. The screensaver code might have experienced an error while initializing.", currentLang), "neutralText", modFile)
                            End If
                        End If
                    End If
                End If
            Next
        Else
            Wln(DoTranslation("Screensaver {0} does not exist.", currentLang), "neutralText", file)
        End If

    End Sub

    Sub SetDefaultScreensaver(ByVal saver As String, Optional ByVal setDef As Boolean = True)

        If (ScrnSvrdb.ContainsKey(saver)) Then
            ScrnSvrdb(defSaverName) = False
            defSaverName = saver
            ScrnSvrdb(saver) = setDef
            Wln(DoTranslation("{0} is set to default screensaver.", currentLang), "neutralText", saver)
        Else
            Wln(DoTranslation("Screensaver {0} not found in database. Initialize with ""loadsaver {0}"".", currentLang), "neutralText", saver)
        End If

    End Sub

    Function GenSaver(ByVal code As String) As ICustomSaver
        DoneFlag = False
        Using provider As New VBCodeProvider()
            Dim prm As New CompilerParameters With {
                .GenerateExecutable = False,
                .GenerateInMemory = True
            }
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
                Wln(DoTranslation("Screensaver can't be loaded because of the following: ", currentLang), "neutralText")
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
