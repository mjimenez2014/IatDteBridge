﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Globalization;

namespace IatDteBridge
{
    class TxtReader
    {
        String strConn = @"Data Source="+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/iatDB.sqlite;Pooling=true;FailIfMissing=false;Version=3";
       
        public Documento lectura(string fileJson, bool moveFile, string dirOrigen)
        {
            Documento doc = new Documento();
            fileAdmin file = new fileAdmin();
            string fileName = String.Empty;

            if (dirOrigen == "")
            {
                dirOrigen = @""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/file";
            }
            

            if (fileJson == "")
            {
                fileName = file.nextFile(dirOrigen, "*.json");
            }
            else
            {
                fileName = dirOrigen + fileJson;
            }


            if (fileName != null)
            {
 
                StreamReader objReader = new StreamReader(fileName, Encoding.Default,true);
                objReader.ToString();
                //Leo el json hasta el final
                string data = objReader.ReadToEnd();

                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Documento));
                
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data));

                try
                {
                    doc = (Documento)js.ReadObject(ms);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    MessageBox.Show("Error de lectura JSON"+ e.Message);
                }
  
                // Datos del Emisor
                // Cargo datos en laclase Documento desde sqlite
               
                if (doc.RUTEmisor == null)
                {
                    try
                    {

                        SQLiteConnection myConn = new SQLiteConnection(strConn);
                        myConn.Open();

                        string sql = "select * from empresa";
                        SQLiteCommand command = new SQLiteCommand(sql, myConn);
                        SQLiteDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (doc.TipoDTE == 30)
                            {
                                doc.TipoDTE = 33;
                            }
                            doc.RUTEmisor = reader["RutEmisor"].ToString();
                            doc.RznSoc = reader["RznSoc"].ToString();
                            doc.GiroEmis = reader["GiroEmis"].ToString();
                            doc.Telefono = reader["Telefono"].ToString();
                            doc.CorreoEmisor = reader["CorreoEmisor"].ToString();
                            doc.Acteco = Convert.ToInt32(reader["Acteco"]);
                            doc.CdgSIISucur = Convert.ToInt32(reader["CdgSIISucur"]);
                            doc.DirMatriz = reader["DirMatriz"].ToString();
                            doc.CmnaOrigen = reader["CmnaOrigen"].ToString();
                            doc.CiudadOrigen = reader["CiudadOrigen"].ToString();
                            doc.DirOrigen = reader["DirOrigen"].ToString();
                            doc.NombreCertificado = reader["NomCertificado"].ToString();
                            doc.SucurEmisor = reader["SucurEmisor"].ToString();
                            doc.FchResol = reader["FchResol"].ToString();
                            doc.RutEnvia = reader["RutCertificado"].ToString();
                            doc.NumResol = reader["NumResol"].ToString();
                            doc.CondEntrega = reader["CondEntrega"].ToString();
                            doc.PrnMtoNeto = reader["PrnMtoNeto"].ToString();
                            doc.PrnTwoCopy = reader["PrnTwoCopy"].ToString();
                            doc.DirRegionalSII = reader["sucurSII"].ToString();
                            if (doc.TipoDTE == 30 && doc.NumResol == "0")
                            {
                                doc.TipoDTE = 33;
                            }

                        }
                        myConn.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERROR: {0}", e.ToString());
                    }
                }
                else
                {
                    try
                    {

                        SQLiteConnection myConn = new SQLiteConnection(strConn);
                        myConn.Open();

                        string sql = "select * from empresa where empresa.RutEmisor = '"+ doc.RUTEmisor.ToString() +"'";
                        SQLiteCommand command = new SQLiteCommand(sql, myConn);
                        SQLiteDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {

                            doc.Telefono = reader["Telefono"].ToString();
                            doc.CorreoEmisor = reader["CorreoEmisor"].ToString();
                            doc.Acteco = Convert.ToInt32(reader["Acteco"]);
                            doc.DirRegionalSII = reader["sucurSII"].ToString();
                            doc.DirMatriz = reader["DirMatriz"].ToString();
                            doc.NombreCertificado = reader["NomCertificado"].ToString();
                            doc.SucurEmisor = reader["SucurEmisor"].ToString();
                            doc.FchResol = reader["FchResol"].ToString();
                            doc.RutEnvia = reader["RutCertificado"].ToString();
                            doc.NumResol = reader["NumResol"].ToString();
                            doc.CondEntrega = reader["CondEntrega"].ToString();
                            doc.PrnMtoNeto = reader["PrnMtoNeto"].ToString();
                            doc.PrnTwoCopy = reader["PrnTwoCopy"].ToString();
                            if (doc.TipoDTE == 30 && doc.NumResol == "0")
                            {
                                doc.TipoDTE = 33;
                            }


                        }

                        myConn.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERROR: {0}", e.ToString());
                    }
               }

                objReader.Close();
                ms.Close();
                if (moveFile)
                {
                    file.mvFile(fileName, dirOrigen, ""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/fileProcess/");
                }
                
                Caf caf = new Caf();

                if(!caf.isValid(doc))
                {
                    doc = null;
                }

                if (fileJson == "")
                {
                    doc.fileName = fileName;
                }
                else
                {
                    doc.fileName = fileJson;
                }

                foreach(var dsc in doc.dscRcgGlobal)
                {
                    string valor = dsc.ValorDR.ToString("N2", CultureInfo.CreateSpecificCulture("es-ES"));
                    valor = valor.Replace(",", ".");
                    dsc.ValorDR = Convert.ToDecimal(valor);
                }
                return doc;
            }
            else
            {
                return null;
            }
            


        }

    }

}
