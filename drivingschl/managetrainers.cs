using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace PassITDrivingSchool
{
    public partial class managetrainers : Form
    {
        private bool isTenNumbersEntered = false;
        // ArrayLists to store data
        List<string> trainerIds = new List<string>();
        List<string> trainerFirstNames = new List<string>();
        List<string> trainerLastNames = new List<string>();
        List<DateTime> trainerDoBs = new List<DateTime>();
        List<string> trainerGenders = new List<string>();
        List<string> trainerMobiles = new List<string>();
        List<string> trainerEmails = new List<string>();
        List<string> trainerJobTypes = new List<string>();
        List<string> trainerAddresses = new List<string>();
        public managetrainers()
        {
            InitializeComponent();
            textBox4.KeyPress += textBox4_KeyPress;
            textBox4.TextChanged += textBox4_TextChanged;
            this.KeyPreview = true;
            this.KeyDown += managetrainers_KeyDown;
            // Adding columns to the DataGridView
            dataGridView1.Columns.Add("TrainerID", "Trainer ID");
            dataGridView1.Columns.Add("FirstName", "First Name");
            dataGridView1.Columns.Add("LastName", "Last Name");
            dataGridView1.Columns.Add("DoB", "Date of Birth");
            dataGridView1.Columns.Add("Gender", "Gender");
            dataGridView1.Columns.Add("Mobile", "Mobile");
            dataGridView1.Columns.Add("Email", "Email");
            dataGridView1.Columns.Add("JobType", "Job Type");
            dataGridView1.Columns.Add("Address", "Address");

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.ReadOnly = true;
            }

            // Set initial state of buttons
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            // Enable or disable buttons based on whether any data is loaded
            button4.Enabled = IsDataLoaded();
            button5.Enabled = IsDataLoaded();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allowing to enter only numbers (0-9) and control characters
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ignore the key press
            }
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Checking the entered characters length.
            if (textBox4.Text.Length > 10)
            {
                MessageBox.Show("Maximum length is 10 characters for Mobile Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox4.Text = textBox4.Text.Substring(0, 10);
            }
            // Check if exactly 10 numbers are entered
            isTenNumbersEntered = textBox4.Text.Length == 10;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            // Check if Trainer ID is entered
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter Trainer ID to load the data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Search for Trainer ID in the DataGridView
            string trainerIdToLoad = textBox1.Text;
            DataGridViewRow rowToLoad = null;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["TrainerID"].Value != null && row.Cells["TrainerID"].Value.ToString() == trainerIdToLoad)
                {
                    rowToLoad = row;
                    break;
                }
            }

            // Check if Trainer ID is found
            if (rowToLoad == null)
            {
                MessageBox.Show("Trainer ID not found. Please enter a valid Trainer ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Load data from DataGridView to form fields
            textBox2.Text = rowToLoad.Cells["FirstName"].Value?.ToString();
            textBox3.Text = rowToLoad.Cells["LastName"].Value?.ToString();
            textBox4.Text = rowToLoad.Cells["Mobile"].Value?.ToString();
            textBox5.Text = rowToLoad.Cells["Email"].Value?.ToString();
            textBox6.Text = rowToLoad.Cells["Address"].Value?.ToString();

            // Parse and set Date of Birth
            if (DateTime.TryParse(rowToLoad.Cells["DoB"].Value?.ToString(), out DateTime dob))
            {
                dateTimePicker1.Value = dob;
            }

            // Set Gender and Job Type from DataGridView
            comboBox1.SelectedItem = rowToLoad.Cells["Gender"].Value?.ToString();
            comboBox2.SelectedItem = rowToLoad.Cells["JobType"].Value?.ToString();

            // Show success message
            MessageBox.Show("Trainer Details Loaded Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Update the state of buttons
            UpdateButtonStates();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Check if there is any data loaded
            if (!IsDataLoaded())
            {
                MessageBox.Show("Please load Trainer details first before deleting.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get Trainer ID from the textbox
            string trainerId = textBox1.Text;
            // Check if the Trainer ID is present in the DataGridView
            DataGridViewRow rowToDelete = null;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["TrainerID"].Value != null && row.Cells["TrainerID"].Value.ToString() == trainerId)
                {
                    rowToDelete = row;
                    break;
                }
            }
            if (rowToDelete == null)
            {
                MessageBox.Show("Trainer ID not found in the data. Please enter a valid Trainer ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Remove the row from the DataGridView
            dataGridView1.Rows.Remove(rowToDelete);
            // Show success message
            MessageBox.Show("Trainer Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Clear textboxes after successful delete
            ClearInputFields();

            // Update the state of buttons
            UpdateButtonStates();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            // Check if there is any data loaded
            if (!IsDataLoaded())
            {
                MessageBox.Show("Please load trainer details first before updating.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Check if any form fields are empty
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) ||
                comboBox1.SelectedIndex == -1 ||
                comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("All Fields are Mandatory, Mobile Number must contain 10 digits and the Email must be in valid format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Get values from textboxes
            string trainerId = textBox1.Text;
            string firstName = textBox2.Text;
            string lastName = textBox3.Text;
            string mobile = textBox4.Text;
            string email = textBox5.Text;
            string address = textBox6.Text;
            string gender = comboBox1.SelectedItem.ToString();
            string jobType = comboBox2.SelectedItem.ToString();
            // Check if the Trainer ID is present in the DataTable
            DataGridViewRow rowToUpdate = null;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["TrainerID"].Value != null && row.Cells["TrainerID"].Value.ToString() == trainerId)
                {
                    rowToUpdate = row;
                    break;
                }
            }
            if (rowToUpdate == null)
            {
                MessageBox.Show("Please select a valid Trainer ID to update the details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate email format before updating
            if (!ValidateEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update data in the DataGridView
            rowToUpdate.Cells["FirstName"].Value = firstName;
            rowToUpdate.Cells["LastName"].Value = lastName;
            rowToUpdate.Cells["DoB"].Value = dateTimePicker1.Value.ToShortDateString();
            rowToUpdate.Cells["Mobile"].Value = mobile;
            rowToUpdate.Cells["Email"].Value = email;
            rowToUpdate.Cells["JobType"].Value = jobType;
            rowToUpdate.Cells["Address"].Value = address;
            // Show success message
            MessageBox.Show("Trainer Details Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Clear textboxes after successful update
            ClearInputFields();

            // Update the state of buttons
            UpdateButtonStates();

        }
        private bool IsDataLoaded()
        {
            // Check if there is any data loaded in the form fields
            return !string.IsNullOrWhiteSpace(textBox1.Text) || !string.IsNullOrWhiteSpace(textBox2.Text) ||
                   !string.IsNullOrWhiteSpace(textBox3.Text) || !string.IsNullOrWhiteSpace(textBox4.Text) ||
                   !string.IsNullOrWhiteSpace(textBox5.Text) || !string.IsNullOrWhiteSpace(textBox6.Text) ||
                   comboBox1.SelectedIndex != -1 || comboBox2.SelectedIndex != -1;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            // Call the ResetFormFields method
            ResetFormFields();

            // Update the state of buttons
            UpdateButtonStates();

        }
        private void ResetFormFields()
        {
            // Clear textboxes and reset other controls
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

            // Show success message
            MessageBox.Show("Form fields reset successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ValidateEmail(string email)
        {
            // Use a regular expression to validate the email address format
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailPattern);
            return regex.IsMatch(email);
        }

        private bool AreAnyFieldsEmptyExceptTextBox13()
        {
            // Check if any of the textboxes or comboboxes are empty, excluding textBox13
            foreach (Control control in Controls)
            {
                if (control != null && control != textBox13)
                {
                    if (control is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
                    {
                        return true;
                    }
                    else if (control is ComboBox comboBox && comboBox.SelectedIndex == -1)
                    {
                        return true;
                    }
                    else if (control is DateTimePicker dateTimePicker && dateTimePicker.Value == DateTimePicker.MinimumDateTime)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool AreAllFieldsFilledExceptTextBox13()
        {
            // Check if all of the textboxes or comboboxes are filled, excluding textBox13
            foreach (Control control in Controls)
            {
                if (control != null && control != textBox13)
                {
                    if ((control is TextBox || control is ComboBox) && string.IsNullOrEmpty(control.Text))
                    {
                        return false;
                    }
                    else if (control is DateTimePicker dateTimePicker && dateTimePicker.Value == DateTimePicker.MinimumDateTime)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            bool anyFieldEmpty = AreAnyFieldsEmptyExceptTextBox13();

            if (anyFieldEmpty || !isTenNumbersEntered)
            {
                MessageBox.Show("All Fields are Mandatory, Mobile Number must contain 10 digits and the Email must be in valid format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Check if all fields are filled
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) ||
                comboBox1.SelectedIndex == -1 ||
                comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("All Fields are Mandatory, Mobile Number must contain 10 digits and the Email must be in valid format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Check if Trainer ID is unique
            string trainerId = textBox1.Text;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["TrainerID"].Value != null && row.Cells["TrainerID"].Value.ToString() == trainerId)
                {
                    MessageBox.Show("Trainer ID must be unique. Please enter a new Trainer ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Validate email format
            string email = textBox5.Text;
            if (!ValidateEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Add data to DataGridView
            dataGridView1.Rows.Add(
                textBox1.Text,
                textBox2.Text,
                textBox3.Text,
                dateTimePicker1.Value.ToShortDateString(),
                comboBox1.SelectedItem.ToString(),
                textBox4.Text,
                email,
                comboBox2.SelectedItem.ToString(),
                textBox6.Text);
            // Show success message
            MessageBox.Show("Trainer Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearInputFields();

            // Update the state of buttons
            UpdateButtonStates();
        }
        private void ClearInputFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }
        private bool AreAnyFieldsEmpty()
        {
            // Check if any of the textboxes or comboboxes are empty
            foreach (Control control in Controls)
            {
                if (control is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
                {
                    return true;
                }
                else if (control is ComboBox comboBox && comboBox.SelectedIndex == -1)
                {
                    return true;
                }
                else if (control is DateTimePicker dateTimePicker && dateTimePicker.Value == DateTimePicker.MinimumDateTime)
                {
                    return true;
                }
            }
            return false;
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            // Show a confirmation message before logging out
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the current form
            // Open the managestudents form
            managestudents managestudentsForm = new managestudents();
            managestudentsForm.Show(); // Show managestudents form
        }
        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the current form
            // Open the managecourses form
            managelessons manageCoursesForm = new managelessons();
            manageCoursesForm.Show(); // Show the managecourses form
        }
        private void managetrainers_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }
        private void managetrainers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.L)
            {
                button6.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.U)
            {
                button4.PerformClick();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                button5.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                button3.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.E)
            {
                button7.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.X)
            {
                button1.PerformClick();
            }
            else if (e.Alt && e.KeyCode == Keys.L)
            {
                button8.PerformClick();
            }
            else if (e.Alt && e.KeyCode == Keys.S)
            {
                button9.PerformClick();
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            adminselection adminselectionForm = new adminselection();
            // Show the adminselection form
            adminselectionForm.Show();
        }
        private void label28_Click(object sender, EventArgs e)
        {
            this.Hide();
            adminselection adminselectionForm = new adminselection();
            // Show the adminselection form
            adminselectionForm.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
         
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void label8_Click(object sender, EventArgs e)
        {

        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void label7_Click(object sender, EventArgs e)
        {

        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label14_Click(object sender, EventArgs e)
        {

        }
        private void label12_Click(object sender, EventArgs e)
        {

        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Show the SaveFileDialog to choose the export file location
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.Title = "Export Data";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Call the method to export data to the selected file
                ExportDataToCSV(saveFileDialog.FileName);
            }
        }

        private void ExportDataToCSV(string filePath)
        {
            // Create a StringBuilder to store the CSV content
            StringBuilder csvContent = new StringBuilder();

            // Add column headers to the CSV
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                csvContent.Append($"{col.HeaderText},");
            }
            csvContent.AppendLine(); // Move to the next line after adding headers

            // Add data rows to the CSV
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    csvContent.Append($"{cell.Value},");
                }
                csvContent.AppendLine(); // Move to the next line after adding a row
            }

            // Write the CSV content to the selected file
            File.WriteAllText(filePath, csvContent.ToString());

            MessageBox.Show("Data exported successfully!", "Export Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            // Filter data in dataGridView based on textBox13 text
            FilterData(textBox13.Text);
        }

        private void FilterData(string filterText)
        {
            // Convert filter text to lowercase for case-insensitive comparison
            filterText = filterText.ToLower();

            // Iterate through each row in the dataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Skip rows that are in edit mode (uncommitted new rows)
                if (row.IsNewRow || (dataGridView1.IsCurrentCellInEditMode && row.Index == dataGridView1.NewRowIndex))
                {
                    continue;
                }

                // Check if any cell in the row contains the filter text
                bool matchFound = false;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().ToLower().Contains(filterText))
                    {
                        matchFound = true;
                        break;
                    }
                }

                // Set the row visibility based on the match status
                row.Visible = matchFound;
            }
        }

    }
}
