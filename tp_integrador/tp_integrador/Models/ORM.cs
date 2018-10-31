using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
			//if (type == typeof(Sensor)) GuardarSensor(unaClase);
			//if (type == typeof(Actuador)) GuardarActuador(unaClase);
			//if (type == typeof(Regla)) GuardarRegla(unaClase);
			if (type == typeof(EstadoGuardado)) GuardarEstado(unaClase);
			//if (type == typeof(TemplateDispositivo)) GuardarTemplate(unaClase);
		}

		public void Update(dynamic unaClase)
		{

		}

		public void Delete(dynamic unaClase)
		{

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
			DateTime fechaAlta;

			nombre = userData["usua_nombre"].ToString();
			apellido = userData["usua_apellido"].ToString();
			domicilio = userData["usua_domicilio"].ToString();
			username = userData["usua_username"].ToString();
			password = userData["usua_password"].ToString();
			fechaAlta = (DateTime)data.Rows[0]["clie_fechaAlta"];

			telefono = data.Rows[0]["clie_telefono"].ToString();
			docNum = data.Rows[0]["clie_doc_numero"].ToString();
			docTipo = data.Rows[0]["clie_doc_tipo"].ToString();
			categoria = data.Rows[0]["clie_categoria"].ToString();
			puntos = (Int32)data.Rows[0]["clie_puntos"];

			return new Cliente(idCliente, nombre, apellido, domicilio, username, password, telefono, fechaAlta, GetCategoria(categoria), docTipo, docNum, dispositivos);
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
			Query(String.Format(query, admin.nombre, admin.apellido, admin.domicilio, admin.usuario, HashThis.Instancia.GetHash(admin.password)));

			admin.idUsuario = GetIDUsuarioIfExists(admin.usuario, admin.password);

			query = "INSERT INTO SGE.Administrador VALUES ('{0}', CONVERT(DATETIME,'{1}',121))";
			Query(String.Format(query, admin.idUsuario, admin.AltaSistema.ToString("yyyy-MM-dd HH:mm:ss.mmm")));
		}

		private void GuardarCliente(Cliente cliente)
		{
			if (GetIDUsuarioIfExists(cliente.usuario, cliente.password) != -1) return;

			var query = "INSERT INTO SGE.Usuario VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')";
			Query(String.Format(query, cliente.nombre, cliente.apellido, cliente.domicilio, cliente.usuario, HashThis.Instancia.GetHash(cliente.password)));

			cliente.idUsuario = GetIDUsuarioIfExists(cliente.usuario, cliente.password);
			
			query = "INSERT INTO SGE.Cliente VALUES ('{0}', '{1}', CONVERT(DATETIME,'{2}',121), '{3}', '{3}', '{5}', '{6}', '{7}')";
			Query(String.Format(query, cliente.idUsuario, cliente.Telefono, cliente.AltaServicio.ToString("yyyy-MM-dd HH:mm:ss.mmm"), cliente.Documento_numero, cliente.Documento_tipo, cliente.Categoria.IdCategoria, cliente.Puntos, DAOzona.Instancia.AsignarTransformador(cliente)));
		}

		// ------------------------------------ UPDATES ------------------------------------
		// ------------------------------------ DELETE ------------------------------------

		#endregion

		#region Dispositivo

		// ------------------------------------ SELECT ------------------------------------

		public List<Dispositivo> GetDispositivos(int idCliente)
		{
			var query = "SELECT * FROM SGE.DispositivoPorCliente JOIN SGE.DispositivoGenerico ON(dpc_idDispositivo = disp_idDispositivo) WHERE dpc_idCliente == '{0}'";
			var data = Query(String.Format(query, idCliente)).Tables[0];
			if (data.Rows.Count == 0) return null;

			return GetDispositivosFromData(data);
		}

		public List<Dispositivo> GetAllDispositivos()
		{
			var query = "SELECT * FROM SGE.DispositivoPorCliente JOIN SGE.DispositivoGenerico ON(dpc_idDispositivo = disp_idDispositivo)";
			var data = Query(query).Tables[0];
			if (data.Rows.Count == 0) return null;

			return GetDispositivosFromData(data);
		}

		private List<Dispositivo> GetDispositivosFromData(DataTable data)
		{
			var lista = new List<Dispositivo>();

			foreach (DataRow row in data.Rows)
			{
				if ((Boolean)row["disp_inteligente"]) lista.Add(GetInteligenteFromData(row));
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

			idD = (Int32)row["dpc_idDispositivo"];
			idC = (Int32)row["dpc_idUsuario"];
			numero = (Int32)row["dpc_numero"];
			nombre = row["disp_dispositivo"].ToString();
			concreto = row["disp_concreto"].ToString();
			consumo = Double.Parse(row["disp_consumo"].ToString());
			estado = (Byte)row["dpc_estado"];
			fechaEstado = (DateTime)row["dpc_fechaEstado"];

			return new Inteligente(idD, idC, numero, nombre + " " + concreto, consumo, estado, fechaEstado);
		}

		private Estandar GetEstandarFromData(DataRow row)
		{
			int idD, idC, numero;
			byte usoDiario;
			double consumo;
			string nombre, concreto;

			idD = (Int32)row["dpc_idDispositivo"];
			idC = (Int32)row["dpc_idUsuario"];
			numero = (Int32)row["dpc_numero"];
			nombre = row["disp_dispositivo"].ToString();
			concreto = row["disp_concreto"].ToString();
			consumo = Double.Parse(row["disp_consumo"].ToString());
			usoDiario = (Byte)row["dpc_usoDiario"];

			return new Estandar(idD, idC, numero, nombre + " " + concreto, consumo, usoDiario);
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

		// ------------------------------------ INSERTS ------------------------------------

		public void GuardarDispositivo(dynamic disp)
		{
			var type = disp.GetType();
			if (type == typeof(Inteligente)) GuardarInteligente(disp);
			if (type == typeof(Estandar)) GuardarEstandar(disp);
		}

		private void GuardarInteligente(Inteligente disp)
		{
			var query = "SELECT * FROM SGE.DispositivoPorCliente WHERE dpc_idDispositivo = '{0}', dpc_idUsuario = '{0}', dpc_numero = '{0}'";
			if (Query(String.Format(query, disp.IdDispositivo, disp.IdCliente, disp.Numero)).Tables[0].Rows.Count != 0) return;

			query = "INSERT INTO SGE.DispositivoPorCliente VALUES ('{0}', '{1}', '{2}', '{3}', CONVERT(DATETIME,'{4}',121),'{5}')";
			Query(String.Format(query, disp.IdCliente, disp.IdDispositivo, disp.Numero, disp.Estado, disp.fechaEstado.ToString("yyyy-MM-dd HH:mm:ss.mmm"), DBNull.Value));
		}

		private void GuardarEstandar(Estandar disp)
		{
			var query = "SELECT * FROM SGE.DispositivoPorCliente WHERE dpc_idDispositivo = '{0}', dpc_idUsuario = '{0}', dpc_numero = '{0}'";
			if (Query(String.Format(query, disp.IdDispositivo, disp.IdCliente, disp.Numero)).Tables[0].Rows.Count != 0) return;

			query = "INSERT INTO SGE.DispositivoPorCliente VALUES ('{0}', '{1}', '{2}', '{3}', '{4}','{5}')";
			Query(String.Format(query, disp.IdCliente, disp.IdDispositivo, disp.Numero, DBNull.Value, DBNull.Value, disp.usoDiario));
		}

		// ------------------------------------ UPDATES ------------------------------------
		// ------------------------------------ DELETE ------------------------------------

		#endregion

		#region Categoria

		// ------------------------------------ SELECT ------------------------------------

		public Categoria GetCategoria(string idCategoria)
		{
			var query = "SELECT * FROM SGE.Categoria WHERE categ_idCategoria = '{0}'";
			var data = Query(String.Format(query, idCategoria)).Tables[0];
			if (data.Rows.Count == 0) return null;

			byte cmin, cmax;
			decimal carfijo, carvar;

			cmin = (Byte)data.Rows[0]["categ_consumo_min"];
			cmax = (Byte)data.Rows[0]["categ_consumo_max"];
			carfijo = (Decimal)data.Rows[0]["categ_cargoFijo"];
			carvar = (Decimal)data.Rows[0]["categ_cargoVariable"];

			return new Categoria(idCategoria, cmin, cmax, carfijo, carvar);
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
						
			return new Transformador(id, latitud, longitud, activo, GetClientesIDOfTransformador(id));
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

		private void GuardarTransformadores(int idZona, List<Transformador> transformadores)
		{
			var query = "INSERT INTO SGE.Transformador VALUES ('{0}', '{1}', '{2}', '{3}')";
			foreach (Transformador t in transformadores)
			{
				Query(String.Format(query, t.EstaActivo ? 1 : 0, t.location.Latitude, t.location.Longitude, idZona));
			}
		}

		// ------------------------------------ UPDATES ------------------------------------
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
			if (Query(String.Format(query, zona.Latitude, zona.Longitude, zona.Radio)).Tables[0].Rows.Count != 0) return;

			query = "INSERT INTO SGE.Zona VALUES ('{0}', '{1}', '{2})'";
			Query(String.Format(query, zona.Latitude, zona.Longitude, zona.Radio));

			if (zona.Transformadores.Count > 0)
			{
				query = "SELECT zona_idZona FROM SGE.Zona WHERE zona_latitud = '{0}' AND zona_longitud = '{1}' AND zona_radio = '{2}'";
				var idZona = (Int32)Query(String.Format(query, zona.Latitude, zona.Longitude, zona.Radio)).Tables[0].Rows[0][0];

				GuardarTransformadores(idZona, zona.Transformadores);
			}
		}

		// ------------------------------------ UPDATES ------------------------------------
		// ------------------------------------ DELETE ------------------------------------

		#endregion

		#region Sensor

		// ------------------------------------ SELECT ------------------------------------

		public List<Sensor> GetAllSensores()
		{
			var query = "SELECT * FROM SGE.Sensor";
			var data = Query(query).Tables[0];
			if (data.Rows.Count == 0) return null;

			return GetSensoresFromData(data);
		}

		private List<Sensor> GetSensoresFromData(DataTable data)
		{
			var lista = new List<Sensor>();

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
		// ------------------------------------ UPDATES ------------------------------------
		// ------------------------------------ DELETE ------------------------------------

		#endregion

		#region Regla

		// ------------------------------------ SELECT ------------------------------------

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

			id = (Int32)row["regla_idRegla"];
			sensor = (Int32)row["regla_idSensor"];
			valor = (Int32)row["regla_valor"];

			return new Regla(id, sensor, valor, GetActuadores(id));
		}

		// ------------------------------------ INSERTS ------------------------------------
		// ------------------------------------ UPDATES ------------------------------------
		// ------------------------------------ DELETE ------------------------------------

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
			int id = (Int32)row["actua_idActuador"];
			string detalle = row["actua_detalle"].ToString();

			return new Actuador(id, detalle, GetDispositivosOfActuador(id));
		}

		// ------------------------------------ INSERTS ------------------------------------
		// ------------------------------------ UPDATES ------------------------------------
		// ------------------------------------ DELETE ------------------------------------
		#endregion

		#region EstadoGuardado

		// ------------------------------------ SELECT ------------------------------------
		public List<EstadoGuardado> GetEstadosEntre(int idC, int idD, int numero, DateTime desde, DateTime hasta)
		{
			var lista = new List<EstadoGuardado>();

			var query = "SELECT * FROM SGE.EstadoDispositivo WHERE (edisp_idUsuario = '{0}' AND edisp_idDispositivo = '{1}' AND edisp_numero = '{2}') AND NOT (edisp_fechaFin <= CONVERT(datetime, '{3}', 121) OR edisp_fechaInicio >= CONVERT(datetime, '{4}', 121)) ";
			var data = Query(String.Format(query, idC, idD, numero, desde.ToString("yyyy-MM-dd HH:mm:ss.mmm"), hasta.ToString("yyyy-MM-dd HH:mm:ss.mmm"))).Tables[0];
			if (data.Rows.Count == 0) return lista;

			foreach (DataRow row in data.Rows)
			{
				lista.Add(GetEstadoFromData(row));
			}

			return lista;
		}

		private EstadoGuardado GetEstadoFromData(DataRow row)
		{
			int idC, idD, numero;
			DateTime fInicio, fFin;

			byte estado = (Byte)row["edisp_estado"];
			idC = (Int32)row["edisp_idUsuario"];
			idD = (Int32)row["edisp_idDispositivo"];
			numero = (Int32)row["edisp_numero"];
			fInicio = (DateTime)row["edisp_fechaInicio"];
			fFin = (DateTime)row["edisp_fechaFin"];

			return new EstadoGuardado(idC, idD, numero, estado, fInicio, fFin);
		}

		// ------------------------------------ INSERTS ------------------------------------

		private void GuardarEstado(EstadoGuardado eg)
		{
			var query = "INSERT INTO SGE.EstadoDispositivo VALUES ('{0}', '{1}', '{2}', CONVERT(datetime, '{3}', 121), CONVERT(datetime, '{4}', 121), '{5}')";
			Query(String.Format(query, eg.Usuario, eg.Dispositivo, eg.DispNumero, eg.FechaInicio, eg.FechaFin, eg.Estado));
		}
		
		// ------------------------------------ UPDATES ------------------------------------
		// ------------------------------------ DELETE ------------------------------------
		#endregion
		

		public int GetIDUsuarioIfExists(string username, string password)
		{			
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				var query = "SELECT usua_idUsuario FROM SGE.Usuario WHERE usua_username = @user AND usua_password = @pass";
				password = HashThis.Instancia.GetHash(password);

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


	}


}

