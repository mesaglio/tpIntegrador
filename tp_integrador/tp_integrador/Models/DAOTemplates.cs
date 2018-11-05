using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class DAOTemplates
    {
        public List<TemplateDispositivo> templateDisps; 
        
        public DAOTemplates()
        {
			templateDisps = ORM.Instancia.GetAllTemplates();
        }

		public TemplateDispositivo Searchtemplatebyid(int id)
		{
			foreach (TemplateDispositivo tem in templateDisps)
			{
				if (tem.ID == id) return tem;
			}
			return null;
		}
	}    
}