using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;   

namespace tp_integrador.Models
{
    
    public class DBSQLcontext : DbContext
    {
        // seteo las tablas
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Administrador> Administradores { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Transformador> Transformadores { get; set; }
        public virtual DbSet<Zona> Zonas { get; set; }
        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Sensor> Sensors { get; set; }
        public virtual DbSet<Dispositivo> Dispositivos { get; set; }
        public virtual DbSet<Regla> Reglas { get; set; }
        public virtual DbSet<Actuador> Actuadores { get; set; }

        public DBSQLcontext() : base("DBSQLcontextString")
        {
            // ESTO VA CUANDO ESTE BIEN SETEADO PARA QUE NO CREE LA DB CADA VEZ DESDE 0
            //  Database.SetInitializer<DBSQLcontext>(new CreateDatabaseIfNotExists<DBSQLcontext>());//  Database.SetInitializer<DBSQLcontext>(new DropCreateDatabaseIfModelChanges<DBSQLcontext>());
          
        }




    }
   
    public class ORMClass
    {


    }
}