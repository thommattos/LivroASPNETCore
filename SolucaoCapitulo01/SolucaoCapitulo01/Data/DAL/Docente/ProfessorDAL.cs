﻿using Capitulo01.Data;
using Modelo.Docente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolucaoCapitulo01.Data.DAL.Docente
{
    public class ProfessorDAL
    {
        private IESContext _context;

        public ProfessorDAL(IESContext context)
        {
            _context = context;
        }

        public IQueryable<Professor> ObterProfessoresClassificadosPorNome()
        {
            return _context.Professores.OrderBy(b => b.Nome);
        }
    }
}
