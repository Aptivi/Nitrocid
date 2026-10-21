Imports Terminaux.Shell.Commands
Imports Terminaux.Shell.Shells

Namespace KSModVB
    Friend Class Command
        Inherits BaseCommand
        Implements ICommand

        Public Overrides ReadOnly Property Command As String
            Get
                Return "command"
            End Get
        End Property

        Public Overrides ReadOnly Property HelpDefinition As String
            Get
                Return "Definition"
            End Get
        End Property

        Public Overrides Function Execute(shell As IShell, parameters As CommandParameters, ByRef variableValue As String) As Integer
            Return 0
        End Function
    End Class
End Namespace
