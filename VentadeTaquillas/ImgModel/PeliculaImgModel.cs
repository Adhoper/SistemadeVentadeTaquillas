using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VentadeTaquillas.ImgModel
{
    public class PeliculaImgModel
    {
        public Guid PeliculaId { get; set; }
        public string NombrePeli { get; set; }
        public IFormFile ImagenPeli { get; set; }

        public string Descripcion { get; set; }

        public DateTime FechaPeli { get; set; }
    }
}
