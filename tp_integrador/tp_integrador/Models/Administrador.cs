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
        public bool NuevoAdministrador(Administrador administrador)
        {
			var contrasenia = HashThis.Instancia.GetHash(administrador.password);
			if(ORM.Instancia.GetIDUsuarioIfExists(administrador.usuario, contrasenia) != -1) return false;

			var nuevoAdmin = new Administrador(0, administrador.nombre, administrador.apellido, administrador.domicilio, administrador.usuario, contrasenia, DateTime.Now);

            ORM.Instancia.Insert(nuevoAdmin);
			return true;
        }

		public bool NuevoCliente(Cliente cliente)
		{
			var contrasenia = HashThis.Instancia.GetHash(cliente.password);
			if (ORM.Instancia.GetIDUsuarioIfExists(cliente.usuario, contrasenia) != -1) return false;
						
			var coordenadas = new Location(-34.553750, -58.468923);
			//TODO: Buscar como conseguir Goolge API Key Gratis o alguna forma alternativa de obtener las coordenadas reales
			var nuevoCliente = new Cliente(0, cliente.nombre, cliente.apellido, cliente.domicilio, coordenadas, cliente.usuario, contrasenia, cliente.Telefono, DateTime.Now, ORM.Instancia.GetCategoria("R1"), cliente.Documento_tipo, cliente.Documento_numero, false);

			ORM.Instancia.Insert(nuevoCliente);
			return true;
		}				

        public Cliente BuscarCliente(int id)
        {
            return DAOUsuario.Instancia.BuscarCliente(id);
        }

        public void BajaCliente(int id)
        {
            DAOUsuario.Instancia.QuitarUsuario(id);
        }

		public Dictionary<string, int> GetClientesIDUsername()
		{
			return ORM.Instancia.GetClientesIDUsername();
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

        public bool NuevoTemplateDisp(DispositivoGenerico dispositivo)
        {
			if (ORM.Instancia.ExisteTemplate(dispositivo)) return false;

            ORM.Instancia.Insert(dispositivo);
			return true;
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