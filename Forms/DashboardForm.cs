using System;
using System.Windows.Forms;
using studentManagementSyatem.Database;
using studentManagementSyatem.Models;

namespace studentManagementSyatem.Forms
{
    public partial class DashboardForm : Form
    {
        private readonly User _currentUser;
        private readonly StudentRepository _studentRepo;
        private readonly CourseRepository _courseRepo;

        public DashboardForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _studentRepo = new StudentRepository();
            _courseRepo = new CourseRepository();

            // to show the Welcome message 
            label1.Text = $"Welcome, {_currentUser.FullName}! ({_currentUser.UserType.ToUpper()})";

            LoadDashboardStats();
        }

        private void LoadDashboardStats()
        {
            try
            {
                lblStudentCount.Text = "Total Students: " + _studentRepo.GetStudentCount();
                lblCourseCount.Text = "Total Courses: " + _courseRepo.GetCourseCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading stats: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //  -----------------------shows statistics and navigation-------------
        private void btnStudent_Click(object sender, EventArgs e)
        {
            StudentForm student = new StudentForm();
            student.ShowDialog();
            LoadDashboardStats(); // on return ,it will refresh the page
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            AttendanceForm attendance = new AttendanceForm();
            attendance.ShowDialog();
        }

        private void btnCourse_Click(object sender, EventArgs e)
        {
            CourseForm course = new CourseForm();
            course.ShowDialog();
            LoadDashboardStats(); // on return, it will refresh the page
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            ResultForm result = new ResultForm();
            result.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to logout?",
                "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                LoginForm login = new LoginForm();
                login.Show();
                this.Close();
            }
        }
    }
}