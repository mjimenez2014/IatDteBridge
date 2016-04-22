using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IatDteBridge
{
    class PdfMasivo
    {

        public void pdfMasivo()
        {
            Documento docLectura = new Documento();
            // inatancia txt_reader
            TxtReader lec = new TxtReader();
            // Ejecuta metodo de txt_reader que llena y obtienen Clase Documento

            docLectura = lec.lectura("",false,"");



            while (docLectura != null)
            {
                String fileNameXML = @"DTE_" + docLectura.RUTEmisor + "_" + docLectura.TipoDTE + "_" + docLectura.Folio + "_";

                System.Console.WriteLine("Nombre de Archivo leido " + fileNameXML);

                fileAdmin f = new fileAdmin();

                String fileXml = f.fileAprox(fileNameXML, @""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/file/xml/", "*.xml");

                if (fileXml !=null )
                {
                    ProcesoPaqueteXml procesa = new ProcesoPaqueteXml();
                    procesa.procesoPaqueteXml(@""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/file/" + docLectura.fileName,  @""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/file/xml/"+ fileXml);
                }

                f.mvFile(docLectura.fileName, ""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/file/", ""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+":/IatFiles/fileProcess/");

                docLectura = lec.lectura("", false,"");
            }


            
        }

    }
}
