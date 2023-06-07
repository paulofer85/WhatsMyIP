using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using System.Configuration;
using BusinessLogicWhatsMyIp;
using System.IO;

namespace SvcWhatsMyIP
{
	public partial class SvcSpam : ServiceBase
	{
		private Timer timer;
		private BusinessLogicSpam bLSpam;
		
		public string LocalIP { get; set; }
		public string PublicIP { get; set; }
		public DateTime lastSendTime;
		public String message;
		public String[] mails;

		
		public SvcSpam()
		{
			InitializeComponent();

			bLSpam = new BusinessLogicSpam();
			
			this.EventLog.Source = this.ServiceName;
			this.EventLog.Log = this.ServiceName + "Log";
			this.EventLog.WriteEntry("WhatsMyIPSpammer: se inicializa el svc");

			int time = Convert.ToInt32(ConfigurationManager.AppSettings["TimeInterval"]);
			timer = new Timer(time);

			message = "Amigos de la Torre<br /><br />El viernes 21/03/14 comienza el curso de <b>'Astronomía Observacional'</b>, el objeto de este curso es que el alumno adquiera un conocimiento integral del cielo nocturno y aprenda paulatinamente conceptos como: sistemas de coordenadas, constelaciones, movimientos de los planetas, principales estrellas y lectura e interpretación de diferentes cartas celestes. El curso está basado principalmente en observaciones prácticas, con diferentes instrumentos, a fin de que los participantes logren familiarizarse rápidamente con el firmamento. Las observaciones con los instrumentos del observatorio se harán de acuerdo a las efemérides y a las condiciones meteorológicas."
							+ "<br /><br /><b>Dados los contenidos y metodologías aplicadas en el curso, el mismo es para adultos y chicos de 12 años en adelante y no se necesitan conocimientos previos de matemática, física o astronomía.</b>"
								+ "<b><br /><br /><ul><li>Comienzo: viernes 21/03/14</li><li>Duración: 4 clases</li><li>Horario: viernes de 20 a 22 hs</li><li>Arancel: $300 (se abonan el mismo día del inicio del curso) - Alumnos del Colegio San José gratis - Todo lo recaudado se destina al mantenimiento del observatorio</li><li>Dirección: Bartolomé Mitre 2455, Capital Federal (Colegio San José)</li><li>Informes e inscripción:<br />www.observatoriosanjose.com.ar<br />informes@observatoriosanjose.com.ar<br />infoosj@yahoo.com.ar</b></li></ul>";

			mails = bLSpam.GetMailsFromFile("C:/mails.txt");

			timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
			timer.Start();						
		}

		
		private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
				string to;
				for (int i = 0; i < mails.Length; i++)
				{
					to = mails[i];
					if (to != String.Empty)
					{
						if (bLSpam.sendCount < 150)
						{
							lastSendTime = DateTime.Now;
                            //bLSpam.SendMail("[OSJ] NUEVO Curso de Telescopios", message);
                            bLSpam.SendMail("[OSJ] NUEVO Curso de Astronomía Observacional", message);
							this.EventLog.WriteEntry("WhatsMyIPSpammer: se envio el mail nro " + bLSpam.sendCount);
							System.Threading.Thread.Sleep(1000);
						}
						else if (DateTime.Now.AddHours(1) > lastSendTime)
						{
							bLSpam.sendCount = 0;
							this.EventLog.WriteEntry("WhatsMyIPSpammer: se cumplio la hora luego de haber envido lote de mails");
						}
					}
					mails[i] = "";					
				}
            }
            catch (Exception x)
            {
                this.EventLog.WriteEntry(new System.IO.ErrorEventArgs(x).ToString());
                
				timer.Start();
                this.EventLog.WriteEntry("WhatsMyIPSpammer: Se reactiva el timer");
            }
 
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

    }
}

