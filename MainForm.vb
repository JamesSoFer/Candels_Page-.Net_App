Public Class MainForm
    Private Sub RegistroDeProductosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegistroDeProductosToolStripMenuItem.Click
        productosForm.Show()

    End Sub

    Private Sub VentasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VentasToolStripMenuItem.Click
        ventasForm.Show()
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AbrirConexion()
    End Sub
End Class
