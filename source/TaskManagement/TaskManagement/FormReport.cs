using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaskManagement.BD.Entity;
using TaskManagement.BD.Services;
using TaskManagement.Repository;
using TaskManagement.Repository.ResultadoConsulta;
using TaskManagement.ViewModel.Filtro;


namespace TaskManagement
{
    public partial class FormReport : Form
    {
        public FormReport()
        {
            InitializeComponent();

            CarregarComboJiras();

        }

        private void CarregarComboJiras()
        {
            using (IssueContext db = new IssueContext())
            {
                IList<TaskManagement.BD.Entity.Issue> listIssue = db.Issues.ToList();
                comboBox1.DataSource = listIssue;
                comboBox1.DisplayMember = "IssueName";
                comboBox1.ValueMember = "Id";
            }

            this.comboBox1.SelectedIndex = -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReportFilterDTO reportFilter = new ReportFilterDTO();
            if ( this.comboBox1.SelectedValue != null )
                reportFilter.IssueId = (int)this.comboBox1.SelectedValue;
            reportFilter.StartDateTime = this.dateTimePickerStart.Value;
            reportFilter.EndDateTime = this.dateTimePickerEnd.Value;

            TaskManagementRepository repository = new TaskManagementRepository();
            IList<ReportDTO> listaResultado = repository.ObterTarefasExecutadas(reportFilter);

            this.dataGridView1.DataSource = listaResultado.ToList();

        }

        

    }
}
