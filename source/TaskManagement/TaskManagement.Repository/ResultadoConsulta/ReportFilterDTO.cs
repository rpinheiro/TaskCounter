using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Repository.ResultadoConsulta
{
    public class ReportFilterDTO
    {
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int IssueId { get; set; }
    }
}
