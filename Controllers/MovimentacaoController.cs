using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LedAmbiental.Models;
using System.Globalization;

namespace LedAmbiental.Controllers
{
    public class MovimentacaoController : Controller
    {
        private readonly Contexto _context;

        public MovimentacaoController(Contexto context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.Movimentacao != null ? 
                          View(await _context.Movimentacao.ToListAsync()) :
                          Problem("Entity set 'Contexto.Movimentacao'  is null.");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movimentacao movimentacao, List<string> materiais, List<string> quantidades)
        {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    for (int i = 0; i < materiais.Count; i++)
                    {
                        var material = new Movimentacao
                        {
                            Caminhao = movimentacao.Caminhao,
                            Local = movimentacao.Local,
                            Tipo = movimentacao.Tipo,
                            Material = materiais[i],
                            Quantidade = decimal.Parse(quantidades[i], CultureInfo.InvariantCulture),

                        };

                        _context.Movimentacao.Add(material);
                    }

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return View(movimentacao);
                }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movimentacao == null)
            {
                return NotFound();
            }

            var movimentacao = await _context.Movimentacao.FindAsync(id);
            if (movimentacao == null)
            {
                return NotFound();
            }
            return View(movimentacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int IDMovimentacao,Movimentacao movimentacao)
        {
            if (IDMovimentacao != movimentacao.IDMovimentacao)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movimentacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovimentacaoExists(movimentacao.IDMovimentacao))
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
            return View(movimentacao);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movimentacao == null)
            {
                return NotFound();
            }

            var movimentacao = await _context.Movimentacao
                .FirstOrDefaultAsync(m => m.IDMovimentacao == id);
            if (movimentacao == null)
            {
                return NotFound();
            }

            return View(movimentacao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IDMovimentacao)
        {
            if (_context.Movimentacao == null)
            {
                return Problem("Entity set 'Contexto.Movimentacao'  is null.");
            }
            var movimentacao = await _context.Movimentacao.FindAsync(IDMovimentacao);
            if (movimentacao != null)
            {
                _context.Movimentacao.Remove(movimentacao);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovimentacaoExists(int id)
        {
          return (_context.Movimentacao?.Any(e => e.IDMovimentacao == id)).GetValueOrDefault();
        }
    }
}
