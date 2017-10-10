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
                TaskManagementRepository repositorio = new TaskManagementRepository();
                IssueExecution tarefaEmAndamento = repositorio.ObterTarefaEmAndamento(id);
                if (tarefaEmAndamento != null)
                {
                    MessageBox.Show("Esta tarefa já está em andamento");
                    return;
                }

                repositorio.IniciarAtendimentoTarefa(id);
                var listaTarefasEmExecucacao = repositorio.ObterTarefas(true);
                this.dataGridView1.DataSource = listaTarefasEmExecucacao.Select(c => new { c.Id, c.CodigoJira, c.StartTime, c.EndTime }).ToList();             
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione uma ou mais linhas para encerrar o atendimento.");
                return;
            }

            TaskManagementRepository repositorio = new TaskManagementRepository();
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                int idTarefa = Convert.ToInt32(row.Cells[0].Value.ToString());
                repositorio.EncerrarAtendimentoTarefa(idTarefa);
            }

            var listaTarefasEmExecucacao = repositorio.ObterTarefas(true);
            this.dataGridView1.DataSource = listaTarefasEmExecucacao;
        }

        private void cadastroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAddData formAddJira = new FormAddData();
            if (formAddJira.ShowDialog() == DialogResult.OK)
                CarregarComboJiras();
        }

        private void relatórioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReport formReport = new FormReport();
            formReport.ShowDialog();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            TaskManagementRepository repositorio = new TaskManagementRepository();
            var listaTarefasEmExecucacao = repositorio.ObterTarefasUltimos7Dias();
            this.dataGridView1.DataSource = listaTarefasEmExecucacao.Select(c => new { c.Id, c.CodigoJira, c.StartTime, c.EndTime }).ToList();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            TaskManagementRepository repositorio = new TaskManagementRepository();
            var listaTarefasEmExecucacao = repositorio.ObterTarefas(true);
            this.dataGridView1.DataSource = listaTarefasEmExecucacao.Select(c => new { c.Id, c.CodigoJira, c.StartTime, c.EndTime }).ToList();
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditData formEdit = new FormEditData();
            formEdit.ShowDialog();
        }

        private void excluirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDeleteData formDelete = new FormDeleteData();
            formDelete.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (int)this.comboBox1.SelectedValue;
                if (id > 0)
                {
                    if (MessageBox.Show("Tem certeza que deseja remover a tarefa " + this.comboBox1.SelectedText + " ?", "Aviso", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        TaskManagementRepository repositorio = new TaskManagementRepository();
                        repositorio.RemoverTarefaPai(id);
                    }
                    MessageBox.Show("Tarefa removida com sucesso.");
                    CarregarComboJiras();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao remover a tarefa: " + ex.Message);
            }
        }

    }
}
