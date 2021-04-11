using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VentadeTaquillas.ViewModels
{
    public class ViewModelTaquilla
    {

        public Guid TaquillaId { get; set; }
        public string ClienteNombre { get; set; }

        public string ClienteApellido { get; set; }
        public string AsientoNombre { get; set; }
        public string PeliculaNombre { get; set; }

        public string SalaNombre { get; set; }

        public string CineNombre { get; set; }
    }
}

