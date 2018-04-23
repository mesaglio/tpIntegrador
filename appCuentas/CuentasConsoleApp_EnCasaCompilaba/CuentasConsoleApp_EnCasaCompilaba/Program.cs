using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuentasConsoleApp_EnCasaCompilaba
{
	class Program
	{
		static void Main(string[] args)
		{
			Cajero cajero = new Cajero();
			double valorEnNumeros;
			int cant = 000;

			Console.WriteLine("Sistema de Conteo de Cuentas - En Casa Compilaba");
            Console.WriteLine("En caso de querer salir en el dni escriba exit");
			Console.WriteLine(string.Empty);
            while (true) {
                Console.Write("Ingrese el DNI del cliente: ");
                string dni = Console.ReadLine();
                if (dni == "exit") { break; }
                Console.Write("Ingrese el saldo minimo: ");
                string valor = Console.ReadLine();

                while (true)
                {
                    if (Double.TryParse(valor, out valorEnNumeros))
                    {
                        cant = cajero.contarCuentas(dni, valorEnNumeros);
                        break;
                    }
                    Console.Write("No es un numero, intente otra vez: ");
                    valor = Console.ReadLine();
                }

                Console.WriteLine(String.Empty);
                Console.WriteLine(String.Empty);

                if (cant != -1)
                {
                    Console.WriteLine("Cuentas del Cliente DNI {0} con Saldo superior a {1} = {2}", dni, valor, cant);
                }
                else
                {
                    Console.WriteLine("El Cliente no existe");
                }

                Console.WriteLine(String.Empty);
                Console.WriteLine(String.Empty);
                }

            Environment.Exit(0);
            

        }
	}
}
