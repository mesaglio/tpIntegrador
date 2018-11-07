using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gmap.net;
using tp_integrador.Models;


namespace tp_integrador.Tests
{
    [TestClass]
    public class UnitTest1
    {
        static Location coordenadas = new Location(-34.6083, -58.3712);
        static Categoria categoria = new Categoria("R1", 10, 150, 18.76m, 0.644m);
        static Cliente userCreado = new Cliente(3, "nicolas", "perez", "calle cualquiera 123", coordenadas, "nico", "1234", "44112233", DateTime.Now.AddYears(-20).AddMonths(-3), categoria, "DNI", "12345678", false);
        public static List<int> clientes = new List<int>(3);
        static Transformador transf = new Transformador(1, 1, -34.60, -58.3713, true, clientes);
        public TestContext TestContext;
        public DataRow data;

        [TestMethod]
        public void TestDeCliente()
        {
            DAOzona.Instancia.InitialLoad();
            //crear un cliente, modificarlo y fijarse que no se updeteo en la base
            ORM.Instancia.Insert(transf);
            ORM.Instancia.Insert(categoria);
            ORM.Instancia.Insert(userCreado);

            //modifico el cliente
            userCreado.Coordenadas.Latitude = -32.8833;
            userCreado.Coordenadas.Longitude = -68.8167;
            ORM.Instancia.Update(userCreado);

            //traigo al user updeteado
            Cliente userModificado = ORM.Instancia.GetUsuario(3);

            //corroboro que no son iguales
            Assert.AreNotEqual(userCreado, userModificado);
        }

        [TestMethod]
        public void TestDeDispositivo()
        {
            //Crear un dispositivo, mostrar por consola los intervalos que estuvo encendido
            //durante el ultimo mes, modificar su nombre updetearlo y verificar que se updeteo
            //creo user
            DAOzona.Instancia.InitialLoad();
            ORM.Instancia.Insert(transf);
            ORM.Instancia.Insert(categoria);
            ORM.Instancia.Insert(userCreado);

            //obtengo los dispositivos del cliente
            List<Dispositivo> dispo = ORM.Instancia.GetDispositivos(2);
            Dispositivo dispositivo = dispo.First();
            //int id = dispositivo.IdDispositivo;
            String nombreOriginal = dispo.First().Nombre;

            //mostrar los intervalos
            DateTime desde = new DateTime(2018, 3, 11, 0, 0,0);
            DateTime hasta = DateTime.Today;
            List<EstadoDispositivo> estados= ORM.Instancia.GetEstadosEntre(2, 8, 1, desde, hasta);
            String intervalos = "Intervalos que estuvo encendido\r\n" +
                "Encendido                     Apagado \r\n";
            foreach(EstadoDispositivo d in estados)
            {

                intervalos += d.FechaInicio.ToString();
                intervalos += " - ";
                intervalos += d.FechaFin.ToString();
                intervalos += "\r\n";
            }

            //modifico el televisor cambiandole el nombre
            dispositivo.Nombre = "Televisor Led 40' Samsung";
            ORM.Instancia.Update(dispositivo);

            //recupero el dispositivo modificado
            List<Dispositivo> dispositivoss = ORM.Instancia.GetDispositivos(2);
            Dispositivo disCambiado = dispo.First();
            String nombreCambiado = dispo.First().Nombre;
            Console.WriteLine(intervalos);
            Assert.AreNotEqual(nombreCambiado, nombreOriginal);
        }

        [TestMethod]
        public void TestRegla()
        {
            DAOzona.Instancia.InitialLoad();
            DateTime dia = new DateTime(2018, 10, 01, 10, 00, 30);
            List<Inteligente> dispositivo = new List<Inteligente>();
            Inteligente televisor = new Inteligente(1, 31, 1, "Televisor", 40, 0, dia, false);
            dispositivo.Add(televisor);
            List<int> reglas = new List<int>();
            Actuador actuador = new Actuador(1, "actuador", reglas, 31, dispositivo);
            List<Actuador> lista = new List<Actuador>();
            //Regla regla = new Regla(1, 1, "detalle", 10, lista)

        }
        [TestMethod]
        public void TestTransformadores()
        {
            DAOzona.Instancia.InitialLoad();
            ORM.Instancia.Insert(transf);
            ORM.Instancia.Insert(categoria);
            ORM.Instancia.Insert(userCreado);
            String sql = (ORM.Instancia.Query("select count(*) from SGE.Transformador").Tables[0].Rows[0][0].ToString());
            string cantidad = "La cantidad de Transformadores es de " + sql;
            Console.WriteLine(cantidad);
        }
    }
}
