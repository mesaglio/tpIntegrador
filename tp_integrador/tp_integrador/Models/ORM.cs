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
			//if (type == typeof(Transformador)) GuardarTransformador(unaClase);
			//if (type == typeof(Zona)) GuardarZona(unaClase);
			if (type == typeof(Categoria)) GuardarCategoria(unaClase);
			//if (type == typeof(Sensor)) GuardarSensor(unaClase);
			//if (type == typeof(Actuador)) GuardarActuador(unaClase);
			//if (type == typeof(Regla)) GuardarRegla(unaClase);
			//if (type == typeof(EstadoGuardado)) GuardarEstado(unaClase);

		}

		public void Update(dynamic unaClase)
		{

		}

		public void Delete(dynamic unaClase)
		{

		}

		#region Usuario

		// ------------------------------------ SELECTS ------------------------------------

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
			fechaAlta = DateTime.Parse(data.Rows[0]["admin_fechaAlta"].ToString());

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
			fechaAlta = DateTime.Parse(data.Rows[0]["clie_fechaAlta"].ToString());

			telefono = data.Rows[0]["clie_telefono"].ToString();
			docNum = data.Rows[0]["clie_doc_numero"].ToString();
			docTipo = data.Rows[0]["clie_doc_tipo"].ToString();
			categoria = data.Rows[0]["clie_categoria"].ToString();
			puntos = Int32.Parse(data.Rows[0]["clie_puntos"].ToString());

			return new Cliente(idCliente, nombre, apellido, domicilio, username, password, telefono, fechaAlta, GetCategoria(categoria), docTipo, docNum, dispositivos);
		}

		private List<int> GetClientesIDOfTransformador(int idTransformador)
		{
			var lista = new List<int>();

			var query = "SELECT clie_idUsuario FROM SGE.Cliente WHERE clie_transformador = '{0}'";
			var data = Query(String.Format(query, idTransformador)).Tables[0];

			foreach (DataRow row in data.Rows)
			{
				lista.Add(Int32.Parse(row["clie_idUsuario"].ToString()));
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
			if (admin.idUsuario != 0) return;

			var query = "INSERT INTO SGE.Usuario ('{0}', '{1}', '{2}', '{3}', '{4}')";
			Query(String.Format(query, admin.nombre, admin.apellido, admin.domicilio, admin.usuario, HashThis.Instancia.GetHash(admin.password)));

			var idAsignado = GetIDUsuarioIfExists(admin.usuario, admin.password);
			if (idAsignado == -1) return;

			query = "INSERT INTO SGE.Administrador ('{}', '{}')";
			Query(String.Format(query, idAsignado, admin.AltaSistema));
		}

		private void GuardarCliente(Cliente cliente)
		{
			if (cliente.idUsuario != 0) return;

			var query = "INSERT INTO SGE.Usuario ('{0}', '{1}', '{2}', '{3}', '{4}')";
			Query(String.Format(query, cliente.nombre, cliente.apellido, cliente.domicilio, cliente.usuario, HashThis.Instancia.GetHash(cliente.password)));

			cliente.idUsuario = GetIDUsuarioIfExists(cliente.usuario, cliente.password);
			if (cliente.idUsuario == -1) return;

			var idTransformador = DAOzona.Instancia.AsignarTransformador(cliente);			
			
			query = "INSERT INTO SGE.Cliente ('{0}', '{1}', '{2}', '{3}', '{3}', '{5}', '{6}', '{7}')";
			Query(String.Format(query, cliente.idUsuario, cliente.Telefono, cliente.AltaServicio, cliente.Documento_numero, cliente.Documento_tipo, cliente.Categoria.IdCategoria, cliente.Puntos, idTransformador));
		}

		// ------------------------------------ UPDATES ------------------------------------

		#endregion

		#region Dispositivo

		// ------------------------------------ SELECTS ------------------------------------

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
				if (row["disp_inteligente"].ToString() == "True") lista.Add(GetInteligenteFromData(row));
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

			idD = Int32.Parse(row["dpc_idDispositivo"].ToString());
			idC = Int32.Parse(row["dpc_idUsuario"].ToString());
			numero = Int32.Parse(row["dpc_numero"].ToString());
			nombre = row["disp_dispositivo"].ToString();
			concreto = row["disp_concreto"].ToString();
			consumo = Double.Parse(row["disp_consumo"].ToString());
			estado = Byte.Parse(row["dpc_estado"].ToString());
			fechaEstado = DateTime.Parse(row["dpc_fechaEstado"].ToString());

			return new Inteligente(idD, idC, numero, nombre + " " + concreto, consumo, estado, fechaEstado);
		}

		private Estandar GetEstandarFromData(DataRow row)
		{
			int idD, idC, numero;
			byte usoDiario;
			double consumo;
			string nombre, concreto;

			idD = Int32.Parse(row["dpc_idDispositivo"].ToString());
			idC = Int32.Parse(row["dpc_idUsuario"].ToString());
			numero = Int32.Parse(row["dpc_numero"].ToString());
			nombre = row["disp_dispositivo"].ToString();
			concreto = row["disp_concreto"].ToString();
			consumo = Double.Parse(row["disp_consumo"].ToString());
			usoDiario = Byte.Parse(row["dpc_usoDiario"].ToString());

			return new Estandar(idD, idC, numero, nombre + " " + concreto, consumo, usoDiario);
		}

		// ------------------------------------ INSERTS ------------------------------------

		public void GuardarDispositivo(Dispositivo disp)
		{
			//Hacer
		}

		#endregion

		#region Categoria

		// ------------------------------------ SELECTS ------------------------------------

		public Categoria GetCategoria(string idCategoria)
		{
			var query = "SELECT * FROM {0} WHERE {1} = '{2}'";
			var data = Query(String.Format(query, "SGE.Categoria", "categ_idCategoria", idCategoria)).Tables[0];
			if (data.Rows.Count == 0) return null;

			byte cmin, cmax;
			decimal carfijo, carvar;

			cmin = Byte.Parse(data.Rows[0]["categ_consumo_min"].ToString());
			cmax = Byte.Parse(data.Rows[0]["categ_consumo_max"].ToString());
			carfijo = Decimal.Parse(data.Rows[0]["categ_cargoFijo"].ToString());
			carvar = Decimal.Parse(data.Rows[0]["categ_cargoVariable"].ToString());

			return new Categoria(idCategoria, cmin, cmax, carfijo, carvar);
		}

		// ------------------------------------ INSERTS ------------------------------------

		private void GuardarCategoria(Categoria categoria)
		{
			var query = "SELECT * FROM SGE.Categoria WHERE categ_idCategoria = '{0}'";
			var data = Query(String.Format(query, categoria.IdCategoria)).Tables[0];
			if (data.Rows.Count != 0) return;

			query = "INSERT INTO SGE.Categoria ('{0}', '{1}', '{2}', '{3}', '{4}')";
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

		#endregion

		#region Transformador

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

			id = Int32.Parse(row["trans_idTransformador"].ToString());
			if (row["trans_activo"].ToString() == "True") activo = true;
			else activo = false;
			latitud = Double.Parse(row["trans_latitud"].ToString());
			longitud = Double.Parse(row["trans_longitud"].ToString());
			zona = id = Int32.Parse(row["trans_zona"].ToString());

			var clientes = GetClientesIDOfTransformador(id);

			return new Transformador(id, latitud, longitud, activo, clientes);
		}

		public List<Transformador> GetTransformadores(int idZona)
		{
			var lista = new List<Transformador>();

			var query = "SELECT * FROM SGE.Transformador WHERE trans_zona = '{0}'";
			var data = Query(String.Format(query, idZona)).Tables[0];
			if (data.Rows.Count == 0) return null;

			foreach (DataRow row in data.Rows)
			{
				lista.Add(GetTransformadorFromData(row));
			}

			return lista;
		}

		#endregion

		#region Zona

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
				id = Int32.Parse(row["zona_idZona"].ToString());
				latitud = Double.Parse(row["zona_latitud"].ToString());
				longitud = Double.Parse(row["zona_longitud"].ToString());
				radio = Int32.Parse(row["zona_radio"].ToString());

				var transformadores = GetTransformadores(id);

				lista.Add(new Zona(id, radio, latitud, longitud, transformadores));
			}

			return lista;
		}

        #endregion

        #region Sensor
        #endregion

        #region Regla
        #endregion

        public bool IsAdministrador(int idusuari)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT count(*) FROM SGE.Administrador WHERE admin_idUsuario = @iduser ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("iduser", idusuari));

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        bool valor = (reader.GetInt32(0) > 0);
                        return valor;
                    }
                    else
                    { return false; }
                }
            }
            
        }
          
        public int GetIDUsuarioIfExists(string username, string password)
		{			
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
                password = HashThis.Instancia.GetHash(password);
                var query = "SELECT usua_idUsuario FROM SGE.Usuario WHERE usua_username = @user AND usua_password = @pass";
				

				using (SqlCommand command = new SqlCommand(query, connection))
				{					
					command.Parameters.Add(new SqlParameter("user", username));
					command.Parameters.Add(new SqlParameter("pass", password));

					SqlDataReader reader = command.ExecuteReader();
					if (reader.HasRows) {
						reader.Read();
						return (int)reader.GetValue(0);
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

