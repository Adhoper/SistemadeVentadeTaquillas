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
    public class AsientoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AsientoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Asientoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Asientos.ToListAsync());
        }

        // GET: Asientoes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asiento = await _context.Asientos
                .FirstOrDefaultAsync(m => m.AsientoId == id);
            if (asiento == null)
            {
                return NotFound();
            }

            return View(asiento);
        }

        // GET: Asientoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Asientoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AsientoId,NumeroAsiento,Estado,SalaId")] Asiento asiento)
        {
            if (ModelState.IsValid)
            {
                asiento.AsientoId = Guid.NewGuid();
                _context.Add(asiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(asiento);
        }

        // GET: Asientoes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asiento = await _context.Asientos.FindAsync(id);
            if (asiento == null)
            {
                return NotFound();
            }
            return View(asiento);
        }

        // POST: Asientoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AsientoId,NumeroAsiento,Estado,SalaId")] Asiento asiento)
        {
            if (id != asiento.AsientoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsientoExists(asiento.AsientoId))
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
            return View(asiento);
        }

        // GET: Asientoes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asiento = await _context.Asientos
                .FirstOrDefaultAsync(m => m.AsientoId == id);
            if (asiento == null)
            {
                return NotFound();
            }

            return View(asiento);
        }

        // POST: Asientoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var asiento = await _context.Asientos.FindAsync(id);
            _context.Asientos.Remove(asiento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AsientoExists(Guid id)
        {
            return _context.Asientos.Any(e => e.AsientoId == id);
        }
    }
}
