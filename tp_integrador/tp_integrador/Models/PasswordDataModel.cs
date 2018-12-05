using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
	public class PasswordDataModel
	{
		public int ID { get; set; }
		public string Password { get; set; }
		public string NewPassword { get; set; }
		public string ReNewPassword { get; set; }

		public string NewPasswordHash { get; private set; }

		public bool IsOK(string oldPassword)
		{
			var inputOld = HashThis.Instancia.GetHash(Password);
			if (oldPassword != inputOld) return false;

			var inputNew = HashThis.Instancia.GetHash(NewPassword);
			var inputReNew = HashThis.Instancia.GetHash(ReNewPassword);

			if (inputNew != inputReNew) return false;

			NewPasswordHash = inputNew;
			return true;
		}
		
		
	}
}