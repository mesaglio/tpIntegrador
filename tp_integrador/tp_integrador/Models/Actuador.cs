using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Actuador
    {
        [Key]
        public int idActor { get; set; }
        public string actuadorTipo { get; set; }
        List<Inteligente> dispositivos { get; set; }

        // TODO: hacer la logica para mandar acciones al dispositivo
        public void Actuar()
        {
            throw new NotImplementedException();
        }
    }
}