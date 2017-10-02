using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.ViewModel.Filtro
{
    public class ReportFilter
    {
        public ReportFilter()
        {
        }

        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int IssueId { get; set; }
    }
}
