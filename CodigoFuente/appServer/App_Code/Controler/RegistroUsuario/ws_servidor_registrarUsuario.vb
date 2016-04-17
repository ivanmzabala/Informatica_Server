Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports MySql.Data.MySqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
'<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class ws_servidor_registrarUsuario
    Inherits System.Web.Services.WebService

    <WebMethod()> Public Function agregarUsuario(ByVal u As OEUsuario) As OSUsuario


        Dim os As New OSUsuario
        Dim b As New Conection

        Try
            b.SQLConnection = New MySqlConnection()
            b.SQLConnection.ConnectionString = b.connectionString
            Dim sqlCommand As New MySqlCommand
            Dim str_carSql As String

            str_carSql = "insert into usuario (nombre,correo,clave,telefono,nombreAmor,correoAmor,telefonoAmor,facebookAmor) Values ('" + u.nombre.ToString + "','" + u.correo.ToString + "','" + u.clave.ToString + "','" + u.telefono.ToString + "','" + u.nombreAmor.ToString + "','" + u.correoAmor.ToString + "','" + u.telefonoAmor.ToString + "','" + u.facebookAmor.ToString + "')"

            sqlCommand.Connection = b.SQLConnection
            sqlCommand.CommandText = str_carSql
            b.SQLConnection.Open()

            sqlCommand.ExecuteNonQuery()

            b.SQLConnection.Close()

            os.codigoRespuesta = 1
            os.mensajeRespuesta = "El usuario ha sido registrado correctamente"

        Catch ex As Exception

            os.codigoRespuesta = 0
            os.mensajeRespuesta = ex.ToString

        End Try

        Return os

    End Function

End Class