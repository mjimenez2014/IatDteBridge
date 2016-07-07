using System;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace IatDteBridge
{
    public partial class FormMain : Form
    {

        PingProcess ping = new PingProcess();
        ProcesoIat proc = new ProcesoIat();

        ProcesoPaqueteXml procFromXml = new ProcesoPaqueteXml();
        ProcesoContingencia procContig = new ProcesoContingencia();
        public CheckBox checkbox1 = new CheckBox();
        LocalDataBase ldb = new LocalDataBase();

        public FormMain()
        {
            InitializeComponent();            
        }

        private string verificarProceso()
        {
            string estado = string.Empty;

            System.Diagnostics.Process[] procesos = System.Diagnostics.Process.GetProcesses();
            // recorrer los procesos existentes
            foreach (System.Diagnostics.Process proceso in procesos)
            {
                // verificar si existe el que buscamos
                if (proceso.ProcessName == "IatDteBridge")
                {
                    estado = "True";
                    MessageBox.Show(estado);
                }

              
            }
            return estado;  
        } 

        private void FormMain_Load(object sender, EventArgs e)
        {
            if ((Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat",null) == null))Registry.SetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", "C");
            if ((Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "pluPdf", null) == null)) Registry.SetValue(@"HKEY_CURRENT_USER\Iat", "pluPdf", "True");
            if ((Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "cantDecimales", null) == null)) Registry.SetValue(@"HKEY_CURRENT_USER\Iat", "cantDecimales", "4");
            if ((Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "guiaPdf", null) == null)) Registry.SetValue(@"HKEY_CURRENT_USER\Iat", "guiaPdf", "False");
            if ((Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "DescuentoPct", null) == null)) Registry.SetValue(@"HKEY_CURRENT_USER\Iat", "DescuentoPct", "False");

            //Creadirectorios
            Directory.CreateDirectory(@""+ Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+"://IatFiles");
            Directory.CreateDirectory(@""+ Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+"://IatFiles/cafs");
            Directory.CreateDirectory(@""+ Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+"://IatFiles/config");
            Directory.CreateDirectory(@""+ Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+"://IatFiles/file");
            Directory.CreateDirectory(@""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+"://IatFiles/file/libroCompra");
            Directory.CreateDirectory(@""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+"://IatFiles/file/libroVenta");
            Directory.CreateDirectory(@""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+"://IatFiles/file/libroGuia");
            Directory.CreateDirectory(@""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+"://IatFiles/file/pdf");
            Directory.CreateDirectory(@""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+"://IatFiles/file/xml");
            Directory.CreateDirectory(@""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+"://IatFiles/file/xml/enviado");
            Directory.CreateDirectory(@""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+"://IatFiles/file/xml/enviomasivo");
            Directory.CreateDirectory(@""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+"://IatFiles/file/xml/enviounitario");
            Directory.CreateDirectory(@""+Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Iat", "unidadIat", null).ToString()+"://IatFiles/fileprocess");

            // crea base de datos
            ldb.creaDB();
            // Inicia proceso IAt
            proc.StartProcessIat();
            this.label4.Text = "IatProcess En Ejecución";
            this.timer1.Start();
            //Inicia Proceso contingencia
            procContig.StartProcessConting();
            //Oculta FormMain, funciona con propiedad WindowState minimizado.          
            ocultaForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connect conn = new Connect();
            
            string response = conn.ping();

           Console.WriteLine("respuesta = {0}.", response);

        }


        private void button6_Click(object sender, EventArgs e)
        {
            ping.StartPing();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ping.StopPing();
        }

        private void datosEmpresaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Administracion admin = new Administracion();
            admin.Show();
        }

        private void empresaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEmisor emisor = new FormEmisor();
            emisor.Show();
        }

        private void adminCAFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAdminCaf adminCaf = new FormAdminCaf();
            adminCaf.Show();
        }

        private void ocultaForm()
        {
            this.Hide();
            notifyIcon1.Text = "IAT Dte Bridge";
            notifyIcon1.BalloonTipTitle = "IAT Dte Bridge";
            notifyIcon1.BalloonTipText = "Puente para el S.I.I";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;

            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(3000);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            proc.StartProcessIat();
            this.label4.Text = "IatProcess En Ejecución";
            this.timer1.Start();

            procContig.StartProcessConting();
            
        }


        private void button9_Click(object sender, EventArgs e)
        {
            procFromXml.procesoPaqueteXml(textBox1.Text,textBox2.Text);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                System.IO.FileInfo fi = null;
                try
                {
                    fi = new System.IO.FileInfo(openFileDialog1.FileName);

                    textBox1.Text = openFileDialog1.FileName;
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {

                System.IO.FileInfo fi = null;
                try
                {
                    fi = new System.IO.FileInfo(openFileDialog2.FileName);

                    textBox2.Text = openFileDialog2.FileName;
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }


        private void button12_Click_1(object sender, EventArgs e)
        {
            PdfMasivo pdfM = new PdfMasivo();

            pdfM.pdfMasivo();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Log l = new Log();
 
            if (ldb.creaDB())
            {
                l.addLog("Creacion de DB", "OK");
                MessageBox.Show("Base de Datos Se ha creado con Exito");
            }
            else
            {
                MessageBox.Show("Base de datos ya Existe");
            }

            l.verLog();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            notifyIcon1.Dispose();
            Environment.Exit(0);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            
            EnvioMasivo enmas = new EnvioMasivo();
            enmas.envioMasivo();
            
        }


 

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value == 100)
            {
                progressBar1.Value = 1;
                
            }else{
            this.progressBar1.Increment(1);
            }
        }


        private void button16_Click(object sender, EventArgs e)
        {
            EnvioMasivo envM = new EnvioMasivo();
            envM.envioMasivo();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Log l = new Log();
            l.verLog();

        }

        private void button18_Click(object sender, EventArgs e)
        {
            ReenvioSql reenv = new ReenvioSql();
            reenv.verReenv();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            ProcesoContingencia pc = new ProcesoContingencia();
            pc.procesoContingencia();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            LocalDataBase l = new LocalDataBase();
            l.addCollumnToReenvio();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Empresa empresa = new Empresa();
            empresa.creaTabla();
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            proc.StopProcessIat();
            procContig.StopProcessConting();
            this.label4.Text = "IatProcess Detenido";
            this.timer1.Stop();

        }


        private void button_Ocultar_Click(object sender, EventArgs e)
        {
            ocultaForm();
        }



        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
