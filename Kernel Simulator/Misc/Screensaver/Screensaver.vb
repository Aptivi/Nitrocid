
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

    'Variables
    Public LockMode As Boolean = False
    Public InSaver As Boolean = False
    Public defSaverName As String = "matrix"
    Public ScrnSvrdb As New Dictionary(Of String, Boolean) From {{"colorMix", False}, {"matrix", False}, {"disco", False}}
    Public WithEvents ColorMix As New BackgroundWorker
    Public WithEvents Matrix As New BackgroundWorker
    Public WithEvents Disco As New BackgroundWorker
    Public WithEvents Custom As New BackgroundWorker
    Public colors() As ConsoleColor = CType([Enum].GetValues(GetType(ConsoleColor)), ConsoleColor())  'Console Colors
    Private execCustomSaver As CompilerResults
    Private finalSaver As ICustomSaver
    Private DoneFlag As Boolean = False

    'Interface
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
        '                           Substitute: TextWriterColor.W() with System.Console.WriteLine() or System.Console.Write().
        finalSaver.PreDisplay()
        Do While True
            'Thread.Sleep(1)
            If Custom.CancellationPending = True Then
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = CType(inputColor, ConsoleColor)
                Console.BackgroundColor = CType(backgroundColor, ConsoleColor)
                Load()
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
            If ColorMix.CancellationPending = True Then
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = CType(inputColor, ConsoleColor)
                Console.BackgroundColor = CType(backgroundColor, ConsoleColor)
                Load()
                Console.CursorVisible = True
                Exit Do
            Else
                Console.BackgroundColor = CType(colorrand.Next(1, 16), ConsoleColor) : Console.Write(" ")
            End If
        Loop
    End Sub

    Sub Matrix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Matrix.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.Green
        Console.Clear()
        Console.CursorVisible = False
        Dim random As New Random()
        Do While True
            If Matrix.CancellationPending = True Then
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = CType(inputColor, ConsoleColor)
                Console.BackgroundColor = CType(backgroundColor, ConsoleColor)
                Load()
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
                If Disco.CancellationPending = True Then
                    e.Cancel = True
                    Console.Clear()
                    Console.ForegroundColor = CType(inputColor, ConsoleColor)
                    Console.BackgroundColor = CType(backgroundColor, ConsoleColor)
                    Load()
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
        InSaver = True
        EventManager.RaisePreShowScreensaver()
        If saver = "colorMix" Then
            ColorMix.WorkerSupportsCancellation = True
            ColorMix.RunWorkerAsync()
            Console.ReadKey()
            ColorMix.CancelAsync()
            Thread.Sleep(150)
        ElseIf saver = "matrix" Then
            Matrix.WorkerSupportsCancellation = True
            Matrix.RunWorkerAsync()
            Console.ReadKey()
            Matrix.CancelAsync()
            Thread.Sleep(150)
        ElseIf saver = "disco" Then
            Disco.WorkerSupportsCancellation = True
            Disco.RunWorkerAsync()
            Console.ReadKey()
            Disco.CancelAsync()
            Thread.Sleep(150)
        ElseIf ScrnSvrdb.ContainsKey(saver) Then
            'Only one custom screensaver can be used.
            Custom.WorkerSupportsCancellation = True
            Custom.RunWorkerAsync()
            Console.ReadKey()
            Custom.CancelAsync()
            Thread.Sleep(150)
        Else
            W(DoTranslation("The requested screensaver {0} is not found.", currentLang), True, ColTypes.Neutral, saver)
        End If
        EventManager.RaisePostShowScreensaver()
        InSaver = False
    End Sub

    Sub CompileCustom(ByVal file As String)
        Dim modPath As String = paths("Mods")
        If FileIO.FileSystem.FileExists(modPath + file) Then
            For Each modFile As String In FileIO.FileSystem.GetFiles(modPath)
                modFile = modFile.Replace(modPath, "")
                If modFile = file Then
                    If Not modFile.EndsWith("SS.m") Then
                        Wdbg("{0} is not a screensaver. A screensaver code should have ""SS.m"" at the end.", modFile)
                    Else
                        finalSaver = GenSaver(IO.File.ReadAllText(modPath + modFile))
                        If DoneFlag = True Then
                            finalSaver.InitSaver()
                            If finalSaver.Initialized = True Then
                                If Not ScrnSvrdb.ContainsKey(modFile) Then
                                    W(DoTranslation("{0} has been initialized properly.", currentLang), True, ColTypes.Neutral, modFile)
                                    ScrnSvrdb.Add(modFile, False)
                                Else
                                    ScrnSvrdb.Remove(modFile)
                                    CompileCustom(file)
                                    Exit Sub
                                End If
                            Else
                                W(DoTranslation("{0} did not initialize. The screensaver code might have experienced an error while initializing.", currentLang), True, ColTypes.Neutral, modFile)
                            End If
                        End If
                    End If
                End If
            Next
        Else
            W(DoTranslation("Screensaver {0} does not exist.", currentLang), True, ColTypes.Neutral, file)
        End If
    End Sub

    Sub SetDefaultScreensaver(ByVal saver As String, Optional ByVal setDef As Boolean = True)
        If ScrnSvrdb.ContainsKey(saver) Then
            Dim ksconf As New IniFile()
            Dim pathConfig As String = paths("Configuration")
            ksconf.Load(pathConfig)
            ksconf.Sections("Misc").Keys("Screensaver").Value = saver
            ksconf.Save(pathConfig)
            ScrnSvrdb(defSaverName) = False
            defSaverName = saver
            ScrnSvrdb(saver) = setDef
            W(DoTranslation("{0} is set to default screensaver.", currentLang), True, ColTypes.Neutral, saver)
        Else
            W(DoTranslation("Screensaver {0} not found in database. Initialize with ""loadsaver {0}"".", currentLang), True, ColTypes.Neutral, saver)
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
            If execCustomSaver.Errors.HasErrors Then
                W(DoTranslation("Screensaver can't be loaded because of the following: ", currentLang), True, ColTypes.Neutral)
                For Each errorName In execCustomSaver.Errors
                    W(errorName.ToString, True, ColTypes.Neutral) : Wdbg(errorName.ToString, True)
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
