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
		
		private ORM() { }

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
			return null;
		}

		public dynamic GetDispositivos(int idCliente)
		{
			return null;
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
				
		private bool HasSQLInyection(string linea)
		{
			//Hacer
			return false;
		}

		public int GetIDUsuarioIfExists(string username, string password)
		{
			if (HasSQLInyection(username) || HasSQLInyection(password)) return -2;
			password = HashThis.Instancia.GetHash(password);

			var query = "SELECT usua_idUsuario FROM SGE.Usuario WHERE usua_username = '{0}' AND usua_password = '{1}'";
			var data = Query(String.Format(query, username, password)).Tables[0];
			if (data.Rows.Count == 0) return -1;

			return Int32.Parse(data.Rows[0][0].ToString());

		}

		public DataSet Query(string q)
		{
			connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SGE"].ConnectionString;
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
