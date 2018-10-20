﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tp_integrador.Models
{
    public class Cliente : Usuarios
    {
        [Key]
        public string Telefono { get; set; }
        public DateTime AltaServicio { get; set; }
        public Categoria Categoria { get; set; }
        public string Documento_tipo { get; set; }
        public string Documento_numero { get; set; }
        public int Puntos { get; set; }
        //[ForeignKey("dispositivos")]
       // public int iddispositivo { get; set; }
        public List<Dispositivo> dispositivos { get; set; }

        public List<Dispositivo> GetDispositivosInteligentes()
        {
            return dispositivos.FindAll(i => i.EsInteligente);
        }

        public List<Dispositivo> GetDispositivosEstandar()
        {
            return dispositivos.FindAll(i => !i.EsInteligente);
        }

        //TODO: verificar si funciona estandar


        public Cliente(int id, string name, string lastname, string home, string user, string clave, string phone, DateTime alta, Categoria categ, string doc_t, string doc_n) : base(id, name, lastname, home, user, clave)
        {
            Telefono = phone;
            Categoria = categ;
            Documento_tipo = doc_t;
            Documento_numero = doc_n;
            AltaServicio = alta;
            dispositivos = new List<Dispositivo>();
        }


        public int CantDispositivos()
        {
            return dispositivos.Count;
        }

        public bool EstasEncendido(Inteligente aparato)
        {
            return aparato.Encendido();
        }

        public int CantEncendidos()
        {
            int i = 0;            
            foreach (Inteligente aparato in dispositivos.OfType<Inteligente>())
            {
                if (aparato.Encendido()) { i++; }
            }
            return i;
        }

        public bool AlgunoEncendido()
        {
            return (CantEncendidos() != 0);
        }

        public int CantApagados()
        {
            return dispositivos.OfType<Inteligente>().Count() - CantEncendidos();
        }

        
        public void ApagarDispositivo(Inteligente aparato)
        {
            aparato.Apagar();
        }

        public void EncenderDispositivo(Inteligente aparato)
        {
            aparato.Encender();
        }

        public void ModoAhorroDispositivo(Inteligente aparato)
        {
            aparato.ModoAhorro();
        }

        public double GetEstimado(Estandar aparato)
        {
            return aparato.ConsumoEstimado();
        }

        public void UsoDiario(Estandar aparato, byte horas)
        {
            aparato.SetUsoDiario(horas);
        }

        public void NuevoDispositivoInteligente(string nombre, double consumo)
        {
            dispositivos.Add(new Inteligente(idUsuario, dispositivos.Count()+1, nombre, consumo, 0, DateTime.Now));
            Puntos += 15;
        }

        public void NuevoDispositivoEstandar(string nombre, double consumo, byte usoPromedio)
        {
            dispositivos.Add(new Estandar(dispositivos.Count+1, nombre, consumo, usoPromedio));
        }

        public void ConvertirAInteligente(Estandar aparato)
        {
            Inteligente adaptado = new Inteligente(idUsuario, aparato.Numero, aparato.Nombre, aparato.Consumo, 0, DateTime.Now);
            dispositivos.Remove(aparato);
            dispositivos.Add(adaptado);
            Puntos += 10;
        }

        public void AgregarDispositivo(Dispositivo dispositivo)
        {
            dispositivos.Add(dispositivo);
        }
    }
}