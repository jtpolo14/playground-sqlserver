Imports System.Data.SqlClient

Public Class SQLControl
    Private DBCon As New SqlConnection("YOURSERVERCONNECTIONHERE")
    Private DBCmd As SqlCommand

    ' DB Data
    Public DBDA As SqlDataAdapter
    Public DBDT As DataTable
    Public DBDS As DataSet

    ' Query Params
    Public Params As New List(Of SqlParameter)

    ' Query Stats
    Public RecordCount As Integer
    Public Exception As String

    Public Sub New()
    End Sub

    'Test the database connection
    Public Function HasConnection() As Boolean
        Try
            DBCon.Open()

            DBCon.Close()
            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return False
    End Function

    ' Allow Connection String Override
    Public Sub New(ConnectionString As String)
        DBCon = New SqlConnection(ConnectionString)
    End Sub

    ' Query Exec
    Public Sub ExecQuery(Query As String)
        ' Reset query stats
        RecordCount = 0
        Exception = ""

        Try
            DBCon.Open()

            ' create db command
            DBCmd = New SqlCommand(Query, DBCon)

            ' load params into db command
            Params.ForEach(Sub(p) DBCmd.Parameters.Add(p))

            ' clear param list
            Params.Clear()

            ' run query
            DBDT = New DataTable
            DBDA = New SqlDataAdapter(DBCmd)
            RecordCount = DBDA.Fill(DBDT)

        Catch ex As Exception

            'capture errors
            Exception = "ExecQuery Error: " & vbNewLine & ex.Message

        Finally
            ' CLOSE CONNECTION
            If DBCon.State = ConnectionState.Open Then DBCon.Close()
        End Try
    End Sub

    ' Add Params
    Public Sub AddParam(Name As String, Value As Object)
        Dim NewParam As New SqlParameter(Name, Value)
        Params.Add(NewParam)
    End Sub

    ' Error Checking
    Public Function HasException(Optional Report As Boolean = False) As Boolean
        If String.IsNullOrEmpty(Exception) Then Return False
        If Report = True Then MsgBox(Exception, MsgBoxStyle.Critical, "Exception:")
        Return True
    End Function
End Class
