Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Serialization
Imports MySql.Data.MySqlClient


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class ws_servidor_autorizarEntrgaDulce
    Inherits System.Web.Services.WebService



    <WebMethod()>
    Public Function notificarEntregarDulce() As String

        Dim os As New OSEntregaDulce
        Dim b As New Conection
        Dim json As String
        Dim jss As New JavaScriptSerializer

        json = "["

        Dim data As MySqlDataReader

        Try
            b.SQLConnection = New MySqlConnection()
            b.SQLConnection.ConnectionString = b.connectionString
            Dim sqlCommand As New MySqlCommand
            Dim str_carSql As String

            str_carSql = "SELECT  idUsuario, enviarDulce   FROM serverlove.reconocimiento where enviardulce = '1';"

            sqlCommand.Connection = b.SQLConnection
            sqlCommand.CommandText = str_carSql
            b.SQLConnection.Open()


            data = sqlCommand.ExecuteReader
            Dim cuenta As String
            cuenta = 0
            While data.Read()

                If (cuenta >= 1) Then
                    json = json + ","
                End If

                os.codigoRespuesta = data(1)

                os.mensajeRespuesta = "Entregar Dulce"

                os.idUsuario = data(0)

                json = json + jss.Serialize(os)

                cuenta = cuenta + 1
            End While

            b.SQLConnection.Close()


        Catch ex As Exception

        End Try





        json = json + "]"



        Return json


    End Function

End Class