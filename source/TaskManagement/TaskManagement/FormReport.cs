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

                if( this.dateTimePickerStart.Value != null )
                {
                    string startDateString = this.dateTimePickerStart.Value.Date.ToString("yyyy-MM-dd");
                    resultado = resultado.Where(c => c.StartDate.Contains(startDateString));
                }

                if (this.dateTimePickerEnd.Value != null)
                {
                    string endDateString = this.dateTimePickerEnd.Value.Date.ToString("yyyy-MM-dd"); 
                    resultado = resultado.Where(c => c.EndDate != null && c.EndDate.Contains(endDateString));
                }

                IList<IssueExecution> listaTarefas = resultado.ToList();
                                
            }

        }


    }
}
