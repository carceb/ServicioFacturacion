using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServicioFacturacion
{
    public partial class ServicioFacturacion : ServiceBase
    {
        private Timer timer1 = null;
        public ServicioFacturacion()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //CHEQUEA EL INTERVALO EN LA BASE DE DATOS E INICIA EL TIMER
            timer1 = new Timer();
            timer1.Interval = Convert.ToDouble(Parametros.ObtenerParametro("INTERVALO TIMER"));
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
            timer1.Enabled = true;

            //ESCRIBE EN EL LOG EL INICIO DEL SERVCIO
            Library.WriteErrorLog("Servicio de facturación BETA 1 Inicio del Servicio." );
        }
        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            //REALIZA EL PROCESO SEGUN EL INTERVALO INDICADO EN LA BASE DE DATOS
            string resultadoImpresion;
            resultadoImpresion = Imprimir.ImprimirCola();
            //********************************************************

            //ESCRIBE LOS SUCESOS DE CADA PROCESO EN EL LOG
            Library.WriteErrorLog("Servicio de facturación BETA 1 iniciado y en ejecución. " + resultadoImpresion);
            //*********************************************************

            //CHEQUEA EL INTERVALO EN LA BASE DE DATOS
            timer1.Interval = Convert.ToDouble(Parametros.ObtenerParametro("INTERVALO TIMER"));
            //*********************************************************

            resultadoImpresion = "";
        }

        protected override void OnStop()
        {
            //DETIENE EL TIMER
            timer1.Enabled = false;
            //*********************************************************

            //ESCRIBE EN EL LOG EL FIN DEL SERVCIO
            Library.WriteErrorLog("Servicio de facturación BETA 1 detenido, gracias.");
        }
    }
}
