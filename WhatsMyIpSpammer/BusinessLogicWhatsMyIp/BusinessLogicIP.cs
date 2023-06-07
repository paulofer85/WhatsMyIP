using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Configuration;

namespace BusinessLogicWhatsMyIp
{
	public class BusinessLogicIP
	{
		public SMTPEmail email;
		public string LocalIP { get; set; }
		public string PublicIP { get; set; }
		

		public BusinessLogicIP()
		{			
			email = new SMTPEmail();
			ConfigureMailSettings();
		}

		public bool CheckNewPublicIPAdress()
		{
			string auxPublicIP = IPUtils.GetPublicIP();

			if ((auxPublicIP != PublicIP) 
				|| (String.IsNullOrEmpty(PublicIP) && String.IsNullOrEmpty(LocalIP)))
			{
				PublicIP = auxPublicIP;
				LocalIP = IPUtils.GetLocalIP();

				email.Subject = "[WhatsMyIP] Se cambió la IP pública a " + PublicIP;
				email.Message = "<b>IP pública:</b> " + PublicIP + "\n <br />" + "<b>IP local:</b> " + LocalIP;
				email.Message+= "\n <br /> \n <br /> <b><u>NETSTAT</u></b> \n <br />" + IPUtils.GetNetStat();
				email.Message += "\n <br /> \n <br /> <b><u>GEOLOCALIZACION</u></b> \n <br />" + IPUtils.GetGeoLocationWithIP(this.PublicIP);
				MailUtils.SendEmail(email);

				return true;
			}
			return false;
		}

		private void ConfigureMailSettings()
		{
			email.From = ConfigurationManager.AppSettings["FromAddress"];
			email.To = ConfigurationManager.AppSettings["ToAddress"];
			email.SMTPClient = ConfigurationManager.AppSettings["SMTP"];
			email.SMTPServer = ConfigurationManager.AppSettings["SMTP"];
			email.SMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);

			email.Password = ConfigurationManager.AppSettings["Password"];			
			email.Domain = ConfigurationManager.AppSettings["Domain"];
			email.IsSSL = bool.Parse(ConfigurationManager.AppSettings["EnableSSL"]);
			email.UseDefaultCredentials= bool.Parse(ConfigurationManager.AppSettings["UseDefaultCredentials"]);
		}
	}
}
