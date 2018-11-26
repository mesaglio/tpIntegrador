using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tp_integrador.Models
{
    public class ReporteTransf : Reporte
    {
        [BsonElement("transformadorID")]
        public string transformadorID { get; set; }

        
        public ReporteTransf(string transfID,string anio,string mes, string consumo) : base(anio,mes,consumo)
        {
            this.transformadorID = transfID;
        }

    }
}