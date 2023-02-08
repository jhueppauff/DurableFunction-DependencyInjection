using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp2
{
	internal class Service : IService
	{
		public string Hello()
		{
			return "Hello";
		}
	}
}
