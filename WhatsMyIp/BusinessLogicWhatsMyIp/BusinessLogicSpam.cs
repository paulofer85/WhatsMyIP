using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Configuration;

namespace BusinessLogicWhatsMyIp
{
	public class BusinessLogicSpam
	{
		public int sendCount;
		public SMTPEmail email;
		public string LocalIP { get; set; }
		public string PublicIP { get; set; }

		public BusinessLogicSpam()
		{			
			email = new SMTPEmail();
			ConfigureMailSettings();
		}

		public bool SendMail(string subject, string message)
		{
			email.Subject = (subject == String.Empty) ? "[OSJ] NUEVO Curso de Astronomía Observacional" : subject;
			email.Message = message;

			sendCount++;
			return MailUtils.SendEmail(email);
		}


		public bool SendMail(string to, string subject, string message)
		{
			email.Subject = (subject == String.Empty) ? "[OSJ] NUEVO Curso de Astronomía Observacional" : subject;
			email.Message = message;

			sendCount++;
			return MailUtils.SendEmail(email, to);
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


		public string[] GetMailsFromFile(string file)
		{
			return MailUtils.GetMailsFromFile(file);
		}
	}
}
