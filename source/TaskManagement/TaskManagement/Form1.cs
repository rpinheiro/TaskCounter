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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (IssueContext db = new IssueContext())
                {
                    string issueName = this.textBox1.Text;

                    Issue newIssue = new Issue();
                    newIssue.IssueName = issueName;

                    db.Issues.Add(newIssue);
                    db.SaveChanges();

                    CarregarComboJiras();

                    MessageBox.Show("Jira cadastrado");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar o jira: " + ex.Message);
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
                        IssueExecution issueExecution = new IssueExecution();
                        issueExecution.Issue = issue;
                        issueExecution.StartDate = DateTime.Now.ToString();

                        db.IssueExecutions.Add(issueExecution);
                        db.SaveChanges();

                        DataGridViewRow dataRow = (DataGridViewRow)this.dataGridView1.Rows[0].Clone();
                        dataRow.Cells[0].Value = issue.IssueName;
                        dataRow.Cells[1].Value = issueExecution.StartDate;
                        dataRow.Cells[2].Value = string.Empty;

                        dataGridView1.Rows.Add(dataRow);
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                row.Cells[2].Value = DateTime.Now.ToString();
            }
        }

    }
}
