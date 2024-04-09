using ClosedXML.Excel;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tabel.Model;
using Tabel.ViewModel;

namespace Tabel.View
{
    /// <summary>
    /// Логика взаимодействия для WindowTabel.xaml
    /// </summary>
    public partial class WindowTabel : Window
    {
        public DayTypesViewModel ViewModel { get; set; }
        private const string connectionString = "Data Source=C:\\Users\\Darya\\OneDrive\\Рабочий стол\\курсач РПМ\\Tabel\\bin\\Debug\\net6.0-windows\\Tabel.db";
        public WindowTabel()
        {
            InitializeComponent();
            FillDivisionComboBox();
            ViewModel = new DayTypesViewModel();
            DataContext = ViewModel;




        }

        private void FillDivisionComboBox()
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Divisions";
                SqliteCommand command = new SqliteCommand(query, connection);
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBoxDivision.Items.Add(new Division
                    {
                        DivisionID = Convert.ToInt32((long)reader["DivisionID"]), // Явное приведение типа с помощью Convert.ToInt32
                        DivisionName = reader["DivisionName"].ToString()
                    });
                }

                reader.Close();
            }
        }

        private void ComboBoxDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxDivision.SelectedItem != null)
            {
                Division selectedDivision = comboBoxDivision.SelectedItem as Division;
                txtDivisionName.Text = selectedDivision.DivisionName;
            }
        }


        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxDivision.SelectedItem != null)
            {
                int selectedDivisionID = (int)(comboBoxDivision.SelectedItem as Division).DivisionID;

                using (var context = new ApplicationContext())
                {
                    var employees = context.Employees.Where(emp => emp.DivisionID == selectedDivisionID)
                                                    .Select(employee => new Employee
                                                    {
                                                        FullName = $"{employee.LastName} {employee.FirstName} {employee.Surname}"
                                                    }).ToList();

                    dataGridEmployees.Items.Clear(); // Очистка перед добавлением новых сотрудников

                    foreach (var employee in employees)
                    {
                        dataGridEmployees.Items.Add(employee);
                    }
                }
            }
        }



        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) // Ctrl+V
            {
                string textToPaste = Clipboard.GetText();
                TextBox textBox = sender as TextBox;
                if (!string.IsNullOrEmpty(textToPaste) && textBox != null)
                {
                    foreach (var item in dataGridEmployees.SelectedCells)
                    {
                        if (item.Item is Employee employee && item.Column is DataGridColumn column)
                        {
                            string columnName = column.Header.ToString();
                            PropertyInfo propertyInfo = employee.GetType().GetProperty(columnName);
                            if (propertyInfo != null)
                            {
                                propertyInfo.SetValue(employee, textToPaste);
                            }
                        }
                    }
                }
            }
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            string timesheetNumber = txtTimesheetNumber.Text;
            DateTime date = datePicker.SelectedDate.Value;
            string divisionName = txtDivisionName.Text;
           

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.Cell("A1").SetValue($"Дата: {date:dd/MM/yyyy}"); // Используйте формат даты, понятный Excel
                worksheet.Cell("I1").SetValue($"Табель № {timesheetNumber}");
                worksheet.Cell("A4").SetValue($"Подразделение: {divisionName}");
              

                string filePath = @"C:\Users\Darya\OneDrive\Рабочий стол\WpfApp1\Tabel.xlsx"; // Укажите путь к файлу
                workbook.SaveAs(filePath);
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
        }





        public class Employee
        {
            public string FullName { get; set; } 
        }

        
    }
}
