using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tp_integrador.Models
{
    public class Reporte
    {
        [BsonId]
        public ObjectId ID { get; set; }

        [BsonElement("anio")]
        public string anio { get; set; }

        [BsonElement("mes")]
        public string mes { get; set; }

        [BsonElement("consumo")]
        public string consumo { get; set; }

        public Reporte(string _anio, string _mes, string _consumo)
        {
            this.anio = _anio;
            this.mes = _mes;
            this.consumo = _consumo;
        }

    }
}