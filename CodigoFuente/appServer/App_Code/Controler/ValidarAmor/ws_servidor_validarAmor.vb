Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports MySql.Data.MySqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class ws_servidor_validarAmor
    Inherits System.Web.Services.WebService
    Dim os As New OSValidarAmor
    Dim b As New Conection


    <WebMethod()> Public Function validarAmor(ByVal u As OEValidarAmor) As OSValidarAmor

        Dim correo As String = ""
        Dim idUsuario As Integer
        Dim amorValido As Integer

        Try
            b.SQLConnection = New MySqlConnection()
            b.SQLConnection.ConnectionString = b.connectionString
            Dim sqlCommand As New MySqlCommand
            Dim str_carSql As String

            str_carSql = "SELECT idUsuario FROM serverlove.usuario WhERE correo='" + u.correo + "'; "

            sqlCommand.Connection = b.SQLConnection
            sqlCommand.CommandText = str_carSql
            b.SQLConnection.Open()

            Dim data As MySqlDataReader

            data = sqlCommand.ExecuteReader

            While data.Read()
                idUsuario = data(0)
            End While

            b.SQLConnection.Close()


            amorValido = validarImagen(idUsuario, u.imagenAmor)
            If amorValido <> 1 Then

                os.codigoRespuesta = 0
                os.mensajeRespuesta = "Amor no valido"
            Else

                os.codigoRespuesta = 1
                os.mensajeRespuesta = "Amor valido"
            End If


        Catch ex As Exception

            os.codigoRespuesta = 0
            os.mensajeRespuesta = ex.ToString

        End Try




        Return os

    End Function


    Public Function validarImagen(ByVal idUsuario As Integer, ByVal img As String) As Integer
        Dim valido As Integer
        valido = 1

        Dim enviarDulce As Integer

        If valido <> 1 Then
            enviarDulce = 0

        Else
            enviarDulce = 1

        End If


        Try

            b.SQLConnection = New MySqlConnection()
            b.SQLConnection.ConnectionString = b.connectionString
            Dim sqlCommand As New MySqlCommand
            Dim str_carSql As String

            str_carSql = "insert into reconocimiento (idUsuario, amorValido,enviarDulce,enviarMsjs) Values ('" + idUsuario.ToString + "','" + valido.ToString + "','" + enviarDulce.ToString + "','" + 0.ToString + "')"

            sqlCommand.Connection = b.SQLConnection
            sqlCommand.CommandText = str_carSql
            b.SQLConnection.Open()

            sqlCommand.ExecuteNonQuery()

            b.SQLConnection.Close()



        Catch ex As Exception

            ex.ToString()

        End Try



        Return valido


    End Function



End Class