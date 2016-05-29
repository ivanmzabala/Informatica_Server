Imports System.IO
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports MySql.Data.MySqlClient


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class ws_servidor_recibirEntregaDulce
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function confirmaEntregaDulce(ByVal r As OERecibirEntregaDulce) As String


        If r.estado = "1" Then

            Dim result As String = "no se envia el mensaje al app"

            Return result

        End If


        Dim arreglo(8) As String



        Dim b As New Conection

        Try

            b.SQLConnection = New MySqlConnection()
            b.SQLConnection.ConnectionString = b.connectionString
            Dim sqlCommand As New MySqlCommand
            Dim str_carSql As String

            str_carSql = "SELECT u.correoAmor, u.telefonoAmor , u.facebookAmor , r.idDevice  FROM serverlove.usuario as u, serverlove.reconocimiento as r where u.idUsuario = '" + r.idTtransaccion.ToString().Trim() + "' and r.idUsuario  = u.idUsuario order by r.fechaValidacion desc  limit 1  ;"

            sqlCommand.Connection = b.SQLConnection
            sqlCommand.CommandText = str_carSql
            b.SQLConnection.Open()

            Dim data As MySqlDataReader

            data = sqlCommand.ExecuteReader

            While data.Read()

                arreglo(0) = data(0).ToString
                arreglo(1) = data(1).ToString
                arreglo(2) = data(2).ToString
                arreglo(3) = data(3).ToString

            End While

            b.SQLConnection.Close()
        Catch ex As Exception

        End Try




        Dim toret As String = ""

        Dim value As String

        value = "{'email':'" + arreglo(0) + "','phone':'" + arreglo(1) + "','fbusername':'" + arreglo(2) + "','msg':'Ve a pescar pispirispis'}"


        Dim regid As String = "[""" + arreglo(3) + """]"
        Dim applicationID = "AIzaSyBTw6dsE3YkhvERyMULDr5W-ohQe-4sBkA"
        Dim SENDER_ID = arreglo(3)


        Dim tRequest As WebRequest
        tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send")
        tRequest.Method = "post"
        tRequest.ContentType = " application/json"
        tRequest.Headers.Add(String.Format("Authorization: key={0}", applicationID))

        tRequest.Headers.Add(String.Format("Sender: id={0}", SENDER_ID))

        Dim postData As String = "{""collapse_key"":""score_update"",""time_to_live"":108,""data"":{""message"":""" & Convert.ToString(value) & """,""time"":""" & System.DateTime.Now.ToString() & """},""registration_ids"":" & regid & "}"
        Console.WriteLine(postData)
        Dim byteArray As [Byte]() = Encoding.UTF8.GetBytes(postData)
        tRequest.ContentLength = byteArray.Length

        Dim dataStream As Stream = tRequest.GetRequestStream()
        dataStream.Write(byteArray, 0, byteArray.Length)
        dataStream.Close()

        Dim tResponse As WebResponse = tRequest.GetResponse()

        dataStream = tResponse.GetResponseStream()

        Dim tReader As New StreamReader(dataStream)

        Dim sResponseFromServer As [String] = tReader.ReadToEnd()

        toret = sResponseFromServer
        tReader.Close()
        dataStream.Close()
        tResponse.Close()

        Return toret



    End Function

End Class