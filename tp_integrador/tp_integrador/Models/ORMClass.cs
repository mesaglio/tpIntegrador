using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;   

namespace tp_integrador.Models
{
    public class ORMClass
    {
    }

    public class DBcontext : DbContext
    {
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Transformador> Transformadores { get; set; }
        public DbSet<Zona> Zonas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Dispositivo> Dispositivos { get; set; }
        public DbSet<Regla> Reglas { get; set; }
        public DbSet<Activator> Activatores { get; set; }

        public DBcontext() : base(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = master; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {}
    }
}