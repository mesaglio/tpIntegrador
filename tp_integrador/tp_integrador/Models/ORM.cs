﻿using Gmap.net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace tp_integrador.Models
{
	public class ORM
	{
		private static ORM _instancia;
		private string connectionString;

		private ORM()
		{
			connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SGE"].ConnectionString;
		}

		public static ORM Instancia
		{
			get
			{
				if (_instancia == null) _instancia = new ORM();
				return _instancia;
			}
		}

		public void Insert(dynamic unaClase)
		{
			var type = unaClase.GetType();
			if (type == typeof(Administrador) || type == typeof(Cliente)) GuardarUsuario(unaClase);
			if (type == typeof(Inteligente) || type == typeof(Estandar)) GuardarDispositivo(unaClase);
			if (type == typeof(Zona)) GuardarZona(unaClase);
			if (type == typeof(Categoria)) GuardarCategoria(unaClase);
			if (type == typeof(Sensor)) GuardarSensor(unaClase);
			if (type == typeof(Actuador)) GuardarActuador(unaClase);
			if (type == typeof(Regla)) GuardarRegla(unaClase);
			if (type == typeof(EstadoDispositivo)) GuardarEstado(unaClase);
			if (type == typeof(DispositivoGenerico)) GuardarTemplate(unaClase);
			if (type == typeof(EstadoSensor)) GuardarEstadoSensor(unaClase);
			if (type == typeof(Transformador)) GuardarTransformador(unaClase);
		}

		public void Update(dynamic unaClase)
		{
			var type = unaClase.GetType();
			if (type == typeof(Administrador) || type == typeof(Cliente)) ActualizarUsuario(unaClase);
			if (type == typeof(Inteligente) || type == typeof(Estandar)) ActualizarDispositivo(unaClase);
			if (type == typeof(Zona)) ActualizarZona(unaClase);
			if (type == typeof(Categoria)) ActualizarCategoria(unaClase);
			if (type == typeof(Sensor)) ActualizarSensor(unaClase);
			if (type == typeof(Actuador)) ActualizarActuador(unaClase);
			if (type == typeof(Regla)) ActualizarRegla(unaClase);			
			if (type == typeof(DispositivoGenerico)) ActualizarTemplate(unaClase);
		}

		public void Delete(dynamic unaClase)
		{
			var type = unaClase.GetType();
			if (type == typeof(Inteligente) || type == typeof(Estandar)) EliminarDispositivo(unaClase);
			if (type == typeof(Sensor)) EliminarSensor(unaClase);
			if (type == typeof(Actuador)) EliminarActuador(unaClase);
			if (type == typeof(Regla)) EliminarRegla(unaClase);
		}

		#region Usuario

		// ------------------------------------ SELECT ------------------------------------

		public dynamic GetUsuario(int idUsuario)
		{
			var query = "SELECT * FROM {0} WHERE {1} = '{2}'";
			var data = Query(String.Format(query, "SGE.Usuario", "usua_idUsuario", idUsuario)).Tables[0];
			if (data.Rows.Count == 0) return null;

			var admin = GetAdministrador(idUsuario, data.Rows[0]);
			if (admin != null) return admin;

			return GetCliente(idUsuario, data.Rows[0]);
		}

		private Administrador GetAdministrador(int idAdmin, DataRow userData)
		{
			var query = "SELECT * FROM {0} WHERE {1} = '{2}'";
			var data = Query(String.Format(query, "SGE.Administrador", "admin_idUsuario", idAdmin)).Tables[0];
			if (data.Rows.Count == 0) return null;

			string nombre, apellido, domicilio, username, password;
			DateTime fechaAlta;

			nombre = userData["usua_nombre"].ToString();
			apellido = userData["usua_apellido"].ToString();
			domicilio = userData["usua_domicilio"].ToString();
			username = userData["usua_username"].ToString();
			password = userData["usua_password"].ToString();
			fechaAlta = (DateTime)data.Rows[0]["admin_fechaAlta"];

			return new Administrador(idAdmin, nombre, apellido, domicilio, username, password, fechaAlta);
		}

		private Cliente GetCliente(int idCliente, DataRow userData)
		{
			var query = "SELECT * FROM {0} WHERE {1} = '{2}'";
			var data = Query(String.Format(query, "SGE.Cliente", "clie_idUsuario", idCliente)).Tables[0];
			if (data.Rows.Count == 0) return null;

			var dispositivos = DAODispositivo.Instancia.FindAllFromCliente(idCliente);

			string nombre, apellido, domicilio, username, password, categoria;
			string telefono, docNum, docTipo;
			int puntos;
			double latitud, longitud;
			bool autoSimplex;
			DateTime fechaAlta;

			nombre = userData["usua_nombre"].ToString();
			apellido = userData["usua_apellido"].ToString();
			domicilio = userData["usua_domicilio"].ToString();
			username = userData["usua_username"].ToString();
			password = userData["usua_password"].ToString();
			fechaAlta = (DateTime)data.Rows[0]["clie_fechaAlta"];

			latitud = Double.Parse(data.Rows[0]["clie_latitud"].ToString());
			longitud = Double.Parse(data.Rows[0]["clie_longitud"].ToString());
			telefono = data.Rows[0]["clie_telefono"].ToString();
			docNum = data.Rows[0]["clie_doc_numero"].ToString();
			docTipo = data.Rows[0]["clie_doc_tipo"].ToString();
			categoria = data.Rows[0]["clie_categoria"].ToString();
			puntos = (Int32)data.Rows[0]["clie_puntos"];
			autoSimplex = (Boolean)data.Rows[0]["clie_autoSimplex"];

			Location coordenadas = new Location(latitud, longitud);

			return new Cliente(idCliente, nombre, apellido, domicilio, coordenadas, username, password, telefono, fechaAlta, GetCategoria(categoria), docTipo, docNum, autoSimplex, dispositivos);
		}

		private List<int> GetClientesIDOfTransformador(int idTransformador)
		{
			var lista = new List<int>();

			var query = "SELECT clie_idUsuario FROM SGE.Cliente WHERE clie_transformador = '{0}'";
			var data = Query(String.Format(query, idTransformador)).Tables[0];

			foreach (DataRow row in data.Rows)
			{
				lista.Add((Int32)row["clie_idUsuario"]);
			}

			return lista;
		}

		private List<Cliente> GetClientes(string query)
		{
			var lista = new List<Cliente>();
						
			var data = Query(query).Tables[0];
			if (data.Rows.Count == 0) return lista;

			var dispositivos = new List<Dispositivo>();
			string nombre, apellido, domicilio, username, password, categoria;
			string telefono, docNum, docTipo;
			int puntos, idCliente;
			double latitud, longitud;
			bool autoSimplex;
			DateTime fechaAlta;

			foreach (DataRow row in data.Rows)
			{
				idCliente = (Int32)row["clie_idUsuario"];
				nombre = row["usua_nombre"].ToString();
				apellido = row["usua_apellido"].ToString();
				domicilio = row["usua_domicilio"].ToString();
				username = row["usua_username"].ToString();
				password = row["usua_password"].ToString();
				fechaAlta = (DateTime)row["clie_fechaAlta"];

				latitud = Double.Parse(row["clie_latitud"].ToString());
				longitud = Double.Parse(row["clie_longitud"].ToString());
				telefono = row["clie_telefono"].ToString();
				docNum = row["clie_doc_numero"].ToString();
				docTipo = row["clie_doc_tipo"].ToString();
				categoria = row["clie_categoria"].ToString();
				puntos = (Int32)row["clie_puntos"];
				autoSimplex = (Boolean)row["clie_autoSimplex"];

				dispositivos = DAODispositivo.Instancia.FindAllFromCliente(idCliente);
				Location coordenadas = new Location(latitud, longitud);

				lista.Add(new Cliente(idCliente, nombre, apellido, domicilio, coordenadas, username, password, telefono, fechaAlta, GetCategoria(categoria), docTipo, docNum, autoSimplex, dispositivos));
			}

			return lista;
		}

		public List<Cliente> GetAllClientes()
		{
			return GetClientes("SELECT * FROM SGE.Usuario JOIN SGE.Cliente ON (usua_idUsuario = clie_idUsuario)");
		}

        public List<Cliente> GetAllMyClientes( string idtrafo)
        {
            return GetClientes("SELECT * FROM SGE.Usuario JOIN SGE.Cliente ON (usua_idUsuario = clie_idUsuario) Where clie_transformador ="+ idtrafo);
        }

        public List<Cliente> GetClientesAutoSimplex()
		{
			return GetClientes("SELECT * FROM SGE.Usuario JOIN SGE.Cliente ON (usua_idUsuario = clie_idUsuario) WHERE clie_autoSimplex = 1");			
		}


		public Dictionary<string, int> GetClientesIDUsername()
		{
			var lista = new Dictionary<string, int>();

			var query = "SELECT * FROM SGE.Usuario JOIN SGE.Cliente ON (usua_idUsuario = clie_idUsuario)";
			var data = Query(query).Tables[0];
			if (data.Rows.Count == 0) return lista;

			foreach (DataRow row in data.Rows)
			{
				lista.Add(row["usua_username"].ToString(), (Int32)row["clie_idUsuario"]);
			}

			return lista;
		}

		// ------------------------------------ INSERTS ------------------------------------

		private void GuardarUsuario(dynamic unUsuario)
		{
			var type = unUsuario.GetType();
			if (type == typeof(Administrador)) GuardarAdministrador(unUsuario);
			if (type == typeof(Cliente)) GuardarCliente(unUsuario);
		}

		private void GuardarAdministrador(Administrador admin)
		{
			if (GetIDUsuarioIfExists(admin.usuario, admin.password) != -1) return;

			var query = "INSERT INTO SGE.Usuario VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')";
			Query(String.Format(query, admin.nombre, admin.apellido, admin.domicilio, admin.usuario, admin.password));

			admin.idUsuario = GetIDUsuarioIfExists(admin.usuario, admin.password);

			query = "INSERT INTO SGE.Administrador VALUES ('{0}', CONVERT(DATETIME,'{1}',121))";
			Query(String.Format(query, admin.idUsuario, admin.AltaSistema.ToString("yyyy-MM-dd HH:mm:ss.mmm")));
		}

		private void GuardarCliente(Cliente cliente)
		{
			if (GetIDUsuarioIfExists(cliente.usuario, cliente.password) != -1) return;
			var trans = DAOzona.Instancia.AsignarTransformador(cliente);
			if (trans == -1) return;

            Location l = getlocationbyapi(cliente.domicilio);
            cliente.Coordenadas = l;

            var query = "INSERT INTO SGE.Usuario VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')";
			Query(String.Format(query, cliente.nombre, cliente.apellido, cliente.domicilio, cliente.usuario, cliente.password));

			cliente.idUsuario = GetIDUsuarioIfExists(cliente.usuario, cliente.password);

			query = "INSERT INTO SGE.Cliente VALUES ('{0}', '{1}', '{2}', '{3}', CONVERT(DATETIME,'{4}',121), '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')";
			Query(String.Format(query, cliente.idUsuario, (Int32)cliente.Coordenadas.Latitude, (Int32)cliente.Coordenadas.Longitude, cliente.Telefono, cliente.AltaServicio.ToString("yyyy-MM-dd HH:mm:ss.mmm"), cliente.Documento_numero, cliente.Documento_tipo, cliente.Categoria.IdCategoria, cliente.Puntos, trans, cliente.AutoSimplex));
		}

		// ------------------------------------ UPDATES ------------------------------------

		private void ActualizarUsuario(dynamic usuario)
		{
			var type = usuario.GetType();
			if (type == typeof(Administrador)) ActualizarAdministrador(usuario);
			if (type == typeof(Cliente)) ActualizarCliente(usuario);
		}

		private void ActualizarCliente(Cliente cliente)
		{
			if (GetIDUsuarioIfExists(cliente.usuario, cliente.password) == -1) if (!cliente.CambioContrasenia) return;

            Location l = getlocationbyapi(cliente.domicilio);
            cliente.Coordenadas = l;

            var query = "UPDATE SGE.Usuario SET usua_nombre = '{0}', usua_apellido = '{1}', usua_domicilio = '{2}', usua_password = '{3}' WHERE usua_idUsuario = '{4}'";
			Query(String.Format(query, cliente.nombre, cliente.apellido, cliente.domicilio, cliente.password, cliente.idUsuario));

			query = "UPDATE SGE.Cliente SET clie_telefono = '{0}', clie_fechaAlta = CONVERT(DATETIME,'{1}',121), clie_doc_numero = '{2}', clie_doc_tipo = '{3}', clie_categoria = '{4}', clie_puntos = '{5}', clie_transformador = '{6}', clie_latitud = '{7}', clie_longitud = '{8}', clie_autoSimplex = '{10}' WHERE clie_idUsuario = '{9}'";
			Query(String.Format(query, cliente.Telefono, cliente.AltaServicio.ToString("yyyy-MM-dd HH:mm:ss.mmm"), cliente.Documento_numero, cliente.Documento_tipo, cliente.Categoria.IdCategoria, cliente.Puntos, DAOzona.Instancia.BuscarTransformadorDeCliente(cliente.idUsuario), (Int32)cliente.Coordenadas.Latitude, (Int32)cliente.Coordenadas.Longitude, cliente.idUsuario, cliente.AutoSimplex ? 1 : 0));
		}

		private void ActualizarAdministrador(Administrador admin)
		{
			if (GetIDUsuarioIfExists(admin.usuario, admin.password) == -1) if (!admin.CambioContrasenia) return;

			var query = "UPDATE SGE.Usuario SET usua_nombre = '{0}', usua_apellido = '{1}', usua_domicilio = '{2}', usua_password = '{3}' WHERE usua_idUsuario = '{4}'";
			Query(String.Format(query, admin.nombre, admin.apellido, admin.domicilio, admin.password, admin.idUsuario));

			query = "UPDATE SGE.Administrador SET admin_fechaAlta = CONVERT(DATETIME,'{0}',121) WHERE admin_idUsuario = '{1}'";
			Query(String.Format(query, admin.AltaSistema.ToString("yyyy-MM-dd HH:mm:ss.mmm"), admin.idUsuario));
		}

		// ------------------------------------ DELETE ------------------------------------

		#endregion

		#region Dispositivo

		// ------------------------------------ SELECT ------------------------------------

		public List<Dispositivo> GetDispositivos(int idCliente)
		{
			var query = "SELECT * FROM SGE.DispositivoPorCliente JOIN SGE.DispositivoGenerico ON(dpc_idDispositivo = disp_idDispositivo) WHERE dpc_idUsuario = '{0}'";
			var data = Query(String.Format(query, idCliente)).Tables[0];
			if (data.Rows.Count == 0) return null;

			return GetDispositivosFromData(data);
		}

		public List<Dispositivo> GetAllDispositivos()
		{
			var lista = new List<Dispositivo>();

			var query = "SELECT * FROM SGE.DispositivoPorCliente JOIN SGE.DispositivoGenerico ON(dpc_idDispositivo = disp_idDispositivo) WHERE dpc_eliminado IS NULL";
			var data = Query(query).Tables[0];
			if (data.Rows.Count == 0) return lista;

			return GetDispositivosFromData(data);
		}

		private List<Dispositivo> GetDispositivosFromData(DataTable data)
		{
			var lista = new List<Dispositivo>();

			foreach (DataRow row in data.Rows)
			{
				if ((Boolean)row["disp_inteligente"] || (Boolean)row["dpc_convertido"]) lista.Add(GetInteligenteFromData(row));
				else lista.Add(GetEstandarFromData(row));
			}

			return lista;
		}

		private Inteligente GetInteligenteFromData(DataRow row)
		{
			int idD, idC, numero;
			byte estado;
			DateTime fechaEstado;
			double consumo;
			string nombre, concreto;
			bool convertido, bajoconsumo;

			idD = (Int32)row["dpc_idDispositivo"];
			idC = (Int32)row["dpc_idUsuario"];
			numero = (Int32)row["dpc_numero"];
			nombre = row["disp_dispositivo"].ToString();
			concreto = row["disp_concreto"].ToString();
			consumo = Double.Parse(row["disp_consumo"].ToString());
			estado = (Byte)row["dpc_estado"];
			fechaEstado = (DateTime)row["dpc_fechaEstado"];
			convertido = (Boolean)row["dpc_convertido"];
			bajoconsumo = (Boolean)row["disp_bajoConsumo"];

			return new Inteligente(idD, idC, numero, nombre + " " + concreto, consumo, bajoconsumo, estado, fechaEstado, convertido);
		}

		private Estandar GetEstandarFromData(DataRow row)
		{
			int idD, idC, numero;
			byte usoDiario;
			double consumo;
			string nombre, concreto;
			bool bajoconsumo;

			idD = (Int32)row["dpc_idDispositivo"];
			idC = (Int32)row["dpc_idUsuario"];
			numero = (Int32)row["dpc_numero"];
			nombre = row["disp_dispositivo"].ToString();
			concreto = row["disp_concreto"].ToString();
			consumo = Double.Parse(row["disp_consumo"].ToString());
			usoDiario = (Byte)row["dpc_usoDiario"];
			bajoconsumo = (Boolean)row["disp_bajoConsumo"];

			return new Estandar(idD, idC, numero, nombre + " " + concreto, consumo, bajoconsumo, usoDiario);
		}

		private List<Inteligente> GetDispositivosOfActuador(int idActuador)
		{
			var lista = new List<Inteligente>();

			var query = "SELECT * FROM SGE.DispositivoPorActuador WHERE dpa_idActuador = '{0}'";
			var data = Query(String.Format(query, idActuador)).Tables[0];
			if (data.Rows.Count == 0) return lista;

			int idDisp, idClie, numero;

			foreach (DataRow row in data.Rows)
			{
				idDisp = (Int32)row["dpa_dpc_idDispositivo"];
				idClie = (Int32)row["dpa_dpc_idUsuario"];
				numero = (Int32)row["dpa_dpc_numero"];

				lista.Add(DAODispositivo.Instancia.FindInteligente(idClie, idDisp, numero));
			}

			return lista;
		}

		public List<Dispositivo> GetDispositivosEliminadosEnFrom(PeriodoData periodo, int idCliente)
		{
			var lista = new List<Dispositivo>();

			var query = "SELECT * FROM SGE.DispositivoPorCliente JOIN SGE.DispositivoGenerico ON(dpc_idDispositivo = disp_idDispositivo) WHERE dpc_idUsuario = '{0}' AND dpc_eliminado BETWEEN CONVERT(DATETIME,'{1}',121) AND CONVERT(DATETIME,'{2}',121)";
			var data = Query(String.Format(query, idCliente, periodo.FechaInicio.ToString("yyyy-MM-dd HH:mm:ss.mmm"), periodo.FechaFin.ToString("yyyy-MM-dd HH:mm:ss.mmm"))).Tables[0];
			if (data.Rows.Count == 0) return lista;

			return GetDispositivosFromData(data);
		}


		// ------------------------------------ INSERTS ------------------------------------

		public void GuardarDispositivo(dynamic disp)
		{
			var type = disp.GetType();
			if (type == typeof(Inteligente)) GuardarInteligente(disp);
			if (type == typeof(Estandar)) GuardarEstandar(disp);
		}

		private void GuardarInteligente(Inteligente disp)
		{
			var query = "SELECT * FROM SGE.DispositivoPorCliente WHERE dpc_idDispositivo = '{0}' AND dpc_idUsuario = '{1}' AND dpc_numero = '{2}'";
			if (Query(String.Format(query, disp.IdDispositivo, disp.IdCliente, disp.Numero)).Tables[0].Rows.Count != 0) return;

			query = "INSERT INTO SGE.DispositivoPorCliente VALUES ('{0}', '{1}', '{2}', '{3}', CONVERT(DATETIME,'{4}',121), NULL, '{5}', NULL)";
			Query(String.Format(query, disp.IdCliente, disp.IdDispositivo, disp.Numero, disp.Estado, disp.fechaEstado.ToString("yyyy-MM-dd HH:mm:ss.mmm"), disp.Convertido ? 1 : 0));
		}

		private void GuardarEstandar(Estandar disp)
		{
			var query = "SELECT * FROM SGE.DispositivoPorCliente WHERE dpc_idDispositivo = '{0}' AND dpc_idUsuario = '{1}' AND dpc_numero = '{2}'";
			if (Query(String.Format(query, disp.IdDispositivo, disp.IdCliente, disp.Numero)).Tables[0].Rows.Count != 0) return;

			query = "INSERT INTO SGE.DispositivoPorCliente VALUES ('{0}', '{1}', '{2}', NULL, NULL,'{3}', '{4}', NULL)";
			Query(String.Format(query, disp.IdCliente, disp.IdDispositivo, disp.Numero, disp.usoDiario, 0));
		}

		// ------------------------------------ UPDATES ------------------------------------

		private void ActualizarDispositivo(dynamic disp)
		{
			var type = disp.GetType();
			if (type == typeof(Inteligente)) ActualizarInteligente(disp);
			if (type == typeof(Estandar)) ActualizarEstandar(disp);
		}

		private void ActualizarInteligente(Inteligente disp)
		{
			var query = "SELECT * FROM SGE.DispositivoPorCliente WHERE dpc_idDispositivo = '{0}' AND dpc_idUsuario = '{1}' AND dpc_numero = '{2}'";
			if (Query(String.Format(query, disp.IdDispositivo, disp.IdCliente, disp.Numero)).Tables[0].Rows.Count == 0) return;

			query = "UPDATE SGE.DispositivoPorCliente SET dpc_estado = '{0}', dpc_fechaEstado = CONVERT(DATETIME,'{1}',121), dpc_convertido = '{5}', dpc_usoDiario = NULL WHERE dpc_idDispositivo = '{2}' AND dpc_idUsuario = '{3}' AND dpc_numero = '{4}'";
			Query(String.Format(query, disp.Estado, disp.fechaEstado.ToString("yyyy-MM-dd HH:mm:ss.mmm"), disp.IdDispositivo, disp.IdCliente, disp.Numero, disp.Convertido ? 1 : 0));

		}

		private void ActualizarEstandar(Estandar disp)
		{
			var query = "SELECT * FROM SGE.DispositivoPorCliente WHERE dpc_idDispositivo = '{0}' AND dpc_idUsuario = '{1}' AND dpc_numero = '{2}'";
			if (Query(String.Format(query, disp.IdDispositivo, disp.IdCliente, disp.Numero)).Tables[0].Rows.Count == 0) return;

			query = "UPDATE SGE.DispositivoPorCliente SET dpc_usoDiario = '{0}' WHERE dpc_idDispositivo = '{1}' AND dpc_idUsuario = '{2}' AND dpc_numero = '{3}'";
			Query(String.Format(query, disp.usoDiario, disp.IdDispositivo, disp.IdCliente, disp.Numero));
		}

		// ------------------------------------ DELETE ------------------------------------

		private void EliminarDispositivo(dynamic dispositivo)
		{
			var type = dispositivo.GetType();
			if (type == typeof(Inteligente)) EliminarInteligente(dispositivo);
			if (type == typeof(Estandar)) EliminarEstandar(dispositivo);
		}

		private void EliminarInteligente(Inteligente dispositivo)
		{
			var query = "SELECT * FROM SGE.DispositivoPorCliente WHERE dpc_idDispositivo = '{0}' AND dpc_idUsuario = '{1}' AND dpc_numero = '{2}'";
			if (Query(String.Format(query, dispositivo.IdDispositivo, dispositivo.IdCliente, dispositivo.Numero)).Tables[0].Rows.Count == 0) return;

			query = "DELETE FROM SGE.DispositivoPorActuador WHERE dpa_dpc_idDispositivo = '{0}' AND dpa_dpc_idUsuario = '{1}' AND dpa_dpc_numero = '{2}'";
			Query(String.Format(query, dispositivo.IdDispositivo, dispositivo.IdCliente, dispositivo.Numero));

			query = "UPDATE SGE.DispositivoPorCliente SET dpc_eliminado = CONVERT(DATETIME,'{0}',121) WHERE dpc_idDispositivo = '{1}' AND dpc_idUsuario = '{2}' AND dpc_numero = '{3}'";
			Query(String.Format(query, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.mmm"), dispositivo.IdDispositivo, dispositivo.IdCliente, dispositivo.Numero));
		}

		private void EliminarEstandar(Estandar dispositivo)
		{
			var query = "SELECT * FROM SGE.DispositivoPorCliente WHERE dpc_idDispositivo = '{0}' AND dpc_idUsuario = '{1}' AND dpc_numero = '{2}'";
			if (Query(String.Format(query, dispositivo.IdDispositivo, dispositivo.IdCliente, dispositivo.Numero)).Tables[0].Rows.Count == 0) return;

			query = "UPDATE SGE.DispositivoPorCliente SET dpc_eliminado = CONVERT(DATETIME,'{0}',121) WHERE dpc_idDispositivo = '{1}' AND dpc_idUsuario = '{2}' AND dpc_numero = '{3}'";
			Query(String.Format(query, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.mmm"), dispositivo.IdDispositivo, dispositivo.IdCliente, dispositivo.Numero));
		}

		#endregion

		#region Categoria

		// ------------------------------------ SELECT ------------------------------------

		public Categoria GetCategoria(string idCategoria)
		{
			var query = "SELECT * FROM SGE.Categoria WHERE categ_idCategoria = '{0}'";
			var data = Query(String.Format(query, idCategoria)).Tables[0];
			if (data.Rows.Count == 0) return null;

			return GetCategoriaFromData(data.Rows[0]);
		}

		public Categoria GetCatgoriaFor(double consumo)
		{
			var query = "SELECT * FROM SGE.Categoria WHERE '{0}' BETWEEN categ_consumo_min AND categ_consumo_max";
			var data = Query(String.Format(query, consumo)).Tables[0];
			if (data.Rows.Count != 0) return null;

			return GetCategoriaFromData(data.Rows[0]);
		}

		private Categoria GetCategoriaFromData(DataRow data)
		{			
			int cmin, cmax;
			decimal carfijo, carvar;

			string id = data["categ_idCategoria"].ToString();

			cmin = (Int16)data["categ_consumo_min"];
			cmax = (Int16)data["categ_consumo_max"];
			carfijo = (Decimal)data["categ_cargoFijo"];
			carvar = (Decimal)data["categ_cargoVariable"];

			return new Categoria(id, cmin, cmax, carfijo, carvar);
		}				

		// ------------------------------------ INSERTS ------------------------------------

		private void GuardarCategoria(Categoria categoria)
		{
			var query = "SELECT * FROM SGE.Categoria WHERE categ_idCategoria = '{0}'";
			var data = Query(String.Format(query, categoria.IdCategoria)).Tables[0];
			if (data.Rows.Count != 0) return;

			query = "INSERT INTO SGE.Categoria VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')";
			Query(String.Format(query, categoria.IdCategoria, categoria.ConsumoMin, categoria.ConsumoMax, categoria.CargoFijo, categoria.CargoVariable));
		}

		// ------------------------------------ UPDATES ------------------------------------

		private void ActualizarCategoria(Categoria categoria)
		{
			var query = "SELECT * FROM SGE.Categoria WHERE categ_idCategoria = '{0}'";
			var data = Query(String.Format(query, categoria.IdCategoria)).Tables[0];
			if (data.Rows.Count == 0) return;

			query = "UPDATE SGE.Categoria SET categ_consumo_min = '{0}', categ_consumo_max = '{1}', categ_cargoFijo = '{2}', categ_cargoVariable = '{3}' WHERE categ_idCategoria = '{4}'";
			Query(String.Format(query, categoria.ConsumoMin, categoria.ConsumoMax, categoria.CargoFijo, categoria.CargoVariable, categoria.IdCategoria));
		}

		// ------------------------------------ DELETE ------------------------------------

		#endregion

		#region Transformador

		// ------------------------------------ SELECT ------------------------------------

		public Transformador GetTransformador(int idTransformador)
		{
			//Hacer
			return null;
		}

		private Transformador GetTransformadorFromData(DataRow row)
		{
			int id, zona;
			double latitud, longitud;
			bool activo;

			id = (Int32)row["trans_idTransformador"];
			activo = (Boolean)row["trans_activo"];
			latitud = Double.Parse(row["trans_latitud"].ToString());
			longitud = Double.Parse(row["trans_longitud"].ToString());
			zona = (Int32)row["trans_zona"];

			return new Transformador(id, zona, latitud, longitud, activo, GetClientesIDOfTransformador(id));
		}

		public List<Transformador> GetTransformadores(int idZona)
		{
			var lista = new List<Transformador>();

			var query = "SELECT * FROM SGE.Transformador WHERE trans_zona = '{0}'";
			var data = Query(String.Format(query, idZona)).Tables[0];
			if (data.Rows.Count == 0) return lista;

			foreach (DataRow row in data.Rows)
			{
				lista.Add(GetTransformadorFromData(row));
			}

			return lista;
		}

		// ------------------------------------ INSERTS ------------------------------------

		private void GuardarTransformadores(List<Transformador> transformadores)
		{
			foreach (Transformador t in transformadores)
			{
				GuardarTransformador(t);
			}
		}

		private void GuardarTransformador(Transformador t)
		{
			var query = "SELECT * FROM SGE.Transformador WHERE trans_zona = '{0}' AND trans_latitud = '{1}' AND trans_longitud = '{2}'";
			if (Query(String.Format(query, t.idZona, (Int32)t.location.Latitude, (Int32)t.location.Longitude)).Tables[0].Rows.Count != 0) return;

			query = "INSERT INTO SGE.Transformador VALUES ('{0}', '{1}', '{2}', '{3}')";
			Query(String.Format(query, t.EstaActivo ? 1 : 0, (Int32)t.location.Latitude, (Int32)t.location.Longitude, t.idZona));
		}

		// ------------------------------------ UPDATES ------------------------------------

		private void ActualizarTransformador(Transformador t)
		{
			var query = "SELECT * FROM SGE.Transformador WHERE trans_idTransformador = '{0}'";
			if (Query(String.Format(query, t.id)).Tables[0].Rows.Count == 0) return;

			query = "UPDATE SGE.Transformador SET trans_activo = '{0}', trans_latitud = '{1}', trans_longitud = '{2}' WHERE trans_idTransformador = '{3}' ";
			Query(String.Format(query, t.EstaActivo ? 1 : 0, (Int32)t.location.Latitude, (Int32)t.location.Longitude, t.id));
		}

		// ------------------------------------ DELETE ------------------------------------

		#endregion

		#region Zona

		// ------------------------------------ SELECT ------------------------------------

		public Zona GetZona(int idZona)
		{
			//Hacer
			return null;
		}

		public List<Zona> GetAllZonas()
		{
			var query = "SELECT * FROM SGE.Zona";
			var data = Query(query).Tables[0];
			if (data.Rows.Count == 0) return null;

			return GetZonasFromData(data);
		}

		private List<Zona> GetZonasFromData(DataTable data)
		{
			var lista = new List<Zona>();

			int id, radio;
			double latitud, longitud;

			foreach (DataRow row in data.Rows)
			{
				id = (Int32)row["zona_idZona"];
				latitud = Double.Parse(row["zona_latitud"].ToString());
				longitud = Double.Parse(row["zona_longitud"].ToString());
				radio = (Int32)row["zona_radio"];

				var transformadores = GetTransformadores(id);

				lista.Add(new Zona(id, radio, latitud, longitud, transformadores));
			}

			return lista;
		}

		// ------------------------------------ INSERTS ------------------------------------

		private void GuardarZona(Zona zona)
		{
			var query = "SELECT zona_idZona FROM SGE.Zona WHERE zona_latitud = '{0}' AND zona_longitud = '{1}' AND zona_radio = '{2}'";
			if (Query(String.Format(query, (Int32)zona.Latitude, (Int32)zona.Longitude, zona.Radio)).Tables[0].Rows.Count != 0) return;

			query = "INSERT INTO SGE.Zona VALUES ('{0}', '{1}', '{2})'";
			Query(String.Format(query, (Int32)zona.Latitude, (Int32)zona.Longitude, zona.Radio));

			if (zona.Transformadores.Count > 0)
			{
				query = "SELECT zona_idZona FROM SGE.Zona WHERE zona_latitud = '{0}' AND zona_longitud = '{1}' AND zona_radio = '{2}'";
				var idZona = (Int32)Query(String.Format(query, (Int32)zona.Latitude, (Int32)zona.Longitude, zona.Radio)).Tables[0].Rows[0][0];

				GuardarTransformadores(zona.Transformadores);
			}
		}

		// ------------------------------------ UPDATES ------------------------------------

		private void ActualizarZona(Zona zona)
		{
			var query = "SELECT zona_idZona FROM SGE.Zona WHERE zona_idZona = '{0}'";
			if (Query(String.Format(query, zona.idZona)).Tables[0].Rows.Count == 0) return;

			query = "UPDATE SGE.Zona SET zona_latitud = '{0}', zona_longitud = '{1}', zona_radio = '{2}' WHERE zona_idZona = '{2}'";
			Query(String.Format(query, (Int32)zona.Latitude, (Int32)zona.Longitude, zona.Radio, zona.idZona));
		}

		// ------------------------------------ DELETE ------------------------------------

		#endregion

		#region Sensor

		// ------------------------------------ SELECT ------------------------------------

		public Sensor GetSensor(int idCliente, string detalle)
		{
			var query = "SELECT * FROM SGE.Sensor WHERE sensor_idCliente = '{0}' AND sensor_detalle = '{1}'";
			var data = Query(String.Format(query, idCliente, detalle));

			if (data.Tables[0].Rows.Count != 1) return null;

			return GetSensorFromData(data.Tables[0].Rows[0]);			
		}

		public List<Sensor> GetAllSensores()
		{
			var query = "SELECT * FROM SGE.Sensor WHERE sensor_eliminado IS NULL";
			var data = Query(query).Tables[0];			

			return GetSensoresFromData(data);
		}

		private List<Sensor> GetSensoresFromData(DataTable data)
		{
			var lista = new List<Sensor>();
			if (data.Rows.Count == 0) return lista;

			foreach (DataRow row in data.Rows)
			{
				lista.Add(GetSensorFromData(row));
			}

			return lista;
		}

		private Sensor GetSensorFromData(DataRow row)
		{
			int id, cliente, magnitud;
			string detalle;

			id = (Int32)row["sensor_idSensor"];
			cliente = (Int32)row["sensor_idCliente"];
			magnitud = (Int32)row["sensor_magnitud"];
			detalle = row["sensor_detalle"].ToString();

			return new Sensor(id, detalle, cliente, magnitud, GetReglas(id));
		}

		// ------------------------------------ INSERTS ------------------------------------

		private void GuardarSensor(Sensor sensor)
		{
			var query = "SELECT * FROM SGE.Sensor WHERE sensor_idCliente = '{0}' AND sensor_detalle = '{1}'";
			if (Query(String.Format(query, sensor.idCliente, sensor.TipoSensor)).Tables[0].Rows.Count != 0) return;

			query = "INSERT INTO SGE.Sensor VALUES ('{0}', '{1}', '{2}', NULL)";
			Query(String.Format(query, sensor.idCliente, sensor.TipoSensor, sensor.Magnitud));

			if (sensor.Observadores.Count == 0) return;

			foreach (Regla regla in sensor.Observadores)
			{
				GuardarRegla(regla);
			}
		}

		// ------------------------------------ UPDATES ------------------------------------

		private void ActualizarSensor(Sensor sensor)
		{
			var query = "SELECT * FROM SGE.Sensor WHERE sensor_idSensor = '{0}'";
			if (Query(String.Format(query, sensor.idSensor)).Tables[0].Rows.Count == 0) return;

			query = "UPDATE SGE.Sensor SET sensor_detalle = '{0}', sensor_magnitud = '{1}' WHERE sensor_idSensor = '{2}'";
			Query(String.Format(query, sensor.TipoSensor, sensor.Magnitud, sensor.idSensor));
		}

		// ------------------------------------ DELETE ------------------------------------

		private void EliminarSensor(Sensor sensor)
		{
			var query = "SELECT * FROM SGE.Sensor WHERE sensor_idSensor = '{0}'";
			if (Query(String.Format(query, sensor.idSensor)).Tables[0].Rows.Count == 0) return;

			query = "UPDATE SGE.Sensor SET sensor_eliminado = CONVERT(DATETIME,'{0}',121) WHERE sensor_idSensor = '{1}'";
			Query(String.Format(query, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.mmm"), sensor.idSensor));
		}
		
		#endregion
		
		#region Regla

		// ------------------------------------ SELECT ------------------------------------

		public int GetReglaID(int idSensor, string detalle, int valor, string operador, string accion)
		{
			var query = "SELECT * FROM SGE.Regla WHERE regla_idSensor = '{0}' AND regla_detalle = '{1}' AND regla_valor = '{2}' AND regla_operador = '{3}' AND regla_accion = '{4}'";
			var data = Query(String.Format(query, idSensor, detalle, valor, operador, accion)).Tables[0];
			if (data.Rows.Count == 1) return (Int32)data.Rows[0]["regla_idRegla"];

			return -1;
		}		

		private List<Regla> GetReglas(int idSensor)
		{
			var lista = new List<Regla>();

			var query = "SELECT * FROM SGE.Regla WHERE regla_idSensor = '{0}'";
			var data = Query(String.Format(query, idSensor)).Tables[0];
			if (data.Rows.Count == 0) return lista;

			foreach (DataRow row in data.Rows)
			{
				lista.Add(GetReglaFromData(row));
			}

			return lista;
		}

		private Regla GetReglaFromData(DataRow row)
		{
			int id, sensor, valor;
			string detalle, operador, accion;

			detalle = row["regla_detalle"].ToString();
			operador = row["regla_operador"].ToString();
			accion = row["regla_accion"].ToString();
			id = (Int32)row["regla_idRegla"];
			sensor = (Int32)row["regla_idSensor"];
			valor = (Int32)row["regla_valor"];
			

			return new Regla(id, sensor, detalle, operador, valor, accion, GetActuadores(id));
		}

		// ------------------------------------ INSERTS ------------------------------------

		private void GuardarRegla(Regla regla)
		{
			if (GetReglaID(regla.idSensor, regla.Detalle, regla.Valor, regla.Operador, regla.Accion) != -1) return;

			var query = "INSERT INTO SGE.Regla VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')";
			Query(String.Format(query, regla.idSensor, regla.Detalle, regla.Valor, regla.Operador, regla.Accion));

			regla.idRegla = GetReglaID(regla.idSensor, regla.Detalle, regla.Valor, regla.Operador, regla.Accion);
		}

		// ------------------------------------ UPDATES ------------------------------------

		private void ActualizarRegla(Regla regla)
		{
			var query = "UPDATE SGE.Regla SET regla_detalle = '{0}', regla_valor = '{1}', regla_operador = '{2}', regla_accion = '{3}', regla_idSensor = '{4}' WHERE regla_idRegla = '{5}'";
			Query(String.Format(query, regla.Detalle, regla.Valor, regla.Operador, regla.Accion, regla.idSensor, regla.idRegla));
		}

		// ------------------------------------ DELETE ------------------------------------

		private void EliminarRegla(Regla regla)
		{
			var query = "SELECT * FROM SGE.Regla WHERE regla_idRegla = '{0}'";
			if (Query(String.Format(query, regla.idRegla)).Tables[0].Rows.Count == 0) return;

			query = "DELETE FROM SGE.ActuadorPorRegla WHERE apr_idRegla = '{0}' AND apr_idActuador = '{1}'";
			foreach (var actua in regla.Actuadores)
			{
				Query(String.Format(query, regla.idRegla, actua.IdActuador));
			}

			query = "DELETE FROM SGE.Regla WHERE regla_idRegla = '{0}'";
			Query(String.Format(query, regla.idRegla));			
		}

		#endregion

		#region Actuador

		// ------------------------------------ SELECT ------------------------------------

		private List<Actuador> GetActuadores(int idRegla)
		{
			var lista = new List<Actuador>();

			var query = "SELECT * FROM SGE.ActuadorPorRegla JOIN SGE.Actuador ON(apr_idActuador = actua_idActuador) WHERE apr_idRegla = '{0}'";
			var data = Query(String.Format(query, idRegla)).Tables[0];
			if (data.Rows.Count == 0) return lista;

			foreach (DataRow row in data.Rows)
			{
				lista.Add(GetActuadorFromData(row));
			}

			return lista;
		}

		private Actuador GetActuadorFromData(DataRow row)
		{
			int id, cliente;

			id = (Int32)row["actua_idActuador"];
			cliente = (Int32)row["actua_idCliente"];
			string detalle = row["actua_detalle"].ToString();

			var query = "SELECT * FROM SGE.ActuadorPorRegla WHERE apr_idActuador = '{0}'";
			var data = Query(String.Format(query, id)).Tables[0];
			var reglas = new List<int>();
			foreach (DataRow drow in data.Rows)
			{
				reglas.Add((Int32)row["apr_idRegla"]);
			}

			return new Actuador(id, detalle, reglas, cliente, GetDispositivosOfActuador(id));
		}

		public int GetActuadorID(int idCliente, string detalle)
		{
			var query = "SELECT * FROM SGE.Actuador WHERE actua_idCliente = '{0}' AND actua_detalle = '{1}'";
			var data = Query(String.Format(query, idCliente, detalle)).Tables[0];
			if (data.Rows.Count == 1) return (Int32)data.Rows[0]["actua_idActuador"];

			return -1;
		}

		// ------------------------------------ INSERTS ------------------------------------

		public void GuardarActuador(Actuador actua)
		{
			var query = "INSERT INTO SGE.Actuador VALUES ('{0}', '{1}')";
			if (GetActuadorID(actua.IdCliente, actua.ActuadorTipo) == -1)
			{
				Query(String.Format(query, actua.IdCliente, actua.ActuadorTipo));
				actua.IdActuador = GetActuadorID(actua.IdCliente, actua.ActuadorTipo);
			}

			query = "SELECT * FROM SGE.ActuadorPorRegla WHERE apr_idActuador = '{0}'";
			var data = Query(String.Format(query, actua.IdActuador)).Tables[0];
			var dbReglas = new List<int>();
			foreach (DataRow row in data.Rows)
			{
				dbReglas.Add((Int32)row["apr_idRegla"]);
			}

			foreach (int regla in actua.Reglas)
			{
				if (!dbReglas.Contains(regla))
				{
					query = "INSERT INTO SGE.ActuadorPorRegla VALUES ('{0}', '{1}')";
					Query(String.Format(query, regla, actua.IdActuador));
				}
			}

			if (actua.Dispositivos.Count == 0) return;

			foreach (Inteligente di in actua.Dispositivos)
			{
				query = "SELECT * FROM SGE.DispositivoPorActuador WHERE dpa_idActuador = '{0}' AND dpa_dpc_idUsuario = '{1}' AND dpa_dpc_idDispositivo = '{2}' AND dpa_dpc_numero = '{3}'";
				if (Query(String.Format(query, actua.IdActuador, di.IdCliente, di.IdDispositivo, di.Numero)).Tables[0].Rows.Count == 0)
				{
					query = "INSERT INTO SGE.DispositivoPorActuador VALUES ('{0}', '{1}', '{2}', '{3}')";
					Query(String.Format(query, actua.IdActuador, di.IdCliente, di.IdDispositivo, di.Numero));
				}
			}
		}

		// ------------------------------------ UPDATES ------------------------------------

		private void ActualizarActuador(Actuador actua)
		{
			var query = "SELECT * FROM SGE.Actuador WHERE actua_idActuador = '{0}'";
			if (Query(String.Format(query, actua.IdActuador)).Tables[0].Rows.Count == 0) return;

			query = "UPDATE SGE.Actuador SET actua_detalle = '{0}' WHERE actua_idActuador = '{1}'";
			Query(String.Format(query, actua.ActuadorTipo, actua.IdActuador));

			query = "SELECT * FROM SGE.ActuadorPorRegla WHERE apr_idActuador = '{0}'";
			var data = Query(String.Format(query, actua.IdActuador)).Tables[0];
			var dbReglas = new List<int>();
			foreach (DataRow row in data.Rows)
			{
				dbReglas.Add((Int32)row["apr_idRegla"]);
			}

			var nuevas = actua.Reglas.FindAll(x => !dbReglas.Contains(x));
			var quitar = dbReglas.FindAll(x => !actua.Reglas.Contains(x));

			foreach (int regla in nuevas)
			{
				query = "INSERT INTO SGE.ActuadorPorRegla VALUES ('{0}', '{1}')";
				Query(String.Format(query, regla, actua.IdActuador));
			}

			query = "DELETE FROM SGE.ActuadorPorRegla WHERE apr_idRegla = '{0}' AND apr_idActuador = '{1}'";
			foreach (int regla in quitar)
			{				
				Query(String.Format(query, regla, actua.IdActuador));
			}

			query = "SELECT * FROM SGE.DispositivoPorActuador WHERE dpa_idActuador = '{0}'";
			data = Query(String.Format(query, actua.IdActuador)).Tables[0];
			var dbDispoPorActua = new List<(int idC, int idD, int num)>();
			foreach (DataRow row in data.Rows)
			{
				dbDispoPorActua.Add(((Int32)row["dpa_dpc_idUsuario"], (Int32)row["dpa_dpc_idDispositivo"], (Int32)row["dpa_dpc_numero"]));
			}

			var nuevosDisp = actua.Dispositivos.FindAll(x => !dbDispoPorActua.Contains((x.IdCliente, x.IdDispositivo, x.Numero)));
			var quitarDisp = dbDispoPorActua.FindAll(x => !actua.Dispositivos.Exists(z => z.IdCliente == x.idC && z.IdDispositivo == x.idD && z.Numero == x.num));

			query = "INSERT INTO SGE.DispositivoPorActuador VALUES ('{0}', '{1}', '{2}', '{3}')";
			foreach (Inteligente di in nuevosDisp)
			{				
				Query(String.Format(query, actua.IdActuador, di.IdCliente, di.IdDispositivo, di.Numero));
			}

			query = "DELETE FROM SGE.DispositivoPorActuador WHERE dpa_idActuador = '{0}' AND dpa_dpc_idUsuario = '{1}' AND dpa_dpc_idDispositivo = '{2}' AND dpa_dpc_numero = '{3}'";
			foreach (var (idC, idD, num) in quitarDisp)
			{				
				Query(String.Format(query, actua.IdActuador, idC, idD, num));
			}						
		}

		// ------------------------------------ DELETE ------------------------------------

		private void EliminarActuador(Actuador actuador)
		{
			var query = "SELECT * FROM SGE.Actuador WHERE actua_idActuador = '{0}'";
			if (Query(String.Format(query, actuador.IdActuador)).Tables[0].Rows.Count == 0) return;

			query = "DELETE FROM SGE.DispositivoPorActuador WHERE dpa_idActuador = '{0}' AND dpa_dpc_idUsuario = '{1}' AND dpa_dpc_idDispositivo = '{2}' AND dpa_dpc_numero = '{3}'";
			foreach (var dispo in actuador.Dispositivos)
			{				
				Query(String.Format(query, actuador.IdActuador, dispo.IdCliente, dispo.IdDispositivo, dispo.Numero));
			}

			query = "DELETE FROM SGE.ActuadorPorRegla WHERE apr_idActuador = '{0}'";
			Query(String.Format(query, actuador.IdActuador));

			query = "DELETE FROM SGE.Actuador WHERE actua_idActuador = '{0}'";
			Query(String.Format(query, actuador.IdActuador));
		}

		#endregion

		#region EstadoDispositivo

		// ------------------------------------ SELECT ------------------------------------

		public List<EstadoDispositivo> GetEstadosEntre(int idC, int idD, int numero, DateTime desde, DateTime hasta)
		{
			var lista = new List<EstadoDispositivo>();

			var query = "SELECT * FROM SGE.EstadoDispositivo WHERE (edisp_idUsuario = '{0}' AND edisp_idDispositivo = '{1}' AND edisp_numero = '{2}') AND NOT (edisp_fechaFin <= CONVERT(datetime, '{3}', 121) OR edisp_fechaInicio >= CONVERT(datetime, '{4}', 121)) ";
			var data = Query(String.Format(query, idC, idD, numero, desde.ToString("yyyy-MM-dd HH:mm:ss.mmm"), hasta.ToString("yyyy-MM-dd HH:mm:ss.mmm"))).Tables[0];
			if (data.Rows.Count == 0) return lista;

			foreach (DataRow row in data.Rows)
			{
				lista.Add(GetEstadoFromData(row));
			}

			return lista;
		}

		private EstadoDispositivo GetEstadoFromData(DataRow row)
		{
			int idC, idD, numero;
			DateTime fInicio, fFin;

			byte estado = (Byte)row["edisp_estado"];
			idC = (Int32)row["edisp_idUsuario"];
			idD = (Int32)row["edisp_idDispositivo"];
			numero = (Int32)row["edisp_numero"];
			fInicio = (DateTime)row["edisp_fechaInicio"];
			fFin = (DateTime)row["edisp_fechaFin"];

			return new EstadoDispositivo(idC, idD, numero, estado, fInicio, fFin);
		}

		// ------------------------------------ INSERTS ------------------------------------

		private void GuardarEstado(EstadoDispositivo eg)
		{
			var query = "INSERT INTO SGE.EstadoDispositivo VALUES ('{0}', '{1}', '{2}', CONVERT(datetime, '{3}', 121), CONVERT(datetime, '{4}', 121), '{5}')";
			Query(String.Format(query, eg.Usuario, eg.Dispositivo, eg.DispNumero, eg.FechaInicio.ToString("yyyy-MM-dd HH:mm:ss.mmm"), eg.FechaFin.ToString("yyyy-MM-dd HH:mm:ss.mmm"), eg.Estado));
		}

		// ------------------------------------ UPDATES ------------------------------------
		// No se actualizan los Estados de los Dispositivos
		// ------------------------------------ DELETE ------------------------------------

		#endregion

		#region EstadoSensor

		// ------------------------------------ SELECT ------------------------------------

		public List<EstadoSensor> GetAllEstadoSensorFromSensor(int idSensor)
		{
			var lista = new List<EstadoSensor>();

			var query = "SELECT * FROM SGE.EstadoSensor WHERE esensor_idSensor = '{0} ORDER BY esensor_idEstadoSensor DESC'";
			var data = Query(String.Format(query, idSensor)).Tables[0];
			if (data.Rows.Count == 0) return lista;

			foreach (DataRow row in data.Rows)
			{
				lista.Add(new EstadoSensor((Int32)row["esensor_idEstadoSensor"], idSensor, (Int32)row["esensor_magnitud"]));
			}

			return lista;
		}

		// ------------------------------------ INSERTS ------------------------------------

		private void GuardarEstadoSensor(EstadoSensor estado)
		{
			var query = "INSERT INTO SGE.EstadoSensor VALUES ('{0}', '{1}')";
			Query(String.Format(query, estado.IdSensor, estado.Magnitud));
		}

		// ------------------------------------ UPDATES ------------------------------------
		// No se actualizan los Estados de los Sensores
		// ------------------------------------ DELETE ------------------------------------

		#endregion

		#region TemplateDispositivo

		// ------------------------------------ SELECT ------------------------------------

		public List<DispositivoGenerico> GetAllTemplates()
		{
			var lista = new List<DispositivoGenerico>();

			var query = "SELECT * FROM SGE.DispositivoGenerico";
			var data = Query(query).Tables[0];
			if (data.Rows.Count == 0) return lista;

			foreach (DataRow row in data.Rows)
			{
				lista.Add(GetTemplateFromData(row));
			}

			return lista;
		}

		private DispositivoGenerico GetTemplateFromData(DataRow row)
		{
			int id = (Int32)row["disp_idDispositivo"];

			string nombre, concreto;
			bool inteligente, bajoconsumo;
			double consumo;

			nombre = row["disp_dispositivo"].ToString();
			concreto = row["disp_concreto"].ToString();
			inteligente = (Boolean)row["disp_inteligente"];
			bajoconsumo = (Boolean)row["disp_bajoConsumo"];
			consumo = Double.Parse(row["disp_consumo"].ToString());

			return new DispositivoGenerico(id, nombre, concreto, inteligente, bajoconsumo, consumo);
		}

		// ------------------------------------ INSERTS ------------------------------------

		public bool ExisteTemplate(DispositivoGenerico tdisp)
		{
			var query = "SELECT * FROM SGE.DispositivoGenerico WHERE disp_dispositivo = '{0}' AND disp_concreto = '{1}' AND disp_inteligente = '{2}' AND disp_bajoConsumo = '{3}'";
			return (Query(String.Format(query, tdisp.Dispositivo, tdisp.Concreto, tdisp.Inteligente ? 1 : 0, tdisp.Bajoconsumo ? 1 : 0)).Tables[0].Rows.Count != 0);			
		}

		private void GuardarTemplate(DispositivoGenerico tdisp)
		{
			var query = "SELECT * FROM SGE.DispositivoGenerico WHERE disp_dispositivo = '{0}' AND disp_concreto = '{1}' AND disp_inteligente = '{2}' AND disp_bajoConsumo = '{3}'";
			if (Query(String.Format(query, tdisp.Dispositivo, tdisp.Concreto, tdisp.Inteligente ? 1 : 0, tdisp.Bajoconsumo ? 1 : 0)).Tables[0].Rows.Count != 0) return;

			query = "INSERT INTO SGE.DispositivoGenerico VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')";
			Query(String.Format(query, tdisp.Dispositivo, tdisp.Concreto, tdisp.Inteligente ? 1 : 0, tdisp.Bajoconsumo ? 1 : 0, tdisp.Consumo));
		}

		// ------------------------------------ UPDATES ------------------------------------

		private void ActualizarTemplate(DispositivoGenerico tdisp)
		{
			var query = "SELECT * FROM SGE.DispositivoGenerico WHERE disp_idDispositivo = '{0}'";
			if (Query(String.Format(query, tdisp.ID)).Tables[0].Rows.Count != 0) return;

			query = "UPDATE SGE.DispositivoGenerico SET disp_dispositivo = '{0}', disp_concreto = '{1}', disp_inteligente = '{2}', disp_bajoConsumo = '{3}', disp_consumo = '{4}' WHERE disp_idDispositivo = '{5}'";
			Query(String.Format(query, tdisp.Dispositivo, tdisp.Concreto, tdisp.Inteligente ? 1 : 0, tdisp.Bajoconsumo ? 1 : 0, tdisp.Consumo, tdisp.ID));
		}

		// ------------------------------------ DELETE ------------------------------------

		#endregion
		
		public int GetIDUsuarioIfExists(string username, string password)
		{			
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				var query = "SELECT usua_idUsuario FROM SGE.Usuario WHERE usua_username = @user AND usua_password = @pass";
				
				using (SqlCommand command = new SqlCommand(query, connection))
				{					
					command.Parameters.Add(new SqlParameter("user", username));
					command.Parameters.Add(new SqlParameter("pass", password));

					SqlDataReader reader = command.ExecuteReader();
					if (reader.HasRows) {
						reader.Read();
						return reader.GetInt32(0);
					}					
				}
			}

			return -1;
		}

		public DataSet Query(string q)
		{			
			var conn = new SqlConnection();
			conn.ConnectionString = connectionString;
			var dataAdapter = new SqlDataAdapter(q, conn);			
			var commandBuilder = new SqlCommandBuilder(dataAdapter);
			var ds = new DataSet();
			dataAdapter.Fill(ds);
			return ds;			
		}

        #region Reportes
        private string Rename(string i) { if (i == "0") return "Estandar"; else return "Inteligente"; }
        /*
        public List<KeyValuePair<string, double>> GetLastPeriodoDispositivoIE2()
            // retorna una lista donde string es inteligente o estandar y el consumo toral
        {
            var query = "SELECT disp_inteligente, SUM(dpc_usoDiario*disp_consumo) FROM SGE.DispositivoPorCliente JOIN SGE.DispositivoGenerico ON(dpc_idDispositivo = disp_idDispositivo)  group by disp_inteligente";
            var data = Query(query).Tables[0];
            if (data.Rows.Count == 0) return null;
            List<KeyValuePair<string, double>> datalist = new List<KeyValuePair<string, double>>();
            foreach (DataRow dr in data.Rows)
            {
                datalist.Add(new KeyValuePair<string, double>(Rename(dr[0].ToString()),dr.[2]));
            }
            return datalist;
        }
		*/
        public List<KeyValuePair<string, double>> GetLastPeriodoDispositivoIE()
        // retorna una lista donde string es inteligente o estandar y el consumo toral
        {
            var dispos = GetAllDispositivos();
            double consumoInteligente = 0;
            double consumoEstandar = 0;

            List<KeyValuePair<string, double>> datalist = new List<KeyValuePair<string, double>>();
            foreach (var dip in dispos)
            {
                if (dip.GetType() == typeof(Inteligente)) consumoInteligente = consumoInteligente + dip.Consumo;
                if (dip.GetType() == typeof(Estandar)) consumoEstandar = consumoEstandar + dip.Consumo;
                
            }
            datalist.Add(new KeyValuePair<string, double>("Inteligente", consumoInteligente));
            datalist.Add(new KeyValuePair<string, double>("Estandar", consumoEstandar));
            return datalist;
        }

        public List<KeyValuePair<string, double>> GetConsumoPorTranformador()
        {
            var query = "SELECT trans_idTransformador,SUM(dpc_usoDiario*disp_consumo) FROM SGE.DispositivoPorCliente JOIN SGE.DispositivoGenerico ON(dpc_idDispositivo = disp_idDispositivo) JOIN SGE.Cliente on (clie_idUsuario = dpc_idUsuario) JOIN SGE.Transformador on trans_idTransformador = clie_transformador group by trans_idTransformador";
            var data = Query(query).Tables[0];
            if (data.Rows.Count == 0) return null;
            List<KeyValuePair<string, double>> datalist = new List<KeyValuePair<string, double>>();
            foreach (DataRow dr in data.Rows)
            {
                datalist.Add(new KeyValuePair<string, double>(Rename(dr[0].ToString()), (double)dr[2]));
            }
            return datalist;
        }


        public List<KeyValuePair<string, double>> GetPeriodoDispositivoPorUser()
        // retorna una lista donde string es idusuario y el int el consumo toral
        {
            var dispos = GetAllDispositivos();
            double consumo = 0;

            List<KeyValuePair<string, double>> datalist = new List<KeyValuePair<string, double>>();
           
                //dispos.GroupBy
                
                foreach (var dip in dispos)
            {
                { 
                // ToDo: hacer que esta mierda agrupe por idcliente
                
                    consumo = consumo + dip.Consumo;
                    datalist.Add(new KeyValuePair<string, double>(dip.IdCliente.ToString(), consumo));
                }
            }
                

        
            return datalist;
        }

        #endregion

        public Location getlocationbyapi(string direccion)
        {
             string url = "http://www.mapquestapi.com/geocoding/v1/address?key=bVQyl1XhrO9qxrh15EKSunKM4ipFRDq3&location=";
            string pais = ",Argentina";

            WebRequest request = WebRequest.Create(url + direccion+ pais);
            WebResponse response = request.GetResponse();
            Stream datastream = response.GetResponseStream();
            StreamReader lector = new StreamReader(datastream);
            string str = lector.ReadToEnd();

            RootObject p = new RootObject();
            p = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(str);

            LatLng latLng = p.results.First().locations.First().latLng;

            return new Location(latLng.lat,latLng.lng);
        }

    }

    public class Copyright
    {
        public string text { get; set; }
        public string imageUrl { get; set; }
        public string imageAltText { get; set; }
    }

    public class Info
    {
        public int statuscode { get; set; }
        public Copyright copyright { get; set; }
        public List<object> messages { get; set; }
    }

    public class Options
    {
        public int maxResults { get; set; }
        public bool thumbMaps { get; set; }
        public bool ignoreLatLngInput { get; set; }
    }

    public class ProvidedLocation
    {
        public string location { get; set; }
    }

    public class LatLng
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class DisplayLatLng
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Location2
    {
        public string street { get; set; }
        public string adminArea6 { get; set; }
        public string adminArea6Type { get; set; }
        public string adminArea5 { get; set; }
        public string adminArea5Type { get; set; }
        public string adminArea4 { get; set; }
        public string adminArea4Type { get; set; }
        public string adminArea3 { get; set; }
        public string adminArea3Type { get; set; }
        public string adminArea1 { get; set; }
        public string adminArea1Type { get; set; }
        public string postalCode { get; set; }
        public string geocodeQualityCode { get; set; }
        public string geocodeQuality { get; set; }
        public bool dragPoint { get; set; }
        public string sideOfStreet { get; set; }
        public string linkId { get; set; }
        public string unknownInput { get; set; }
        public string type { get; set; }
        public LatLng latLng { get; set; }
        public DisplayLatLng displayLatLng { get; set; }
        public string mapUrl { get; set; }
    }

    public class Result
    {
        public ProvidedLocation providedLocation { get; set; }
        public List<Location2> locations { get; set; }
    }

    public class RootObject
    {
        public Info info { get; set; }
        public Options options { get; set; }
        public List<Result> results { get; set; }
    }
}

