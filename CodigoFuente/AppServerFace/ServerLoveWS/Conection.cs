using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerLoveWS
{
    public class Conection
    {
        public String connectionString = "Server=127.0.0.1; User Id=root; Password=mysqlroot*; Database=serverlove;";
        public MySqlConnection SQLConnection  = new MySqlConnection();
    }
}