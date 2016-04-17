Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports MySql.Data.MySqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class ws_servidor_autenticarUsuario
    Inherits System.Web.Services.WebService


    <WebMethod()> Public Function autenticarUsuario(ByVal u As OEAutenticar) As OSAutenticar

        Dim os As New OSAutenticar
        Dim b As New Conection


        'u.token inicio validacion token -----------
        Dim validacionToken As Boolean
        Dim tokenv As New ValidacionToken
        validacionToken = tokenv.validar(u.token)
        If (validacionToken) Then

        Else

            os.codigoRespuesta = 3
            os.mensajeRespuesta = "Token incorrecto"

            Return os
        End If
        'Fin validacion token ----------------------



        Dim correo As String = ""
        Dim clave As String = ""


        Try
            Try

                b.SQLConnection = New MySqlConnection()
                b.SQLConnection.ConnectionString = b.connectionString
                Dim sqlCommand As New MySqlCommand
                Dim str_carSql As String

                str_carSql = "SELECT correo FROM serverlove.usuario WhERE correo='" + u.correo + "'; "

                sqlCommand.Connection = b.SQLConnection
                sqlCommand.CommandText = str_carSql
                b.SQLConnection.Open()

                Dim data As MySqlDataReader

                data = sqlCommand.ExecuteReader

                While data.Read()
                    correo = data(0).ToString
                End While

                b.SQLConnection.Close()
            Catch ex As Exception

            End Try
            Try
                b.SQLConnection = New MySqlConnection()
                b.SQLConnection.ConnectionString = b.connectionString
                Dim sqlCommand As New MySqlCommand
                Dim str_carSql As String

                str_carSql = "SELECT clave FROM serverlove.usuario WhERE clave='" + u.clave + "'; "

                sqlCommand.Connection = b.SQLConnection
                sqlCommand.CommandText = str_carSql
                b.SQLConnection.Open()

                Dim data As MySqlDataReader

                data = sqlCommand.ExecuteReader

                While data.Read()
                    clave = data(0).ToString
                End While

                b.SQLConnection.Close()
            Catch ex As Exception

            End Try

            If correo = u.correo And clave = u.clave Then
                os.codigoRespuesta = 1
                os.mensajeRespuesta = "Operación correcta Usuario y clave existen"
            Else
                If correo <> u.correo Then
                    os.codigoRespuesta = 2
                    os.mensajeRespuesta = "Usuario no existe"
                ElseIf clave <> u.clave Then
                    os.codigoRespuesta = 0
                    os.mensajeRespuesta = "Clave incorrecta"

                End If


            End If

        Catch ex As Exception

            os.codigoRespuesta = 0
            os.mensajeRespuesta = ex.ToString

        End Try

        Return os

    End Function

End Class