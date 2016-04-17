Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports MySql.Data.MySqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class ws_servidor_crearLoginToken
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function GetTokenForApp(ByVal a As OEToken) As OSToken

        Dim numeroCaracteres As String
        Dim os As New OSToken

        Dim validacion As New ValidacionUsuario
        Dim val As Boolean

        val = validacion.validar(a.Usuario, a.Contraseña)

        If (val) Then

            numeroCaracteres = 30

        Else

            os.token = "0"
            os.mensajeRespuesta = "Token no generado"
            os.codigoRespuesta = "0"

            Return os

        End If

        ' Dimensionamos un array para almacenar tanto las 
        ' letras mayúsculas como minúsculas (52 letras). 


        Dim letras(51) As String

        ' Rellenamos el array. 
        ' 
        Dim n As Integer
        For item As Int32 = 65 To 90
            letras(n) = Chr(item)
            letras(n + 1) = letras(n).ToLower
            n += 2
        Next

        Dim cadenaAleatoria As String = String.Empty

        ' Iniciamos el generador de números aleatorios 
        ' 
        Dim rnd As New Random(DateTime.Now.Millisecond)

        For n = 0 To numeroCaracteres

            Dim numero As Integer = rnd.Next(0, 51)

            cadenaAleatoria &= letras(numero)

        Next

        'Return cadenaAleatoria

        Dim b As New Conection

        Try
            b.SQLConnection = New MySqlConnection()
            b.SQLConnection.ConnectionString = b.connectionString
            Dim sqlCommand As New MySqlCommand
            Dim str_carSql As String

            str_carSql = "INSERT INTO serverlove.seguridad(`seguridadApp`,`claveSeguridad`,`tokenSeguridad`)VALUES('" + a.Usuario + "','" + a.Contraseña + "','" + cadenaAleatoria + "');"

            sqlCommand.Connection = b.SQLConnection
            sqlCommand.CommandText = str_carSql
            b.SQLConnection.Open()

            sqlCommand.ExecuteNonQuery()

            b.SQLConnection.Close()

            os.codigoRespuesta = 1
            os.mensajeRespuesta = "El usuario ha sido registrado correctamente"

        Catch ex As Exception

        End Try

        os.token = cadenaAleatoria
        os.mensajeRespuesta = "Token generado"
        os.codigoRespuesta = "1"

        Return os

    End Function


End Class