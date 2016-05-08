using System.Drawing.Printing;
using System.Windows.Forms;

namespace IatDteBridge
{
    class PdfDocument
    {
        public Documento doc { set; get; }
        public string dd { set; get; }
        public int copias { set; get; }
        public string tipoCopia { set; get; }
        public PrintDocument printDocument = new PrintDocument();
        public PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
        public void openPdfdocument(object sender, PrintPageEventArgs ev)
        {
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();

        }


    }
}
