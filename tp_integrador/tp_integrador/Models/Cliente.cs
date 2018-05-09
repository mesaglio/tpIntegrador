using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Cliente : Usuarios
    {
        public string Telefono { get; set; }
        public DateTime AltaServicio { get; set; }
        public Categoria Categoria { get; set; }
        public string Documento_tipo { get; set; }
        public string Documento_numero { get; set; }
        private List<Dispositivo> dispositivos = new List<Dispositivo>();

        
        public int CantDispositivos()
        {
            return dispositivos.Count;
        }
        
        public int CantEncendidos()
        {
            int i = 0;
            foreach (Dispositivo aparato in dispositivos)
            {
                if (aparato.Estado) { i++; }
            }
            return i;
        }

        public bool AlgunoEncendido()
        {
            return (CantEncendidos() != 0);
        }

        public int CantApagados()
        {
            return CantDispositivos() - CantEncendidos();
        }

        
        public void NuevoDispositivo(string nombre, int consumo)
        {
            dispositivos.Add(new Dispositivo(nombre, consumo));
        }
        
        public void ApagarDispositivo(Dispositivo aparato)
        {
            aparato.Apagar();
        }

        public void EncenderDispositivo(Dispositivo aparato)
        {
            aparato.Encender();
        }

        public Cliente(int id, string name, string lastname, string home, string user, string clave, string phone, DateTime alta, Categoria categ, string doc_t, string doc_n) : base(id, name, lastname, home, user, clave)
        {
            Telefono = phone;
            Categoria = categ;
            Documento_tipo = doc_t;
            Documento_numero = doc_n;
            AltaServicio = alta;
        }

    }
}