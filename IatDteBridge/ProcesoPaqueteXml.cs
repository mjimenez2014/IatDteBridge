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
    class ProcesoPaqueteXml
    {
        Empresa empresa = new Empresa();

        public void procesoPaqueteXml(String fileJson, String fileXml)
        {
            empresa = empresa.getEmpresa();
            // inatancia txt_reader
            TxtReader lec = new TxtReader();

            Documento docLectura = new Documento();

            // Ejecuta metodo de txt_reader que llena y obtienen Clase Documento
            docLectura = lec.lectura(fileJson, true, " ");
            // instancia XML_admin
            xmlPaquete xml = new xmlPaquete();

            DateTime thisDay = DateTime.Now;
            String fchName = String.Format("{0:yyyyMMddTHHmmss}", thisDay);
            
   
            String firsRut = String.Empty;
            if (docLectura != null)
            {

                GetTed ted = new GetTed();

                String TimbreElec = ted.getTed(fileXml);

                //si es Thermal
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
                            if (docLectura.TipoDTE == 33 || docLectura.TipoDTE == 34)
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
                        pd.Print();
                    }
                }
                //si no es thermal else

                //Generar PDF                   
                Pdf docpdf = new Pdf();

                String fileNamePDF = @""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/file/pdf/DTE_" + docLectura.RUTEmisor + "_" + docLectura.TipoDTE + "_" + docLectura.Folio + "_" + fchName + ".pdf";
                docpdf.OpenPdf(TimbreElec, docLectura, fileNamePDF, " ");

                docpdf.OpenPdf(TimbreElec, docLectura, fileNamePDF, " ");


                String fileNamePDFCed = @""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/file/pdf/DTE_" + docLectura.RUTEmisor + "_" + docLectura.TipoDTE + "_" + docLectura.Folio + "_" + fchName + "CEDIBLE.pdf";
  
                if (docLectura.TipoDTE == 33 || docLectura.TipoDTE == 34)
                {
                    docpdf.OpenPdf(TimbreElec, docLectura, fileNamePDFCed, "CEDIBLE");
                }

                if (docLectura.TipoDTE == 52)
                {
                    docpdf.OpenPdf(TimbreElec, docLectura, fileNamePDFCed, "CEDIBLE CON SU FACTURA");
                }

                //fin si no es thermal else
 
            }


        }

        public void firmaxml(String fileXml)
        {

           

        }
    }
}
