using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuentasConsoleApp_EnCasaCompilaba
{
	class Cliente
	{
		public string dni { get; private set; }

		private List<Cuenta> cuentas = new List<Cuenta>();

		public Cliente(string num, double valor)
		{
			this.dni = num;
			this.cuentas.Add(new Cuenta(001, valor + 1000));
			this.cuentas.Add(new Cuenta(002, valor + 5000));
			this.cuentas.Add(new Cuenta(003, valor + 10));
			this.cuentas.Add(new Cuenta(004, valor + 500));
			this.cuentas.Add(new Cuenta(005, valor + 250));
		}

		public int cuentasQueSuperan(double valor)
		{
			int i = 0;
			foreach (Cuenta cuenta in cuentas)
			{
				if (cuenta.saldo > valor)
				{
					i++;
				}
			}
			return i;
		}
	}
}
