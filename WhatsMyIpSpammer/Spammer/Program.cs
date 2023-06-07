using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogicWhatsMyIp;
using System.Diagnostics;

namespace Spammer
{
	class Program
	{

		private static BusinessLogicSpam bLSpam;
		
		public static DateTime lastSendTime;
		public static String message;
        public static String[] mails;
        public static String[] deleteMails;
		

		private static void ProcessMails()
        {
            try
            {
				//EventLog eLog = new EventLog();
				//eLog.Source = "Spammer";
				//eLog.Log = "Spammer" + "Log";
				//eLog.WriteEntry("WhatsMyIPSpammer: se inicializa el svc");

				while (mails.Last() != String.Empty)
				{
					for (int i = 0; i < mails.Length; i++)
					{
						if (mails[i] != String.Empty && !deleteMails.Contains(mails[i]))
						{
							if (bLSpam.sendCount < 150)
							{
								lastSendTime = DateTime.Now;

                                bLSpam.SendMail(mails[i], "[OSJ] Curso de Historia de la Astronomía", message);
                                //bLSpam.SendMail(mails[i], "[OSJ] Curso presencial de Astronomía Observacional", message);
                                //bLSpam.SendMail(mails[i], "", message);
                                //bLSpam.SendMail(mails[i], "[OSJ] Inauguración de telescopio", message);
                                //bLSpam.SendMail(mails[i], "[OSJ] Curso de Telescopios", message);
                                //bLSpam.SendMail(mails[i], "[OSJ] Curso de Misiones espaciales no tripuladas", message);
                                //bLSpam.SendMail(mails[i], "[OSJ] Curso de Astronomía estelar", message);
                                //bLSpam.SendMail(mails[i], "[OSJ] Charla sobre Ciencia y Fe", message);
                                //bLSpam.SendMail(mails[i], "[OSJ] 2do Curso Intensivo de Introducción a la Astrofotografía – 2022", message);

								System.Console.WriteLine("WhatsMyIPSpammer: se envio el mail nro " + i + " mail " + mails[i]);
								//eLog.WriteEntry("WhatsMyIPSpammer: se envio el mail nro " + i + " mail " + mails[i]);
								
								mails[i] = String.Empty;								
							}
							else if (DateTime.Now > lastSendTime.AddHours(1))
							{
								bLSpam.sendCount = 0;
								System.Console.WriteLine("WhatsMyIPSpammer: se cumplio la hora luego de haber envido lote de mails");
							}
						}
						System.Threading.Thread.Sleep(5000);
					}					
				}
                System.Console.ReadKey();
            }
            catch (Exception x)
            {
				System.Console.WriteLine(new System.IO.ErrorEventArgs(x).ToString());
            }
 
        }
		static void Main(string[] args)
		{

			bLSpam = new BusinessLogicSpam();

            //message = "Amigos de la Torre, tenemos el agrado de invitarlo <b>'nuevo telescopio Padre Pommes'</b>, en el Observatorio San José. <br />"
            //    + "El antiguo telescopio Mailhat de principios del S. XX es preservado hoy en nuestro museo, en su lugar se ha instalado un moderno Meade LX600-ACF14 que lleva el nombre de Padre Pommes, el fundador de nuestro Observatorio."
            //                    + "<b><br /><br /><ul><li>Fecha: </b>Viernes 20/10/17</li><li><b>Hora: </b>de 20 a 22 hs</li><li><b>Entrada:</b> Gratuita</li><li><b>Gesto solidario: </b>Agradecemos la entrega de un alimento no perecedero o un elemento de higiene personal para ser entregado a la Parroquia Sagrado Corazón de Barracas</li><li><b>Dirección: </b>Bartolomé Mitre 2455, Capital Federal (Colegio San José)</li><li><b>Informes e inscripción:</b><br />www.observatoriosanjose.com.ar<br />informes@observatoriosanjose.com.ar<br />infoosj@yahoo.com.ar<br />https://www.facebook.com/observatoriosj/</b></li></ul>";

            ////Curso Observacional
            //message = "Amigos de la Torre<br /><br />El Viernes 23/09/22 comienza el curso de <b>'Astronomía Observacional'</b>, el objeto de este curso es que el alumno adquiera un conocimiento integral del cielo nocturno aprendiendo conceptos como: sistemas de coordenadas, constelaciones, movimientos de los planetas, principales estrellas y lectura e interpretación de diferentes cartas celestes. <br /> <br />  El curso está basado principalmente en observaciones prácticas, con diferentes instrumentos, con el fin de que el alumno logre familiarizarse rápidamente con el firmamentoy pueda identificar constelaciones y planetas, usar cartas celestes y manejar un telescopio y su puesta en estación. <br /><br /> Dados los contenidos, el curso está dirigido a adultos y adolescntes y no se requieren conocimientos previos."
            //                    + "<b><br /><br /><ul><li>Comienzo: Viernes 23/09/22</li><li>Duración: 6 clases</li><li>Horario: Jueves de 20 a 21 hs</li><li>Arancel: 1 único pago de $4500 (se abonan el mismo día del inicio del curso)</li><li>Dirección: Azcuenaga 158, CABA</li><li>Informes e inscripción:<br />informes@observatoriosanjose.com.ar</b></li></ul>"
            //                    + "<br />Temario:<br /><ul><li>23/9: Introducción- Unidades Astronómicas, Características</li><li>30/9 Eclíptica, Sistemas de Coordenadas.</li><li>7/10 Sistema Solar Características Observables.</li><li>14/10 Estrellas y Espacio Profundo Caracteristicas observables.</li><li>21/10 Tipos de Telescopios. Óptica y monturas.</li><li>28/10 Observación con práctica.</li></ul>";

            ////Curso telescopios
            //message = "Amigos de la Torre<br /><br />El viernes 11/10/19 comienza el curso de <b>'Telescopios'</b>, el temario comprende nociones de óptica, funcionamiento de los diversos tipos de telescopios, selección de oculares y accesorios, monturas y su empleo, así como uso de instrumentos y prácticas de observación con los mismos. El curso está ideado para todos los que quieran aprender a manejar un telescopio ya sea que estén por comprar uno o que ya lo posean y deseen sacarle el mayor provecho."
            //   + "<br /><br /><b>Dados los contenidos y metodologías aplicadas en el curso, el mismo es para adultos y chicos de 12 años en adelante y no se necesitan conocimientos previos de matemática, física o astronomía.</b>"
            //        + "<b><br /><br /><ul><li>Comienzo: viernes 11/10/19</li><li>Duración: 6 clases</li><li>Horario: Viernes de 20 a 22 hs</li><li>Arancel: $1500 (se abonan el mismo día del inicio del curso) - Alumnos del Colegio San José gratis - Todo lo recaudado se destina al mantenimiento del observatorio</li><li>Lugar: Bartolomé Mitre 2455, Capital Federal (Colegio San José) - El acceso al Observatorio es <b>por escalera y equivale a 8 pisos </b></li><li>Inscripción: se realiza el mismo día de inicio al abonar el curso</li><li>Informes:<br />www.observatoriosanjose.com.ar<br />informes@observatoriosanjose.com.ar<br />infoosj@yahoo.com.ar</b></li></ul>";

            //Curso misiones espaciales
            //message = "Amigos de la Torre<br /><br />El viernes 08/08/19 comenzará el curso sobre <b>Misiones espaciales no tripuladas</b>. Este curso brinda al participante conocimientos básicos sobre astronáutica y exploración del espacio por medio de sondas no tripuladas. Entre otros temas, se propone descubrir cuáles fueron los acontecimientos claves en la historia del desarrollo de los primeros cohetes, la construcción y lanzamiento de las primeras naves al espacio, la exploración de los cuerpos que componen el sistema solar y temas relacionados con el futuro de las misiones no tripuladas."
            //  + "<br /><br />Cada clase será además una interesante oportunidad para aprender más sobre los planetas, asteroides y otros cuerpos celestes que han podido ser estudiados con mayor detalle gracias a los avances tecnológicos de los últimos 3 siglos, a la vez que podremos hallar inspiración en las biografías de algunos hombres de ciencia cuyas mentes sobresalientes estuvieron un paso más allá de su tiempo."
            //        + "<br /><br /><ul><li>El acceso al Observatorio es <b>por escalera y equivale a 8 pisos. </b></li><li>Comienzo: Viernes 10/08/18</li><li>Duración: 6 clases</li><li>Horario: Viernes de 20 a 22 hs</li><li>Arancel: $1000 (se abonan el mismo día del inicio del curso) - Alumnos del Colegio San José gratis - Todo lo recaudado se destina al mantenimiento del observatorio</li><li>Lugar: Bartolomé Mitre 2455, Capital Federal (Colegio San José) - El acceso al Observatorio es <b>por escalera y equivale a 8 pisos </b></li><li>Inscripción: se realiza el mismo día de inicio al abonar el curso</li><li>Informes:<br />www.observatoriosanjose.com.ar<br />informes@observatoriosanjose.com.ar<br />infoosj@yahoo.com.ar</b></li></ul>";

            //Curso Estelar
            //message = "Amigos de la Torre<br /><br />El viernes 05/10/18 comenzará el curso sobre <b>Astronomía estelar</b> este curso brinda al participante conocimientos sobre estrellas y sistemas estelares. Comenzando con las bases de la dinámica estelar y continuando con sistemas estelares simples el alumno tiene un primer acercamiento a la vida de las estrellas, para finalizar luego con sistemas más complejos como cúmulos y galaxias. El curso es acompañado por observaciones de diversos sistemas y cúmulos estelares a través del telescopio principal del observatorio acorde a las efemérides."
            //  + "<br /><br />Dados los contenidos y metodologías aplicadas en el curso, el mismo es para adultos y chicos de 12 años en adelante y no se necesitan conocimientos previos de matemática, física o astronomía."
            //        + "<br /><br /><ul><li>El acceso al Observatorio es <b>por escalera y equivale a 8 pisos. </b></li><li>Comienzo: Viernes 05/10/18</li><li>Duración: 5 clases</li><li>Horario: Viernes de 20 a 22 hs</li><li>Arancel: $1000 (se abonan el mismo día del inicio del curso) - Alumnos del Colegio San José gratis - Todo lo recaudado se destina al mantenimiento del observatorio</li><li>Lugar: Bartolomé Mitre 2455, Capital Federal (Colegio San José) - El acceso al Observatorio es <b>por escalera y equivale a 8 pisos </b></li><li>Inscripción: se realiza el mismo día de inicio al abonar el curso</li><li>Informes:<br />www.observatoriosanjose.com.ar<br />informes@observatoriosanjose.com.ar<br />infoosj@yahoo.com.ar</b></li></ul>";

            //Curso Historia de la astronomia

            message = "Amigos de la Torre<br /><br />El Viernes 23/06/23 comenzará el curso sobre <b>Historia de la Astronomía. </b> Este curso está dirigido a todos aquellos, que con o sin nociones previas deseen adentrarse en la aventura del pensamiento científico y conocer más de cerca a sus protagonistas que tuvieron que mover los cielos luchando contra ideas preestablecidas desde por siglos. Aristarco, Copérnico, Kepler, Galileo, Newton, Einstein, Lemaitre y muchos otros nos acompañarán por la historia de la astronomía mostrando sus aciertos y errores, ampliando el conocimiento del universos para expandir una frontera que aún hoy nos sorprende."
              + "<br /><br />Dados los contenidos y metodologías aplicadas en el curso, el mismo es para adultos y chicos de 12 años en adelante y no se necesitan conocimientos previos de matemática, física o astronomía."
            //  + "<b><br /><br /><ul><li>Comienzo: Jueves 08/08/19</li><li>Duración: 5 clases</li><li>Horario: Jueves de 19 a 21 hs</li><li>Edad mínima: 12 años</li><li>Arancel: $1500 (se abonan el mismo día del inicio del curso) - Alumnos del Colegio San José gratis - Todo lo recaudado se destina al mantenimiento del observatorio</li><li>Dirección: Asociación de Fomento Villa Devoto, Biblioteca Presidente Roque Sáenz Peña, Joaquín V. González y Habana, CABA</li><li>Informes e inscripción:<br />www.observatoriosanjose.com.ar<br />informes@observatoriosanjose.com.ar<br />infoosj@yahoo.com.ar</b></li></ul>";
            //Colegio San Jose
                    + "<br /><br /><ul><li>El acceso al Observatorio es <b>por escalera y equivale a 8 pisos. </b></li><li>Comienzo: Viernes 23 de Junio</li><li>Duración: 6 clases, finaliza 11 de agosto con 2 viernes de receso por vacaciones 21 y 28 de Julio.</li><li>Horario: Viernes de 20 a 22 hs</li><li>Arancel: $1000 (se abonan el mismo día del inicio del curso) - Alumnos del Colegio San José gratis - Todo lo recaudado se destina al mantenimiento del observatorio</li><li>Lugar: Bartolomé Mitre 2455, Capital Federal (Colegio San José) - El acceso al Observatorio es <b>por escalera y equivale a 8 pisos </b></li><li>Inscripción: se realiza el mismo día de inicio al abonar el curso</li><li>Informes:<br />www.observatoriosanjose.com.ar<br />informes@observatoriosanjose.com.ar<br />infoosj@yahoo.com.ar</b></li></ul>";

            ////Charla sobre Ciencia y Fe
            //message = "Amigos de la Torre<br /><br />El Observatorio San José los invita a participar de la charla sobre Ciencia y Fe que tendrá lugar el próximo Viernes 06/12/19 a cargo del <b>Ingeniero Daniel Julián Checa</b> donde el tema principal será ¿Hay lugar en el Universo para Dios? En el pasado la cuestión de la ciencia y la fe ha originado enfrentamientos y persecuciones y hasta el día de hoy genera polémica y, a veces, sentimientos enfervorizados pero entendemos que la cuestión puede ser abordada y debatida desde el respeto en la diversidad de opiniones."
            //  + "<br />La charla se dividirá en cuatro partes:"
            //  + "<br /><ol><li>¿Hay lugar en el Universo para Dios ?</li><li>Relación de la Ciencia con la Fe. Naturaleza, consecuencias, implicancias.</li><li>Relación de la Ciencia con la Religión. </li><li>Espacio para preguntas e intercambio de ideas</li></ol>"
            //  + "<b><br /><ul><li><b>Actividad libre y gratuita</b></li> <li>Fecha: Viernes 06/12/19</li><li>Horario: Viernes de 20 a 21:30 hs</li><li>Lugar: Bartolomé Mitre 2455, Capital Federal (Colegio San José)</li>";

            //Astrofotografia
            //message = "<br />¿Te interesa la astrofotografía y no sabes por dónde empezar?<br /><br />¿Tenés un equipo y lo querés aprovechar al máximo?<br /><br />¿Querés mejorar tu equipamiento?<br /><br />¿No querés malgastar tu dinero comprando equipamiento innecesario?"
            //  + "<br /><br />Te ofrecemos este curso donde vas a poder adquirir todos los conocimiento y destrezas necesarias para iniciarte en la astrofotografía. A pesar de la modalidad virtual, el curso es teórico-práctico. Con clases grabadas, pero también con encuentros virtuales donde podés sacarte todas las dudas y donde planteamos problemas para resolver."
            //  + "<br /><ul><li><b>Destinatarios:</b> toda aquella persona que desee iniciarse en la astrofotografía.</li><li>Requisitos: tener una cuenta de Gmail, contar con conexión a internet con micrófono, parlantes y cámara web. Conocimientos básicos de navegación en páginas web, con los exploradores habituales (Chrome®, Firefox®, Opera®, Microsoft Edge®, Internet Explorer®)</li>"
            //  + "<li> <b>Conocimientos previos:</b> El curso no requiere tener conocimientos previos de astronomía ya que, aquellos necesarios para fotografía astronómica son impartidos en forma virtual al inicio del curso. De cualquier manera, es de mucha utilidad que los alumnos hayan realizado o tengan conocimiento de astronomía observacional y telescopios.</li>"
            //  + "<li><b>Fecha de comienzo:</b> 1 de Marzo 2022</li><li><b>Duración:</b> 8 semanas</li><li><b>Web: </b>https://observatoriosanjose.com.ar/moodle30/ </li><li><b>Descargar el programa completo: </b>https://drive.google.com/file/d/1LesYTBxdVc9bwCQWd9NzPTo-GplfoSgq/view </li><li>Informes: informes@observatoriosanjose.com.ar</li></ul>";


            mails = bLSpam.GetMailsFromFile("mails.txt");
            deleteMails = bLSpam.GetMailsFromFile("sacar.txt");

			ProcessMails();

			Console.ReadKey();
		}
	}
}
