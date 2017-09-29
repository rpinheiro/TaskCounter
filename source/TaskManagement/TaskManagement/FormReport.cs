using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaskManagement.BD.Entity;


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
            using (IssueContext db = new IssueContext())
            {
                var resultado = db.IssueExecutions.AsQueryable();

                if (this.comboBox1.SelectedIndex > -1)
                    resultado = resultado.Where(c => c.Issue.Id == (int)this.comboBox1.SelectedValue);



                if (this.dateTimePickerStart.Value != null)
                {
                    DateTime tempDate = new DateTime(this.dateTimePickerStart.Value.Year, this.dateTimePickerStart.Value.Month, this.dateTimePickerStart.Value.Day, 0, 0, 0);
                    resultado = resultado.Where(c => c.StartDate >= tempDate);
                }

                if (this.dateTimePickerEnd.Value != null)
                {
                    DateTime tempDate = new DateTime(this.dateTimePickerEnd.Value.Year, this.dateTimePickerEnd.Value.Month, this.dateTimePickerEnd.Value.Day, 23, 59, 59);
                    resultado = resultado.Where(c => c.EndDate != null && c.EndDate.Value <= tempDate);
                }

                var listaTarefas = resultado.ToList();

                var listaResultado = listaTarefas.Select(c => new { CodigoJira = c.Issue.IssueName, TempoGasto = ((TimeSpan)(c.EndDate - c.StartDate)).TotalMinutes })
                         .GroupBy(g => new { g.CodigoJira, g.TempoGasto })
                         .Select(g => new { CodigoJira = g.Key.CodigoJira, Total = g.Sum(z => z.TempoGasto) }).ToList();



            }

        }


    }
}
