using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace tp_integrador.Models
{
    public class CargarJson
    {
        public DAOUsuario DaoUser;

        public CargarJson()
        {
			//TODO: Cambiar por singletons
            DaoUser = MvcApplication.Daobjeto;           
        }


        public void LoadJson<T>(Stream path)
        {
            Type type = typeof(T);
            string json = (new StreamReader(path)).ReadToEnd();
            dynamic djson;
			            
            if (type == typeof(Cliente)) djson = JsonConvert.DeserializeObject<List<Cliente>>(json);			
            else if (type == typeof(Administrador)) djson = JsonConvert.DeserializeObject<List<Administrador>>(json);
			else if (type == typeof(Inteligente)) djson = JsonConvert.DeserializeObject<List<Inteligente>>(json);
			else if (type == typeof(Estandar)) djson = JsonConvert.DeserializeObject<List<Estandar>>(json);
			else if (type == typeof(Transformador)) djson = JsonConvert.DeserializeObject<List<Transformador>>(json);
			else return;

			if (type == typeof(Cliente) || type == typeof(Administrador))
			{
				foreach (var usuario in djson)
				{
					DaoUser.CargarUsuario(usuario);					
				}
			}
			else if (type == typeof(Inteligente) || type == typeof(Estandar))
			{
				foreach (var dispositivo in djson)
				{
					DaoUser.BuscarCliente(dispositivo.IdCliente).AgregarDispositivoDesdeJson(dispositivo);										
				}
			}
			else if (type == typeof(Transformador))
			{
				foreach (var t in djson)
				{
					DAOzona.Instancia.AgregarTransformadorAZona(t);
				}
			}		
            
        }

    }
}