Imports System.IO

Public Class productosForm
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Limpiar()
    End Sub

    Private Sub Limpiar()
        txtID.Text = "0"
        txtDescripcion.Clear()
        txtPrecio.Clear()

        txtDescripcion.Focus()
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        PicFoto.Image.Save("miFoto.jpg")

        If txtID.Text = "0" Then
            Dim imageData As Byte()
            imageData = IO.File.ReadAllBytes("mifoto.jpg")


            Dim comando As New OleDb.OleDbCommand("Insert into producto(descripcion,foto,precio) values('" & txtDescripcion.Text & "',@FOTO,'" & txtPrecio.Text & "')", Conexion)
            comando.Parameters.Add("@FOTO", System.Data.OleDb.OleDbType.Binary).Value = imageData
            comando.ExecuteNonQuery()
        Else
            Dim imageData As Byte()
            imageData = IO.File.ReadAllBytes("mifoto.jpg")


            Dim comando As New OleDb.OleDbCommand("update producto set descripcion='" & txtDescripcion.Text & "',foto=@foto,precio='" & txtPrecio.Text & "' where id= " & txtID.Text, Conexion)
            comando.Parameters.Add("@FOTO", System.Data.OleDb.OleDbType.Binary).Value = imageData
            comando.ExecuteNonQuery()
        End If



        LlenarGrid()
    End Sub

    Private Sub LlenarGrid()

        Dim da As New OleDb.OleDbDataAdapter("Select * from producto", Conexion)
        Dim ds As New DataSet
        da.Fill(ds)
        If ds.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = ds.Tables(0)
        Else
            DataGridView1.DataSource = Nothing
        End If

    End Sub

    Private Sub cmdBorrar_Click(sender As Object, e As EventArgs) Handles cmdBorrar.Click
        Dim cmd As New OleDb.OleDbCommand("delete * from producto where id=" & DataGridView1.CurrentRow.Cells("ID").Value, Conexion)
        cmd.ExecuteNonQuery()

        LlenarGrid()
    End Sub

    Private Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click
        Dim ofd As OpenFileDialog = New OpenFileDialog
        ofd.Filter = "All files|*.*|Image files|*.jpg"
        ofd.Title = "Select file"
        If ofd.ShowDialog() <> DialogResult.Cancel Then
            PicFoto.Image = Image.FromFile(ofd.FileName)
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEnter
        If DataGridView1.RowCount > 0 Then
            Dim stream As New IO.MemoryStream
            Dim img() As Byte
            img = DataGridView1.CurrentRow.Cells("FOTO").Value
            Dim ms As New MemoryStream(img)
            PicFoto.Image = Image.FromStream(ms)

            txtDescripcion.Text = DataGridView1.CurrentRow.Cells("descripcion").Value.ToString
            txtPrecio.Text = DataGridView1.CurrentRow.Cells("precio").Value.ToString
            txtID.Text = DataGridView1.CurrentRow.Cells("id").Value.ToString
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) 
        LlenarGrid()
    End Sub

    Private Sub productosForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.MdiParent = MainForm
        LlenarGrid()

    End Sub
End Class