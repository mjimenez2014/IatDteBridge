using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IatDteBridge
{
    public partial class FrmCaf : Form
    {
        Caf caf = new Caf();
        public FrmCaf()
        {
            InitializeComponent();
        }

        private void FrmCaf_Load(object sender, EventArgs e)
        {
            actDataG();
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }
        public void actDataG()
        {
            dataGridView1.Rows.Clear();
            DataTable dt = new Caf().getCafs();

            foreach (DataRow row in dt.Rows)
            {
                Int32 n = this.dataGridView1.Rows.Add();

                dataGridView1.Rows[n].Cells[0].Value = row["id"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = row["rutEmpresa"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = row["tipoDte"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = row["folioInicial"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = row["folioFinal"].ToString();
                dataGridView1.Rows[n].Cells[5].Value = Convert.ToDateTime(row["fecha"].ToString()).ToString("yyyy-MM-dd");
                dataGridView1.Rows[n].Cells[6].Value = "  Edita  ";

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            openFileDialog1.FileName.ToString();
            caf = caf.xmlToCaf(openFileDialog1.FileName.ToString());
            labelRutEmpresa.Text = caf.rutEmpresa;
            labelFchCaf.Text = caf.fecha.ToString("yyyy-MM-dd");
            labelFolioIni.Text = caf.folioInicial.ToString();
            labelFolioFinal.Text = caf.folioFinal.ToString();
            labelTipoDte.Text = caf.tipoDte;
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            //verificar si ya existe
            if (caf.exist(openFileDialog1.SafeFileName) == "False")
            {
                if (caf.tipoDte != null)
                {
                    //
                    this.caf.nomXml = openFileDialog1.SafeFileName;
                    this.caf.save(caf);
                   // actDataG();
                }
                else
                {
                    MessageBox.Show("Tiene que buscar el XML antes de guardar");
                }

            }
            else
            {
                MessageBox.Show("El CAF ya existe!!!");
            }
        }
    }
}
