﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IatDteBridge
{
    class CargaDb
    {
        string unidadDisco = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString();
        public void procesoCarga()
        {
            // Ir al directorio del envío Unitario
            // obtener siguiente .xml
            fileAdmin file = new fileAdmin();
            String xmlName = file.nextFile(@"" +unidadDisco + ":/IatFiles/file/xml/enviounitario", "*.xml");

            // deducir nombre de los PDF y del json
            String pdf = xmlName.Substring(20, xmlName.Length - 20); //no incluir la extención se debe incluir el path
            String pdft = pdf + ".pdf";
            String pdfc = pdf + "CEDIBLE.pdf";

            String json = xmlName.Substring(20, xmlName.Length - 20);

            // llamar txrReader con nombre del archivo para obtener objeto tipo Documento
            TxtReader lec = new TxtReader();
            Documento docLectura = new Documento();

            docLectura = lec.lectura(json, false,"");

            // llamar a sendInvoice conEnvio = N

            Connect conn = new Connect();
            conn.sendInvoice(docLectura, pdft, pdfc, xmlName,"","", "N");

            // mover archivo a carpeta de enviados
            file.mvFile(xmlName,@"" + unidadDisco + ":/IatFiles/file/xml/enviounitario",@"" + unidadDisco + ":/IatFiles/file/xml/envioEnviado");
 
        }
       
    }
}
