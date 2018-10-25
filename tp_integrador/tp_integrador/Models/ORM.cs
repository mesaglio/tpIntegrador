﻿using System;
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

			var dispositivos = GetDispositivos(idCliente);

			string nombre, apellido, domicilio, username, password, categoria, transformador;
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
			transformador = data.Rows[0]["clie_transformador"].ToString();
			puntos = Int32.Parse(data.Rows[0]["clie_puntos"].ToString());

			return new Cliente(idCliente, nombre, apellido, domicilio, username, password, telefono, fechaAlta, GetCategoria(categoria), docTipo, docNum, GetTransformador(transformador), dispositivos);
		}

		public List<Dispositivo> GetDispositivos(int idCliente)
		{
			//Hacer
			return new List<Dispositivo>();
		}

		public void GuardarDispositivo(Dispositivo disp)
		{
			//Hacer
		}

		private void GuardarAdministrador(Administrador admin)
		{
			//Hacer
		}
		private void GuardarCliente(Cliente cliente)
		{
			//Hacer
		}

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

		public Transformador GetTransformador(string idTransformador)
		{
			//Hacer			
			return null;
		}
				
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

