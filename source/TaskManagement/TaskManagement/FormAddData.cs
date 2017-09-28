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
    public partial class FormAddData : Form
    {
        public FormAddData()
        {
            InitializeComponent();

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

                    MessageBox.Show("Jira cadastrado");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar o jira: " + ex.Message);
            }
        }

    }
}
