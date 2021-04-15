using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VentadeTaquillas.ViewModels
{
    public class AsientoVM
    {
        public Guid AsientoId { get; set; }
        public string NumAsiento { get; set; }

        public string Estado { get; set; }

        public string NombreSala { get; set; }
    }
}
