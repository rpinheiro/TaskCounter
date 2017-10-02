using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaskManagement.BD.Entity;
using TaskManagement.Repository;

namespace TaskManagement
{
    public partial class Form1 : Form
    {
        public Form1()
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
        }        

        private void button3_Click(object sender, EventArgs e)
        {
            int id = (int)this.comboBox1.SelectedValue;
            if (id > 0)
            {
                using (IssueContext db = new IssueContext())
                {
                    Issue issue = db.Issues.Where(c => c.Id == id).ToList().FirstOrDefault();
                    if (issue != null)
                    {
                        IssueExecution issueExecution = db.IssueExecutions.Where(c => c.Issue.Id == id && c.EndDate == null).ToList().FirstOrDefault();
                        if (issueExecution != null)
                        {
                            MessageBox.Show("Esta tarefa já está em andamento");
                            return;
                        }

                        IssueExecution newIssueExecution = new IssueExecution();
                        newIssueExecution.Issue = issue;
                        newIssueExecution.StartDateTime = DateTime.Now;

                        db.IssueExecutions.Add(newIssueExecution);
                        db.SaveChanges();

                        DataGridViewRow dataRow = (DataGridViewRow)this.dataGridView1.Rows[0].Clone();
                        dataRow.Cells[0].Value = newIssueExecution.Id;
                        dataRow.Cells[1].Value = newIssueExecution.Issue.IssueName;
                        dataRow.Cells[2].Value = newIssueExecution.StartDate;
                        dataRow.Cells[3].Value = string.Empty;
                        

                        dataGridView1.Rows.Add(dataRow);
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                using(IssueContext db = new IssueContext())
                {
                    int idTarefa = Convert.ToInt32(row.Cells[0].Value.ToString());
                    
                    IssueExecution issue = db.IssueExecutions.Where(c => c.Id == idTarefa  && c.EndDate == null).ToList().FirstOrDefault();
                    if (issue != null)
                    {
                        DateTime terminateExecution = DateTime.Now;
                        issue.EndDateTime = terminateExecution;
                        db.SaveChanges();
                        row.Cells[3].Value = terminateExecution.ToString();
                    }
                }
            }
        }

        private void cadastroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAddData formAddJira = new FormAddData();
            if ( formAddJira.ShowDialog() == DialogResult.OK ) 
                CarregarComboJiras();
        }

        private void relatórioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReport formReport = new FormReport();
            formReport.ShowDialog();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            TaskManagementRepository repositorio = new TaskManagementRepository();
            var listaTarefasEmExecucacao = repositorio.ObterTarefas(this.checkBox1.Checked);
            this.dataGridView1.DataSource = listaTarefasEmExecucacao;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            TaskManagementRepository repositorio = new TaskManagementRepository();
            var listaTarefasEmExecucacao = repositorio.ObterTarefasUltimos7Dias();
            this.dataGridView1.DataSource = listaTarefasEmExecucacao;
        }

    }
}
