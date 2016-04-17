Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient

Public Class ValidacionToken


    Public Function validar(ByVal Token As String) As Boolean


        Dim validado As Boolean
        Dim b As New Conection


        validado = False



        Try

            b.SQLConnection = New MySqlConnection()
            b.SQLConnection.ConnectionString = b.connectionString
            Dim sqlCommand As New MySqlCommand
            Dim str_carSql As String

            str_carSql = "SELECT tokenSeguridad FROM serverlove.seguridad where tokenSeguridad ='" + Token + "' ;"

            sqlCommand.Connection = b.SQLConnection
            sqlCommand.CommandText = str_carSql
            b.SQLConnection.Open()

            Dim data As MySqlDataReader

            data = sqlCommand.ExecuteReader

            While data.Read()

                Token = data(0).ToString
                validado = True

            End While

            b.SQLConnection.Close()
        Catch ex As Exception

        End Try

        Return validado

    End Function



End Class
