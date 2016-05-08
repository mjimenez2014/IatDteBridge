using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace IatDteBridge
{
    class LocalDataBase
    {
        string strConn = @"Data Source="+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/iatDB.sqlite;Pooling=true;FailIfMissing=false;Version=3";


        public bool creaDB()
        {
            try
            {
                if (!System.IO.File.Exists(""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/iatDB.sqlite"))
                {
                    SQLiteConnection.CreateFile(""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/iatDB.sqlite");
                    SQLiteConnection myConn = new SQLiteConnection(strConn);
                    myConn.Open();

                    string sql1 = "CREATE TABLE IF NOT EXISTS log (fch VARCHAR(20), suceso VARCHAR(255), estado VARCHAR(20)) ";
                    string sql2 = "CREATE TABLE IF NOT EXISTS reenvio (fch VARCHAR(20), jsonname VARCHAR(255), envunit VARCHAR(255), pdft VARCHAR(255), pdfc VARCHAR(255), estado VARCHAR(20)) ";
                    string sql3 = "CREATE TABLE IF NOT EXISTS empresa (RutEmisor VARCHAR(10), RznSoc VARCHAR(255), GiroEmis VARCHAR(255),Telefono VARCHAR(255), CorreoEmisor VARCHAR(255), Acteco VARCHAR(50), CdgSIISucur VARCHAR(50), DirMatriz VARCHAR(255), CiudadOrigen VARCHAR(255), CmnaOrigen VARCHAR(255), DirOrigen VARCHAR(255), SucurSII VARCHAR(100), NomCertificado VARCHAR(255), SucurEmisor VARCHAR(255), FchResol VARCHAR(50), RutCertificado VARCHAR(10), NumResol VARCHAR(20), CondEntrega VARCHAR(10),PrnMtoNeto VARCHAR(5),PrnTwoCopy VARCHAR(5),PrnThermal VARCHAR(5),UrlCore VARCHAR(255), PrnOC VARCHAR(5), VistaPrevia VARCHAR(5), DirLocal VARCHAR(5))";

                    //carga bd inicial
                    string sql4 = "insert into empresa " + 
                                        "(RutEmisor, RznSoc, GiroEmis,Telefono,CorreoEmisor,Acteco,CdgSIISucur,DirMatriz,"+
                                        "CiudadOrigen,CmnaOrigen,DirOrigen,SucurSII,NomCertificado,SucurEmisor,FchResol,RutCertificado,NumResol,CondEntrega,PrnMtoNeto,PrnTwoCopy,PrnThermal,UrlCore,PrnOC,VistaPrevia,DirLocal)"+ 
                                        "values ('12891016-6','Razon Social','Giro Emisor','Telefonos casa matriz','Correo Emisor',0,0,'Dirección casa matriz',"+
                                        "'Ciudad Origen','Comuna de origen','Dirección de Origen','Sucursal de SII','Nombre del certificado','Sucursales del emisor',"+
                                        "'Fecha de resolución','Rut del certificado','Numero de resolucion','False','False','False','False','http','False','False','Direccion Local')";

                    string sql5 = "CREATE TABLE IF NOT EXISTS ultimodte (RutEmisor VARCHAR(10), RznSoc VARCHAR(255), CdgSIISucur INTEGER,   RutRecep VARCHAR(10), RznSocRecep VARCHAR(255), Folio INTEGER, TipoDTE INTEGER, fch VARCHAR(20)) ";
                    string sql6 = "CREATE TABLE IF NOT EXISTS printers (printername VARCHAR(255), directory VARCHAR(255)) ";


                    SQLiteCommand cmd = new SQLiteCommand(sql1, myConn);
                    cmd.ExecuteNonQuery();

                    SQLiteCommand cmd2 = new SQLiteCommand(sql2, myConn);
                    cmd2.ExecuteNonQuery();

                    SQLiteCommand cmd3 = new SQLiteCommand(sql3, myConn);
                    cmd3.ExecuteNonQuery();

                    SQLiteCommand cmd4 = new SQLiteCommand(sql4, myConn);
                    cmd4.ExecuteNonQuery();

                    SQLiteCommand cmd5 = new SQLiteCommand(sql5, myConn);
                    cmd5.ExecuteNonQuery();

                    SQLiteCommand cmd6 = new SQLiteCommand(sql6, myConn);
                    cmd5.ExecuteNonQuery();

                    //agrega campos
                    addCollumnToReenvio();

                    myConn.Close();
                    
                    Log l = new Log();
                    l.addLog("Creacion de DB", "OK");
                    MessageBox.Show("Base de Datos Se ha creado con Exito");
                }
                else
                {
                    SQLiteConnection myConn = new SQLiteConnection(strConn);
                    myConn.Open();

                    string sql1 = "CREATE TABLE IF NOT EXISTS log (fch VARCHAR(20), suceso VARCHAR(255), estado VARCHAR(20)) ";
                    string sql2 = "CREATE TABLE IF NOT EXISTS reenvio (fch VARCHAR(20), jsonname VARCHAR(255), envunit VARCHAR(255), pdft VARCHAR(255), pdfc VARCHAR(255), estado VARCHAR(20)) ";
                    string sql3 = "CREATE TABLE IF NOT EXISTS empresa (RutEmisor VARCHAR(10), RznSoc VARCHAR(255), GiroEmis VARCHAR(255), Telefono VARCHAR(255), CorreoEmisor VARCHAR(255), Acteco VARCHAR(50), CdgSIISucur VARCHAR(50), DirMatriz VARCHAR(255), CiudadOrigen VARCHAR(255), CmnaOrigen VARCHAR(255),DirOrigen VARCHAR(255), SucurSII VARCHAR(100), NomCertificado VARCHAR(255), DirOrigen VARCHAR(255), FchResol VARCHAR(50), RutCertificado VARCHAR(10), NumResol VARCHAR(20), CondEntrega VARCHAR(10)) ";
                    string sql4 = "CREATE TABLE IF NOT EXISTS ultimodte (RutEmisor VARCHAR(10), RznSoc VARCHAR(255), CdgSIISucur INTEGER,   RutRecep VARCHAR(10), RznSocRecep VARCHAR(255), Folio INTEGER, TipoDTE INTEGER, fch VARCHAR(20) ) ";
                    string sql5 = "CREATE TABLE IF NOT EXISTS printers (printername VARCHAR(255), directory VARCHAR(255)) ";
                    string sql6 = "CREATE TABLE IF NOT EXISTS folio (rut VARCHAR(10), rsnsocial VARCHAR(255),tipoDte INTEGER,folioIni INTEGER,folioFin INTEGER, fecha VARCHAR(12), rango VARCHAR(255))";



                    SQLiteCommand cmd = new SQLiteCommand(sql1, myConn);
                    cmd.ExecuteNonQuery();

                    SQLiteCommand cmd2 = new SQLiteCommand(sql2, myConn);
                    cmd2.ExecuteNonQuery();

                    SQLiteCommand cmd3 = new SQLiteCommand(sql3, myConn);
                    cmd3.ExecuteNonQuery();

                    SQLiteCommand cmd4 = new SQLiteCommand(sql4, myConn);
                    cmd4.ExecuteNonQuery();

                    SQLiteCommand cmd5 = new SQLiteCommand(sql5, myConn);
                    cmd5.ExecuteNonQuery();

                    SQLiteCommand cmd6 = new SQLiteCommand(sql6, myConn);
                    cmd6.ExecuteNonQuery();

                    //agrega campos
                    addCollumnToReenvio();
                    addColPrnMtoNetoToEmpresa();
                    addColPrnTwoCopyToEmpresa();

                    myConn.Close();

                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.ToString());
                return false;
            }
            return true;
        }

        public bool addCollumnToReenvio()
        {

            try
            {
                SQLiteConnection myConn = new SQLiteConnection(strConn);
                myConn.Open();

                string sql = "PRAGMA table_info(reenvio)";
                SQLiteCommand command = new SQLiteCommand(sql, myConn);
                SQLiteDataReader reader = command.ExecuteReader();

                bool existecampo = false;
                while (reader.Read())
                {
                    if (@"filecliente" == reader["name"].ToString())
                    {
                        existecampo = true;
                    }
                }

                if (!existecampo)
                {
                    string sql1 = "ALTER TABLE reenvio ADD COLUMN filecliente VARCHAR(255)";
                    SQLiteCommand cmd = new SQLiteCommand(sql1, myConn);
                    cmd.ExecuteNonQuery();

                }

                myConn.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.ToString());
                return false;
            }

            return true;
        }

        public bool addColPrnMtoNetoToEmpresa()
        {

            try
            {
                SQLiteConnection myConn = new SQLiteConnection(strConn);
                myConn.Open();

                string sql = "PRAGMA table_info(empresa)";
                SQLiteCommand command = new SQLiteCommand(sql, myConn);
                SQLiteDataReader reader = command.ExecuteReader();

                bool existecampo = false;
                while (reader.Read())
                {
                    if (@"PrnMtoNeto" == reader["name"].ToString())
                    {
                        existecampo = true;
                    }
                }

                if (!existecampo)
                {
                    string sql1 = "ALTER TABLE empresa ADD COLUMN PrnMtoNeto VARCHAR(5) DEFAULT 'True' ";
                    SQLiteCommand cmd = new SQLiteCommand(sql1, myConn);
                    cmd.ExecuteNonQuery();
                    

                }

                myConn.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.ToString());
                return false;
            }

            return true;
        }

        public bool addColPrnTwoCopyToEmpresa()
        {

            try
            {
                SQLiteConnection myConn = new SQLiteConnection(strConn);
                myConn.Open();

                string sql = "PRAGMA table_info(empresa)";
                SQLiteCommand command = new SQLiteCommand(sql, myConn);
                SQLiteDataReader reader = command.ExecuteReader();

                bool existecampo = false;
                while (reader.Read())
                {
                    if (@"PrnTwoCopy" == reader["name"].ToString())
                    {
                        existecampo = true;
                    }
                }

                if (!existecampo)
                {
                    string sql1 = "ALTER TABLE empresa ADD COLUMN PrnTwoCopy VARCHAR(5) DEFAULT 'False' ";
                    SQLiteCommand cmd = new SQLiteCommand(sql1, myConn);
                    cmd.ExecuteNonQuery();

                }

                myConn.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.ToString());
                return false;
            }

            return true;
        }
    }
}
