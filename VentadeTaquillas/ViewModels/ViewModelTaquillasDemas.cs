using VentadeTaquillas.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace VentadeTaquillas.ViewModels
{
    public class ViewModelTaquillasDemas:Taquilla
    {

        public IEnumerable<Cliente> Clientes { get; set; }
        public IEnumerable<Pelicula> Peliculas { get; set; }
        public IEnumerable<Cine> Cines { get; set; } // lISTA DE pRODUCTOS
        public IEnumerable<Sala> Salas { get; set; } //LISTA DE CLIENTES
        public IEnumerable<Asiento> Asientos { get; set; } // LIsta de servicios
    }
}
