using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Cliente : Usuarios
    {
        public string Telefono { get; private set; }
        public DateTime AltaServicio { get; private set; }
        public Categoria Categoria { get; private set; }
        public string Documento_tipo { get; private set; }
        public string Documento_numero { get; private set; }
        private List<Dispositivo> dispositivos;

        
        private int CantDispositivos()
        {
            return dispositivos.Count;
        }
        
        private int CantEncendidos()
        {
            int i = 0;
            foreach (Dispositivo aparato in dispositivos)
            {
                if (aparato.Estado) { i++; }
            }
            return i;
        }

        private bool AlgunoEncendido()
        {
            return (CantEncendidos() != 0);
        }

        private int CantApagados()
        {
            return CantDispositivos() - CantEncendidos();
        }

        
        private void NuevoDispositivo(string nombre, int consumo)
        {
            dispositivos.Add(new Dispositivo(nombre, consumo));
        }
        
        private void ApagarDispositivo(Dispositivo aparato)
        {
            aparato.Apagar();
        }

        private void EncenderDispositivo(Dispositivo aparato)
        {
            aparato.Encender();
        }

        public Cliente(int id, string name, string lastname, string home, string user, string clave, string phone, Categoria categ, string doc_t, string doc_n) : base(id,name,lastname,home,user,clave)
        {
            Telefono = phone;
            Categoria = categ;
            Documento_tipo = doc_t;
            Documento_numero = doc_n;
            AltaServicio = DateTime.Now;
            dispositivos = new List<Dispositivo>();
        }
        
    }
}