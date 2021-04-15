using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VentadeTaquillas.Data;

namespace VentadeTaquillas.ViewModels
{
    public class VMPublicaciones:Publicacion
    {
        public IEnumerable<Pelicula> Peliculas { get; set; }
    }
}
