using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VentadeTaquillas.Data;

namespace VentadeTaquillas.ViewModels
{
    public class VMCinesyMas
    {

         public IEnumerable<Cine> Cines { get; set; } // lISTA DE Cines
        public IEnumerable<Sala> Salas { get; set; } //LISTA DE Salas
        public IEnumerable<Asiento> Asientos { get; set; } // LIsta de Asientos
    }
}
