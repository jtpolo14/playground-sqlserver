Public Class Inventory
    Public SQL As New SQLControl

    Public Sub Loadgrid(Optional Query As String = "")
        If Query = "" Then
            SQL.ExecQuery("SELECT TOP 5* FROM Person.Person;")
        Else
            SQL.ExecQuery(Query)
        End If

        ' exit on error
        If SQL.HasException(True) Then Exit Sub
        dgvData.DataSource = SQL.DBDT
    End Sub

    Private Sub LoadCBX()
        ' refresh combo box
        cbxItems.Items.Clear()
        'SQL.ExecQuery("SELECT TOP 5 * ")
    End Sub

    Private Sub FindPerson()
        SQL.AddParam("@person", "%" & txtSearch.Text & "%")
        Loadgrid("SELECT * FROM Person.Person WHERE LastName LIKE @person;")
    End Sub



    Private Sub Inventory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'MdiParent = Form1
        Loadgrid()
    End Sub

    Private Sub CmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        FindPerson()
    End Sub
End Class