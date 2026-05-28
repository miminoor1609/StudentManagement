using System;
using System.Windows.Forms;
using studentManagementSyatem.Database;
using studentManagementSyatem.Models;

namespace studentManagementSyatem.Forms
{
    public partial class ResultForm : Form
    {
        private readonly ResultRepository _repository;
        private int _selectedId = 0;

        public ResultForm()
        {
            InitializeComponent();
            _repository = new ResultRepository();
            LoadResults();
        }

        private void LoadResults()
        {
            dataGridView1.Rows.Clear();
            var results = _repository.GetAllResults();
            foreach (var r in results)
            {
                dataGridView1.Rows.Add( r.StudentName, r.Subject, r.Marks, r.Grade);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudent.Text))
            {
                MessageBox.Show("Please enter student name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = new Result
            {
                StudentName = txtStudent.Text.Trim(),
                Subject = txtSubject.Text.Trim(),
                Marks = txtMarks.Text.Trim(),
                Grade = txtGrade.Text.Trim()
            };

            if (_repository.AddResult(result))
            {
                LoadResults();
                ClearFields();
                MessageBox.Show("Result added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                MessageBox.Show("Please select a record to update.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = new Result
            {
                Id = _selectedId,
                StudentName = txtStudent.Text.Trim(),
                Subject = txtSubject.Text.Trim(),
                Marks = txtMarks.Text.Trim(),
                Grade = txtGrade.Text.Trim()
            };

            if (_repository.UpdateResult(result))
            {
                LoadResults();
                ClearFields();
                MessageBox.Show("Result updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                MessageBox.Show("Please select a record to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Delete this result?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                if (_repository.DeleteResult(_selectedId))
                {
                    LoadResults();
                    ClearFields();
                    MessageBox.Show("Result deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        
        private void ClearFields()
        {
            txtStudent.Clear();
            txtSubject.Clear();
            txtMarks.Clear();
            txtGrade.Clear();
            _selectedId = 0;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                var all = _repository.GetAllResults();
                var selected = all.Find(r => r.StudentName == row.Cells[0].Value?.ToString() && r.Subject == row.Cells[1].Value?.ToString());
                _selectedId = selected != null ? selected.Id : 0;

                txtStudent.Text = row.Cells[0].Value?.ToString();
                txtSubject.Text = row.Cells[1].Value?.ToString();
                txtMarks.Text = row.Cells[2].Value?.ToString();
                txtGrade.Text = row.Cells[3].Value?.ToString();
            }
        }
    }
}