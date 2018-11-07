using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace tp_integrador.Models
{
	public class HashThis
	{
		private static HashThis _instancia;

		private HashThis()
		{
		}

		public static HashThis Instancia
		{
			get
			{
				if (_instancia == null) _instancia = new HashThis();
				return _instancia;
			}

		}

		public string GetHash(string texto)
		{
			var crypt = new System.Security.Cryptography.SHA256Managed();
			var hash = new StringBuilder();

			byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(texto));
			foreach (byte theByte in crypto)
			{
				hash.Append(theByte.ToString("x2"));
			}

			return hash.ToString();
		}

		public bool EsCorrecto(string clave, string hash)
		{
			return hash == GetHash(clave);
		}

	}
}