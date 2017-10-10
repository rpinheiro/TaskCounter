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
using TaskManagement.Repository.ResultadoConsulta;

namespace TaskManagement
{
    public partial class FormDeleteData : Form
    {
        public FormDeleteData()
        {
            InitializeComponent();
            this.dateTimePicker2.Format = DateTimePickerFormat.Time;
            this.dateTimePicker2.ShowUpDown = true;

            this.dateTimePicker3.Format = DateTimePickerFormat.Time;
            this.dateTimePicker3.ShowUpDown = true;

            CarregarComboTarefasPais();
        }

        private void CarregarComboTarefasPais()
        {
            TaskManagementRepository repositorio = new TaskManagementRepository();
            var listaTarefasPais = repositorio.ObterTodasTarefasPaisCadastradas();
            this.comboBox1.BeginUpdate();
            this.comboBox1.DataSource = listaTarefasPais;
            comboBox1.DisplayMember = "CodigoJira";
            comboBox1.ValueMember = "Id";
            this.comboBox1.EndUpdate();

        }




        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Selecione uma ou mais linhas para excluir a tarefa desejada.");
                    return;
                }

                IList<int> listaIdsTarefasSelecionadas = new List<int>();
                foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
                {
                    int idTarefa = Convert.ToInt32(row.Cells[0].Value.ToString());
                    listaIdsTarefasSelecionadas.Add(idTarefa);
                }

                TaskManagementRepository repositorio = new TaskManagementRepository();
                repositorio.RemoverTarefasExecutadas(listaIdsTarefasSelecionadas);

                var listaTarefas = repositorio.ObterTodasAsExecucoesDaTarefa(ObterIdTarefaSelecionada());
                this.dataGridView1.DataSource = listaTarefas.Select(c => new { c.Id, c.CodigoJira, c.StartTime, c.EndTime }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar o jira: " + ex.Message);
            }
        }

        private int ObterIdTarefaSelecionada()
        {
            int idSelected = 0;
            IssueDTO issueDTO = this.comboBox1.SelectedValue as IssueDTO;
            if (issueDTO != null)
                idSelected = issueDTO.Id;
            else
                idSelected = (int)this.comboBox1.SelectedValue;

            return idSelected;
        }

        private void OnSelCombBoxChanged(object sender, EventArgs e)
        {
            int indexSelected = (int)this.comboBox1.SelectedIndex;
            if (indexSelected >= 0)
            {
                int idSelected = ObterIdTarefaSelecionada();
                TaskManagementRepository repositorio = new TaskManagementRepository();
                var listaTarefas = repositorio.ObterTodasAsExecucoesDaTarefa(idSelected);
                this.dataGridView1.DataSource = listaTarefas.Select(c => new { c.Id, c.CodigoJira, c.StartTime, c.EndTime }).ToList();
            }
        }

    }
}
