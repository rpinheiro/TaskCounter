using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Repository.ResultadoConsulta
{
    public class IssueExecutionDTO
    {
        public int Id { get; set; }
        public string CodigoJira { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public int CodigoTarefaPai { get; set; }
    }
}
