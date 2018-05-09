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
        public DAOUsuario Dao;

        public CargarJson()
        {
            Dao = MvcApplication.Daobjeto;
        }

        public void LoadUsuarios(Stream path)
        {
            string json = (new StreamReader(path)).ReadToEnd();            
            List<Cliente> djson = JsonConvert.DeserializeObject<List<Cliente>>(json);

            foreach(Cliente usuario in djson)
            {
                Dao.CargarUsuario(usuario);
            }
        }        
    }
}