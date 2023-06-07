using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Configuration;
using System.ServiceProcess;
using System.Diagnostics;


namespace SvcSpam
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : System.Configuration.Install.Installer
	{
		public ProjectInstaller()
		{
			InitializeComponent();

			this.AfterInstall += new InstallEventHandler(this.OnAfterInstall);
		}

		public override void Install(System.Collections.IDictionary stateSaver)
		{
			base.Install(stateSaver);

			//get Configuration section 
			//name from custom action parameter
			string sectionName = this.Context.Parameters["sectionName"];

			//get Protected Configuration Provider 
			//name from custom action parameter
			string provName = this.Context.Parameters["provName"];

			// get the exe path from the default context parameters
			string exeFilePath = this.Context.Parameters["assemblypath"];

			//encrypt the configuration section
			EncodeAppConfiguration(sectionName, "DataProtectionConfigurationProvider", exeFilePath);
		}


		/// <summary>
		/// Encripta el app.config
		/// </summary>
		/// <param name="sectionName">el tag que se va a encroptar que se encuentra dentro del .config</param>
		/// <param name="provName">el nombre del encriptado que se va a usar DataProtectionConfigurationProvider o SHA1 </param>
		/// <param name="exeFilePath">path del .exe compilado</param>
		private void EncodeAppConfiguration(string sectionName, string provName, string exeFilePath)
		{
			Configuration config = ConfigurationManager.OpenExeConfiguration(exeFilePath);
			ConfigurationSection section = config.GetSection(sectionName);

			if (!section.SectionInformation.IsProtected)
				section.SectionInformation.ProtectSection(provName);//Protecting the specified section with the specified provider
			
			section.SectionInformation.ForceSave = true;
			config.Save(ConfigurationSaveMode.Modified);
		}


		protected void OnAfterInstall(object sender, InstallEventArgs e)
		{
			try
			{
				ServiceController svcController = new ServiceController(this.serviceInstaller1.ServiceName);
				if (svcController.Status != ServiceControllerStatus.Running)
					svcController.Start();
			}
			catch (Exception ex)
			{
				System.Diagnostics.EventLog appLog = new System.Diagnostics.EventLog();
				appLog.Source = "Installer WhatsMyIP";
				appLog.WriteEntry("Installer WhatsMyIP: " + ex.Message);
				throw ex;
			}
		}
	}
}
