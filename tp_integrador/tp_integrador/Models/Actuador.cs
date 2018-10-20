using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace tp_integrador.Models
{
    public class Actuador
    {
        [Key]
        public int idActor { get; set; }
        public string actuadorTipo { get; set; }
        List<Inteligente> dispositivos { get; set; }

        public virtual ICollection<Inteligente> dispositivo_Inteligente { get; set; }

    // TODO: hacer la logica para mandar acciones al dispositivo
    public void Actuar()
        {
            throw new NotImplementedException();
        }
    }
}