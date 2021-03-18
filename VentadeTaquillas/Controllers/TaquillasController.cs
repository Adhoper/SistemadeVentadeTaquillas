using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VentadeTaquillas.Data;

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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Taquillas.ToListAsync());
        }

        // GET: Taquillas/Details/5
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
        public IActionResult Create()
        {
            return View();
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
                taquilla.TaquillaId = Guid.NewGuid();
                _context.Add(taquilla);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taquilla);
        }

        public IActionResult Factura()
        {
            return View();
        }

        // GET: Taquillas/Edit/5
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
