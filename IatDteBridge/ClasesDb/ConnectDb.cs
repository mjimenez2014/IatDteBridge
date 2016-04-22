using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace IatDteBridge
{
    class ConnectDb
    {
        String strConn = @"Data Source=" + Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString() + ":/IatFiles/iatDB.sqlite;Pooling=true;FailIfMissing=false;Version=3";

        public String GetUrl()
        {
            String url = "";         
            try
            {
                SQLiteConnection myConn = new SQLiteConnection(strConn);
                myConn.Open();
                string sql = "SELECT * FROM EMPRESA";
                SQLiteCommand command = new SQLiteCommand(sql, myConn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    url = reader["UrlCore"].ToString();
                }

                myConn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.ToString());
                
            }

            return url;
        }


        public String GetOC()
        {
            String oc = "";
            try
            {
                SQLiteConnection myConn = new SQLiteConnection(strConn);
                myConn.Open();
                string sql = "SELECT * FROM EMPRESA";
                SQLiteCommand command = new SQLiteCommand(sql, myConn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    oc = reader["prnOC"].ToString();
                }

                myConn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.ToString());

            }

            return oc;
        }


        public String getDirLocal()
        {
            String ciudad = "";
            try
            {
                SQLiteConnection myConn = new SQLiteConnection(strConn);
                myConn.Open();
                string sql = "SELECT * FROM EMPRESA";
                SQLiteCommand command = new SQLiteCommand(sql, myConn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ciudad = reader["DirLocal"].ToString();
                }

                myConn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.ToString());

            }

            return ciudad;
        }


        public SQLiteConnection ConnectSqlite()
        {

            SQLiteConnection myConn = new SQLiteConnection(strConn);
            try
            {
                myConn.Open();
            }
            catch (SQLiteException exSqlite)
            {
                Console.WriteLine("Problemas al conectar a la base datos", "Error de Sqlite");
                Console.WriteLine(exSqlite.Message + "\n\n" + "*********************StackTrace: \n\n" + exSqlite.StackTrace);
                //Environment.Exit(0);
                return null;
            }
            return myConn;
        }

        
    }
}
