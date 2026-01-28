Imports System.Text.Json
Imports System.Text.Json.Nodes
Imports System.Text.Json.Serialization

<Serializable>
Public Class EntitySaveData
    Public Property Id As Integer

    Private _components As Dictionary(Of String, JsonElement)

    <JsonPropertyName("Components")>
    Public Property Components As Dictionary(Of String, JsonElement)
        Get
            If _components Is Nothing Then _components = New Dictionary(Of String, JsonElement)
            Return _components
        End Get
        Set(value As Dictionary(Of String, JsonElement))
            _components = value
        End Set
    End Property

    Public Sub New()
        _components = New Dictionary(Of String, JsonElement)
    End Sub
End Class
