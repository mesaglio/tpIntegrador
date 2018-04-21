using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuentasConsoleApp_EnCasaCompilaba
{
	class Cajero
	{
		Control control = new Control();

		public int contarCuentas(string doc, double cant)
		{
			return control.cantCuentasDeConSaldoMinimo(doc, cant);
		}
	}
}
