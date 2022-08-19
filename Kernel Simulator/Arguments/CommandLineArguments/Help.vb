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

Imports KS.Arguments.ArgumentBase

Namespace Arguments.CommandLineArguments
    Class CommandLine_HelpArgument
        Inherits ArgumentExecutor
        Implements IArgument

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements IArgument.Execute
            'Command-line arguments
            Write(DoTranslation("Command-line arguments:"), True, ColTypes.ListTitle)
            ShowArgsHelp(ArgumentType.CommandLineArgs)
            Console.WriteLine()

            'Pre-boot command-line arguments
            Write(DoTranslation("Pre-boot command-line arguments:"), True, ColTypes.ListTitle)
            ShowArgsHelp(ArgumentType.PreBootCommandLineArgs)
            Console.WriteLine()

            'Either start the kernel or exit it
            Write(DoTranslation("* Press any key to start the kernel or ESC to exit."), True, ColTypes.Tip)
            If Console.ReadKey(True).Key = ConsoleKey.Escape Then
                'Clear the console and reset the colors
                Console.ResetColor()
                Console.Clear()
                Environment.Exit(0)
            End If
        End Sub

    End Class
End Namespace