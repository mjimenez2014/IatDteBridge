using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace IatDteBridge
{
  public  class TipoTrasladoDb
    {
      String strConn = @"Data Source=" + Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString() + ":/IatFiles/iatDB.sqlite;Pooling=true;FailIfMissing=false;Version=3";

      public String getTipoTrasXCod(int idTras)
     {
         String tipotraslado = String.Empty;

                SQLiteConnection myConn = new SQLiteConnection(strConn);
                myConn.Open();

                string sql = "select * from tipotraslado where id = " + idTras;
                SQLiteCommand command = new SQLiteCommand(sql, myConn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tipotraslado = reader.GetString(reader.GetOrdinal("nombre"));
                }
         return tipotraslado;
     }
    }
}
