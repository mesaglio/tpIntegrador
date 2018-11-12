using Gmap.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;


namespace tp_integrador.Models
{
    public class Cliente : Usuarios
    {
        public string Telefono { get; set; }
        public DateTime AltaServicio { get; set; }
        public Categoria Categoria { get; set; }
        public string Documento_tipo { get; set; }
        public string Documento_numero { get; set; }
        public int Puntos { get; set; }
        public bool AutoSimplex { get; set; }
        public Location Coordenadas { get; set; }

        public List<Dispositivo> dispositivos;

        public List<Dispositivo> DispositivosInteligentes => dispositivos.FindAll(i => i.EsInteligente);
        public List<Dispositivo> DispositivosEstandar => dispositivos.FindAll(i => !i.EsInteligente);
        //TODO: verificar si funciona estandar

        public Cliente(int id, string name, string lastname, string home, Location coords, string user, string clave, string phone, DateTime alta, Categoria categ, string doc_t, string doc_n, bool simplex, List<Dispositivo> disp = null) : base(id, name, lastname, home, user, clave)
        {
            Coordenadas = coords;
            Telefono = phone;
            Categoria = categ;
            Documento_tipo = doc_t;
            Documento_numero = doc_n;
            AltaServicio = alta;
            AutoSimplex = simplex;
            dispositivos = new List<Dispositivo>();
            if (disp != null) dispositivos = disp;
        }

        public int CantDispositivos()
        {
            return dispositivos.Count;
        }

        public bool EstasEncendido(Inteligente aparato)
        {
            return aparato.Encendido();
        }

        public int CantEncendidos()
        {
            int i = 0;
            foreach (Inteligente aparato in dispositivos.OfType<Inteligente>())
            {
                if (aparato.Encendido()) { i++; }
            }
            return i;
        }

        public bool AlgunoEncendido()
        {
            return (CantEncendidos() != 0);
        }

        public int CantApagados()
        {
            return dispositivos.OfType<Inteligente>().Count() - CantEncendidos();
        }


        public void ApagarDispositivo(Inteligente aparato)
        {
            aparato.Apagar();
        }

        public void EncenderDispositivo(Inteligente aparato)
        {
            aparato.Encender();
        }

        public void ModoAhorroDispositivo(Inteligente aparato)
        {
            aparato.ModoAhorro();
        }

        public double GetEstimado(Estandar aparato)
        {
            return aparato.ConsumoEstimado();
        }

        public void UsoDiario(Estandar aparato, byte horas)
        {
            aparato.SetUsoDiario(horas);
        }

        public int NuevoDispositivoInteligente(int idDisp, string nombre, double consumo, bool bajoconsumo)
        {
			int numero = CalcularNumero(nombre);

			dispositivos.Add(new Inteligente(idDisp, idUsuario, numero, nombre, consumo, bajoconsumo, 0, DateTime.Now, false));
            Puntos += 15;

			return numero;
        }

        public int NuevoDispositivoEstandar(int idDisp, string nombre, double consumo, bool bajoconsumo, byte usoPromedio)
        {
			int numero = CalcularNumero(nombre);

			dispositivos.Add(new Estandar(idDisp, idUsuario, numero, nombre, consumo, bajoconsumo, usoPromedio));

			return numero;
        }

        public void ConvertirAInteligente(Estandar aparato)
        {
            Inteligente adaptado = new Inteligente(aparato.IdDispositivo, idUsuario, aparato.Numero, aparato.Nombre, aparato.Consumo, aparato.BajoConsumo, 0, DateTime.Now, true);
            dispositivos.Remove(aparato);
            dispositivos.Add(adaptado);
            Puntos += 10;
        }

        public void AgregarDispositivo(Dispositivo dispositivo)
        {
            dispositivos.Add(dispositivo);
        }

        private int CalcularNumero(string nombre)
        {
            return (dispositivos.FindAll(x => x.Nombre == nombre)).Count + 1;
        }

		public dynamic BuscarDispositivo(int idDispositivo, int idCliente, int numero)
		{
			return dispositivos.Find(x => (x.IdDispositivo == idDispositivo) && (x.IdCliente == idCliente) && (x.Numero == numero));
		}

        #region INTERFAZ CONTROLLER
        public dynamic RunSimplex()
        {
            SIMPLEX sim = new SIMPLEX();

            var listaDisp = this.dispositivos;
            listaDisp.RemoveAll(x => x.Nombre.Split(' ')[0] == "Heladera");

            var respuesta = sim.GetSimplexData(sim.CrearConsulta(listaDisp));

            var sb = new StringBuilder();
            sb.AppendLine("<b>Consumo Optimo Para Sus Dispositivos: " + "</b><br/>");
            sb.AppendLine("" + "<br/>");
            sb.AppendLine("<b>Maximo: </b>" + respuesta[0] + "<br/>");
            var cantDisp = this.dispositivos.Count;

            for (int i = 1; i < respuesta.Length; i++)
            {
                sb.AppendLine("<b>" + this.dispositivos[cantDisp - i].Nombre + ": </b>" + respuesta[i] + "<br/>");
            }
            return sb;
        }

        public void ApagarDispositivo(string nombreDisp)
        {
            foreach (Inteligente undispo in this.DispositivosInteligentes)
            { if (undispo.Nombre == nombreDisp) undispo.Apagar(); }
        }
        public void EncenderDispositivo(string nombreDisp)
        {
            foreach (Inteligente undispo in this.DispositivosInteligentes)
            { if (undispo.Nombre == nombreDisp) undispo.Encender(); }
        }


        public void CargarDispositivos(HttpPostedFileBase file, int flag)
        {
            CargarJson cargar = new CargarJson();
            if (flag == 1)
                cargar.LoadJson<Inteligente>(file.InputStream);
            else
                cargar.LoadJson<Estandar>(file.InputStream);
        }

        public void AgregarDispositivoDeTemplate(int disp)
        {
            DAOTemplates a = new DAOTemplates();
            TemplateDispositivo dispositivo = a.Searchtemplatebyid(disp);
			int numero;

            if (dispositivo.Inteligente) numero = NuevoDispositivoInteligente(dispositivo.ID, dispositivo.getNombreEntero(), dispositivo.Consumo, dispositivo.Bajoconsumo);
            else numero = NuevoDispositivoEstandar(dispositivo.ID, dispositivo.getNombreEntero(), dispositivo.Consumo, dispositivo.Bajoconsumo, 0);

			ORM.Instancia.Insert(BuscarDispositivo(dispositivo.ID, idUsuario, numero));
        }

		public List<TemplateDispositivo> GetTemplateDisp()
		{
			var daot = new DAOTemplates();

			return daot.templateDisps;
		}

		public void CambiarEstado(Inteligente disp)
		{
			if (disp.Estado == 0) EncenderDispositivo(disp);
			else if (disp.Estado == 1)
			{
				if (disp.BajoConsumo) ModoAhorroDispositivo(disp);
				else ApagarDispositivo(disp);
			}
			else if (disp.Estado == 2) ApagarDispositivo(disp);
		}

    #endregion
}
}