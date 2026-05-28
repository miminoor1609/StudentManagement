using System;
using System.Windows.Forms;
using studentManagementSyatem.Database;
using studentManagementSyatem.Models;

namespace studentManagementSyatem.Forms
{
    public partial class StudentForm : Form
    {
        private readonly StudentRepository _repository;
        private int _selectedId = 0;

        public StudentForm()
        {
            InitializeComponent();
            _repository = new StudentRepository();
            LoadStudents();
        }

        private void LoadStudents()
        {
            dataGridView1.Rows.Clear();
            var students = _repository.GetAllStudents();
            foreach (var s in students)
            {
                dataGridView1.Rows.Add( s.Id, s.StudentName, s.RollNumber, s.ClassName, s.Phone);

            }
           
        }
        // Student Form - CRUD operations for managing students
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentName.Text))
            {
                MessageBox.Show("Please enter student name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var student = new Student
            {
                StudentName = txtStudentName.Text.Trim(),
                RollNumber = txtRollNumber.Text.Trim(),
                ClassName = txtClass.Text.Trim(),
                Phone = txtPhone.Text.Trim()
            };

            if (_repository.AddStudent(student))
            {
                LoadStudents();
                ClearFields();
                MessageBox.Show("Student added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to add student.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                MessageBox.Show("Please select a student to update.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var student = new Student
            {
                Id = _selectedId,
                StudentName = txtStudentName.Text.Trim(),
                RollNumber = txtRollNumber.Text.Trim(),
                ClassName = txtClass.Text.Trim(),
                Phone = txtPhone.Text.Trim()
            };

            if (_repository.UpdateStudent(student))
            {
                LoadStudents();
                ClearFields();
                MessageBox.Show("Student updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                MessageBox.Show("Please select a student to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete this student?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                if (_repository.DeleteStudent(_selectedId))
                {
                    LoadStudents();
                    ClearFields();
                    MessageBox.Show("Student deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

       
        private void ClearFields()
        {
            txtStudentName.Clear();
            txtRollNumber.Clear();
            txtClass.Clear();
            txtPhone.Clear();
            _selectedId = 0;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                _selectedId = Convert.ToInt32(row.Cells[0].Value);
                txtStudentName.Text = row.Cells[1].Value?.ToString();
                txtRollNumber.Text = row.Cells[2].Value?.ToString();
                txtClass.Text = row.Cells[3].Value?.ToString();
                txtPhone.Text = row.Cells[4].Value?.ToString();

            }
        }
    }
}