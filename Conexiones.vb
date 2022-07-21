Module Conexiones

    Public Conexion As OleDb.OleDbConnection

    Public Sub AbrirConexion()
        Conexion = New OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source= basedatos.mdb;")
        Conexion.Open()
    End Sub
End Module
