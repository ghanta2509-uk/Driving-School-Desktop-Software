using System;
using System.Windows.Forms;

namespace PassITDrivingSchool
{
    public partial class adminlogin : Form
    {
        private System.Windows.Forms.ToolTip toolTip1;
        // Constant values for username and password.
        private const string ValidUsername = "gha22195677";
        private const string ValidPassword = "gha22195677";
        public adminlogin()
        {
            InitializeComponent();

            // Set the AcceptButton property to the login button
            this.AcceptButton = button1;

            // Initialize the tooltip
            toolTip1 = new System.Windows.Forms.ToolTip();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ValidateLogin();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Clear the text in both textboxes
            textBox1.Clear();
            textBox2.Clear();
        }
        private void adminlogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if Enter key is pressed
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Perform login validation when Enter key is pressed
                ValidateLogin();
            }
        }
        private void adminlogin_KeyDown(object sender, KeyEventArgs e)
        {
            // Check for key combinations and perform corresponding actions

            // Enter key for Login
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }

            // Ctrl + R for Reset
            if (e.Control && e.KeyCode == Keys.R)
            {
                button2.PerformClick();
            }

            // Ctrl + X for Exit
            if (e.Control && e.KeyCode == Keys.X)
            {
                button3.PerformClick();
            }
        }
        private void ValidateLogin()
        {
            // Get the entered username and password from textboxes
            string enteredUsername = textBox1.Text;
            string enteredPassword = textBox2.Text;
            // Check if entered username and password are empty
            if (string.IsNullOrWhiteSpace(enteredUsername) || string.IsNullOrWhiteSpace(enteredPassword))
            {
                // Show an error message if either username or password is empty
                MessageBox.Show("Please enter username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit the method to prevent further processing
            }
            // Validate against constant values
            if (IsLoginValid(enteredUsername, enteredPassword))
            {
                MessageBox.Show("User Details are verified, Please Continue…!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Open the next form (adminselection.cs)
                adminselection adminSelectionForm = new adminselection();
                adminSelectionForm.Show();
                // Close the current login form
                this.Hide();
            }
            else
            {
                MessageBox.Show("Incorrect Username or Password! Please contact IT Support for any login issues.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool IsLoginValid(string enteredUsername, string enteredPassword)
        {
            // Compare with constant values
            return enteredUsername == ValidUsername && enteredPassword == ValidPassword;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle the password visibility
            textBox2.PasswordChar = checkBox1.Checked ? '\0' : '*'; // Use '*' or any other character you prefer
        }
        private void adminlogin_Load(object sender, EventArgs e)
        {
            // Attach the KeyDown event handler to the form
            this.KeyDown += adminlogin_KeyDown;

            // Set KeyPreview to true
            this.KeyPreview = true;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {
            // Show a confirmation message before exit.
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
