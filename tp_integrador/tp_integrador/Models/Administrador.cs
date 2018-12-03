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

        public void CargarClientes(HttpPostedFileBase file)
        {
            CargarJson cargar = new CargarJson();
            cargar.LoadJson<Cliente>(file.InputStream);
        }

        public void CargarAdmins(HttpPostedFileBase file)
        {
            CargarJson cargar = new CargarJson();
            cargar.LoadJson<Administrador>(file.InputStream);
        }

        #endregion

        public Administrador(string v1, string v2)
        {
            usuario = v1;
            password = v2;
            esadmin = true;
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
        public void NuevoAdministrador(Administrador administrador)
        {
            ORM.Instancia.Insert(administrador);
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

        #region Reportes
        public List<Transformador> GetTransformadors()
        {
            List<Transformador> transformadors = new List<Transformador>();
            foreach (Zona zona in ORM.Instancia.GetAllZonas())
            {
                foreach (Transformador tr in ORM.Instancia.GetTransformadores(zona.idZona))
                {
                    transformadors.Add(tr);
                }
            }
            return transformadors;
        }

        public List<Cliente> GetClientes()
        {
            return ORM.Instancia.GetAllClientes();
        }

        public List<double> GetInteligenteVsEstandar()
        {
            double inte = 0;
            double esta = 0;
            foreach (dynamic dip in ORM.Instancia.GetAllDispositivos())
            {
                if (dip.GetType() == typeof(Inteligente)) inte = inte + dip.Consumo;
                if (dip.GetType() == typeof(Estandar)) esta = esta + dip.Consumo;
            }
            List<double> vs = new List<double>();
            vs.Add(inte);
            vs.Add(esta);
            return vs;
            }

        public void UpdateMyData(Administrador admUpdate)
        {
            this.apellido = admUpdate.apellido;
            this.nombre = admUpdate.nombre;
            this.domicilio = admUpdate.domicilio;
            
            ORM.Instancia.Update(this);
            
        }
        #endregion
    }
}