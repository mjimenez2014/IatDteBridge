using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;


namespace IatDteBridge
{
    class ReimpThermal
    {
        public void reimp(Documento docLectura, String xmlFilename, String impresora)
        {
            GetTed ted = new GetTed();
            String TimbreElec = ted.getTed(xmlFilename);
            for (int copies = 0; copies < 3; copies++)
            {

                Thermal thermal = new Thermal();
                thermal.doc = docLectura;
                thermal.dd = TimbreElec;
                thermal.copias = copies;
                if (docLectura.PrnTwoCopy == "True")
                    copies = 1; 
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
                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.PaperSize = new PaperSize("", 284, 1800);
                pd.PrintPage += new PrintPageEventHandler(thermal.OpenThermal);
                pd.PrinterSettings.PrinterName = impresora;
                Console.WriteLine(pd.ToString());
                    pd.Print();
            }
                                                   

        }
    }
}
