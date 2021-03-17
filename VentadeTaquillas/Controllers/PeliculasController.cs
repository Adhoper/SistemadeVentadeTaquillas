using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VentadeTaquillas.Data;
using VentadeTaquillas.ImgModel;
using VentadeTaquillas.Models;

namespace VentadeTaquillas.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        ViewModel vm = new ViewModel();
 

        public PeliculasController(ApplicationDbContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Peliculas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Peliculas.ToListAsync());
        }

        public IActionResult PeliculasDisp()
        {
            return View();
        }


        public IActionResult PeliculasProx()
        {
            return View();
        }


        // GET: Peliculas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .FirstOrDefaultAsync(m => m.PeliculaId == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // GET: Peliculas/Create
        public IActionResult Create()
        {
            return View(vm);
        }

        // POST: Peliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PeliculaId,NombrePeli,ImagenPeli,Descripcion,FechaPeli")] Pelicula pelicula,PeliculaImgModel imgModel)
        {
            if (ModelState.IsValid)
            {

                string stringFileName = UploadFile(imgModel);

                pelicula.ImagenPeli = stringFileName;


                pelicula.PeliculaId = Guid.NewGuid();
                _context.Add(pelicula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pelicula);
        }


        private string UploadFile(PeliculaImgModel imgModel)
        {
            string fileName = null;
            if (imgModel.ImagenPeli != null)
            {
                string uploadDir = Path.Combine(_webHostEnviroment.WebRootPath, "Imagenes");
                fileName = Guid.NewGuid().ToString() + "-" + imgModel.ImagenPeli.FileName;
                string FilePath = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(FilePath, FileMode.Create))
                {
                    imgModel.ImagenPeli.CopyTo(fileStream);
                }


            }
            return fileName;
        }

        // GET: Peliculas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }
            return View(pelicula);
        }

        // POST: Peliculas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PeliculaId,NombrePeli,ImagenPeli,Descripcion,FechaPeli")] Pelicula pelicula)
        {
            if (id != pelicula.PeliculaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pelicula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaExists(pelicula.PeliculaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pelicula);
        }

        // GET: Peliculas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .FirstOrDefaultAsync(m => m.PeliculaId == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // POST: Peliculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaExists(Guid id)
        {
            return _context.Peliculas.Any(e => e.PeliculaId == id);
        }
    }
}
