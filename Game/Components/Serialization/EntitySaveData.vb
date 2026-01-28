Imports System.Text.Json
Imports System.Text.Json.Nodes

<Serializable>
Public Class EntitySaveData
    Public Property Id As Integer
    Public Property Components As New Dictionary(Of String, String)
End Class
