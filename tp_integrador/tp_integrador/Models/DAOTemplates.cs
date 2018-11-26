using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class DAOTemplates
    {
        public List<DispositivoGenerico> templateDisps;		
		
        public DAOTemplates()
        {
			templateDisps = ORM.Instancia.GetAllTemplates();			
        }

		public DispositivoGenerico Searchtemplatebyid(int id)
		{
			foreach (DispositivoGenerico tem in templateDisps)
			{
				if (tem.ID == id) return tem;
			}
			return null;
		}
	}    
}