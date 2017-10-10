using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.BD.Entity;
using TaskManagement.Repository.ResultadoConsulta;


namespace TaskManagement.Repository
{
    public class TaskManagementRepository
    {
        public TaskManagementRepository()
        {
        }

        public IList<ReportDTO> ObterTarefasExecutadas(ReportFilterDTO reportFilter)
        {
            using (IssueContext db = new IssueContext())
            {
                var resultado = db.IssueExecutions.AsQueryable();

                if (reportFilter.IssueId > 0)
                    resultado = resultado.Where(c => c.Issue.Id == reportFilter.IssueId);

                var listaTarefas = resultado.ToList();

                if (reportFilter.StartDateTime != null)
                {
                    listaTarefas = listaTarefas.Where(c => c.StartDateTime.Value.Date >= reportFilter.StartDateTime.Value.Date).ToList();
                }

                if (reportFilter.EndDateTime != null)
                {
                    listaTarefas = listaTarefas.Where(c => c.EndDateTime != null && c.EndDateTime.Value.Date <= reportFilter.EndDateTime.Value.Date).ToList();
                }

                IList<ReportDTO> listaResultado = listaTarefas.Select(c => new { CodigoJira = c.Issue.IssueName, TempoGasto = ((TimeSpan)(c.EndDateTime.Value - c.StartDateTime.Value)).TotalHours })
                         .GroupBy(g => new { g.CodigoJira, g.TempoGasto })
                         .Select(g => new ReportDTO() { CodigoJira = g.Key.CodigoJira, TempoTotal = g.Sum(z => z.TempoGasto) }).OrderBy(c => c.CodigoJira).ToList();

                return listaResultado;
            }
        }

        public IList<IssueExecutionDTO> ObterTarefas(bool bEmAndamento = false, bool bUltimos7dias = false)
        {
            using (IssueContext db = new IssueContext())
            {
                var resultado = db.IssueExecutions.AsQueryable();

                if (bEmAndamento)
                    resultado = resultado.Where(c => c.EndDate == null);

                if (bUltimos7dias)
                {

                }

                var listaTarefas = resultado.ToList();

                var listaTarefasDTO = listaTarefas.Select(c => new IssueExecutionDTO() { CodigoJira = c.Issue.IssueName, StartTime = c.StartDateTime.Value, EndTime = c.EndDateTime, Id = c.Id, CodigoTarefaPai = c.Issue.Id }).ToList();

                return listaTarefasDTO;
            }
        }

        public IList<IssueExecutionDTO> ObterTarefasUltimos7Dias()
        {
            using (IssueContext db = new IssueContext())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT IE.Id, IE.Issue_Id, IE.StartDate, IE.EndDate ");
                sb.Append("FROM IssueExecution IE, Issue I ");
                sb.Append("where IE.Issue_Id = I.Id and  IE.StartDate >= '");
                sb.Append(DateTime.Now.Date.AddDays(-7).ToString("yyyy-MM-dd"));
                sb.Append("'");

                IList<IssueExecutionDTO> lista = db.IssueExecutions.SqlQuery(sb.ToString()).ToList<IssueExecution>().Select(c => new IssueExecutionDTO() { CodigoJira = c.Issue.IssueName, StartTime = c.StartDateTime.Value, EndTime = c.EndDateTime, Id = c.Id, CodigoTarefaPai = c.Issue.Id }).ToList();

                return lista.OrderBy(c => c.StartTime).ToList();
            }
        }

        public IssueExecution ObterTarefaEmAndamento(int idTarefa)
        {
            using (IssueContext db = new IssueContext())
            {
                IssueExecution issue = db.IssueExecutions.Where(c => c.Issue.Id == idTarefa && c.EndDate == null).ToList().FirstOrDefault();
                return issue;
            }
        }

        public IssueExecution ObterTarefaEmAndamentoPorTarefaExecucao(int idTarefa)
        {
            using (IssueContext db = new IssueContext())
            {
                IssueExecution issue = db.IssueExecutions.Where(c => c.Id == idTarefa && c.EndDate == null).ToList().FirstOrDefault();
                return issue;
            }
        }

        public void IniciarAtendimentoTarefa(int idTarefa)
        {
            try
            {
                using (IssueContext db = new IssueContext())
                {
                    Issue issue = db.Issues.Where(c => c.Id == idTarefa).ToList().FirstOrDefault();

                    IssueExecution newIssueExecution = new IssueExecution();
                    newIssueExecution.Issue = issue;
                    newIssueExecution.StartDateTime = DateTime.Now;

                    db.IssueExecutions.Add(newIssueExecution);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EncerrarAtendimentoTarefa(int idTarefa)
        {
            using (IssueContext db = new IssueContext())
            {
                IssueExecution issue = db.IssueExecutions.Where(c => c.Id == idTarefa && c.EndDate == null).ToList().FirstOrDefault();
                if (issue != null)
                {
                    DateTime terminateExecution = DateTime.Now;
                    issue.EndDateTime = terminateExecution;
                    db.SaveChanges();
                }
            }
        }

        public void AtualizarTarefa(IssueExecution newIssue)
        {
            using (IssueContext db = new IssueContext())
            {
                IssueExecution oldIssue = db.IssueExecutions.Where(c => c.Id == newIssue.Id).ToList().FirstOrDefault();
                if (oldIssue != null)
                {
                    oldIssue.EndDateTime = newIssue.EndDateTime;
                    db.SaveChanges();
                }
            }
        }

        public IList<IssueExecutionDTO> ObterTodasAsExecucoesDaTarefa(int idTarefa)
        {
            using (IssueContext db = new IssueContext())
            {
                var resultado = db.IssueExecutions.AsQueryable();

                resultado = resultado.Where(c => c.Issue.Id == idTarefa);

                var listaTarefas = resultado.ToList();

                var listaTarefasDTO = listaTarefas.Select(c => new IssueExecutionDTO() { CodigoJira = c.Issue.IssueName, StartTime = c.StartDateTime.Value, EndTime = c.EndDateTime, Id = c.Id, CodigoTarefaPai = c.Issue.Id }).ToList();

                return listaTarefasDTO;
            }
        }

        public IList<IssueDTO> ObterTodasTarefasPaisCadastradas()
        {
            using (IssueContext db = new IssueContext())
            {
                return db.Issues.Select(c => new IssueDTO() { Id = c.Id, CodigoJira = c.IssueName }).ToList();
            }
        }

        public void RemoverTarefasExecutadas(IList<int> listaIdsTarefasSelecionadas)
        {
            using (IssueContext db = new IssueContext())
            {
                try
                {
                    var resultado = db.IssueExecutions.AsQueryable();
                    resultado = resultado.Where(c => listaIdsTarefasSelecionadas.Contains(c.Id));
                    var listaTarefas = resultado.ToList();
                    foreach (var tarefa in listaTarefas)
                    {
                        db.IssueExecutions.Remove(tarefa);
                    }

                    db.SaveChanges();
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
        }

        public IssueDTO ObterTarefaPai(int id)
        {
            using (IssueContext db = new IssueContext())
            {
                return db.Issues.Where(c => c.Id == id).Select(c => new IssueDTO() { Id = c.Id, CodigoJira = c.IssueName }).FirstOrDefault();
            }
        }

        public void RemoverTarefaPai(int id)
        {
            using (IssueContext db = new IssueContext())
            {
                Issue issue = db.Issues.Where(c => c.Id == id).FirstOrDefault();
                if (issue != null)
                {
                    db.Issues.Remove(issue);
                    db.SaveChanges();
                }
            }
        }
    }
}
