using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Drawing.Printing;

namespace IatDteBridge
{
    class ProcesoPaquete
    {
        public void procesoPaquete()
        {

            string paquete = string.Empty;
                
                // instancia fileadmin, para tener las herramientas para mover archivos
                fileAdmin fileAdm = new fileAdmin();

                // inatancia txt_reader
                TxtReader lec = new TxtReader();

                Documento docLectura = new Documento();

                // Ejecuta metodo de txt_reader que llena y obtienen Clase Documento
                docLectura = lec.lectura("", true,"");
                // instancia XML_admin
                xmlPaquete xml = new xmlPaquete();

                List<int> tipos = new List<int>();
                // eliminar despues de la simulacion

                Empresa empresa = new Empresa();
                empresa = empresa.getEmpresa();
                
                DateTime thisDay = DateTime.Now;

                String fch = String.Format("{0:yyyy-MM-ddTHH:mm:ss}", thisDay);
                String fchName = String.Format("{0:yyyyMMddTHHmmss}", thisDay);

                int folio33 = 735;
                int folio34 = 1000; //Factura Exenta
                int folio52 = 10; //Guia Despacho
                int folio61 = 15;
                int folio56 = 15;
                int folio = 0;
                int folio46 = 0;
                
                int i = 0;
                
              String firsRut = String.Empty;
              String rutenvia = String.Empty;
              String fchresol = String.Empty;
              String nomcertificado = String.Empty;
              String numResol= String.Empty;



                while (docLectura != null)
                {

                    switch (docLectura.TipoDTE)
                    {
                        case 33: { folio33++; folio = folio33; }
                            break;
                        case 34: { folio34++; folio = folio34; }
                            break;
                        case 52: { folio52++; folio = folio52; }
                            break;
                        case 61: { folio61++; folio = folio61; }
                            break;
                        case 56: { folio56++; folio = folio56; }
                            break;
                        case 46: { folio46++; folio = folio46; }
                            break;
                    }



                    tipos.Add(docLectura.TipoDTE);

                   // docLectura.Folio = folio;

                    String TimbreElec = xml.ted_to_xmlSii(docLectura,fch);
                    String docXmlSign = xml.doc_to_xmlSii(docLectura,TimbreElec,fch);

 


                    // Guarda DTE xml
                    String DTE = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\r\n" + docXmlSign;
                    String fileNameXML = @""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/file/xml/DTE_"+docLectura.RUTEmisor+"_"+ docLectura.TipoDTE+"_"+ docLectura.Folio  +"_"+ fchName + ".xml";
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileNameXML, false, Encoding.GetEncoding("ISO-8859-1")))
                    {
                        file.WriteLine(docXmlSign);
                    }


                    //Generar PDF                   
                    Pdf docpdf = new Pdf();

                    String fileNamePDF = @""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/file/pdf/DTE_" + docLectura.RUTEmisor + "_" + docLectura.TipoDTE + "_" + docLectura.Folio + "_" + fchName + "TRIBUTABLE.pdf";
                    docpdf.OpenPdf(TimbreElec, docLectura,fileNamePDF, " ");

                    docpdf.OpenPdf(TimbreElec, docLectura, fileNamePDF, " ");

                    String fileNamePDFCed = @""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/file/pdf/DTE_" + docLectura.RUTEmisor + "_" + docLectura.TipoDTE + "_" + docLectura.Folio + "_" + fchName + "CEDIBLE.pdf";


                    if (docLectura.TipoDTE == 33 || docLectura.TipoDTE == 34 || docLectura.TipoDTE == 46)
                    {
                        docpdf.OpenPdf(TimbreElec, docLectura, fileNamePDFCed, "CEDIBLE");
                    }

                    if (docLectura.TipoDTE == 52 )
                    {
                        docpdf.OpenPdf(TimbreElec, docLectura, fileNamePDFCed, "CEDIBLE CON SU FACTURA");
                    }
                    
                    //Genere Print Thermal

                    if (empresa.PrnThermal == "True")
                    {

                        for (int copies = 0; copies < 3; copies++)
                        {
                            Thermal thermal = new Thermal();
                            thermal.doc = docLectura;
                            thermal.dd = TimbreElec;
                            thermal.copias = copies;
                            if (docLectura.PrnTwoCopy == "True")
                                copies = 1;
                            docLectura.PrnTwoCopy = "False";
                            if (copies == 2)
                            {
                                if (docLectura.TipoDTE == 33 || docLectura.TipoDTE == 34 || docLectura.TipoDTE == 46)
                                {
                                    thermal.tipoCopia = "CEDIBLE";
                                }
                                if (docLectura.TipoDTE == 52)
                                {
                                    thermal.tipoCopia = "CEDIBLE CON SU FACTURA";
                                }

                                if (docLectura.TipoDTE == 61)
                                {
                                    break;
                                }
                            }
                            //  
                            PrintDocument pd = new PrintDocument();
                            pd.DefaultPageSettings.PaperSize = new PaperSize("", 284, 1800);
                            pd.PrintPage += new PrintPageEventHandler(thermal.OpenThermal);
                            pd.PrinterSettings.PrinterName = "prnPdf";
                            Console.WriteLine(pd.ToString());
                            if (copies == 2)
                            {
                                pd.DocumentName = docLectura.RUTEmisor + "_" + docLectura.TipoDTE + "_" + docLectura.Folio + "_" + fchName + "_CEDIBLE";
                            }
                            else
                            {
                                pd.DocumentName = docLectura.RUTEmisor + "_" + docLectura.TipoDTE + "_" + docLectura.Folio + "_" + fchName+ "_TRIBUTABLE";
                            }
                            pd.Print();
                        }
                    }

                    // Agrega el DTE timbrado al paquete
                    paquete = paquete + docXmlSign;

                    //Estrae el rut del emisor de la primera factura del paquete
                    if (i == 0) 
                    {
                        firsRut = docLectura.RUTEmisor;
                        rutenvia = docLectura.RutEnvia;
                        fchresol = docLectura.FchResol;
                        numResol = docLectura.NumResol;
                        nomcertificado = docLectura.NombreCertificado;
                    }
                    i++;

                    //Sgte Documento
                    docLectura = lec.lectura("", true,"");

                    // Verifica que el siguiente documento sea del mismo emisor
                  /*  if (docLectura != null)
                    {
                        while (docLectura.RUTEmisor != firsRut)
                        {
                            // si no tiene el mismo rut
                            // lo saca del directorio
                            fileAdm.mvFile(docLectura.fileName, @""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":\IatFiles\file\", @""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":\IatFiles\file\noincluidos\");

                            //Sgte Documento
                            docLectura = lec.lectura();
                        }
                    }
                    */
                }
                


                // Firma POaquete    
                String envio = xml.creaEnvio(paquete,firsRut,"", tipos,rutenvia,fchresol,"",numResol);
                

                X509Certificate2 cert = FuncionesComunes.obtenerCertificado(nomcertificado);
                String enviox509 = xml.firmarDocumento(envio, cert);

       
                enviox509 = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\r\n" + enviox509;

                String fileNameEnvio = @""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/file/xml/EnvioPAck_" + firsRut +"_"+ fchName +".xml";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileNameEnvio, false, Encoding.GetEncoding("ISO-8859-1")))
                {
                    file.WriteLine(enviox509);
                }
                
                Console.WriteLine(enviox509);
                

            }

     }

    
}
