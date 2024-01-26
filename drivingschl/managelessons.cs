using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PassITDrivingSchool
{
    public partial class managelessons : Form
    {
        private DataTable courseDataTable = new DataTable();

        // Add these private variables to track the initial state of the form fields
        private bool isCourseIdFieldEmpty = true;
        private bool isLessonIdFieldEmpty;
        private bool isDataLoaded = false;
        public managelessons()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeEventHandlers();
        }
        private void InitializeDataGridView()
        {
            // Initialize DataGridView columns if not already done in the designer
            if (dataGridView1.Columns.Count == 0)
            {
                dataGridView1.Columns.Add("LessonID", "Lesson ID");
                dataGridView1.Columns.Add("LessonName", "Lesson Name");
                dataGridView1.Columns.Add("LessonType", "Lesson Type");
                dataGridView1.Columns.Add("LessonFee", "Lesson Fee");
            }
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.ReadOnly = true;
            }
        }
        private void InitializeEventHandlers()
        {
            this.KeyPreview = true;
            this.KeyDown += managecourses_KeyDown;
        }
        private void managecourses_KeyDown(object sender, KeyEventArgs e)
        {
            // Handle key combinations
            if (e.KeyCode == Keys.Enter) button2.PerformClick();
            if (e.Control && e.KeyCode == Keys.L) button6.PerformClick();
            if (e.Control && e.KeyCode == Keys.U) button4.PerformClick();
            if (e.KeyCode == Keys.Delete) button5.PerformClick();
            if (e.Control && e.KeyCode == Keys.R) button3.PerformClick();
            if (e.Control && e.KeyCode == Keys.E) button7.PerformClick();
            if (e.Control && e.KeyCode == Keys.X) button1.PerformClick();
            if (e.Alt && e.KeyCode == Keys.T) button8.PerformClick();
            if (e.Alt && e.KeyCode == Keys.S) button9.PerformClick();
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
        private void button2_Click(object sender, EventArgs e)
        {
            // Check if all fields are filled
            if (!AreAllFieldsFilled())
            {
                MessageBox.Show("All fields are mandatory. Please fill in all the fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get values from textboxes
            int lessonId;
            if (!int.TryParse(textBox1.Text, out lessonId))
            {
                MessageBox.Show("Please enter a valid Lesson ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if Lesson ID is unique in the DataGridView
            if (IsCourseIdUniqueInDataGridView(lessonId))
            {
                // Add data to DataGridView directly
                int rowIndex = dataGridView1.Rows.Add(lessonId, textBox2.Text, textBox3.Text, textBox4.Text);
                // Show success message
                MessageBox.Show("Lesson Details Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Clear textboxes after successful insertion
                ClearTextBoxes();
                // Optionally, you can select the newly added row
                dataGridView1.Rows[rowIndex].Selected = true;
            }
            else
            {
                MessageBox.Show("Lesson ID must be unique. Please enter a new Lesson ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private bool IsCourseIdUniqueInDataGridView(int lessonId)
        {
            // Check if the DataGridView is not empty
            if (dataGridView1.Rows.Count > 0)
            {
                // Check if the Lesson ID already exists in the DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    var cellValue = row.Cells["LessonID"].Value;
                    if (cellValue != null && cellValue.ToString() == lessonId.ToString())
                    {
                        // If Lesson ID already exists, return false
                        return false;
                    }
                }
            }
            // If the DataGridView is empty or Lesson ID is not found, consider it as unique
            return true;
        }
        private bool AreAllFieldsFilled()
        {
            // Check if all textboxes are filled
            return !string.IsNullOrWhiteSpace(textBox1.Text) &&
                   !string.IsNullOrWhiteSpace(textBox2.Text) &&
                   !string.IsNullOrWhiteSpace(textBox3.Text) &&
                   !string.IsNullOrWhiteSpace(textBox4.Text);
        }
        private void ClearTextBoxes()
        {
            // Clear textboxes after successful insertion
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            // Call the LoadData method
            LoadData();
        }
        private void LoadData()
        {
            // Check if Lesson ID is entered
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter Lesson ID to load the data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Get Lesson ID from the textbox
            if (!int.TryParse(textBox1.Text, out int lessonId))
            {
                MessageBox.Show("Please enter a valid Lesson ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Search for the Lesson ID in the DataGridView
            DataGridViewRow row = dataGridView1.Rows.Cast<DataGridViewRow>().FirstOrDefault(r =>
            {
                var cell = r.Cells["LessonID"];
                return cell != null && cell.Value != null && (int)cell.Value == lessonId;
            });
            // If Lesson ID is not found, show an error message
            if (row == null)
            {
                MessageBox.Show("No data found for the given Lesson ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Populate form fields with data from the DataGridView
            textBox2.Text = row.Cells["LessonName"].Value.ToString();
            textBox3.Text = row.Cells["LessonType"].Value.ToString();
            textBox4.Text = row.Cells["LessonFee"].Value.ToString();

            // Update the state of form fields
            isLessonIdFieldEmpty = false;
            isDataLoaded = true;

            // Update the button states
            UpdateButtonStates();

            // Show success message
            MessageBox.Show("Lesson Details Loaded Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            // Check if there is any data loaded
            if (!isDataLoaded)
            {
                MessageBox.Show("Please load Lesson details first before updating.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if any form fields are empty
            if (isLessonIdFieldEmpty || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("All the fields are mandatory. Please fill in all the fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get values from textboxes
            int lessonId;
            if (!int.TryParse(textBox1.Text, out lessonId))
            {
                MessageBox.Show("Please enter a valid Lesson ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the course ID is present in the DataGridView
            DataGridViewRow row = dataGridView1.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => (int)r.Cells["LessonID"].Value == lessonId);
            if (row == null)
            {
                MessageBox.Show("Lesson ID not found in the data. Please enter a valid Lesson ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update data in the DataGridView
            row.Cells["LessonName"].Value = textBox2.Text;
            row.Cells["LessonType"].Value = textBox3.Text;
            row.Cells["LessonFee"].Value = textBox4.Text; // Use the entered value directly for updating

            // Show success message
            MessageBox.Show("Lesson Details Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Clear textboxes after successful update
            ClearTextBoxes();

            // Update the state of form fields
            isCourseIdFieldEmpty = string.IsNullOrWhiteSpace(textBox1.Text);
            isDataLoaded = IsDataLoaded();

            // Update the button states
            UpdateButtonStates();
        }



        private void button5_Click(object sender, EventArgs e)
        {
            // Check if there is any data loaded
            if (!isDataLoaded)
            {
                MessageBox.Show("Please load Lesson details first before deleting.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get Lesson ID from the textbox
            if (!int.TryParse(textBox1.Text, out int lessonId))
            {
                MessageBox.Show("Please enter a valid Lesson ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the Lesson ID is present in the DataGridView
            DataGridViewRow row = dataGridView1.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => (int)r.Cells["LessonID"].Value == lessonId);
            if (row == null)
            {
                MessageBox.Show("Lesson ID not found in the data. Please enter a valid Lesson ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Remove data from the DataGridView
            dataGridView1.Rows.Remove(row);

            // Show success message
            MessageBox.Show("Lesson Details Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Clear textboxes after successful delete
            ClearTextBoxes();

            // Update the state of form fields
            isLessonIdFieldEmpty = string.IsNullOrWhiteSpace(textBox1.Text);
            isDataLoaded = IsDataLoaded();

            // Update the button states
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            // Enable or disable buttons based on the state of form fields
            button4.Enabled = isDataLoaded && !isLessonIdFieldEmpty;
            button5.Enabled = isDataLoaded && !isLessonIdFieldEmpty;
        }

        private bool IsDataLoaded()
        {
            // Check if there is any data loaded in the form fields
            return !string.IsNullOrWhiteSpace(textBox1.Text) || !string.IsNullOrWhiteSpace(textBox2.Text) ||
                   !string.IsNullOrWhiteSpace(textBox3.Text) || !string.IsNullOrWhiteSpace(textBox4.Text);
        }

        private void ResetFormFields()
        {
            ClearTextBoxes();

            // Reset the state of form fields
            isLessonIdFieldEmpty = true;
            isDataLoaded = false;

            // Update the button states
            UpdateButtonStates();

            // Show success message
            MessageBox.Show("Form reset successfull", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            // Call the ResetFormFields method
            ResetFormFields();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            managetrainers manageTrainersForm = new managetrainers();
            // Show the managetrainers form
            manageTrainersForm.Show();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            managestudents managestudentsForm = new managestudents();
            // Show the managestudents form
            managestudentsForm.Show();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        private void managecourses_Load(object sender, EventArgs e)
        {
            // Set the initial state of form fields
            isLessonIdFieldEmpty = string.IsNullOrWhiteSpace(textBox1.Text);
            isDataLoaded = IsDataLoaded();

            // Set the initial button states
            UpdateButtonStates();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // Filter data in dataGridView based on textBox5 text
            FilterData(textBox5.Text);
        }

        private void FilterData(string filterText)
        {
            // Convert filter text to lowercase for case-insensitive comparison
            filterText = filterText.ToLower();

            // Iterate through each row in the dataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Check if the row is in edit mode (uncommitted new row)
                if (!row.IsNewRow)
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


        private void button10_Click(object sender, EventArgs e)
        {
           
        }

        private void button11_Click(object sender, EventArgs e)
        {
            
        }
    }
}
