using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace IatDteBridge
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        /// 
        

        [STAThread]
        static void Main()
        {
            string estado = string.Empty;
            estado =  new FuncionesComunes().isRunAplication();
           //MessageBox.Show(estado);
            bool isNew = false;
            Mutex mtx = new Mutex(true, "IatDteBridge", out isNew);
            if (!isNew)
            {
                //MessageBox.Show("se esta ejecutando la aplicación");
                return;
            }
            if (estado != "True")
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
            else
            {
                Environment.Exit(0);
            }
        }

    }


}