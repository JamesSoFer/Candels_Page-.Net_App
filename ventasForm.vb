Imports System.Runtime.InteropServices
Imports System.IO

Public Class ventasForm
    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal lParam As String) As Int32
    End Function



    Dim dt As DataTable

    Private Sub ventasForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.MdiParent = MainForm

        Me.WindowState = FormWindowState.Maximized

        SendMessage(Me.txtBuscar.Handle, &H1501, 0, "Ingrese el nombre del producto que busca") 'Hint Text


        LlenaBoton()
        LimpiarTabla()

    End Sub

    Private Sub bntNuevo_Click(sender As Object, e As EventArgs) Handles bntNuevo.Click
        LimpiarTabla()
        LlenaCarrito()
    End Sub

    Private Sub LimpiarTabla()
        dt = New DataTable

        dt.Columns.Add("descripcion", GetType(System.String))
        dt.Columns.Add("cantidad", GetType(System.Double))
        dt.Columns.Add("precio", GetType(System.Double))

        lbTotal.Text = "0"

    End Sub

    Public Sub LlenaCarrito()
        DataGridView1.DataSource = dt
        lbTotal.Text = "0"
        For Each fil As DataRow In dt.Rows
            lbTotal.Text = CDbl(lbTotal.Text) + (fil("cantidad") * fil("precio"))
        Next

        lbTotal.Text = Format(lbTotal.Text, "standard")
    End Sub


    Public Sub LlenaBoton()
        For Each item As Control In FlowLayoutPanel1.Controls
            FlowLayoutPanel1.Controls.Remove(item)
        Next
        FlowLayoutPanel1.Controls.Clear()


        Dim da As New OleDb.OleDbDataAdapter("select * from producto where descripcion like'%" & txtBuscar.Text & "%'", Conexion)
        Dim ds As New DataSet
        da.Fill(ds)
        Dim MiBoton As New Button

        If ds.Tables(0).Rows.Count > 0 Then

            Dim fila As DataRow
            For Each fila In ds.Tables(0).Rows
                MiBoton = New Button()
                Dim myfont As New Font("Segoe UI", 11, FontStyle.Bold)

                MiBoton.Text = fila("Descripcion")
                MiBoton.Tag = fila("id")

                Dim stream As New IO.MemoryStream
                Dim img() As Byte
                img = fila("foto")
                Dim ms As New MemoryStream(img)

                MiBoton.Width = 180
                MiBoton.Height = 180
                MiBoton.BackgroundImageLayout = ImageLayout.Zoom
                MiBoton.Font = myfont
                MiBoton.ForeColor = Color.White
                MiBoton.BackColor = Color.White
                MiBoton.TextAlign = ContentAlignment.TopLeft
                MiBoton.BackgroundImage = Image.FromStream(ms)

                FlowLayoutPanel1.Controls.Add(MiBoton)
                AddHandler MiBoton.Click, AddressOf Me.btn_done_clicked

            Next
        End If

    End Sub

    Private Sub btn_done_clicked(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim da As New OleDb.OleDbDataAdapter("select * from producto where id=" & (CType(CType(sender, System.Windows.Forms.Button).Tag, Integer)), Conexion)
        Dim ds As New DataSet
        da.Fill(ds)
        If ds.Tables(0).Rows.Count > 0 Then
            Dim fila As DataRow
            For Each fila In ds.Tables(0).Rows
                Dim Valor As Double
                Dim userInput As String = InputBox("Inserte la cantidad deseada", fila("descripcion"), 1)

                Dim validInput As Boolean = Double.TryParse(userInput, Valor)
                If validInput = False Then
                    MsgBox("Entrada Inválida")
                Else
                    dt.Rows.Add(fila("descripcion"), Valor, fila("Precio"))

                    LlenaCarrito()
                End If

            Next
        End If
    End Sub

    Private Sub btnRemover_Click(sender As Object, e As EventArgs) Handles btnRemover.Click
        For Each row In dt.Rows
            If row("descripcion") = DataGridView1.CurrentRow.Cells("Descripcion").Value.ToString Then
                dt.Rows.Remove(row)
                Exit For
            End If
        Next

        LlenaCarrito()
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        LlenaBoton()
    End Sub

    Private Sub btnPagar_Click(sender As Object, e As EventArgs) Handles btnPagar.Click
        If MessageBox.Show("¿Desea completar esta compra?", "Atencion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
            For Each fila In dt.Rows
                Dim cmd As New OleDb.OleDbCommand("insert into ventas(descripcion,cantidad,precio) values('" & fila("descripcion") & "','" & fila("cantidad") & "','" & fila("precio") & "')", Conexion)
                cmd.ExecuteNonQuery()
            Next

            LimpiarTabla()
            LlenaCarrito()
        End If
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub

    Private Sub FlowLayoutPanel1_Paint(sender As Object, e As PaintEventArgs) Handles FlowLayoutPanel1.Paint

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class