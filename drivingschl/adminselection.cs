using System;
using System.Windows.Forms;

namespace PassITDrivingSchool
{
    public partial class adminselection : Form
    {
        public adminselection()
        {
            InitializeComponent();

            // Set KeyPreview to true
            this.KeyPreview = true;

            // Attach the KeyDown event handler
            this.KeyDown += adminselection_KeyDown;
        }
        private void adminselection_KeyDown(object sender, KeyEventArgs e)
        {
            // Check for key combinations and perform corresponding actions
            if (e.Alt && e.KeyCode == Keys.S)
            {
                button1.PerformClick();
            }
            if (e.Alt && e.KeyCode == Keys.L)
            {
                button3.PerformClick();
            }
            if (e.Alt && e.KeyCode == Keys.T)
            {
                button2.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.X)
            {
                button4.PerformClick();
            }
        }
        private void adminselection_Load(object sender, EventArgs e)
        {
            
        }
        private void button4_Click(object sender, EventArgs e)
        {
            // Show a confirmation message before logging out
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Log out and show success message
                adminlogin adminLoginForm = new adminlogin();
                this.Close();
                MessageBox.Show("You are logged out successfully", "Log Out", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Show the adminlogin form
                adminLoginForm.Show();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Upon clicking this button, open managestudents.cs form
            managestudents managestudentsForm = new managestudents();
            managestudentsForm.Show();
            // Close the current adminselection form
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Upon clicking this button, open managetrainers.cs form
            managetrainers trainersForm = new managetrainers();
            trainersForm.Show();
            // Close the current adminselection form
            this.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            // Upon clicking this button, open managecourses.cs form
            managelessons coursesForm = new managelessons();
            coursesForm.Show();
            // Close the current adminselection form
            this.Close();
        }
    }
}
