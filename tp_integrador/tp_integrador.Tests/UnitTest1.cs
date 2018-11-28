using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gmap.net;
using tp_integrador.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using tp_integrador;


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
            //Recuperar un dispositivo, mostrar por consola los intervalos que estuvo encendido
            //durante el ultimo mes, modificar su nombre updetearlo y verificar que se updeteo
            //creo user
            DAOzona.Instancia.InitialLoad();
            ORM.Instancia.Insert(transf);
            ORM.Instancia.Insert(categoria);
            ORM.Instancia.Insert(userCreado);

            //obtengo los dispositivos del cliente
            List<Dispositivo> dispo = ORM.Instancia.GetDispositivos(3);
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
            Inteligente televisor = new Inteligente(1, 31, 1, "Televisor", 40, false, 0, dia, false);
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
        [TestMethod]
        public void TestMongo()
        {/*
            ODM odm = new ODM();
            odm.mapeo();
            IMongoDatabase ba = odm.conection();
            var bas = ba.GetCollection<Reporte>("userreportes");
            Reporte repo = new Reporte("1", "2018", "Enero", "2000");
            odm.agregarReporte(ba, "2","2018","Febrero","300");
            odm.agregarReporte(ba, "5", "2018", "Febrero", "300");
            */


        }
        [TestMethod]
        public void TestNico()
        {
            //serializo la clase para que matchee con la db
            BsonClassMap.RegisterClassMap<repoNico>();
            //connection
            var connectionString = "mongodb://admin1:admin1@ds062097.mlab.com:62097/dbtp0";
            var monguis = new MongoClient(connectionString);
            var ba = monguis.GetDatabase("dbtp0");
            //reportes
            var reportes = ba.GetCollection<repoNico>("reportes");
            //creo reporte para insertar
            DateTime fecha1 = new DateTime(2018, 01, 20);
            var reporte = new repoNico("user","1",20,fecha1,fecha1);
            reportes.InsertOne(reporte);

        }
        [TestMethod]
        public void TestRepo()
        {
            //toda la informacion arranca del 01/01/2018
            DAOzona.Instancia.InitialLoad();
            DAODispositivo.Instancia.InitialLoad();
            List<Cliente> asd = ORM.Instancia.GetClientesAutoSimplex();
            
            DateTime eneroI = new DateTime(2018, 01, 01);
            DateTime eneroF = new DateTime(2018, 01, 31);
            DateTime febreroI = new DateTime(2018, 02, 01);
            DateTime febreroF = new DateTime(2018, 02, 28);
            DateTime marzoI = new DateTime(2018, 03, 01);
            DateTime marzoF = new DateTime(2018, 03, 31);
            DateTime abrilI = new DateTime(2018, 04, 01);
            DateTime abrilF = new DateTime(2018, 04, 30);
            DateTime mayoI = new DateTime(2018, 05, 01);
            DateTime mayoF = new DateTime(2018, 05, 31);
            DateTime junioI = new DateTime(2018, 06, 01);
            DateTime junioF = new DateTime(2018, 06, 30);
            DateTime julioI = new DateTime(2018, 07, 01);
            DateTime julioF = new DateTime(2018, 07, 31);
            DateTime agostoI = new DateTime(2018, 08, 01);
            DateTime agostoF = new DateTime(2018, 08, 31);
            DateTime septiembreI = new DateTime(2018, 09, 01);
            DateTime septiembreF = new DateTime(2018, 09, 30);
            DateTime octubreI = new DateTime(2018, 10, 01);
            DateTime octubreF = new DateTime(2018, 10, 31);
            DateTime noviembreI = new DateTime(2018, 11, 01);
            DateTime noviembreF = new DateTime(2018, 11, 30);
            DateTime diciembreI = new DateTime(2018, 12, 01);
            DateTime diciembreF = new DateTime(2018, 12, 01);
            
            List<ReporteUser> ts = new List<ReporteUser>();
            foreach (Cliente cliente in asd)
            {
                //cada cliente
                PeriodoData perr = cliente.consumoDelPeriodo(marzoI, marzoF);
                ReporteUser repoo = new ReporteUser(cliente.idUsuario.ToString(), perr.FechaInicio.Year.ToString(), perr.FechaInicio.Month.ToString(), perr.Consumo.ToString());
                ts.Add(repoo);

            }
            
            //reporte de dispositivos Estandar/Inteligente
            //idDispo,anio,mes,consumo

            //reporte de Transformador
            //idTransf,anio,mes,consumo             FALTA EL METODO DEL TRANSF QUE CALCULA EL CONSUMO
        }
        [TestMethod]
        public void testeMauro()
        {
            DateTime marzoI = new DateTime(2018, 03, 01);
            DateTime marzoF = new DateTime(2018, 03, 31);
            DateTime mayoI = new DateTime(2018, 05, 01);
            DateTime mayoF = new DateTime(2018, 05, 31);
            DAOzona.Instancia.InitialLoad();
            DAODispositivo.Instancia.InitialLoad();
            PeriodoData asdd;
            List<Cliente> asd = ORM.Instancia.GetClientesAutoSimplex();
            List<ReporteUser> reportes = new List<ReporteUser>();
            foreach(var cliente in asd)
            {
                asdd = cliente.consumoDelPeriodo(marzoI, marzoF);
                ReporteUser reporte = new ReporteUser(cliente.idUsuario.ToString(),asdd.FechaInicio.Year.ToString(),asdd.FechaInicio.Month.ToString(),asdd.Consumo.ToString());
                reportes.Add(reporte);
                asdd = cliente.consumoDelPeriodo(mayoI, mayoF);
                ReporteUser reportee = new ReporteUser(cliente.idUsuario.ToString(), asdd.FechaInicio.Year.ToString(), asdd.FechaInicio.Month.ToString(), asdd.Consumo.ToString());
                reportes.Add(reportee);
            }
            
        }
    }
}
