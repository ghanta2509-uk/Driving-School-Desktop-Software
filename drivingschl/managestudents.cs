using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Linq;

namespace PassITDrivingSchool
{
    public partial class managestudents : Form
    {
        private ArrayList dataList = new ArrayList();
        private bool isTenNumbersEntered = false;
        private bool isTenNumbersEnteredForTextBox10 = false;
        private bool areFormFieldsFilled = false;
        private bool isDataLoaded = false;
        public managestudents()
        {
            InitializeComponent();
            // Set KeyPreview to true
            this.KeyPreview = true;
            // Attach the KeyDown event handler
            this.KeyDown += managetrainers_KeyDown;
        }
        private void managestudents_Load(object sender, EventArgs e)
        {
            textBox4.KeyPress += textBox4_KeyPress;
            textBox4.TextChanged += textBox4_TextChanged;
            textBox10.KeyPress += textBox10_KeyPress;
            textBox10.TextChanged += textBox10_TextChanged;
            this.KeyPreview = true;
            // Initialize DataGridView columns
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            // Add columns
            dataGridView1.Columns.Add("StudentID", "Student ID");
            dataGridView1.Columns.Add("FirstName", "First Name");
            dataGridView1.Columns.Add("LastName", "Last Name");
            dataGridView1.Columns.Add("DOB", "Date of Birth");
            dataGridView1.Columns.Add("Gender", "Gender");
            dataGridView1.Columns.Add("Mobile", "Mobile");
            dataGridView1.Columns.Add("Email", "Email");
            dataGridView1.Columns.Add("Address", "Address");
            dataGridView1.Columns.Add("BookingType", "Booking Type");
            dataGridView1.Columns.Add("Course", "Course");
            dataGridView1.Columns.Add("LearningType", "Learning Type");
            dataGridView1.Columns.Add("LearningHours", "Learning Hours");
            dataGridView1.Columns.Add("BookingStatus", "Booking Status");
            dataGridView1.Columns.Add("StartDate", "Course Start Date");
            dataGridView1.Columns.Add("FinishDate", "Course Finish Date");
            dataGridView1.Columns.Add("CourseStatus", "Course Status");
            dataGridView1.Columns.Add("TrainerID", "Trainer ID");
            dataGridView1.Columns.Add("TrainerFirstName", "Trainer First Name");
            dataGridView1.Columns.Add("TrainerLastName", "Trainer Last Name");
            dataGridView1.Columns.Add("TrainerGender", "Trainer Gender");
            dataGridView1.Columns.Add("TrainerType", "Trainer Type");
            dataGridView1.Columns.Add("TrainerMobile", "Trainer Mobile");
            dataGridView1.Columns.Add("TrainerEmail", "Trainer Email");
            dataGridView1.Columns.Add("TrainerAddress", "Trainer Address");
            // Handle CellValueChanged event to prevent editing
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            // Set the initial button states
            UpdateButtonStates();
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;
            }
        }
        private void managetrainers_KeyDown(object sender, KeyEventArgs e)
        {
            // Check for key combinations and perform actions accordingly

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
            else if (e.Alt && e.KeyCode == Keys.T)
            {
                button9.PerformClick();
            }
        }
        private void button1_Click(object sender, EventArgs e)
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
        private void button3_Click(object sender, EventArgs e)
        {
            // Call the ResetFormFields method
            ResetFormFields();
        }
        private void ResetFormFields()
        {
            // Clear textboxes and reset other controls
            textBox1.Clear(); textBox2.Clear(); textBox3.Clear(); textBox4.Clear(); textBox5.Clear(); textBox6.Clear(); textBox7.Clear(); textBox8.Clear(); textBox9.Clear();
            textBox10.Clear(); textBox11.Clear(); textBox12.Clear(); dateTimePicker1.Value = DateTime.Now; dateTimePicker2.Value = DateTime.Now; dateTimePicker3.Value = DateTime.Now;
            comboBox1.SelectedIndex = -1; comboBox2.SelectedIndex = -1; comboBox3.SelectedIndex = -1; comboBox4.SelectedIndex = -1; comboBox5.SelectedIndex = -1;
            comboBox6.SelectedIndex = -1; comboBox7.SelectedIndex = -1; comboBox8.SelectedIndex = -1; comboBox9.SelectedIndex = -1;
            // Set the areFormFieldsFilled variable
            areFormFieldsFilled = false;
            // Set the isDataLoaded variable
            isDataLoaded = false;
            // Update the button states
            UpdateButtonStates();
            // Show success message
            MessageBox.Show("Form fields reset successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("All Fields are Mandatory, Mobile Number must contain 10 Characters, and Email must be valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool allFieldsFilled = AreAllFieldsFilledExceptTextBox13();
            if (!allFieldsFilled)
            {
                MessageBox.Show("All Fields are Mandatory, Mobile Number must contain 10 Characters, and Email must be valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string studentemail = textBox5.Text;
            string traineremail = textBox11.Text;
            if (!ValidateEmail(studentemail) || !ValidateEmail(traineremail))
            {
                MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Insert data into DataGridView
            string studentID = textBox1.Text;
            if (IsStudentIDUnique(studentID))
            {
                InsertDataIntoDataGridView();
                ClearFields();
                MessageBox.Show("New Student Data inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Student ID must be unique. Same Record has been found in the existing Data. Please Update the existing record or Enter a new record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private bool AreAllFieldsFilled()
        {
            // Check if all of the textboxes or comboboxes are filled
            foreach (Control control in Controls)
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
            return true;
        }
        private void InsertDataIntoDataGridView()
        {
            // Insert data into DataGridView
            dataGridView1.Rows.Add(
                textBox1.Text, textBox2.Text, textBox3.Text, dateTimePicker1.Value, comboBox1.Text,
                textBox4.Text, textBox5.Text, textBox6.Text, comboBox2.Text, comboBox3.Text,
                comboBox4.Text, comboBox5.Text, comboBox6.Text, dateTimePicker2.Value, dateTimePicker3.Value, comboBox9.Text, textBox7.Text, textBox8.Text,
                textBox9.Text, comboBox7.Text, comboBox8.Text, textBox10.Text, textBox11.Text, textBox12.Text);
        }
        private bool IsStudentIDUnique(string studentID)
        {
            // Check if the entered Student ID is unique in the DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["StudentID"].Value != null && row.Cells["StudentID"].Value.ToString() == studentID)
                {
                    return false;
                }
            }
            return true;
        }
        private void ClearFields()
        {
            // Clear all form fields
            foreach (Control control in Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Text = "";
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.SelectedIndex = -1;
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    // Set DateTimePicker controls to their default values
                    dateTimePicker.Value = dateTimePicker.CustomFormat == " " ? DateTimePicker.MinimumDateTime : DateTime.Now;
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            // Check if there is any data loaded
            if (!IsDataLoaded())
            {
                MessageBox.Show("All fields are mandatory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Check if any form fields are empty
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox6.Text) || comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1 ||
                 comboBox4.SelectedIndex == -1 || comboBox5.SelectedIndex == -1 || comboBox6.SelectedIndex == -1 || dateTimePicker2.Value == DateTimePicker.MinimumDateTime ||
                 dateTimePicker3.Value == DateTimePicker.MinimumDateTime || comboBox9.SelectedIndex == -1 || string.IsNullOrWhiteSpace(textBox7.Text) || string.IsNullOrWhiteSpace(textBox8.Text) ||
                 string.IsNullOrWhiteSpace(textBox9.Text) || comboBox7.SelectedIndex == -1 || comboBox8.SelectedIndex == -1 || string.IsNullOrWhiteSpace(textBox10.Text) || string.IsNullOrWhiteSpace(textBox11.Text) ||
                 string.IsNullOrWhiteSpace(textBox12.Text))
            {
                MessageBox.Show("Please load student details first before updating.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get values from textboxes
            string studentId = textBox1.Text; string firstName = textBox2.Text; string lastName = textBox3.Text; string mobile = textBox4.Text;
            string email = textBox5.Text; string address = textBox6.Text; string gender = comboBox1.SelectedItem.ToString(); string bookingType = comboBox2.SelectedItem.ToString();
            string course = comboBox3.SelectedItem.ToString(); string learningType = comboBox4.SelectedItem.ToString(); string learningHours = comboBox5.SelectedItem.ToString();
            string bookingStatus = comboBox6.SelectedItem.ToString(); DateTime startDate = dateTimePicker2.Value; DateTime finishDate = dateTimePicker3.Value;
            string courseStatus = comboBox9.SelectedItem.ToString(); string trainerId = textBox7.Text; string trainerFirstName = textBox8.Text; string trainerLastName = textBox9.Text;
            string trainerGender = comboBox7.SelectedItem.ToString(); string trainerType = comboBox8.SelectedItem.ToString(); string trainerMobile = textBox10.Text; string trainerEmail = textBox11.Text; string trainerAddress = textBox12.Text;
            // Check if the Student ID is present in the DataTable
            DataGridViewRow rowToUpdate = null;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["StudentID"].Value != null && row.Cells["StudentID"].Value.ToString() == studentId)
                {
                    rowToUpdate = row;
                    break;
                }
            }

            if (rowToUpdate == null)
            {
                MessageBox.Show("Please select a valid Student ID to update the details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate email format before updating
            if (!ValidateEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update data in the DataGridView
            rowToUpdate.Cells["FirstName"].Value = firstName; rowToUpdate.Cells["LastName"].Value = lastName; rowToUpdate.Cells["Mobile"].Value = mobile; rowToUpdate.Cells["Email"].Value = email;
            rowToUpdate.Cells["Address"].Value = address; rowToUpdate.Cells["DoB"].Value = dateTimePicker1.Value.ToShortDateString(); rowToUpdate.Cells["Gender"].Value = gender;
            rowToUpdate.Cells["BookingType"].Value = bookingType; rowToUpdate.Cells["Course"].Value = course; rowToUpdate.Cells["LearningType"].Value = learningType;
            rowToUpdate.Cells["LearningHours"].Value = learningHours; rowToUpdate.Cells["BookingStatus"].Value = bookingStatus; rowToUpdate.Cells["StartDate"].Value = startDate;
            rowToUpdate.Cells["FinishDate"].Value = finishDate; rowToUpdate.Cells["CourseStatus"].Value = courseStatus; rowToUpdate.Cells["TrainerID"].Value = trainerId;
            rowToUpdate.Cells["TrainerFirstName"].Value = trainerFirstName; rowToUpdate.Cells["TrainerLastName"].Value = trainerLastName; rowToUpdate.Cells["TrainerGender"].Value = trainerGender;
            rowToUpdate.Cells["TrainerType"].Value = trainerType; rowToUpdate.Cells["TrainerMobile"].Value = trainerMobile; rowToUpdate.Cells["TrainerEmail"].Value = trainerEmail; rowToUpdate.Cells["TrainerAddress"].Value = trainerAddress;
            // Show success message
            MessageBox.Show("Student Details Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private bool IsDataLoaded()
        {
            // Check if there is any data loaded in the form fields
            return !string.IsNullOrWhiteSpace(textBox1.Text) || !string.IsNullOrWhiteSpace(textBox2.Text) ||
                   !string.IsNullOrWhiteSpace(textBox3.Text) || !string.IsNullOrWhiteSpace(textBox4.Text) ||
                   !string.IsNullOrWhiteSpace(textBox5.Text) || !string.IsNullOrWhiteSpace(textBox6.Text) ||
                   comboBox1.SelectedIndex != -1 || comboBox2.SelectedIndex != -1 ||
                   comboBox3.SelectedIndex != -1 || comboBox4.SelectedIndex != -1 ||
                   comboBox5.SelectedIndex != -1 || comboBox6.SelectedIndex != -1 || comboBox9.SelectedIndex != -1 ||
                   dateTimePicker2.Value != DateTimePicker.MinimumDateTime || dateTimePicker3.Value != DateTimePicker.MinimumDateTime ||
                   !string.IsNullOrWhiteSpace(textBox7.Text) || !string.IsNullOrWhiteSpace(textBox8.Text) ||
                   !string.IsNullOrWhiteSpace(textBox9.Text) || comboBox7.SelectedIndex != -1 ||
                   comboBox8.SelectedIndex != -1 || !string.IsNullOrWhiteSpace(textBox10.Text) ||
                   !string.IsNullOrWhiteSpace(textBox11.Text) || !string.IsNullOrWhiteSpace(textBox12.Text);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            // Check if Student ID is entered
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please enter Student ID first to proceed with the delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Check if the entered Student ID is present in DataGridView
            string studentIDToDelete = textBox1.Text;
            bool studentIDFound = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["StudentID"].Value != null && row.Cells["StudentID"].Value.ToString() == studentIDToDelete)
                {
                    // Student ID found, proceed with delete
                    studentIDFound = true;
                    dataGridView1.Rows.Remove(row);
                    ClearFields(); // Clear form fields after deletion
                    MessageBox.Show("Student deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
            }
            // Show error if the entered Student ID is not found
            if (!studentIDFound)
            {
                MessageBox.Show("Entered Student ID not found. Please load the correct Student ID details to proceed with delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateButtonStates()
        {
            // Enable or disable the buttons based on conditions
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true; // Always enable the Reset button
            button4.Enabled = areFormFieldsFilled && isDataLoaded; // Enable only when data is loaded
            button5.Enabled = isDataLoaded && areFormFieldsFilled; // Enable only when data is loaded
            button6.Enabled = true; // Always enable the Load button
            button7.Enabled = true;
            button8.Enabled = true; // Always enable the Courses button
            button9.Enabled = true; // Always enable the Trainers button
        }
        private void button6_Click(object sender, EventArgs e)
        {
            // Check if Student ID is entered
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please enter Student ID first to load the data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Load data based on entered Student ID
            string studentIDToLoad = textBox1.Text;
            bool dataFound = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["StudentID"].Value != null && row.Cells["StudentID"].Value.ToString() == studentIDToLoad)
                {
                    // Data found, load it onto form fields
                    LoadDataToFormFields(row);
                    dataFound = true;
                    break;
                }
            }

            if (dataFound)
            {
                MessageBox.Show("Student details Loaded Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Data not found with the given Student ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadDataToFormFields(DataGridViewRow row)
        {
            // Load data from DataGridView to form fields
            textBox1.Text = row.Cells["StudentID"].Value.ToString();
            textBox2.Text = row.Cells["FirstName"].Value.ToString();
            textBox3.Text = row.Cells["LastName"].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(row.Cells["DOB"].Value);
            comboBox1.Text = row.Cells["Gender"].Value.ToString();
            textBox4.Text = row.Cells["Mobile"].Value.ToString();
            textBox5.Text = row.Cells["Email"].Value.ToString();
            textBox6.Text = row.Cells["Address"].Value.ToString();
            comboBox2.Text = row.Cells["BookingType"].Value.ToString();
            comboBox3.Text = row.Cells["Course"].Value.ToString();
            comboBox4.Text = row.Cells["LearningType"].Value.ToString();
            comboBox5.Text = row.Cells["LearningHours"].Value.ToString();
            comboBox6.Text = row.Cells["BookingStatus"].Value.ToString();
            dateTimePicker2.Value = Convert.ToDateTime(row.Cells["StartDate"].Value);
            dateTimePicker3.Value = Convert.ToDateTime(row.Cells["FinishDate"].Value);
            comboBox9.Text = row.Cells["CourseStatus"].Value.ToString();
            textBox7.Text = row.Cells["TrainerID"].Value.ToString();
            textBox8.Text = row.Cells["TrainerFirstName"].Value.ToString();
            textBox9.Text = row.Cells["TrainerLastName"].Value.ToString();
            comboBox7.Text = row.Cells["TrainerGender"].Value.ToString();
            comboBox8.Text = row.Cells["TrainerType"].Value.ToString();
            textBox10.Text = row.Cells["TrainerMobile"].Value.ToString();
            textBox11.Text = row.Cells["TrainerEmail"].Value.ToString();
            textBox12.Text = row.Cells["TrainerAddress"].Value.ToString();

            // Set the areFormFieldsFilled and isDataLoaded variables
            areFormFieldsFilled = true; // Set to true since data is loaded
            isDataLoaded = true;

            // Update button states
            UpdateButtonStates();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the current form
            // Open the managecourses form
            managelessons manageCoursesForm = new managelessons();
            manageCoursesForm.Show(); // Show the managecourses form
        }
        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            managetrainers manageTrainersForm = new managetrainers();
            // Show the managetrainers form
            manageTrainersForm.Show();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers (0-9) and control characters
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ignore the key press
            }

        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Check for the length of textBox4
            if (textBox4.Text.Length > 10)
            {
                MessageBox.Show("Maximum length of mobile number is 10 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox4.Text = textBox4.Text.Substring(0, 10); // Trim to 10 characters
            }
            // Check if exactly 10 numbers are entered
            isTenNumbersEntered = textBox4.Text.Length == 10;

            // Check if all form fields are filled
            areFormFieldsFilled = AreAllFieldsFilled();
            UpdateButtonStates();

        }
        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers (0-9) and control characters
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ignore the key press
            }
        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            // Check for the length of textBox10
            if (textBox10.Text.Length > 10)
            {
                MessageBox.Show("Maximum length of mobile number is 10 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox10.Text = textBox10.Text.Substring(0, 10); // Trim to 10 characters


                // Check if all form fields are filled
                areFormFieldsFilled = AreAllFieldsFilled();
                UpdateButtonStates();
            }

            // Check if exactly 10 numbers are entered
            isTenNumbersEnteredForTextBox10 = textBox10.Text.Length == 10;
        }

        private bool ValidateEmail(string email)
        {
            // Use a regular expression to validate the email address format
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailPattern);
            return regex.IsMatch(email);
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

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
        private void textBox11_TextChanged(object sender, EventArgs e)
        {

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
