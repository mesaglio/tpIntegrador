using Gmap.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Administrador : Usuarios
    {
        public DateTime AltaSistema { get; set; }        

        public int CantMeses()
        {
            return Math.Abs(DateTime.Now.Year - AltaSistema.Year) * 12 + (DateTime.Now.Month - AltaSistema.Month);
        }

        #region Carga de datos
        public void CargarTransformador(HttpPostedFileBase file)
        {

            CargarJson cargar = new CargarJson();
            cargar.LoadJson<Transformador>(file.InputStream);
        }

        public void CargarClienter(HttpPostedFileBase file)
        {

            CargarJson cargar = new CargarJson();
            cargar.LoadJson<Cliente>(file.InputStream);
        }

        #endregion

        public Administrador(string v1, string v2)
        {
            usuario = v1;
            password = v2;
            esadmin =true;
		}

        public Administrador()
        {            
            esadmin = true;
        }

        public Administrador(int id, string name, string lastname, string home, string user, string clave, DateTime alta) : base(id, name, lastname, home, user, clave)
        {
            AltaSistema = alta;
            esadmin = true;            
        }

        public void NuevoCliente(int id, string name, string lastname, string home, Location coords, string user, string clave, string phone, DateTime alta, Categoria categ, string doc_t, string doc_n)
        {
			Cliente unCliente = new Cliente(id, name, lastname, home, coords, user, clave, phone, alta, categ, doc_t, doc_n, false);
			ORM.Instancia.Insert(unCliente);			
        }

        public Cliente BuscarCliente(int id)
        {
            return DAOUsuario.Instancia.BuscarCliente(id);
        }

        public void BajaCliente(int id)
        {
			DAOUsuario.Instancia.QuitarUsuario(id);
        }

        
    }
}