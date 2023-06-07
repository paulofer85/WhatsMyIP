using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Web;
using System.IO;

namespace Common
{
	public static class IPUtils
	{
		public static IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();

		public static string GetLocalIP()
		{
			IPHostEntry host;
			string localIP = "?";
			host = Dns.GetHostEntry(Dns.GetHostName());

			foreach (IPAddress ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
					localIP = ip.ToString();
			}
			return localIP;
		}


		public static string GetPublicIP()
		{
			try
			{
				var request = (HttpWebRequest)WebRequest.Create("http://ifconfig.me");

				request.UserAgent = "curl"; // this simulate curl linux command

				string publicIPAddress;

				request.Method = "GET";
				using (WebResponse response = request.GetResponse())
				{
					using (var reader = new StreamReader(response.GetResponseStream()))
					{
						publicIPAddress = reader.ReadToEnd();
					}
				}

				return publicIPAddress.Replace("\n", "");
			}
			catch (WebException ex)
			{
				throw ex;
			}
		}


		public static string GetAllIPInfo()
		{
			try
			{
				var request = (HttpWebRequest)WebRequest.Create("http://ifconfig.me/all");

				request.Method = "GET";
				using (WebResponse response = request.GetResponse())
				{
					using (var reader = new StreamReader(response.GetResponseStream()))
					{
						return reader.ReadToEnd();
					}
				}
			}
			catch (WebException ex)
			{
				throw ex;
			}
		}


		/// <summary>
		/// devuelve los datos de geolocalización de la ip pedida
		/// </summary>
		/// <param name="ip">ip de la cual se devuelven los datos geograficos</param>
		/// <returns></returns>
		public static string GetGeoLocationWithIP(string ip)
		{
			try 
			{
				var request = (HttpWebRequest)WebRequest.Create("https://community-telize-json-ip-and-geoip.p.mashape.com/geoip/" + ip + "?callback=getgeoip");

				request.Method = "GET";
				request.Headers.Add("X-Mashape-Authorization", "n5j6re8lb26HZcoIhXSRwuZpu0JjdEVQ");
				
				using (WebResponse response = request.GetResponse())
				{
					using (var reader = new StreamReader(response.GetResponseStream()))
					{
						return reader.ReadToEnd();
					}
				}

			}
			catch (Exception)
			{
		
				throw;
			}
		}


		public static IPAddress GetIPOfAddress(string hostName)
		{
			Ping ping = new Ping();
			var replay = ping.Send(hostName);

			if (replay.Status == IPStatus.Success)
				return replay.Address;
			
			return null;
		}


		public static string GetNetStat()
		{
			try
			{
				string rdoNetstat = String.Empty;
				IPEndPoint[] endPoints = ipProperties.GetActiveTcpListeners();
				TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections();

				foreach (TcpConnectionInformation info in tcpConnections)
				{
					rdoNetstat += ("Local : " + info.LocalEndPoint.Address.ToString()
					+ ":" + info.LocalEndPoint.Port.ToString()
					+ "\n<br />Remote : " + info.RemoteEndPoint.Address.ToString()
					+ ":" + info.RemoteEndPoint.Port.ToString()
					+ "\n<br />State : " + info.State.ToString() + "\n\n<br /><br />");
				}
				return rdoNetstat;
			}
			catch (NetworkInformationException ex)
			{
				throw ex;
			}
		}

	}
}
