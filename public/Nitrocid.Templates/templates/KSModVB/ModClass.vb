Imports Nitrocid.Base.Kernel.Extensions

Namespace KSModVB
    Public Class ModClass
        Implements IMod
        Public ReadOnly Property Name As String Implements IMod.Name
            Get
                Return "KSModVB"
            End Get
        End Property

        Public ReadOnly Property Version As String Implements IMod.Version
            Get
                Return "1.0.0"
            End Get
        End Property

        Public ReadOnly Property MinimumSupportedApiVersion As Version Implements IMod.MinimumSupportedApiVersion
            Get
                Return New Version(4, 0, 28, 22)
            End Get
        End Property

        Public ReadOnly Property LoadPriority As ModLoadPriority Implements IMod.LoadPriority
            Get
                Return ModLoadPriority.Optional
            End Get
        End Property

        Public Sub StartMod() Implements IMod.StartMod

        End Sub

        Public Sub StopMod() Implements IMod.StopMod

        End Sub
    End Class
End Namespace

' Refer to https://aptivi.github.io/Nitrocid for up-to-date API documentation for mod developers.
