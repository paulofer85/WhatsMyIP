using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common;
using System.Configuration;
using System.Text.RegularExpressions;
using Data;

namespace TestWhatsMyIP
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        public void SendMailTest()
        {
            Email email = new Email();

            email.From = ConfigurationManager.AppSettings["FromAddress"];
            email.To = ConfigurationManager.AppSettings["ToAddress"];
            email.SMTPClient = ConfigurationManager.AppSettings["SMTP"];
            email.SMTPServer = ConfigurationManager.AppSettings["SMTP"];
            email.SMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);

            email.Password = ConfigurationManager.AppSettings["Password"];
            email.Domain = ConfigurationManager.AppSettings["Domain"];
            email.IsSSL = bool.Parse(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableSSL"]) ? ConfigurationManager.AppSettings["EnableSSL"] : "false");
            email.UseDefaultCredentials = bool.Parse(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UseDefaultCredentials"]) ? ConfigurationManager.AppSettings["UseDefaultCredentials"] : "false");

            email.Subject = "[OSJ] SendMailTest ";
            email.Message = "Amigos de la Torre<br /><br />El viernes 21/03/14 comienza el curso de <b>'Astronomía Observacional'</b>, el objeto de este curso es que el alumno adquiera un conocimiento integral del cielo nocturno y aprenda paulatinamente conceptos como: sistemas de coordenadas, constelaciones, movimientos de los planetas, principales estrellas y lectura e interpretación de diferentes cartas celestes. El curso está basado principalmente en observaciones prácticas, con diferentes instrumentos, a fin de que los participantes logren familiarizarse rápidamente con el firmamento. Las observaciones con los instrumentos del observatorio se harán de acuerdo a las efemérides y a las condiciones meteorológicas."
                + "<br /><br /><b>Dados los contenidos y metodologías aplicadas en el curso, el mismo es para adultos y chicos de 12 años en adelante y no se necesitan conocimientos previos de matemática, física o astronomía.</b>"
                    + "<b><br /><br /><ul><li>Comienzo: viernes 21/03/14</li><li>Duración: 4 clases</li><li>Horario: viernes de 20 a 22 hs</li><li>Arancel: $300 (se abonan el mismo día del inicio del curso; cupos limitados)  - Alumnos del Colegio San José gratis - Todo lo recaudado se destina al mantenimiento del observatorio</li><li>Dirección: Bartolomé Mitre 2455, Capital Federal (Colegio San José)</li><li>Informes e inscripción:<br />www.observatoriosanjose.com.ar<br />informes@observatoriosanjose.com.ar<br />infoosj@yahoo.com.ar</b></li></ul>";

            Assert.IsTrue(MailUtils.SendEmail(email));
        }

        [TestMethod]
        public void GetLocalIPTest()
        {
            Regex ip = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

            Assert.IsFalse(String.IsNullOrEmpty(ip.Matches(IPUtils.GetLocalIP())[0].ToString()));
        }

        [TestMethod]
        public void GetGlobalIPTest()
        {
            Regex ip = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

            Assert.IsFalse(String.IsNullOrEmpty(ip.Matches(IPUtils.GetPublicIP())[0].ToString()));
        }


        [TestMethod]
        public void GetIPGlobalPropertiesTest()
        {
            string rdo = (!String.IsNullOrEmpty(IPUtils.ipProperties.DhcpScopeName) ? IPUtils.ipProperties.DhcpScopeName : "")
                + (!String.IsNullOrEmpty(IPUtils.ipProperties.DomainName) ? IPUtils.ipProperties.DomainName : "")
                + (!String.IsNullOrEmpty(IPUtils.ipProperties.HostName) ? IPUtils.ipProperties.HostName : "");

            Assert.IsFalse(String.IsNullOrEmpty(rdo));
        }

        [TestMethod]
        public void GetNetStatTest()
        {
            Assert.IsFalse(String.IsNullOrEmpty(IPUtils.GetNetStat()));
        }

        [TestMethod]
        public void GetAllIPInfoTest()
        {
            string rdo = IPUtils.GetAllIPInfo();

            Assert.IsFalse(String.IsNullOrEmpty(rdo));
        }

        [TestMethod]
        public void GetGeoLocationIPTest()
        {
            string rdo = IPUtils.GetGeoLocationWithIP(IPUtils.GetPublicIP());

            Assert.IsFalse(String.IsNullOrEmpty(rdo));
        }


        [TestMethod]
        public void GetMailsFromFileTest()
        {
            string[] mails = MailUtils.GetMailsFromFile("c:/mails.txt");

            Assert.IsTrue(mails.Length > 0);
        }
    }
}
