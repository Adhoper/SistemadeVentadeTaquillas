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
    [Authorize(Roles = "Administrador")]
    public class CinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cines
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cines.ToListAsync());
        }

        public IActionResult CinesyMas()
        {


            var result = new VMCinesyMas
            {
                Cines = _context.Cines.ToList(),
                Asientos = _context.Asientos.ToList(),
                Salas = _context.Salas.ToList(),

            };

            return View(result);
        }

        // GET: Cines/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cine = await _context.Cines
                .FirstOrDefaultAsync(m => m.CineId == id);
            if (cine == null)
            {
                return NotFound();
            }

            return View(cine);
        }

        // GET: Cines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CineId,NombreCine")] Cine cine)
        {
            if (ModelState.IsValid)
            {
                cine.CineId = Guid.NewGuid();
                _context.Add(cine);
                await _context.SaveChangesAsync();
                return RedirectToAction("CinesyMas", "Cines");
            }
            return View(cine);
        }

        // GET: Cines/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cine = await _context.Cines.FindAsync(id);
            if (cine == null)
            {
                return NotFound();
            }
            return View(cine);
        }

        // POST: Cines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CineId,NombreCine")] Cine cine)
        {
            if (id != cine.CineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CineExists(cine.CineId))
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
            return View(cine);
        }

        // GET: Cines/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cine = await _context.Cines
                .FirstOrDefaultAsync(m => m.CineId == id);
            if (cine == null)
            {
                return NotFound();
            }

            return View(cine);
        }

        // POST: Cines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cine = await _context.Cines.FindAsync(id);
            _context.Cines.Remove(cine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CineExists(Guid id)
        {
            return _context.Cines.Any(e => e.CineId == id);
        }
    }
}
