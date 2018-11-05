using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tp_integrador;
using Gmap.net;
using tp_integrador.Models;

namespace tp_integrador.Tests
{
    [TestClass]
    public class UnitTest1
    {
        static Location coordenadas = new Location(-34.6083, -58.3712); 
        static Categoria categoria = new Categoria("R1", 10, 150, 18.76m, 0.644m);
        static Cliente userCreado = new Cliente(3, "nicolas", "perez", "calle cualquiera 123",coordenadas, "nico", "1234", "44112233", DateTime.Now.AddYears(-20).AddMonths(-3), categoria, "DNI", "12345678");
        public static List<int> clientes = new List<int>(3);
        static Transformador transf = new Transformador(1, 1, -34.6082, -58.3713, true,clientes);
        public TestContext TestContext;

        [TestMethod]
        public void TestDeCliente()
        {
            
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
            Assert.AreEqual(userCreado, userModificado);
        }

        [TestMethod]
        public void TestDeDispositivo()
        {
            //Crear un dispositivo, mostrar por consola los intervalos que estuvo encendido
            //durante el ultimo mes, modificar su nombre updetearlo y verificar que se updeteo
            //creo user
            ORM.Instancia.Insert(transf);
            ORM.Instancia.Insert(categoria);
            ORM.Instancia.Insert(userCreado);
            //creo dispositivo
            DateTime dia = new DateTime(2018, 10, 01, 10, 00, 30);
            Dispositivo tele = new Dispositivo(1, 31, 1, "Tele Standar", 40);
            Inteligente televisor = new Inteligente(1, 31, 1, "Televisor", 40, 0, dia);


            ORM.Instancia.Insert(tele);
            ORM.Instancia.Insert(televisor);

            //modifico el televisor cambiandole el nombre
            Inteligente televisorCambiado = new Inteligente(1, 31, 1, "Smart TV", 40, 0, dia);
            ORM.Instancia.Update(televisorCambiado);

            List<Dispositivo> DispositivosDe31 = ORM.Instancia.GetDispositivos(31);
            Dispositivo televi = DispositivosDe31.First();
            String nombre = televi.Nombre;

            Assert.AreNotEqual("Televisor", nombre);
        }

        [TestMethod]
        public void TestRegla()
        {
            DateTime dia = new DateTime(2018, 10, 01, 10, 00, 30);
            List<Inteligente> dispositivo = new List<Inteligente>();
            Inteligente televisor = new Inteligente(1, 31, 1, "Televisor", 40, 0, dia);
            dispositivo.Add(televisor);
            List<int> reglas = new List<int>();
            Actuador actuador = new Actuador(1, "actuador", reglas, 31, dispositivo);
            List<Actuador> lista = new List<Actuador>();
            //Regla regla = new Regla(1, 1, "detalle", 10, lista)

        }
        [TestMethod]
        public void TestTransformadores()
        {
            ORM.Instancia.Insert(transf);
            ORM.Instancia.Insert(categoria);
            ORM.Instancia.Insert(userCreado);

            string cantidad = "La cantidad de Transformadores es de " + (ORM.Instancia.Query("select count(*) from SGE.Transformador").Tables[0].Columns[0]);
            TestContext.WriteLine(cantidad);
        }
    }
}
