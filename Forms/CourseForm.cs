using System;
using System.Windows.Forms;
using studentManagementSyatem.Database;
using studentManagementSyatem.Models;

namespace studentManagementSyatem.Forms
{
    public partial class CourseForm : Form
    {
        private readonly CourseRepository _repository;
        private int _selectedId = 0;

        public CourseForm()
        {
            InitializeComponent();
            _repository = new CourseRepository();
            LoadCourses();
        }

        private void LoadCourses()
        {
            dataGridView1.Rows.Clear();
            var courses = _repository.GetAllCourses();
            foreach (var c in courses)
            {
                dataGridView1.Rows.Add( c.CourseName, c.Teacher, c.Duration);
            }
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCourse.Text))
            {
                MessageBox.Show("Please enter course name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var course = new Course
            {
                CourseName = txtCourse.Text.Trim(),
                Teacher = txtTeacher.Text.Trim(),
                Duration = txtDuration.Text.Trim()
            };

            if (_repository.AddCourse(course))
            {
                LoadCourses();
                ClearFields();
                MessageBox.Show("Course added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                MessageBox.Show("Please select a course to update.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var course = new Course
            {
                Id = _selectedId,
                CourseName = txtCourse.Text.Trim(),
                Teacher = txtTeacher.Text.Trim(),
                Duration = txtDuration.Text.Trim()
            };

            if (_repository.UpdateCourse(course))
            {
                LoadCourses();
                ClearFields();
                MessageBox.Show("Course updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                MessageBox.Show("Please select a course to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Delete this course?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                if (_repository.DeleteCourse(_selectedId))
                {
                    LoadCourses();
                    ClearFields();
                    MessageBox.Show("Course deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        
        private void ClearFields()
        {
            txtCourse.Clear();
            txtTeacher.Clear();
            txtDuration.Clear();
            _selectedId = 0;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                var all = _repository.GetAllCourses();
                var selected = all.Find(c => c.CourseName == row.Cells[0].Value?.ToString());
                _selectedId = selected != null ? selected.Id : 0;

                txtCourse.Text = row.Cells[0].Value?.ToString();
                txtTeacher.Text = row.Cells[1].Value?.ToString();
                txtDuration.Text = row.Cells[2].Value?.ToString();
            }
        }
    }
}