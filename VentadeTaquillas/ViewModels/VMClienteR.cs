using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VentadeTaquillas.ViewModels
{
    public class VMClienteR
    {
        public Guid ClienteId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Ciudad { get; set; }
        public string Telefono { get; set; }
        public string PeliculaNombre { get; set; }

    }
}
