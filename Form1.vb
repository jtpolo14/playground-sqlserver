Public Class Form1

    Private SQL As New SQLControl

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub MiInventory_Click(sender As Object, e As EventArgs)
        Inventory.Show()
    End Sub

    Private Sub MiSales_Click(sender As Object, e As EventArgs)
        Sales.Show()
    End Sub

    Private Sub BtnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If SQL.HasConnection Then
            txtDetails.Text = "Successfully connected to the database."
        Else
            txtDetails.Text = "Cannot connect to database."
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Close()
    End Sub

    Private Function IsAuthenticated() As Boolean
        'Clear existing records
        If SQL.DBDS IsNot Nothing Then
            SQL.DBDS.Clear()
        End If

        SQL.ExecQuery("SELECT Count(username) As UserCount " &
                      "FROM members " &
                      "WHERE username='" & txtUser.Text & "'" &
                      "AND password='" & txtPass.Text & "' COLLATE SQL_Latin1_General_CP1_CS_AS")
        If SQL.DBDS.Tables(0).Rows(0).Item("UserCount") = 1 Then
            Return True
        Else
            txtDetails.Text = "Invalid credentials."
            Return False
        End If
    End Function

    Private Sub BtnInventory_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        Inventory.Show()
    End Sub

    Private Sub BtnSales_Click(sender As Object, e As EventArgs) Handles btnSales.Click
        Sales.Show()
    End Sub
End Class
