using System;
using System.Windows.Forms;
using studentManagementSyatem.Database;
using studentManagementSyatem.Models;

namespace studentManagementSyatem.Forms
{
    public partial class AttendanceForm : Form
    {
        private readonly AttendanceRepository _repository;
        private int _selectedId = 0;

        public AttendanceForm()
        {
            InitializeComponent();
            _repository = new AttendanceRepository();
            LoadAttendance();
        }

        private void LoadAttendance()
        {
            dataGridView1.Rows.Clear();
            var list = _repository.GetAllAttendance();
            foreach (var a in list)
            {
                dataGridView1.Rows.Add( a.StudentName, a.Date, a.Status);
            }
        }
        // Attendance Form - CRUD operations for managing attendance
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudent.Text))
            {
                MessageBox.Show("Please enter student name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var attendance = new Attendance
            {
                StudentName = txtStudent.Text.Trim(),
                Date = txtDate.Text.Trim(),
                Status = cmbStatus.Text
            };

            if (_repository.AddAttendance(attendance))
            {
                LoadAttendance();
                ClearFields();
                MessageBox.Show("Attendance added successfully!", "Success",
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

            var attendance = new Attendance
            {
                Id = _selectedId,
                StudentName = txtStudent.Text.Trim(),
                Date = txtDate.Text.Trim(),
                Status = cmbStatus.Text
            };

            if (_repository.UpdateAttendance(attendance))
            {
                LoadAttendance();
                ClearFields();
                MessageBox.Show("Attendance updated successfully!", "Success",
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

            var confirm = MessageBox.Show("Delete this attendance record?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                if (_repository.DeleteAttendance(_selectedId))
                {
                    LoadAttendance();
                    ClearFields();
                    MessageBox.Show("Attendance deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

      

        private void ClearFields()
        {
            txtStudent.Clear();
            txtDate.Clear();
            cmbStatus.SelectedIndex = -1;
            _selectedId = 0;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                var all = _repository.GetAllAttendance();
                var selected = all.Find(a => a.StudentName == row.Cells[0].Value?.ToString() && a.Date == row.Cells[1].Value?.ToString());
                _selectedId = selected != null ? selected.Id : 0;

                txtStudent.Text = row.Cells[0].Value?.ToString();
                txtDate.Text = row.Cells[1].Value?.ToString();
                cmbStatus.Text = row.Cells[2].Value?.ToString();
            }
        }
    }
}