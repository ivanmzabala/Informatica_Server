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
    Public Function confirmaEntregaDulce(ByVal u As OERecibirEntregaDulce) As String

        Dim toret As String = ""

        'Change these values

        Dim applicationID = "AIzaSyBTw6dsE3YkhvERyMULDr5W-ohQe-4sBkA"
        Dim SENDER_ID = "APA91bFXgT6uHojJxxJ6JFlHwDATp7bcMjafO2E3qHt2oOn-TIiSQoG7OxPCRziW3E2nCMyI5hHO8ePVJDgJa8Y0rAp0lFRVdafTI_w_dstPUGBCGJayVkrRVOeCkayQrJ_UbdL7r9SZiJxJpAPkkFOlWsuQz6ZlQg"


        Dim authstring As String

        Dim senderId As String

        senderId = "APA91bFXgT6uHojJxxJ6JFlHwDATp7bcMjafO2E3qHt2oOn-TIiSQoG7OxPCRziW3E2nCMyI5hHO8ePVJDgJa8Y0rAp0lFRVdafTI_w_dstPUGBCGJayVkrRVOeCkayQrJ_UbdL7r9SZiJxJpAPkkFOlWsuQz6ZlQg"
        authstring = "AIzaSyBTw6dsE3YkhvERyMULDr5W-ohQe-4sBkA"


        ServicePointManager.ServerCertificateValidationCallback = Function(sender As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) True
        Dim request As WebRequest = WebRequest.Create("https://android.googleapis.com/gcm/send")
        request.Method = "POST"
        request.ContentType = "application/x-www-form-urlencoded"


        request.Headers.Add(String.Format("Authorization: key={0}", authstring))
        request.Headers.Add(String.Format("Sender: id={0}", senderId))


        Dim collaspeKey As String = Guid.NewGuid().ToString("n")

        Dim postData As String = String.Format("registration_id={0}&data.payload={1}&collapse_key={2}", applicationID, SENDER_ID, collaspeKey)

        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
        request.ContentLength = byteArray.Length
        Dim dataStream As Stream = request.GetRequestStream()
        dataStream.Write(byteArray, 0, byteArray.Length)
        dataStream.Close()
        Dim response As WebResponse = request.GetResponse()
        dataStream = response.GetResponseStream()
        Dim reader As New StreamReader(dataStream)
        Dim responseFromServer As String = reader.ReadToEnd()
        reader.Close()
        dataStream.Close()
        response.Close()

        Return responseFromServer




    End Function

End Class