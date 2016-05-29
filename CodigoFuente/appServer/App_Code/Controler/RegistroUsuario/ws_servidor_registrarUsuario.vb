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
        Dim idUsuario As Integer
        idUsuario = 0

        'u.token inicio validacion token -----------
        Dim validacionToken As Boolean
        Dim tokenv As New ValidacionToken
        validacionToken = tokenv.validar(u.token)
        If (validacionToken) Then

        Else

            os.codigoRespuesta = 2
            os.mensajeRespuesta = "Token no valido"

            Return os
        End If
        'Fin validacion token ----------------------

        idUsuario = existeCorreo(u.correo)

        If idUsuario <> 0 Then
            os.codigoRespuesta = 2
            os.mensajeRespuesta = "Usuario existe"

            Return os
        End If


        Try
            b.SQLConnection = New MySqlConnection()
            b.SQLConnection.ConnectionString = b.connectionString
            Dim sqlCommand As New MySqlCommand
            Dim str_carSql As String

            str_carSql = "insert into usuario (nombre,correo,clave,telefono,nombreAmor,correoAmor,telefonoAmor,facebookAmor) Values ('" + u.nombre.ToString + "','" + u.correo.ToString + "','" + u.clave.ToString + "','" + u.telefono.ToString() + "','" + u.nombreAmor.ToString + "','" + u.correoAmor.ToString + "','" + u.telefonoAmor.ToString() + "','" + u.facebookAmor.ToString + "')"

            sqlCommand.Connection = b.SQLConnection
            sqlCommand.CommandText = str_carSql
            b.SQLConnection.Open()
            sqlCommand.ExecuteNonQuery()
            b.SQLConnection.Close()
            os.codigoRespuesta = 1
            os.mensajeRespuesta = "El usuario ha sido registrado correctamente"


            newRec(existeCorreo(u.correo))
        Catch ex As Exception

            os.codigoRespuesta = 0
            os.mensajeRespuesta = ex.ToString

        End Try

        Return os
    End Function



    Public Function newRec(ByVal usuario As String) As String
        Dim b As New Conection

        Try
            b.SQLConnection = New MySqlConnection()
            b.SQLConnection.ConnectionString = b.connectionString
            Dim sqlCommand As New MySqlCommand
            Dim str_carSql As String
            str_carSql = "insert into reconocimiento (idUsuario, amorValido,enviarDulce,enviarMsjs) Values ('" + usuario.ToString + "','" + 0.ToString + "','" + 0.ToString + "','" + 0.ToString + "')"

            sqlCommand.Connection = b.SQLConnection
            sqlCommand.CommandText = str_carSql
            b.SQLConnection.Open()
            sqlCommand.ExecuteNonQuery()

            b.SQLConnection.Close()



        Catch ex As Exception

        End Try

        Return usuario
    End Function





    Public Function existeCorreo(ByVal correo As String) As Integer

        Dim b As New Conection
        Dim idUsuario As Integer
        Try
            b.SQLConnection = New MySqlConnection()
            b.SQLConnection.ConnectionString = b.connectionString
            Dim sqlCommand As New MySqlCommand
            Dim str_carSql As String

            str_carSql = "SELECT idUsuario FROM serverlove.usuario WhERE upper(correo)='" + correo.ToUpper + "'; "

            sqlCommand.Connection = b.SQLConnection
            sqlCommand.CommandText = str_carSql
            b.SQLConnection.Open()

            Dim data As MySqlDataReader

            data = sqlCommand.ExecuteReader

            While data.Read()
                idUsuario = data(0)
            End While

            b.SQLConnection.Close()

        Catch ex As Exception

        End Try


        Return idUsuario

    End Function









End Class