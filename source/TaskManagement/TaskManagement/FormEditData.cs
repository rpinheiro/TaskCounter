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
    public partial class FormEditData : Form
    {
        public FormEditData()
        {
            InitializeComponent();
            this.dateTimePicker2.Format = DateTimePickerFormat.Time;
            this.dateTimePicker2.ShowUpDown = true;

            this.dateTimePicker3.Format = DateTimePickerFormat.Time;
            this.dateTimePicker3.ShowUpDown = true;

            CarregarComboTarefasEmAndamento();
        }

        private void CarregarComboTarefasEmAndamento()
        {
            TaskManagementRepository repositorio = new TaskManagementRepository();
            var listaTarefasEmExecucacao = repositorio.ObterTarefas(true);
            this.comboBox1.BeginUpdate();
            this.comboBox1.DataSource = listaTarefasEmExecucacao;
            comboBox1.DisplayMember = "CodigoJira";
            comboBox1.ValueMember = "Id";
            this.comboBox1.EndUpdate();
            
        }




        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (int)this.comboBox1.SelectedValue;
                if (id > 0)
                {
                    TaskManagementRepository repositorio = new TaskManagementRepository();
                    IssueExecution issue = repositorio.ObterTarefaEmAndamentoPorTarefaExecucao(id);
                    var a = this.dateTimePicker1.Value + this.dateTimePicker2.Value.TimeOfDay;

                    issue.StartDateTime = new DateTime(this.dateTimePicker4.Value.Date.Year, this.dateTimePicker4.Value.Date.Month, this.dateTimePicker4.Value.Date.Day, this.dateTimePicker3.Value.Hour, this.dateTimePicker3.Value.Minute, this.dateTimePicker3.Value.Second);
                    issue.EndDateTime = new DateTime(this.dateTimePicker1.Value.Date.Year, this.dateTimePicker1.Value.Date.Month, this.dateTimePicker1.Value.Date.Day, this.dateTimePicker2.Value.Hour, this.dateTimePicker2.Value.Minute, this.dateTimePicker2.Value.Second);
                    repositorio.AtualizarTarefa(issue);
                    MessageBox.Show("Jira atualizado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar o jira: " + ex.Message);
            }
        }

        private void OnSelCombBoxChanged(object sender, EventArgs e)
        {
            int indexSelected = (int)this.comboBox1.SelectedIndex;
            if (indexSelected >= 0)
            {
                int idSelected = 0;
                IssueExecutionDTO issueDTO = this.comboBox1.SelectedValue as IssueExecutionDTO;
                if (issueDTO != null)
                    idSelected = issueDTO.Id;
                else
                    idSelected = (int)this.comboBox1.SelectedValue;

                TaskManagementRepository repositorio = new TaskManagementRepository();
                IssueExecution issue = repositorio.ObterTarefaEmAndamentoPorTarefaExecucao(idSelected);
                if ( issue != null )
                {
                    this.dateTimePicker4.Value = issue.StartDateTime.Value.Date;
                    this.dateTimePicker3.Value = issue.StartDateTime.Value;
                    var endDate = issue.EndDateTime.HasValue ? issue.EndDateTime.Value.Date : DateTime.Now;
                    this.dateTimePicker1.Value = endDate;
                    this.dateTimePicker2.Value = endDate;
                }
            }
        }

    }
}
