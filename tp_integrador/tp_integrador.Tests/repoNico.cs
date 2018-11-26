using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tp_integrador.Tests
{
    public class repoNico
    {
        [BsonId]
        public ObjectId ID { get; set; }
        [BsonElement("tipoReporte")]
        public string tipoReporte { get; set; }
        [BsonElement("id")]
        public string id { get; set; }
        [BsonElement("consumo")]
        public double consumo { set; get; }
        [BsonElement("fechaInicio")]
        public DateTime fechaInicio { set; get; }
        [BsonElement("fechaFin")]
        public DateTime fechaFin { set; get; }

        public repoNico(string _tipo, string _id, double _consumo, DateTime _fechaInicio, DateTime _fechaFin)
        {
            id = _id;
            tipoReporte = _tipo;
            consumo = _consumo;
            fechaInicio = _fechaInicio;
            fechaFin = _fechaFin;
        }
    }
}
