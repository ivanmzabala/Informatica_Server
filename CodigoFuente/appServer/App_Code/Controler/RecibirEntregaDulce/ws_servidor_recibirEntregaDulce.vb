Imports System.IO
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class ws_servidor_recibirEntregaDulce
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function confirmaEntregaDulce(ByVal a As String) As String

        Dim toret As String = ""

        Dim value As String

        value = "{'email':'jeisontriananr14@hotmail.com','phone':'3114668622','msg':'vacio'}"

        Dim regid As String = "[""csrcubX37UY:APA91bFNEHEOVV6fyGHjdzBQwsDOMdebS6eEE60utKBDVPZEyNlwW1sWqCm9wocdGQQbgXs-ur7aFSwj6YRLyjG2P2q7ZoNT_kJZk-qupvycK6fEno-6JaiDmfuVoktZMkqadzAkbLgZ""]"
        Dim applicationID = "AIzaSyBTw6dsE3YkhvERyMULDr5W-ohQe-4sBkA"
        Dim SENDER_ID = "csrcubX37UY:APA91bFNEHEOVV6fyGHjdzBQwsDOMdebS6eEE60utKBDVPZEyNlwW1sWqCm9wocdGQQbgXs-ur7aFSwj6YRLyjG2P2q7ZoNT_kJZk-qupvycK6fEno-6JaiDmfuVoktZMkqadzAkbLgZ"



        Dim tRequest As WebRequest
        tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send")
        tRequest.Method = "post"
        tRequest.ContentType = " application/json"
        tRequest.Headers.Add(String.Format("Authorization: key={0}", ApplicationId))

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