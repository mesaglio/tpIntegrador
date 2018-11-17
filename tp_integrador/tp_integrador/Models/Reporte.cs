using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tp_integrador.Models
{
    class Reporte
    {
        [BsonId]
        public ObjectId ID { get; set; }

        [BsonElement("clienteID")]
        public string clienteID { get; set; }

        [BsonElement("anio")]
        public string anio { get; set; }

        [BsonElement("mes")]
        public string mes { get; set; }

        [BsonElement("consumoTotal")]
        public string consumoTotal { get; set; }

        public Reporte(string cliente, string _anio, string _mes, string _consumo)
        {
            this.clienteID = cliente;
            this.anio = _anio;
            this.mes = _mes;
            this.consumoTotal = _consumo;
        }

    }
}