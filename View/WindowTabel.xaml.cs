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
                        Data31 = item.Data31 ?? "",
                       

                    };
                    //// Присвоение значения itogdays перед сохранением
                    //employee.itogdays = item.CountOccurrencesOfLetterYa(item);

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
                   
                    worksheet.Cell("B15").SetValue($"Ответственное лицо");
                    // Определяем ячейки для вставки формулы
                    var range = worksheet.Range("AH7");
                    var range2 = worksheet.Range("AH8");
                    var range3 = worksheet.Range("AH9");
                    var range4 = worksheet.Range("AH10");
                    var range5 = worksheet.Range("AH11");
                    var range6 = worksheet.Range("AH12");
                    var range7 = worksheet.Range("AH13");
                    var range8 = worksheet.Range("AH14");

                    // Вставляем формулу в указанный диапазон ячеек
                    range.FormulaA1 = "=IF(C7=\"я\",1,0)+IF(D7=\"я\",1,0)+IF(E7=\"я\",1,0)+IF(F7=\"я\",1,0)" +
                        "+IF(G7=\"я\",1,0)+IF(H7=\"я\",1,0)+IF(I7=\"я\",1,0)+IF(J7=\"я\",1,0)+IF(K7=\"я\",1,0)+IF(L7=\"я\",1,0)+IF(M7=\"я\",1,0)+IF(N7=\"я\",1,0)" +
                        "+IF(G7=\"я\",1,0)+IF(O7=\"я\",1,0)+IF(P7=\"я\",1,0)+IF(Q7=\"я\",1,0)+IF(R7=\"я\",1,0)" +
                        "+IF(S7=\"я\",1,0)+IF(T7=\"я\",1,0)+IF(U7=\"я\",1,0)+IF(V7=\"я\",1,0)+IF(W7=\"я\",1,0)+IF(X7=\"я\",1,0)" +
                        "+IF(Y7=\"я\",1,0)+IF(Z7=\"я\",1,0)+IF(AA7=\"я\",1,0)+IF(AB7=\"я\",1,0)+IF(AC7=\"я\",1,0)+IF(AD7=\"я\",1,0)+IF(AE7=\"я\",1,0)+IF(AF7=\"я\",1,0)+IF(AG7=\"я\",1,0)";

                    range2.FormulaA1 = "=IF(C8=\"я\",1,0)+IF(D8=\"я\",1,0)+IF(E8=\"я\",1,0)+IF(F8=\"я\",1,0)" +
                                           "+IF(G8=\"я\",1,0)+IF(H8=\"я\",1,0)+IF(I8=\"я\",1,0)+IF(J8=\"я\",1,0)+IF(K8=\"я\",1,0)+IF(L8=\"я\",1,0)+IF(M8=\"я\",1,0)+IF(N8=\"я\",1,0)" +
                                           "+IF(G8=\"я\",1,0)+IF(O8=\"я\",1,0)+IF(P8=\"я\",1,0)+IF(Q8=\"я\",1,0)+IF(R8=\"я\",1,0)" +
                                           "+IF(S8=\"я\",1,0)+IF(T8=\"я\",1,0)+IF(U8=\"я\",1,0)+IF(V8=\"я\",1,0)+IF(W8=\"я\",1,0)+IF(X8=\"я\",1,0)" +
                                           "+IF(Y8=\"я\",1,0)+IF(Z8=\"я\",1,0)+IF(AA8=\"я\",1,0)+IF(AB8=\"я\",1,0)+IF(AC8=\"я\",1,0)+IF(AD8=\"я\",1,0)+IF(AE8=\"я\",1,0)+IF(AF8=\"я\",1,0)+IF(AG8=\"я\",1,0)";
                    range3.FormulaA1 = "=IF(C9=\"я\",1,0)+IF(D9=\"я\",1,0)+IF(E9=\"я\",1,0)+IF(F9=\"я\",1,0)" +
                       "+IF(G9=\"я\",1,0)+IF(H9=\"я\",1,0)+IF(I9=\"я\",1,0)+IF(J9=\"я\",1,0)+IF(K9=\"я\",1,0)+IF(L9=\"я\",1,0)+IF(M9=\"я\",1,0)+IF(N9=\"я\",1,0)" +
                       "+IF(G9=\"я\",1,0)+IF(O9=\"я\",1,0)+IF(P9=\"я\",1,0)+IF(Q9=\"я\",1,0)+IF(R9=\"я\",1,0)" +
                       "+IF(S9=\"я\",1,0)+IF(T9=\"я\",1,0)+IF(U9=\"я\",1,0)+IF(V9=\"я\",1,0)+IF(W9=\"я\",1,0)+IF(X9=\"я\",1,0)" +
                       "+IF(Y9=\"я\",1,0)+IF(Z9=\"я\",1,0)+IF(AA9=\"я\",1,0)+IF(AB9=\"я\",1,0)+IF(AC9=\"я\",1,0)+IF(AD9=\"я\",1,0)+IF(AE9=\"я\",1,0)+IF(AF9=\"я\",1,0)+IF(AG9=\"я\",1,0)";

                    range4.FormulaA1 = "=IF(C10=\"я\",1,0)+IF(D10=\"я\",1,0)+IF(E10=\"я\",1,0)+IF(F10=\"я\",1,0)" +
                                           "+IF(G10=\"я\",1,0)+IF(H10=\"я\",1,0)+IF(I10=\"я\",1,0)+IF(J10=\"я\",1,0)+IF(K10=\"я\",1,0)+IF(L10=\"я\",1,0)+IF(M10=\"я\",1,0)+IF(N10=\"я\",1,0)" +
                                           "+IF(G10=\"я\",1,0)+IF(O10=\"я\",1,0)+IF(P10=\"я\",1,0)+IF(Q10=\"я\",1,0)+IF(R10=\"я\",1,0)" +
                                           "+IF(S10=\"я\",1,0)+IF(T10=\"я\",1,0)+IF(U10=\"я\",1,0)+IF(V10=\"я\",1,0)+IF(W10=\"я\",1,0)+IF(X10=\"я\",1,0)" +
                                           "+IF(Y10=\"я\",1,0)+IF(Z10=\"я\",1,0)+IF(AA10=\"я\",1,0)+IF(AB10=\"я\",1,0)+IF(AC10=\"я\",1,0)+IF(AD10=\"я\",1,0)+IF(AE10=\"я\",1,0)+IF(AF10=\"я\",1,0)+IF(AG10=\"я\",1,0)";
                    range5.FormulaA1 = "=IF(C11=\"я\",1,0)+IF(D11=\"я\",1,0)+IF(E11=\"я\",1,0)+IF(F11=\"я\",1,0)" +
                        "+IF(G11=\"я\",1,0)+IF(H11=\"я\",1,0)+IF(I11=\"я\",1,0)+IF(J11=\"я\",1,0)+IF(K11=\"я\",1,0)+IF(L11=\"я\",1,0)+IF(M11=\"я\",1,0)+IF(N11=\"я\",1,0)" +
                        "+IF(G11=\"я\",1,0)+IF(O11=\"я\",1,0)+IF(P11=\"я\",1,0)+IF(Q11=\"я\",1,0)+IF(R11=\"я\",1,0)" +
                        "+IF(S11=\"я\",1,0)+IF(T11=\"я\",1,0)+IF(U11=\"я\",1,0)+IF(V11=\"я\",1,0)+IF(W11=\"я\",1,0)+IF(X11=\"я\",1,0)" +
                        "+IF(Y11=\"я\",1,0)+IF(Z11=\"я\",1,0)+IF(AA11=\"я\",1,0)+IF(AB11=\"я\",1,0)+IF(AC11=\"я\",1,0)+IF(AD11=\"я\",1,0)+IF(AE11=\"я\",1,0)+IF(AF11=\"я\",1,0)+IF(AG11=\"я\",1,0)";

                    range8.FormulaA1 = "=IF(C14=\"я\",1,0)+IF(D14=\"я\",1,0)+IF(E14=\"я\",1,0)+IF(F14=\"я\",1,0)" +
                                           "+IF(G14=\"я\",1,0)+IF(H14=\"я\",1,0)+IF(I14=\"я\",1,0)+IF(J14=\"я\",1,0)+IF(K14=\"я\",1,0)+IF(L14=\"я\",1,0)+IF(M14=\"я\",1,0)+IF(N14=\"я\",1,0)" +
                                           "+IF(G14=\"я\",1,0)+IF(O14=\"я\",1,0)+IF(P14=\"я\",1,0)+IF(Q14=\"я\",1,0)+IF(R14=\"я\",1,0)" +
                                           "+IF(S14=\"я\",1,0)+IF(T14=\"я\",1,0)+IF(U14=\"я\",1,0)+IF(V14=\"я\",1,0)+IF(W14=\"я\",1,0)+IF(X14=\"я\",1,0)" +
                                           "+IF(Y14=\"я\",1,0)+IF(Z14=\"я\",1,0)+IF(AA14=\"я\",1,0)+IF(AB14=\"я\",1,0)+IF(AC14=\"я\",1,0)+IF(AD14=\"я\",1,0)+IF(AE14=\"я\",1,0)+IF(AF14=\"я\",1,0)+IF(AG14=\"я\",1,0)";
                    range6.FormulaA1 = "=IF(C12=\"я\",1,0)+IF(D12=\"я\",1,0)+IF(E12=\"я\",1,0)+IF(F12=\"я\",1,0)" +
                       "+IF(G12=\"я\",1,0)+IF(H12=\"я\",1,0)+IF(I12=\"я\",1,0)+IF(J12=\"я\",1,0)+IF(K12=\"я\",1,0)+IF(L12=\"я\",1,0)+IF(M12=\"я\",1,0)+IF(N12=\"я\",1,0)" +
                       "+IF(G12=\"я\",1,0)+IF(O12=\"я\",1,0)+IF(P12=\"я\",1,0)+IF(Q12=\"я\",1,0)+IF(R12=\"я\",1,0)" +
                       "+IF(S12=\"я\",1,0)+IF(T12=\"я\",1,0)+IF(U12=\"я\",1,0)+IF(V12=\"я\",1,0)+IF(W12=\"я\",1,0)+IF(X12=\"я\",1,0)" +
                       "+IF(Y12=\"я\",1,0)+IF(Z12=\"я\",1,0)+IF(AA12=\"я\",1,0)+IF(AB12=\"я\",1,0)+IF(AC12=\"я\",1,0)+IF(AD12=\"я\",1,0)+IF(AE12=\"я\",1,0)+IF(AF12=\"я\",1,0)+IF(AG12=\"я\",1,0)";

                    range7.FormulaA1 = "=IF(C13=\"я\",1,0)+IF(D13=\"я\",1,0)+IF(E13=\"я\",1,0)+IF(F13=\"я\",1,0)" +
                                           "+IF(G13=\"я\",1,0)+IF(H13=\"я\",1,0)+IF(I13=\"я\",1,0)+IF(J13=\"я\",1,0)+IF(K13=\"я\",1,0)+IF(L13=\"я\",1,0)+IF(M13=\"я\",1,0)+IF(N13=\"я\",1,0)" +
                                           "+IF(G13=\"я\",1,0)+IF(O13=\"я\",1,0)+IF(P13=\"я\",1,0)+IF(Q13=\"я\",1,0)+IF(R13=\"я\",1,0)" +
                                           "+IF(S13=\"я\",1,0)+IF(T13=\"я\",1,0)+IF(U13=\"я\",1,0)+IF(V13=\"я\",1,0)+IF(W13=\"я\",1,0)+IF(X13=\"я\",1,0)" +
                                           "+IF(Y13=\"я\",1,0)+IF(Z13=\"я\",1,0)+IF(AA13=\"я\",1,0)+IF(AB13=\"я\",1,0)+IF(AC13=\"я\",1,0)+IF(AD13=\"я\",1,0)+IF(AE13=\"я\",1,0)+IF(AF13=\"я\",1,0)+IF(AG13=\"я\",1,0)";


                    // Заголовки столбцов
                    var columns = new List<string> { "№п/п", "ФИО", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
                        "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", 
                        "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "Итого дней", "Итого отработано часов" };
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
            public int itogdays
            {
                get; set;
            }
            public int itoghours
            {
                get; set;
            }

        //    public int CountOccurrencesOfLetterYa(Employee item)
        //    {
        //        int count = 0;

        //        using (var context = new ApplicationContext())
        //        {
        //            var employeeTable = context.EmployeeTabels.ToList();

        //            foreach (var employee in employeeTable)
        //            {

        //                count += CountOccurrencesInString(employee.Data1, 'Я');
        //                count += CountOccurrencesInString(employee.Data1, 'я');
        //                count += CountOccurrencesInString(employee.Data2, 'Я');
        //                count += CountOccurrencesInString(employee.Data2, 'я');
        //                count += CountOccurrencesInString(employee.Data3, 'Я');
        //                count += CountOccurrencesInString(employee.Data3, 'я');
        //                count += CountOccurrencesInString(employee.Data4, 'Я');
        //                count += CountOccurrencesInString(employee.Data4, 'я');
        //                count += CountOccurrencesInString(employee.Data5, 'Я');
        //                count += CountOccurrencesInString(employee.Data5, 'я');
        //                count += CountOccurrencesInString(employee.Data6, 'Я');
        //                count += CountOccurrencesInString(employee.Data6, 'Я');
        //                count += CountOccurrencesInString(employee.Data7, 'Я');
        //                count += CountOccurrencesInString(employee.Data7, 'я');
        //                count += CountOccurrencesInString(employee.Data8, 'Я');
        //                count += CountOccurrencesInString(employee.Data8, 'я');
        //                count += CountOccurrencesInString(employee.Data9, 'Я');
        //                count += CountOccurrencesInString(employee.Data9, 'я');
        //                count += CountOccurrencesInString(employee.Data10, 'Я');
        //                count += CountOccurrencesInString(employee.Data10, 'я');
        //                count += CountOccurrencesInString(employee.Data11, 'Я');
        //                count += CountOccurrencesInString(employee.Data11, 'я');
        //                count += CountOccurrencesInString(employee.Data12, 'Я');
        //                count += CountOccurrencesInString(employee.Data12, 'я');
        //                count += CountOccurrencesInString(employee.Data13, 'Я');
        //                count += CountOccurrencesInString(employee.Data13, 'я');
        //                count += CountOccurrencesInString(employee.Data14, 'Я');
        //                count += CountOccurrencesInString(employee.Data14, 'я');
        //                count += CountOccurrencesInString(employee.Data15, 'Я');
        //                count += CountOccurrencesInString(employee.Data15, 'я');
        //                count += CountOccurrencesInString(employee.Data16, 'Я');
        //                count += CountOccurrencesInString(employee.Data16, 'я');
        //                count += CountOccurrencesInString(employee.Data17, 'Я');
        //                count += CountOccurrencesInString(employee.Data17, 'я');
        //                count += CountOccurrencesInString(employee.Data18, 'Я');
        //                count += CountOccurrencesInString(employee.Data18, 'я');
        //                count += CountOccurrencesInString(employee.Data19, 'Я');
        //                count += CountOccurrencesInString(employee.Data19, 'я');
        //                count += CountOccurrencesInString(employee.Data20, 'Я');
        //                count += CountOccurrencesInString(employee.Data20, 'я');
        //                count += CountOccurrencesInString(employee.Data21, 'Я');
        //                count += CountOccurrencesInString(employee.Data21, 'я');
        //                count += CountOccurrencesInString(employee.Data22, 'Я');
        //                count += CountOccurrencesInString(employee.Data22, 'я');
        //                count += CountOccurrencesInString(employee.Data23, 'Я');
        //                count += CountOccurrencesInString(employee.Data23, 'я');
        //                count += CountOccurrencesInString(employee.Data24, 'Я');
        //                count += CountOccurrencesInString(employee.Data24, 'я');
        //                count += CountOccurrencesInString(employee.Data25, 'Я');
        //                count += CountOccurrencesInString(employee.Data25, 'я');
        //                count += CountOccurrencesInString(employee.Data26, 'Я');
        //                count += CountOccurrencesInString(employee.Data26, 'я');
        //                count += CountOccurrencesInString(employee.Data27, 'Я');
        //                count += CountOccurrencesInString(employee.Data27, 'я');
        //                count += CountOccurrencesInString(employee.Data28, 'Я');
        //                count += CountOccurrencesInString(employee.Data28, 'я');
        //                count += CountOccurrencesInString(employee.Data29, 'Я');
        //                count += CountOccurrencesInString(employee.Data29, 'я');
        //                count += CountOccurrencesInString(employee.Data30, 'Я');
        //                count += CountOccurrencesInString(employee.Data30, 'я');
        //                count += CountOccurrencesInString(employee.Data31, 'Я');
        //                count += CountOccurrencesInString(employee.Data31, 'я');

        //            }
        //        }

        //        return count;
        //    }

        //    public int CountOccurrencesInString(string input, char letter)
        //    {
        //        if (string.IsNullOrEmpty(input))
        //        {
        //            return 0;
        //        }

        //        return input.Count(c => c == letter);
        //    }
        }

        
    }
}
