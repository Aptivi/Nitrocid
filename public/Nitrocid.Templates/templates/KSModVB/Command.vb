﻿Imports Terminaux.Shell.Commands

Namespace KSModVB
    Friend Class TuiCommand
        Inherits BaseCommand
        Implements ICommand
        Public Overrides Function Execute(parameters As CommandParameters, ByRef variableValue As String) As Integer
            Return 0
        End Function
    End Class
End Namespace
