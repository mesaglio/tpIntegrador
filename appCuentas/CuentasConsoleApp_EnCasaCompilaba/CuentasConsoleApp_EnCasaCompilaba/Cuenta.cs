using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuentasConsoleApp_EnCasaCompilaba
{
	class Cuenta
	{
		public int idCuenta { get; private set; }
		public double saldo { get; private set; }

		public Cuenta(int num, double valor)
		{
			this.idCuenta = num;
			this.saldo = valor;
		}
	}
}
