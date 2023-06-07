using System;
using System.ServiceProcess;
using System.Configuration;

namespace SvcWhatsMyIP
{
	static class Program
	{

		/// <summary>
		/// Se ejecuta el servicio
		/// </summary>
		static void Main()
		{
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[] 
			{ 
			    new SvcSpam() 
			};
			ServiceBase.Run(ServicesToRun);
		}
	}
}
