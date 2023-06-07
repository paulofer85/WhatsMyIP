using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestWhatsMyIP
{
	[TestClass]
	public class BusinessTest
	{
		[TestMethod]
		public void CheckNewPublicIPAdressTest()
		{
			BusinessLogicWhatsMyIp.BusinessLogicIP blIP = new BusinessLogicWhatsMyIp.BusinessLogicIP();

			Assert.IsTrue(blIP.CheckNewPublicIPAdress());
		}
	}
}
