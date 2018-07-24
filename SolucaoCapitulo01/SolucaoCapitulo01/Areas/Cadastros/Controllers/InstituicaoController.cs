using System.Linq;
using System.Threading.Tasks;
using Capitulo01.Data;
using Modelo.Cadastros;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolucaoCapitulo01.Data.DAL.Cadastros;
using Microsoft.AspNetCore.Authorization;

namespace Capitulo01.Areas.Cadastros.Controllers
{
    [Area("Cadastros")]
    [Authorize]
    public class InstituicaoController : Controller
    {
        private readonly IESContext _context;
        private readonly InstituicaoDAL instituicaoDAL;

        public InstituicaoController(IESContext context)
        {
            this._context = context;
            instituicaoDAL = new InstituicaoDAL(context);
        }

        public async Task<IActionResult> Index()
        {
            //return View(await _context.Instituicoes.OrderBy(c => c.Nome).ToListAsync());
            return View(await instituicaoDAL.ObterInstituicoesClassificadasPorNome().ToListAsync());
        }

        private async Task<IActionResult> ObterVisaoInstituicaoPorId(long? id)
        {
            if (id == null)
                return NotFound();

            var instituicao = await instituicaoDAL.ObterInstituicaoPorId((long) id);

            if (instituicao == null)
                return NotFound();

            return View(instituicao);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Endereco")] Instituicao instituicao)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //_context.Add(instituicao);
                    //await _context.SaveChangesAsync();
                    await instituicaoDAL.GravarInstituicao(instituicao);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }

            return View(instituicao);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            //if (id == null)
            //    return NotFound();

            //var instituicao = await _context.Instituicoes.SingleOrDefaultAsync(m => m.InstituicaoID == id);

            //if (instituicao == null)
            //    return NotFound();

            //return View(instituicao);
            return await ObterVisaoInstituicaoPorId(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("InstituicaoID,Nome,Endereco")] Instituicao instituicao)
        {
            if (id != instituicao.InstituicaoID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(instituicao);
                    //await _context.SaveChangesAsync();
                    await instituicaoDAL.GravarInstituicao(instituicao);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await InstituicaoExists(instituicao.InstituicaoID))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(instituicao);
        }

        private async Task<bool> InstituicaoExists(long? id)
        {
            //return _context.Instituicoes.Any(e => e.InstituicaoID == id);
            return await instituicaoDAL.ObterInstituicaoPorId((long) id) != null;
        }

        public async Task<IActionResult> Details(long? id)
        {
            //if (id == null)
            //    return NotFound();

            //var instituicao = await _context.Instituicoes
            //    .Include(d => d.Departamentos)
            //    .SingleOrDefaultAsync(m => m.InstituicaoID == id);

            //if (instituicao == null)
            //    return NotFound();

            //return View(instituicao);
            return await ObterVisaoInstituicaoPorId(id);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            //if (id == null)
            //    return NotFound();

            //var instituicao = await _context.Instituicoes.SingleOrDefaultAsync(m => m.InstituicaoID == id);

            //if (instituicao == null)
            //    return NotFound();

            //return View(instituicao);
            return await ObterVisaoInstituicaoPorId(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)//(Instituicao instituicao)
        {
            //var instituicao = await _context.Instituicoes.SingleOrDefaultAsync(m => m.InstituicaoID == id);

            //_context.Instituicoes.Remove(instituicao);

            var instituicao = await EliminiarInstituicaoPorId((long)id);

            TempData["Message"] = "Instituição " + instituicao.Nome.ToUpper() + " foi removida";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<Instituicao> EliminiarInstituicaoPorId(long id)
        {
            var instituicao = await new InstituicaoDAL(_context).ObterInstituicaoPorId(id);
            _context.Instituicoes.Remove(instituicao);
            await _context.SaveChangesAsync();
            return instituicao;
        }
    }
}