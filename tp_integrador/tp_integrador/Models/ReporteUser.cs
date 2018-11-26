using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tp_integrador.Models
{
    public class ReporteUser : Reporte
    {
        
        [BsonElement("clienteID")]
        public string clienteID { get; set; }

        public ReporteUser(string cliente, string _anio, string _mes, string _consumo) : base(_anio,_mes,_consumo)
        {
            this.clienteID = cliente;
        }

    }
}