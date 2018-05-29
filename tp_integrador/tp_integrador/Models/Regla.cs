using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Regla
    {
        List<Actuador> actuadores { get; set; }
        // recive la notificacion de un cambio en magnitud del sensor
        internal void Cambio(int mag)
        {
            throw new NotImplementedException();
        }
    }
}