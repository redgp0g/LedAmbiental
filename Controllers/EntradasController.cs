using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CadastroFalhas.Models;
using LedAmbiental.Models;
using Microsoft.AspNetCore.Authorization;

namespace LedAmbiental.Controllers
{
    [Authorize]
    public class EntradasController : Controller
    {
        private readonly Contexto _context;

        public EntradasController(Contexto context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.Entrada != null ? 
                          View(await _context.Entrada.ToListAsync()) :
                          Problem("Entity set 'Contexto.Entrada'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Entrada == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entrada
                .FirstOrDefaultAsync(m => m.IDEntrada == id);
            if (entrada == null)
            {
                return NotFound();
            }

            return View(entrada);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Entrada entrada, List<string> materiais, List<decimal> quantidades)
        {
            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Entrada.Add(entrada);
                    await _context.SaveChangesAsync();

                    for (int i = 0; i < materiais.Count; i++)
                    {
                        var material = new Material
                        {
                            Nome = materiais[i],
                            Quantidade = quantidades[i],
                            IDEntrada = entrada.IDEntrada
                        };

                        _context.Material.Add(material);
                    }

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return View("Error");
                }
            }

            return View(entrada);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Entrada == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entrada.FindAsync(id);
            if (entrada == null)
            {
                return NotFound();
            }
            return View(entrada);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Entrada entrada)
        {
            if (id != entrada.IDEntrada)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entrada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntradaExists(entrada.IDEntrada))
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
            return View(entrada);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Entrada == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entrada
                .FirstOrDefaultAsync(m => m.IDEntrada == id);
            if (entrada == null)
            {
                return NotFound();
            }

            return View(entrada);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Entrada == null)
            {
                return Problem("Entity set 'Contexto.Entrada'  is null.");
            }
            var entrada = await _context.Entrada.FindAsync(id);
            if (entrada != null)
            {
                _context.Entrada.Remove(entrada);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntradaExists(int id)
        {
          return (_context.Entrada?.Any(e => e.IDEntrada == id)).GetValueOrDefault();
        }
    }
}
