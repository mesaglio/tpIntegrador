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
        }
        public IMongoDatabase conection()
        {
            var monguis = new MongoClient("mongodb://<disenio>:<disenio2018>@ds031108.mlab.com:31108/sgemdb");
            return monguis.GetDatabase("sgemdb");
        }
        public void agregarReporte(IMongoDatabase data, string cliente,string anio,string mes, string consumo)
        {
            mapeo();
            var reportes = data.GetCollection<Reporte>("userreportes");
            var reporte = new Reporte(cliente, anio, mes, consumo);

            reportes.InsertOne(reporte);
        }
        public void eliminarReporteDeClientePorAnioMes(IMongoDatabase data,string _cliente,string _anio,string _mes)
        {
            mapeo();
            //filtro
            var builder = Builders<Reporte>.Filter;
            var filtro = builder.Eq("clienteID", _cliente) & builder.Eq("anio", _anio) & builder.Eq("mes", _mes);

            var reportes = data.GetCollection<Reporte>("userreportes");

            reportes.DeleteOne(filtro);
            
            
        }

    }
}
