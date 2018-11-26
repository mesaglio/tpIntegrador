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
            
        

        }
        

    }

