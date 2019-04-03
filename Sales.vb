Public Class Sales
    Private SQL As New SQLControl

    Private Sub Sales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'MdiParent = Form1
        dpRequestSaleDate.CustomFormat = " MM, yyyy"
        dpRequestSaleDate.Format = DateTimePickerFormat.Custom
        LoadGrid()
    End Sub

    Private Sub LoadGrid(Optional Query As String = "")
        cbxCustomer.Items.Clear()
        cbxCustomer.Text = ""
        If Query = "" Then
            ' run default query
            SQL.ExecQuery("SELECT * FROM SALES.SalesOrderHeader WHERE OrderDate = GETDATE();")
        Else
            ' run the provided query
            SQL.ExecQuery(Query)
        End If

        dgvSales.DataSource = SQL.DBDT

        ' populate customer combobox
        Dim dataView As New DataView(SQL.DBDT)
        dataView.Sort = "CustomerID ASC"
        Dim sortedCustomerTable As DataTable = dataView.ToTable()
        Dim distinctCustomers = From row In sortedCustomerTable
                                Select row.Field(Of Integer)("CustomerId")
                                Distinct
        For Each r In distinctCustomers
            cbxCustomer.Items.Add(r.ToString)
        Next

    End Sub

    Private Sub GetOrderByDate()
        SQL.AddParam("@requestedDate", dpRequestSaleDate.Value.ToString("yyyy-MM-dd"))
        LoadGrid("SELECT * FROM SALES.SalesOrderHeader WHERE OrderDate >= @requestedDate and OrderDate < dateadd(month,1,@requestedDate) ;")
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles dpRequestSaleDate.ValueChanged
        GetOrderByDate()
    End Sub

    Private Sub CbxCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxCustomer.SelectedIndexChanged

        ' filter data grid view by customer id
        Dim dataView2 As New DataView(SQL.DBDT)
        dataView2.RowFilter = "CustomerID = " & cbxCustomer.Text
        dgvSales.DataSource = dataView2
    End Sub
End Class