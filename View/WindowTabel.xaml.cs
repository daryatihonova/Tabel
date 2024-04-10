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
using Microsoft.Office.Interop.Excel;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.IO;
using TextBox = Microsoft.Office.Interop.Excel.TextBox;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Tabel.View
{
    /// <summary>
    /// Логика взаимодействия для WindowTabel.xaml
    /// </summary>
    public partial class WindowTabel : System.Windows.Window
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



        //private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) // Ctrl+V
        //    {
        //        string textToPaste = Clipboard.GetText();
        //        System.Windows.Controls.TextBox textBox = sender as System.Windows.Controls.TextBox;
        //        if (!string.IsNullOrEmpty(textToPaste) && textBox != null)
        //        {
        //            foreach (var item in dataGridEmployees.SelectedCells)
        //            {
        //                if (item.Item is Employee employee && item.Column is DataGridColumn column)
        //                {
        //                    string columnName = column.Header.ToString();
        //                    PropertyInfo propertyInfo = employee.GetType().GetProperty(columnName);
        //                    if (propertyInfo != null)
        //                    {
        //                        propertyInfo.SetValue(employee, textToPaste);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        //{
        //    Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
        //    excelApp.Visible = true;
        //    Microsoft.Office.Interop.Excel.Workbook workbook = excelApp.Workbooks.Add();
        //    Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Sheets[1];

        //    // Export data from TextBox
        //    string division = txtDivisionName.Text;
        //    string timesheetNumber = txtTimesheetNumber.Text;

        //    worksheet.Cells[1, 1] = "Подразделение:";
        //    worksheet.Cells[1, 2] = division;
        //    worksheet.Cells[2, 1] = "Номер табеля:";
        //    worksheet.Cells[2, 2] = timesheetNumber;

        //    // Export headers from DataGrid
        //    for (int i = 0; i < dataGridEmployees.Columns.Count; i++)
        //    {
        //        worksheet.Cells[4, i + 1] = dataGridEmployees.Columns[i].Header;
        //    }

        //    // Export data from DataGrid
        //    for (int i = 0; i < dataGridEmployees.Items.Count; i++)
        //    {
        //        for (int j = 0; j < dataGridEmployees.Columns.Count; j++)
        //        {
        //            DataGridCell cell = GetCell(dataGridEmployees, i, j);
        //            if (cell != null)
        //            {
        //                TextBlock cellContent = cell.Content as TextBlock;
        //                if (cellContent != null)
        //                {
        //                    worksheet.Cells[i + 5, j + 1] = cellContent.Text;
        //                }
        //                else
        //                {
        //                    TextBox textBox = GetTextBoxFromCell(cell);
        //                    if (textBox != null)
        //                    {
        //                        worksheet.Cells[i + 5, j + 1] = textBox.Text;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    // Save and close the workbook
        //    string filePath = "C:\\Users\\Darya\\OneDrive\\Рабочий стол\\WpfApp1\\Tabel.xlsx";

        //    if (File.Exists(filePath))
        //    {
        //        var result = MessageBox.Show("Файл уже существует. Перезаписать его?", "Подтверждение", MessageBoxButton.YesNo);
        //        if (result == MessageBoxResult.No)
        //        {
        //            // Пользователь отказался от перезаписи, укажите другой путь или прервите операцию
        //            return;
        //        }
        //    }

        //    workbook.SaveAs(filePath);
        //    workbook.Close();
        //}

        //private TextBox GetTextBoxFromCell(DataGridCell cell)
        //{
        //    if (cell == null || cell.Content == null)
        //    {
        //        return null;
        //    }

        //    if (cell.Content is TextBox textBox)
        //    {
        //        return textBox;
        //    }
        //    else
        //    {
        //        ContentPresenter contentPresenter = FindVisualChild2<ContentPresenter>(cell);
        //        DataTemplate dataTemplate = contentPresenter.ContentTemplate;

        //        if (dataTemplate != null)
        //        {
        //            Control control = dataTemplate.LoadContent() as Control;
        //            if (control is TextBox txtBox)
        //            {
        //                return txtBox;
        //            }
        //        }
        //    }

        //    return null;
        //}

        //private childItem FindVisualChild2<childItem>(DependencyObject obj) where childItem : DependencyObject
        //{
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        //    {
        //        DependencyObject child = VisualTreeHelper.GetChild(obj, i);
        //        if (child is childItem)
        //        {
        //            return (childItem)child;
        //        }
        //        else
        //        {
        //            childItem childOfChild = FindVisualChild<childItem>(child);
        //            if (childOfChild != null)
        //            {
        //                return childOfChild;
        //            }
        //        }
        //    }
        //    return null;
        //}









        //private DataGridCell GetCell(DataGrid dg, int row, int column)
        //{
        //    DataGridRow rowContainer = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(row);
        //    if (rowContainer != null)
        //    {
        //        DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
        //        if (presenter == null)
        //        {
        //            dg.ScrollIntoView(rowContainer, dg.Columns[column]);
        //            presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
        //        }
        //        DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
        //        return cell;
        //    }
        //    return null;
        //}

        //private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        //{
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        //    {
        //        DependencyObject child = VisualTreeHelper.GetChild(obj, i);
        //        if (child != null && child is childItem)
        //        {
        //            return (childItem)child;
        //        }
        //        else
        //        {
        //            childItem childOfChild = FindVisualChild<childItem>(child);
        //            if (childOfChild != null)
        //            {
        //                return childOfChild;
        //            }
        //        }
        //    }
        //    return null;
        //}


      
        private void SaveDataToDatabase_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new ApplicationContext()) // Замените YourDbContext на ваш контекст данных
            {
                //// Очистка таблицы перед добавлением новых данных
                var tableName = context.Model.FindEntityType(typeof(EmployeeTabel)).GetTableName();
                string clearTableSql = $"DELETE FROM {tableName};";

                context.Database.ExecuteSqlRaw(clearTableSql);
                foreach (Employee item in dataGridEmployees.Items)
                {
                    EmployeeTabel employee = new EmployeeTabel
                    {
                        Id = item.Id,
                        FullName = item.FullName ?? "",
                        Data1 = item.Data1 ?? "",
                        Data2 = item.Data2 ?? "",
                        Data3 = item.Data3 ?? "",
                        Data4 = item.Data4 ?? "",
                        Data5 = item.Data5 ?? "",
                        Data6 = item.Data6 ?? "",
                        Data7 = item.Data7 ?? "",
                        Data8 = item.Data8 ?? "",
                        Data9 = item.Data9 ?? "",
                        Data10 = item.Data10 ?? "",
                        Data11 = item.Data11 ?? "",
                        Data12 = item.Data12 ?? "",
                        Data13 = item.Data13 ?? "",
                        Data14 = item.Data14 ?? "",
                        Data15 = item.Data15 ?? "",
                        Data16 = item.Data16 ?? "",
                        Data17 = item.Data17 ?? "",
                        Data18 = item.Data18 ?? "",
                        Data19 = item.Data19 ?? "",
                        Data20 = item.Data20 ?? "",
                        Data21 = item.Data21 ?? "",
                        Data22 = item.Data22 ?? "",
                        Data23 = item.Data23 ?? "",
                        Data24 = item.Data24 ?? "",
                        Data25 = item.Data25 ?? "",
                        Data26 = item.Data26 ?? "",
                        Data27 = item.Data27 ?? "",
                        Data28 = item.Data28 ?? "",
                        Data29 = item.Data29 ?? "",
                        Data30 = item.Data30 ?? "",
                        Data31 = item.Data31 ?? ""
                    };

                    context.EmployeeTabels.Add(employee);
                }

                context.SaveChanges();
            }

            MessageBox.Show("Данные успешно сохранены в базу данных.");
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            string timesheetNumber = txtTimesheetNumber.Text;
            DateTime date = datePicker.SelectedDate.Value;
            string divisionName = txtDivisionName.Text;

            using (var context = new ApplicationContext())
            {
                var employees = context.EmployeeTabels.ToList();
                var organizationName = context.Organizations.FirstOrDefault()?.NameOrganization; // Получаем название организации

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Employee Data");
                    worksheet.Cell("R4").SetValue($"Дата: {date:dd/MM/yyyy}");
                    worksheet.Cell("N2").SetValue($"Табель учёта использования рабочего времени № {timesheetNumber}");
                    worksheet.Cell("Q3").SetValue($"Подразделение: {divisionName}");
                   worksheet.Cell("B2").SetValue($"Организация: {organizationName}");
                    worksheet.Cell("AH6").SetValue($"Итого дней");
                    worksheet.Cell("AI6").SetValue($"Итого отработано часов");
                    worksheet.Cell("B15").SetValue($"Ответственное лицо");


                    // Заголовки столбцов
                    var columns = new List<string> { "№п/п", "ФИО", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
                        "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", 
                        "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
                    for (int i = 0; i < columns.Count; i++)
                    {
                        worksheet.Cell(6, i + 1).Value = columns[i];
                    }

                    // Данные из базы данных
                    int row = 7;
                    foreach (var emp in employees)
                    {
                        worksheet.Cell(row, 1).Value = emp.Id;
                        worksheet.Column(1).Width = 5;
                        worksheet.Cell(row, 2).Value = emp.FullName;
                        worksheet.Column(2).Width = 25;
                        for (int i = 3; i <= 33; i++)
                        {
                            var dataValue = emp.GetType().GetProperty($"Data{i - 2}").GetValue(emp);
                            worksheet.Cell(row, i).Value = dataValue != null ? dataValue.ToString() : ""; 
                            worksheet.Column(i).Width = 2; 
                        }
                        row++;
                    }


                    // Сохранение и открытие файла
                    string filePath = @"C:\Users\Darya\OneDrive\Рабочий стол\курсач РПМ\Tabel\Tabel.xlsx";
                    workbook.SaveAs(filePath);
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
            }
        }


        public class Employee
        {
            public int Id { get; set; }
            public string FullName { get; set; }

            public string Data1 { get; set; }

            public string Data2 { get; set; }

            public string Data3 { get; set; }
            public string Data4 { get; set; }

            public string Data5 { get; set; }

            public string Data6 { get; set; }

            public string Data7 { get; set; }
            public string Data8 { get; set; }

            public string Data9 { get; set; }

            public string Data10 { get; set; }

            public string Data11 { get; set; }

            public string Data12 { get; set; }

            public string Data13 { get; set; }
            public string Data14 { get; set; }

            public string Data15 { get; set; }

            public string Data16 { get; set; }

            public string Data17 { get; set; }
            public string Data18 { get; set; }

            public string Data19 { get; set; }

            public string Data20
            {
                get; set;
            }
            public string Data21 { get; set; }

            public string Data22 { get; set; }

            public string Data23 { get; set; }
            public string Data24 { get; set; }

            public string Data25 { get; set; }

            public string Data26 { get; set; }

            public string Data27 { get; set; }
            public string Data28 { get; set; }

            public string Data29 { get; set; }

            public string Data30
            {
                get; set;
            }
            public string Data31
            {
                get; set;
            }
        }

        
    }
}
