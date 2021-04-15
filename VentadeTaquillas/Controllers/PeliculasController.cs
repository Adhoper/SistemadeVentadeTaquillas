using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VentadeTaquillas.Data;
using VentadeTaquillas.Models;

namespace VentadeTaquillas.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly ApplicationDbContext _context;
 

        public PeliculasController(ApplicationDbContext context)
        {
            _context = context;

        }

        // GET: Peliculas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Peliculas.ToListAsync());
        }

        public IActionResult PeliculasDisp()
        {
            return View(_context.Peliculas.ToList());
        }


        public IActionResult PeliculasProx()
        {
            return View(_context.Peliculas.ToList());
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult MantenimientoPelis()
        {
            return View(_context.Peliculas.ToList());
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
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Peliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("PeliculaId,NombrePeli,ImagenPeli,Descripcion,FechaPeli,Valor")] Pelicula pelicula)
        {

            if (ModelState.IsValid)
            {

                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    byte[] pics = null;
                    using (var fileStream = files[0].OpenReadStream())
                    {
                        using(var memorystream = new MemoryStream())
                        {
                            fileStream.CopyTo(memorystream);
                            pics = memorystream.ToArray();
                        }
                    }
                    pelicula.ImagenPeli = pics;
                }

                pelicula.PeliculaId = Guid.NewGuid();
                
                _context.Add(pelicula);
                await _context.SaveChangesAsync();
                return RedirectToAction("MantenimientoPelis", "Peliculas");
            }
            return View(pelicula);
        }


        // GET: Peliculas/Edit/5
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(Guid id, [Bind("PeliculaId,NombrePeli,ImagenPeli,Descripcion,FechaPeli,Valor")] Pelicula pelicula)
        {
            if (id != pelicula.PeliculaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                

                try
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count() > 0)
                    {
                        byte[] pics = null;
                        using (var fileStream = files[0].OpenReadStream())
                        {
                            using (var memorystream = new MemoryStream())
                            {
                                fileStream.CopyTo(memorystream);
                                pics = memorystream.ToArray();
                            }
                        }
                        pelicula.ImagenPeli = pics;
                    }

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
                return RedirectToAction("MantenimientoPelis", "Peliculas");
            }
            return View(pelicula);
        }

        // GET: Peliculas/Delete/5
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();
            return RedirectToAction("MantenimientoPelis", "Peliculas");
        }

        private bool PeliculaExists(Guid id)
        {
            return _context.Peliculas.Any(e => e.PeliculaId == id);
        }
    }
}
