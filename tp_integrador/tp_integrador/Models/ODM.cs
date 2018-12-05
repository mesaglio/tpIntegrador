using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace tp_integrador.Models
{
    public class ODM
    {
        private static ODM _instancia;
        private ODM() { }
        public static ODM Instancia
        {
            get
            {
                if (_instancia == null) _instancia = new ODM();
                return _instancia;
            }
        }
        public void generarTodosLosReportes()
        {
            DateTime hoy = DateTime.Today;
            this.mapeo();
            //DateTime hoy = new DateTime(2018, 04, 01);
            //DateTime hoy = new DateTime(2018, 05, 01);
            if (hoy.Day == 01)
            {

                DateTime fechaDeLosReportes = hoy.AddMonths(-1);
                //chequear que ya no esten hechos los reportes
                if (noHayReportes(fechaDeLosReportes))
                {
                    // si no esta hechos hay que hacerlos
                    this.generarReporteDeDispositivos(fechaDeLosReportes);
                    this.generarReporteDeUsuario(fechaDeLosReportes);
                    this.generarReporteTransformadores(fechaDeLosReportes);
                }
                }
        }
        public void generarReporteTransformadores(DateTime fechaDelReporte)
        {

            List<Zona> zonas = ORM.Instancia.GetAllZonas();
            List<Transformador> transformadores = new List<Transformador>();
            foreach(Zona z in zonas) //traigo todas las zonas
            {
                List<Transformador> trans = ORM.Instancia.GetTransformadores(z.idZona);
                foreach(Transformador t in trans) //traigo todos los 
                {
                    transformadores.Add(t);
                }
            }
            //todos los transformadores en transoformadores
            foreach(Transformador t in transformadores)
            {
                string consumo = t.CantidadEnergia().ToString();
                ReporteTransf reporte = new ReporteTransf(t.id.ToString(),fechaDelReporte.Year.ToString(),fechaDelReporte.Month.ToString(), consumo);//ver tema de las fechas, falta anio mes
                this.agregarReporteTransf(this.conection(), reporte);
            }
            
        }
        public void generarReporteDeUsuario(DateTime fechaDelReporte)//2018/03/01
        {
            // fin del mes del reporte
            DateTime finDelMes = fechaDelReporte.AddMonths(1).AddDays(-1);//2018/03/31
            //todos los clientes
            List<Cliente> clientes = ORM.Instancia.GetAllClientes();
            foreach(Cliente cli in clientes)
            {
                
                string consumo = cli.ConsumoDelPeriodo(fechaDelReporte,finDelMes).Consumo.ToString(); // mes anterior de la fecha actual
                ReporteUser reporte = new ReporteUser(cli.idUsuario.ToString(), fechaDelReporte.Year.ToString(), fechaDelReporte.Month.ToString(), consumo);
                this.agregarReporteUser(this.conection(), reporte);
            }
        }
        public void generarReporteDeDispositivos(DateTime fechaDelReporte)
        {
            DateTime finDelReporte = fechaDelReporte.AddMonths(1).AddDays(-1);
            //todos los dispositivos
            List<Dispositivo> dispositivos = ORM.Instancia.GetAllDispositivos();
            List<Estandar> estandar = dispositivos.OfType<Estandar>().ToList();
            List<Inteligente> inteligentes = dispositivos.OfType<Inteligente>().ToList();
           foreach(Estandar d in estandar)
                {   //Estandar
                    string consumo = d.ConsumoEstimado().ToString();
                    ReporteDispo reporte = new ReporteDispo(d.IdDispositivo.ToString(), fechaDelReporte.Year.ToString(), fechaDelReporte.Month.ToString(), consumo);
                    this.agregarReporteDispo(this.conection(), reporte);
                }
                foreach(Inteligente d in inteligentes)
                {   //inteligente
                    string consumo = d.ConsumoDesdeHasta(fechaDelReporte, finDelReporte).ToString(); // mes anterior
                    ReporteDispo reporte = new ReporteDispo(d.IdDispositivo.ToString(), fechaDelReporte.Month.ToString(), fechaDelReporte.Year.ToString(), consumo);
                    this.agregarReporteDispo(this.conection(), reporte);
                }
            
                
        }
        public void mapeo()
        {
            BsonClassMap.RegisterClassMap<Reporte>();
            BsonClassMap.RegisterClassMap<ReporteDispo>();
            BsonClassMap.RegisterClassMap<ReporteUser>();
            BsonClassMap.RegisterClassMap<ReporteTransf>();
        }
        public IMongoDatabase conection()
        {
            var connectionString = "mongodb://prueba:prueba123@ds031108.mlab.com:31108/sgemdb";
            var monguis = new MongoClient(connectionString);
            return monguis.GetDatabase("sgemdb");
        }
        public void agregarReporteUser(IMongoDatabase data, ReporteUser repo)
        {
            agregarReporte(1,data, repo.clienteID, repo.anio, repo.mes, repo.consumo);
        }
        public void agregarReporteDispo(IMongoDatabase data, ReporteDispo repo)
        {
            agregarReporte(2, data, repo.dispositivoID, repo.anio, repo.mes, repo.consumo);
        }
        public void agregarReporteTransf(IMongoDatabase data, ReporteTransf repo)
        {
            agregarReporte(3, data, repo.transformadorID, repo.anio, repo.mes, repo.consumo);
        }


        public void agregarReporte(int tipo,IMongoDatabase data, string id,string anio,string mes, string consumo)
        {
            switch (tipo) {
                case 1: //USERS
                    var reportesUser = data.GetCollection<ReporteUser>("userreportes");
                    var reporteU = new ReporteUser(id, anio, mes, consumo);
                    reportesUser.InsertOne(reporteU);
                    break;
                case 2: //DISPOSITIVOS
                    var reportesDisp = data.GetCollection<ReporteDispo>("adminreportesdispo");
                    var reporteD = new ReporteDispo(id, anio, mes, consumo);
                    reportesDisp.InsertOne(reporteD);
                    break;
                case 3: //TRANSFORMADORES
                    var reportesTransf = data.GetCollection<ReporteTransf>("adminreportestransf");
                    var reporteT = new ReporteTransf(id, anio, mes, consumo);
                    reportesTransf.InsertOne(reporteT);
                    break;
            }
        }
        public void eliminarReportePorAnioMes(int tipo, IMongoDatabase data, string _id, string _anio, string _mes)
        {
            switch (tipo)
            {
                case 1:
                    var builderU = Builders<ReporteUser>.Filter;
                    var filtroU = builderU.Eq("clienteID", _id) & builderU.Eq("anio", _anio) & builderU.Eq("mes", _mes);
                    var reportesU = data.GetCollection<ReporteUser>("userreportes");
                    reportesU.DeleteOne(filtroU);
                    break;
                case 2:
                    var builderD = Builders<ReporteDispo>.Filter;
                    var filtroD = builderD.Eq("dispositivoID", _id) & builderD.Eq("anio", _anio) & builderD.Eq("mes", _mes);
                    var reportesD = data.GetCollection<ReporteDispo>("adminreportesdispo");
                    reportesD.DeleteOne(filtroD);
                    break;
                case 3:
                    var builderT = Builders<ReporteTransf>.Filter;
                    var filtroT = builderT.Eq("transformadorID", _id) & builderT.Eq("anio", _anio) & builderT.Eq("mes", _mes);
                    var reportesT = data.GetCollection<ReporteTransf>("adminreportestransf");
                    reportesT.DeleteOne(filtroT);
                    break;
            }
        }
        public bool noHayReportes(DateTime primerDiaDelMesDelReporte)
        {
            var data = this.conection();
            var builderU = Builders<ReporteUser>.Filter;
            var filtroU = builderU.Eq("anio", primerDiaDelMesDelReporte.Year) & builderU.Eq("mes", primerDiaDelMesDelReporte.Month);
            var reportesU = data.GetCollection<ReporteUser>("userreportes");
            var filtrado = reportesU.Find(filtroU);
            return (filtrado.Count() == 0);
        }
            
        

        }
        

    }

