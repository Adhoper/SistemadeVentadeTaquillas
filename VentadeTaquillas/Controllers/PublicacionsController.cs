using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VentadeTaquillas.Data;
using VentadeTaquillas.ViewModels;

namespace VentadeTaquillas.Controllers
{
    public class PublicacionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicacionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Publicacions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Publicaciones.ToListAsync());
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult MantenimientoPubli()
        {
            return View(_context.Publicaciones.ToList());
        }

        // GET: Publicacions/Details/5

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicacion = await _context.Publicaciones
                .FirstOrDefaultAsync(m => m.PublicacionId == id);
            if (publicacion == null)
            {
                return NotFound();
            }

            return View(publicacion);
        }

        // GET: Publicacions/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            var result = new VMPublicaciones
            {

                Peliculas = _context.Peliculas.ToList(),

            };

            return View(result);
        }

        // POST: Publicacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("PublicacionId,NombrePubliPeli,Evento,ImagenPubliPeli,Descripcion,FechaPeli,PeliculaId")] Publicacion publicacion)
        {
            if (ModelState.IsValid)
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
                    publicacion.ImagenPubliPeli = pics;
                }



                publicacion.PublicacionId = Guid.NewGuid();
                _context.Add(publicacion);
                await _context.SaveChangesAsync();
                return RedirectToAction("MantenimientoPubli", "Publicacions");
            }
            return View(publicacion);
        }

        // GET: Publicacions/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            ViewBag.ListaPelis = _context.Peliculas.ToList();

            var publicacion = await _context.Publicaciones.FindAsync(id);
            if (publicacion == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: Publicacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(Guid id, [Bind("PublicacionId,NombrePubliPeli,Evento,ImagenPubliPeli,Descripcion,FechaPeli,PeliculaId")] Publicacion publicacion)
        {
            if (id != publicacion.PublicacionId)
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
                        publicacion.ImagenPubliPeli = pics;
                    }

                    _context.Update(publicacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicacionExists(publicacion.PublicacionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("MantenimientoPubli", "Publicacions");
            }
            return View(publicacion);
        }

        // GET: Publicacions/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicacion = await _context.Publicaciones
                .FirstOrDefaultAsync(m => m.PublicacionId == id);
            if (publicacion == null)
            {
                return NotFound();
            }

            return View(publicacion);
        }

        // POST: Publicacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var publicacion = await _context.Publicaciones.FindAsync(id);
            _context.Publicaciones.Remove(publicacion);
            await _context.SaveChangesAsync();
            return RedirectToAction("MantenimientoPubli", "Publicacions");
        }

        private bool PublicacionExists(Guid id)
        {
            return _context.Publicaciones.Any(e => e.PublicacionId == id);
        }
    }
}
