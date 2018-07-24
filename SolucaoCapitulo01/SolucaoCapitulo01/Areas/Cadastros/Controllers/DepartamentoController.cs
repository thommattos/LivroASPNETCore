using System.Linq;
using System.Threading.Tasks;
using Capitulo01.Data;
using Modelo.Cadastros;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SolucaoCapitulo01.Data;
using SolucaoCapitulo01.Data.DAL.Cadastros;

namespace Capitulo01.Areas.Cadastros.Controllers
{
    [Area("Cadastros")]
    public class DepartamentoController : Controller
    {
        private readonly IESContext _context;
        private readonly DepartamentoDAL departamentoDAL;
        private readonly InstituicaoDAL instituicaoDAL;

        public DepartamentoController(IESContext context)
        {
            _context = context;
            instituicaoDAL = new InstituicaoDAL(context);
            departamentoDAL = new DepartamentoDAL(context);
        }

        public async Task<IActionResult> Index()
        {
            /*return View(await _context.Departamentos.Include(i => i.Instituicao)
                .OrderBy(c => c.Nome).ToListAsync());*/
            return View(await departamentoDAL.ObterDepartamentosClassificadosPorNome().ToListAsync());
        }

        public IActionResult Create()
        {
            //var instituicoes = _context.Instituicoes.OrderBy(i => i.Nome).ToList();
            var instituicoes = instituicaoDAL.ObterInstituicoesClassificadasPorNome().ToList();

            instituicoes.Insert(0, new Instituicao() {
                InstituicaoID = 0, Nome = "Selecione a instituição" });

            ViewBag.Instituicoes = instituicoes;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome, InstituicaoID")] Departamento departamento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //_context.Add(departamento);
                    //await _context.SaveChangesAsync();
                    await departamentoDAL.GravarDepartamento(departamento);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
                        
            return View(departamento);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            //if (id == null)
            //    return NotFound();

            //var departamento = await _context.Departamentos.SingleOrDefaultAsync(m => m.DepartamentoID == id);

            //if (departamento == null)
            //    return NotFound();

            //return View(departamento);

            ViewResult visaoDepartamento = (ViewResult)await ObterVisaoDepartamentoPorId(id);
            Departamento departamento = (Departamento)visaoDepartamento.Model;
            ViewBag.Instituicoes = new SelectList(//_context.Instituicoes.OrderBy(b => b.Nome), 
                instituicaoDAL.ObterInstituicoesClassificadasPorNome(),
                "InstituicaoID", "Nome", departamento.InstituicaoID);
            return visaoDepartamento;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("DepartamentoID,Nome,InstituicaoID")] Departamento departamento)
        {
            if (id != departamento.DepartamentoID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(departamento);
                    //await _context.SaveChangesAsync();
                    await departamentoDAL.GravarDepartamento(departamento);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await DepartamentoExists(departamento.DepartamentoID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Instituicoes = new SelectList(_context.Instituicoes.OrderBy(b => b.Nome),
                "InstituicaoID", "Nome", departamento.InstituicaoID);

            return View(departamento);
        }

        /*private bool DepartamentoExists(long? id)
        //{
        //    return _context.Departamentos.Any(e => e.DepartamentoID == id);
        //}*/
        private async Task<bool> DepartamentoExists(long? id)
        {
            return await departamentoDAL.ObterDepartamentoPorId((long)id) != null;
        }

        public async Task<IActionResult> Details(long? id)
        {
            //if (id == null)
            //    return NotFound();

            //var departamento = await _context.Departamentos.SingleOrDefaultAsync(m => m.DepartamentoID == id);

            //_context.Instituicoes.Where(i => departamento.InstituicaoID == i.InstituicaoID).Load();

            //if (departamento == null)
            //    return NotFound();

            //return View(departamento);
            return await ObterVisaoDepartamentoPorId(id);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            //if (id == null)
            //    return NotFound();

            //var departamento = await _context.Departamentos.SingleOrDefaultAsync(m => m.DepartamentoID == id);

            //_context.Instituicoes.Where(i => departamento.InstituicaoID == i.InstituicaoID).Load();

            //if (departamento == null)
            //    return NotFound();

            //return View(departamento);
            return await ObterVisaoDepartamentoPorId(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            //var departamento = await _context.Departamentos.SingleOrDefaultAsync(m => m.DepartamentoID == id);

            //_context.Departamentos.Remove(departamento);

            var departamento = await departamentoDAL.EliminarDepartamentoPorId((long)id);

            TempData["Message"] = "Departamento " + departamento.Nome.ToUpper() + " foi removido";

            //await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        private async Task<IActionResult> ObterVisaoDepartamentoPorId(long? id)
        {
            if (id == null)
                return NotFound();

            var departamento = await departamentoDAL.ObterDepartamentoPorId((long)id);

            if (departamento == null)
                return NotFound();

            return View(departamento);
        }
    }
}