using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LedAmbiental.Models;
using System.Globalization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace LedAmbiental.Controllers
{
    [Authorize]
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

        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
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
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movimentacao);
        }

        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
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

        [HttpGet]
        public string GetMateriais()
        {
            var entradasAterro = _context.Movimentacao
                .Where(m => m.Local == "Aterro" && m.Tipo == "Entrada")
                .GroupBy(m => new { m.Material })
                .Select(group => new
                {
                    Material = group.Key.Material,
                    QuantidadeTotalEntradaAterro = group.Sum(m => m.Quantidade)
                })
                .ToList();

            var saidasAterro = _context.Movimentacao
                .Where(m => m.Local == "Aterro" && m.Tipo == "Saída")
                .GroupBy(m => new { m.Material })
                .Select(group => new
                {
                    Material = group.Key.Material,
                    QuantidadeTotalSaidaAterro = group.Sum(m => m.Quantidade)
                })
                .ToList();

            var entradasUsina = _context.Movimentacao
                .Where(m => m.Local == "Usina" && m.Tipo == "Entrada")
                .GroupBy(m => new { m.Material })
                .Select(group => new
                {
                    Material = group.Key.Material,
                    QuantidadeTotalEntradaUsina = group.Sum(m => m.Quantidade)
                })
                .ToList();

            var saidasUsina = _context.Movimentacao
                .Where(m => m.Local == "Usina" && m.Tipo == "Saída")
                .GroupBy(m => new { m.Material })
                .Select(group => new
                {
                    Material = group.Key.Material,
                    QuantidadeTotalSaidaUsina = group.Sum(m => m.Quantidade)
                })
                .ToList();

            var materiais = entradasAterro.Select(d => d.Material).Union(saidasAterro.Select(d => d.Material))
                .Union(entradasUsina.Select(d => d.Material)).Union(saidasUsina.Select(d => d.Material));

            var resultado = materiais.Select(material => new
            {
                Material = material,
                QuantidadeTotalAterro = (entradasAterro.FirstOrDefault(d => d.Material == material)?.QuantidadeTotalEntradaAterro ?? 0) -
                                       (saidasAterro.FirstOrDefault(d => d.Material == material)?.QuantidadeTotalSaidaAterro ?? 0),
                QuantidadeTotalUsina = (entradasUsina.FirstOrDefault(d => d.Material == material)?.QuantidadeTotalEntradaUsina ?? 0) -
                                      (saidasUsina.FirstOrDefault(d => d.Material == material)?.QuantidadeTotalSaidaUsina ?? 0)
            });

            string json = JsonConvert.SerializeObject(resultado, Formatting.Indented);
            return json;
        }

    }
}
