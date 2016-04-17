Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient

Public Class ValidacionUsuario


    Public Function validar(ByVal u As String, ByVal c As String) As Boolean

        Dim validado As Boolean
        Dim b As New Conection

        Dim usuario As String
        Dim contra As String

        validado = False

        Try

            b.SQLConnection = New MySqlConnection()
            b.SQLConnection.ConnectionString = b.connectionString
            Dim sqlCommand As New MySqlCommand
            Dim str_carSql As String

            str_carSql = "SELECT seguridadApp, claveSeguridad FROM serverlove.seguridad where seguridadApp = '" + u + "' and claveSeguridad= '" + c + "' ;"

            sqlCommand.Connection = b.SQLConnection
            sqlCommand.CommandText = str_carSql
            b.SQLConnection.Open()

            Dim data As MySqlDataReader

            data = sqlCommand.ExecuteReader

            While data.Read()
                usuario = data(0).ToString
                contra = data(1).ToString
                validado = True
            End While

            b.SQLConnection.Close()
        Catch ex As Exception

        End Try

        Return validado

    End Function



End Class
