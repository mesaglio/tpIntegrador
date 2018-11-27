using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tp_integrador.Models
{
    public class ReporteDispo : Reporte
    {
        [BsonElement("dispositivoID")]
        public string dispositivoID { get; set; }

        
        public ReporteDispo(string dispoID,string mes, string anio, string consumo) : base(anio,mes,consumo)
        {
            this.dispositivoID = dispoID;
        }

    }
}