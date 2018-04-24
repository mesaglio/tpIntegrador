using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuentasConsoleApp_EnCasaCompilaba
{
	class Control
	{

		private List<Cliente> clientes = new List<Cliente>();

		public Control()
		{
			this.clientes.Add(new Cliente("10500955", 1654));
			this.clientes.Add(new Cliente("15100500", 321));
			this.clientes.Add(new Cliente("21200100", 795));
			this.clientes.Add(new Cliente("35300500", 10233));
		}
		public Cliente buscarCliente(string num)
		{
			foreach (Cliente cliente in clientes)
			{
				if (cliente.dni == num)
				{
					return cliente;
				}
			}
			return null;
		}

		public int cantCuentasDeConSaldoMinimo(string doc, double valor)
		{
			Cliente cliente = buscarCliente(doc);
			if (cliente != null)
			{
				return cliente.cuentasQueSuperan(valor);
			}
			else
			{
				return -1;
			}

		}

	}
}
