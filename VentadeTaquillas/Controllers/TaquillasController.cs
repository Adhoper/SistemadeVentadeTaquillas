using System;
using System.Collections.Generic;
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
    public class TaquillasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaquillasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Taquillas
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {

            var result = (await _context.Taquillas.Select(s => new ViewModelTaquilla
            {
                TaquillaId = s.TaquillaId,
                ClienteNombre = _context.Clientes.Where(c => c.ClienteId == s.ClienteId).FirstOrDefault().Nombre,
                ClienteApellido = _context.Clientes.Where(c => c.ClienteId == s.ClienteId).FirstOrDefault().Apellido,
                PeliculaNombre = _context.Peliculas.Where(c => c.PeliculaId == s.PeliculaId).FirstOrDefault().NombrePeli,
                CineNombre = _context.Cines.Where(c => c.CineId == s.CineId).FirstOrDefault().NombreCine,
                SalaNombre = _context.Salas.Where(c => c.SalaId == s.SalaId).FirstOrDefault().Nombre,
                AsientoNombre = _context.Asientos.Where(c => c.AsientoId == s.AsientoId).FirstOrDefault().NumeroAsiento.ToString(),

            }).ToListAsync());

            return View(result);
        }

        // GET: Taquillas/Details/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taquilla = await _context.Taquillas
                .FirstOrDefaultAsync(m => m.TaquillaId == id);
            if (taquilla == null)
            {
                return NotFound();
            }

            return View(taquilla);
        }

        // GET: Taquillas/Create
        public IActionResult Create(Guid? id, Guid idcl,ViewModelTaquillasDemas vmtd)
        {

            var result = new ViewModelTaquillasDemas
            {
                Cines = _context.Cines.ToList(),
                Asientos = _context.Asientos.ToList(),
                Clientes = _context.Clientes.ToList(),
                Peliculas = _context.Peliculas.ToList(),
                Salas = _context.Salas.ToList(),

            };



                ViewBag.idCliente = idcl;
            ViewBag.idPelicula = id;


            return View(result);
        }

        [AllowAnonymous]
        [HttpGet("api/models/{id}")]
        public IEnumerable<Sala> Models(Guid id)
        {
            return _context.Salas.ToList()
                .Where(m => m.CineId == id);
        }

        [AllowAnonymous]
        [HttpGet("api/asientos/{id}")]
        public IEnumerable<Asiento> ModelsAsientos(Guid id)
        {
            return _context.Asientos.ToList()
                .Where(m => m.SalaId == id && m.Estado =="Disponible");
        }



        // POST: Taquillas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaquillaId,ClienteId,AsientoId,PeliculaId,SalaId,CineId")] Taquilla taquilla)
        {
            if (ModelState.IsValid)
            {
                var asiento = _context.Asientos.Where(p => p.AsientoId.Equals(taquilla.AsientoId)).FirstOrDefault();
                asiento.Estado = "Ocupado";


                taquilla.TaquillaId = Guid.NewGuid();
                _context.Add(taquilla);
                _context.Asientos.Update(asiento);
                await _context.SaveChangesAsync();
               

                return RedirectToAction("Factura","Taquillas");
            }
            return View(taquilla);
        }

        public IActionResult Factura()
        {
            int IdTaquillas = _context.Taquillas.Max(p => p.NumeroTaquillaId);

            Guid? IdTaquilla = _context.Taquillas.Where(p => p.NumeroTaquillaId == IdTaquillas).FirstOrDefault().TaquillaId;

            var result = (_context.Taquillas.Where(f => f.TaquillaId == IdTaquilla).Select(s => new ViewModelTaquilla
            {
                TaquillaId = s.TaquillaId,
                ClienteNombre = _context.Clientes.Where(c => c.ClienteId == s.ClienteId).FirstOrDefault().Nombre,
                ClienteApellido = _context.Clientes.Where(c => c.ClienteId == s.ClienteId).FirstOrDefault().Apellido,
                PeliculaNombre = _context.Peliculas.Where(c => c.PeliculaId == s.PeliculaId).FirstOrDefault().NombrePeli,
                CineNombre = _context.Cines.Where(c => c.CineId == s.CineId).FirstOrDefault().NombreCine,
                SalaNombre = _context.Salas.Where(c => c.SalaId == s.SalaId).FirstOrDefault().Nombre,
                AsientoNombre = _context.Asientos.Where(c => c.AsientoId == s.AsientoId).FirstOrDefault().NumeroAsiento.ToString(),

            }).ToList());

            return View(result);
        }

        // GET: Taquillas/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taquilla = await _context.Taquillas.FindAsync(id);
            if (taquilla == null)
            {
                return NotFound();
            }
            return View(taquilla);
        }

        // POST: Taquillas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(Guid id, [Bind("TaquillaId,ClienteId,AsientoId,PeliculaId,SalaId,CineId")] Taquilla taquilla)
        {
            if (id != taquilla.TaquillaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taquilla);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaquillaExists(taquilla.TaquillaId))
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
            return View(taquilla);
        }

        // GET: Taquillas/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taquilla = await _context.Taquillas
                .FirstOrDefaultAsync(m => m.TaquillaId == id);
            if (taquilla == null)
            {
                return NotFound();
            }

            return View(taquilla);
        }

        // POST: Taquillas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var taquilla = await _context.Taquillas.FindAsync(id);
            _context.Taquillas.Remove(taquilla);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaquillaExists(Guid id)
        {
            return _context.Taquillas.Any(e => e.TaquillaId == id);
        }
    }
}
