﻿using Capitulo01.Data;
using Microsoft.EntityFrameworkCore;
using Modelo.Cadastros;
using Modelo.Docente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolucaoCapitulo01.Data.DAL.Cadastros
{
    public class CursoDAL
    {
        private IESContext _context;

        public CursoDAL(IESContext context)
        {
            _context = context;
        }

        public IQueryable<Curso> ObterCursosClassificadosPorNome()
        {
            return _context.Cursos.OrderBy(b => b.Nome);
        }

        public IQueryable<Curso> ObterCursosClassificadosPorDepartamento(long departamentoID)
        {
            return _context.Cursos.Where(c => c.DepartamentoID == departamentoID).OrderBy(d => d.Nome);
        }

        public IQueryable<Curso> ObterCursosPorDepartamento(long departamentoID)
        {
            var cursos = _context.Cursos.Where(c => c.DepartamentoID == departamentoID).OrderBy(d => d.Nome);
            return cursos;
        }

        public void RegistrarProfessor(long cursoID, long professorID)
        {
            var curso = _context.Cursos.Where(c => c.CursoID == cursoID)
                .Include(cp => cp.CursosProfessores).First();
            var professor = _context.Professores.Find(professorID);
            curso.CursosProfessores.Add(new CursoProfessor() { Curso = curso, Professor = professor });
            _context.SaveChanges();
        }

        public IQueryable<Professor> ObterProfessoresForaDoCurso(long cursoID)
        {
            var curso = _context.Cursos.Where(c => c.CursoID == cursoID)
                .Include(cp => cp.CursosProfessores).First();
            var professoresDoCurso = curso.CursosProfessores.Select(cp => cp.ProfessorID).ToArray();
            var professoresForaDoCurso = _context.Professores
                .Where(p => !professoresDoCurso.Contains(p.ProfessorID));
            return professoresForaDoCurso;
        }
    }
}
