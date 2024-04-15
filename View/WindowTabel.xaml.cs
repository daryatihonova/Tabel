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
using DocumentFormat.OpenXml.Spreadsheet;

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
                int? dayTypeHours = context.DayTypes
                 .Where(dt => dt.DayTypeName == "Явка")
                 .Select(dt => dt.DayTypeHours)
                 .FirstOrDefault();



                //if (dayTypeHours != null)
                //{

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Employee Data");
                    worksheet.Cell("R4").SetValue($"Дата: {date:dd/MM/yyyy}");
                    worksheet.Cell("N2").SetValue($"Табель учёта использования рабочего времени № {timesheetNumber}");
                    worksheet.Cell("Q3").SetValue($"Подразделение: {divisionName}");
                    worksheet.Cell("B2").SetValue($"Организация: {organizationName}");


                    worksheet.Cell("A400").Value = dayTypeHours;


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
                   
                        range.FormulaA1 = "=IF(B7=\"\", \"\",IF(LOWER(C7)=\"я\",1,0)+IF(LOWER(D7)=\"я\",1,0)+IF(LOWER(E7)=\"я\",1,0)+IF(LOWER(F7)=\"я\",1,0)" +
                                          "+IF(LOWER(G7)=\"я\",1,0)+IF(LOWER(H7)=\"я\",1,0)+IF(LOWER(I7)=\"я\",1,0)+IF(LOWER(J7)=\"я\",1,0)" +
                                          "+IF(LOWER(K7)=\"я\",1,0)+IF(LOWER(L7)=\"я\",1,0)+IF(LOWER(M7)=\"я\",1,0)+IF(LOWER(N7)=\"я\",1,0)" +
                                          "+IF(LOWER(O7)=\"я\",1,0)+IF(LOWER(P7)=\"я\",1,0)+IF(LOWER(Q7)=\"я\",1,0)+IF(LOWER(R7)=\"я\",1,0)" +
                                          "+IF(LOWER(S7)=\"я\",1,0)+IF(LOWER(T7)=\"я\",1,0)+IF(LOWER(U7)=\"я\",1,0)+IF(LOWER(V7)=\"я\",1,0)" +
                                          "+IF(LOWER(W7)=\"я\",1,0)+IF(LOWER(X7)=\"я\",1,0)+IF(LOWER(Y7)=\"я\",1,0)+IF(LOWER(Z7)=\"я\",1,0)" +
                                          "+IF(LOWER(AA7)=\"я\",1,0)+IF(LOWER(AB7)=\"я\",1,0)+IF(LOWER(AC7)=\"я\",1,0)+IF(LOWER(AD7)=\"я\",1,0)" +
                                          "+IF(LOWER(AE7)=\"я\",1,0)+IF(LOWER(AF7)=\"я\",1,0)+IF(LOWER(AG7)=\"я\",1,0))";

                                                           

                    range2.FormulaA1 = "=IF(B8=\"\", \"\",IF(LOWER(C8)=\"я\",1,0)+IF(LOWER(D8)=\"я\",1,0)+IF(LOWER(E8)=\"я\",1,0)+IF(LOWER(F8)=\"я\",1,0)" +
                    "+IF(LOWER(G8)=\"я\",1,0)+IF(LOWER(H8)=\"я\",1,0)+IF(LOWER(I8)=\"я\",1,0)+IF(LOWER(J8)=\"я\",1,0)" +
                    "+IF(LOWER(K8)=\"я\",1,0)+IF(LOWER(L8)=\"я\",1,0)+IF(LOWER(M8)=\"я\",1,0)+IF(LOWER(N8)=\"я\",1,0)" +
                    "+IF(LOWER(O8)=\"я\",1,0)+IF(LOWER(P8)=\"я\",1,0)+IF(LOWER(Q8)=\"я\",1,0)+IF(LOWER(R8)=\"я\",1,0)" +
                    "+IF(LOWER(S8)=\"я\",1,0)+IF(LOWER(T8)=\"я\",1,0)+IF(LOWER(U8)=\"я\",1,0)+IF(LOWER(V8)=\"я\",1,0)" +
                    "+IF(LOWER(W8)=\"я\",1,0)+IF(LOWER(X8)=\"я\",1,0)+IF(LOWER(Y8)=\"я\",1,0)+IF(LOWER(Z8)=\"я\",1,0)" +
                    "+IF(LOWER(AA8)=\"я\",1,0)+IF(LOWER(AB8)=\"я\",1,0)+IF(LOWER(AC8)=\"я\",1,0)+IF(LOWER(AD8)=\"я\",1,0)" +
                    "+IF(LOWER(AE8)=\"я\",1,0)+IF(LOWER(AF8)=\"я\",1,0)+IF(LOWER(AG8)=\"я\",1,0))";


                    range3.FormulaA1 = "=IF(B9=\"\", \"\",IF(LOWER(C9)=\"я\",1,0)+IF(LOWER(D9)=\"я\",1,0)+IF(LOWER(E9)=\"я\",1,0)+IF(LOWER(F9)=\"я\",1,0)" +
                    "+IF(LOWER(G9)=\"я\",1,0)+IF(LOWER(H9)=\"я\",1,0)+IF(LOWER(I9)=\"я\",1,0)+IF(LOWER(J9)=\"я\",1,0)" +
                    "+IF(LOWER(K9)=\"я\",1,0)+IF(LOWER(L9)=\"я\",1,0)+IF(LOWER(M9)=\"я\",1,0)+IF(LOWER(N9)=\"я\",1,0)" +
                    "+IF(LOWER(O9)=\"я\",1,0)+IF(LOWER(P9)=\"я\",1,0)+IF(LOWER(Q9)=\"я\",1,0)+IF(LOWER(R9)=\"я\",1,0)" +
                    "+IF(LOWER(S9)=\"я\",1,0)+IF(LOWER(T9)=\"я\",1,0)+IF(LOWER(U9)=\"я\",1,0)+IF(LOWER(V9)=\"я\",1,0)" +
                    "+IF(LOWER(W9)=\"я\",1,0)+IF(LOWER(X9)=\"я\",1,0)+IF(LOWER(Y9)=\"я\",1,0)+IF(LOWER(Z9)=\"я\",1,0)" +
                    "+IF(LOWER(AA9)=\"я\",1,0)+IF(LOWER(AB9)=\"я\",1,0)+IF(LOWER(AC9)=\"я\",1,0)+IF(LOWER(AD9)=\"я\",1,0)" +
                    "+IF(LOWER(AE9)=\"я\",1,0)+IF(LOWER(AF9)=\"я\",1,0)+IF(LOWER(AG9)=\"я\",1,0))";


                    range4.FormulaA1 = "=IF(B10=\"\", \"\",IF(LOWER(C10)=\"я\",1,0)+IF(LOWER(D10)=\"я\",1,0)+IF(LOWER(E10)=\"я\",1,0)+IF(LOWER(F10)=\"я\",1,0)" +
                    "+IF(LOWER(G10)=\"я\",1,0)+IF(LOWER(H10)=\"я\",1,0)+IF(LOWER(I10)=\"я\",1,0)+IF(LOWER(J10)=\"я\",1,0)" +
                    "+IF(LOWER(K10)=\"я\",1,0)+IF(LOWER(L10)=\"я\",1,0)+IF(LOWER(M10)=\"я\",1,0)+IF(LOWER(N10)=\"я\",1,0)" +
                    "+IF(LOWER(O10)=\"я\",1,0)+IF(LOWER(P10)=\"я\",1,0)+IF(LOWER(Q10)=\"я\",1,0)+IF(LOWER(R10)=\"я\",1,0)" +
                    "+IF(LOWER(S10)=\"я\",1,0)+IF(LOWER(T10)=\"я\",1,0)+IF(LOWER(U10)=\"я\",1,0)+IF(LOWER(V10)=\"я\",1,0)" +
                    "+IF(LOWER(W10)=\"я\",1,0)+IF(LOWER(X10)=\"я\",1,0)+IF(LOWER(Y10)=\"я\",1,0)+IF(LOWER(Z10)=\"я\",1,0)" +
                    "+IF(LOWER(AA10)=\"я\",1,0)+IF(LOWER(AB10)=\"я\",1,0)+IF(LOWER(AC10)=\"я\",1,0)+IF(LOWER(AD10)=\"я\",1,0)" +
                    "+IF(LOWER(AE10)=\"я\",1,0)+IF(LOWER(AF10)=\"я\",1,0)+IF(LOWER(AG10)=\"я\",1,0))";

                    range5.FormulaA1 = "=IF(B11=\"\", \"\",IF(LOWER(C11)=\"я\",1,0)+IF(LOWER(D11)=\"я\",1,0)+IF(LOWER(E11)=\"я\",1,0)+IF(LOWER(F11)=\"я\",1,0)" +
                    "+IF(LOWER(G11)=\"я\",1,0)+IF(LOWER(H11)=\"я\",1,0)+IF(LOWER(I11)=\"я\",1,0)+IF(LOWER(J11)=\"я\",1,0)" +
                    "+IF(LOWER(K11)=\"я\",1,0)+IF(LOWER(L11)=\"я\",1,0)+IF(LOWER(M11)=\"я\",1,0)+IF(LOWER(N11)=\"я\",1,0)" +
                    "+IF(LOWER(O11)=\"я\",1,0)+IF(LOWER(P11)=\"я\",1,0)+IF(LOWER(Q11)=\"я\",1,0)+IF(LOWER(R11)=\"я\",1,0)" +
                    "+IF(LOWER(S11)=\"я\",1,0)+IF(LOWER(T11)=\"я\",1,0)+IF(LOWER(U11)=\"я\",1,0)+IF(LOWER(V11)=\"я\",1,0)" +
                    "+IF(LOWER(W11)=\"я\",1,0)+IF(LOWER(X11)=\"я\",1,0)+IF(LOWER(Y11)=\"я\",1,0)+IF(LOWER(Z11)=\"я\",1,0)" +
                    "+IF(LOWER(AA11)=\"я\",1,0)+IF(LOWER(AB11)=\"я\",1,0)+IF(LOWER(AC11)=\"я\",1,0)+IF(LOWER(AD11)=\"я\",1,0)" +
                    "+IF(LOWER(AE11)=\"я\",1,0)+IF(LOWER(AF11)=\"я\",1,0)+IF(LOWER(AG11)=\"я\",1,0))";
                    
                    range6.FormulaA1 = "=IF(B12=\"\", \"\",IF(LOWER(C12)=\"я\",1,0)+IF(LOWER(D12)=\"я\",1,0)+IF(LOWER(E12)=\"я\",1,0)+IF(LOWER(F12)=\"я\",1,0)" +
                                        "+IF(LOWER(G12)=\"я\",1,0)+IF(LOWER(H12)=\"я\",1,0)+IF(LOWER(I12)=\"я\",1,0)+IF(LOWER(J12)=\"я\",1,0)" +
                                        "+IF(LOWER(K12)=\"я\",1,0)+IF(LOWER(L12)=\"я\",1,0)+IF(LOWER(M12)=\"я\",1,0)+IF(LOWER(N12)=\"я\",1,0)" +
                                        "+IF(LOWER(O12)=\"я\",1,0)+IF(LOWER(P12)=\"я\",1,0)+IF(LOWER(Q12)=\"я\",1,0)+IF(LOWER(R12)=\"я\",1,0)" +
                                        "+IF(LOWER(S12)=\"я\",1,0)+IF(LOWER(T12)=\"я\",1,0)+IF(LOWER(U12)=\"я\",1,0)+IF(LOWER(V12)=\"я\",1,0)" +
                                        "+IF(LOWER(W12)=\"я\",1,0)+IF(LOWER(X12)=\"я\",1,0)+IF(LOWER(Y12)=\"я\",1,0)+IF(LOWER(Z12)=\"я\",1,0)" +
                                        "+IF(LOWER(AA12)=\"я\",1,0)+IF(LOWER(AB12)=\"я\",1,0)+IF(LOWER(AC12)=\"я\",1,0)+IF(LOWER(AD12)=\"я\",1,0)" +
                                        "+IF(LOWER(AE12)=\"я\",1,0)+IF(LOWER(AF12)=\"я\",1,0)+IF(LOWER(AG12)=\"я\",1,0))";

                    range7.FormulaA1 = "=IF(B13=\"\", \"\",IF(LOWER(C13)=\"я\",1,0)+IF(LOWER(D13)=\"я\",1,0)+IF(LOWER(E13)=\"я\",1,0)+IF(LOWER(F13)=\"я\",1,0)" +
                    "+IF(LOWER(G13)=\"я\",1,0)+IF(LOWER(H13)=\"я\",1,0)+IF(LOWER(I13)=\"я\",1,0)+IF(LOWER(J13)=\"я\",1,0)" +
                    "+IF(LOWER(K13)=\"я\",1,0)+IF(LOWER(L13)=\"я\",1,0)+IF(LOWER(M13)=\"я\",1,0)+IF(LOWER(N13)=\"я\",1,0)" +
                    "+IF(LOWER(O13)=\"я\",1,0)+IF(LOWER(P13)=\"я\",1,0)+IF(LOWER(Q13)=\"я\",1,0)+IF(LOWER(R13)=\"я\",1,0)" +
                    "+IF(LOWER(S13)=\"я\",1,0)+IF(LOWER(T13)=\"я\",1,0)+IF(LOWER(U13)=\"я\",1,0)+IF(LOWER(V13)=\"я\",1,0)" +
                    "+IF(LOWER(W13)=\"я\",1,0)+IF(LOWER(X13)=\"я\",1,0)+IF(LOWER(Y13)=\"я\",1,0)+IF(LOWER(Z13)=\"я\",1,0)" +
                    "+IF(LOWER(AA13)=\"я\",1,0)+IF(LOWER(AB13)=\"я\",1,0)+IF(LOWER(AC13)=\"я\",1,0)+IF(LOWER(AD13)=\"я\",1,0)" +
                    "+IF(LOWER(AE13)=\"я\",1,0)+IF(LOWER(AF13)=\"я\",1,0)+IF(LOWER(AG13)=\"я\",1,0))";



                    range8.FormulaA1 = "=IF(B14=\"\", \"\",IF(LOWER(C14)=\"я\",1,0)+IF(LOWER(D14)=\"я\",1,0)+IF(LOWER(E14)=\"я\",1,0)+IF(LOWER(F14)=\"я\",1,0)" +
                    "+IF(LOWER(G14)=\"я\",1,0)+IF(LOWER(H14)=\"я\",1,0)+IF(LOWER(I14)=\"я\",1,0)+IF(LOWER(J14)=\"я\",1,0)" +
                    "+IF(LOWER(K14)=\"я\",1,0)+IF(LOWER(L14)=\"я\",1,0)+IF(LOWER(M14)=\"я\",1,0)+IF(LOWER(N14)=\"я\",1,0)" +
                    "+IF(LOWER(O14)=\"я\",1,0)+IF(LOWER(P14)=\"я\",1,0)+IF(LOWER(Q14)=\"я\",1,0)+IF(LOWER(R14)=\"я\",1,0)" +
                    "+IF(LOWER(S14)=\"я\",1,0)+IF(LOWER(T14)=\"я\",1,0)+IF(LOWER(U14)=\"я\",1,0)+IF(LOWER(V14)=\"я\",1,0)" +
                    "+IF(LOWER(W14)=\"я\",1,0)+IF(LOWER(X14)=\"я\",1,0)+IF(LOWER(Y14)=\"я\",1,0)+IF(LOWER(Z14)=\"я\",1,0)" +
                    "+IF(LOWER(AA14)=\"я\",1,0)+IF(LOWER(AB14)=\"я\",1,0)+IF(LOWER(AC14)=\"я\",1,0)+IF(LOWER(AD14)=\"я\",1,0)" +
                    "+IF(LOWER(AE14)=\"я\",1,0)+IF(LOWER(AF14)=\"я\",1,0)+IF(LOWER(AG14)=\"я\",1,0))";




                    // Определяем ячейки для вставки формулы
                    var range9 = worksheet.Range("AI7");
                    var range10 = worksheet.Range("AI8");
                    var range11 = worksheet.Range("AI9");
                    var range12 = worksheet.Range("AI10");
                    var range13 = worksheet.Range("AI11");
                    var range14 = worksheet.Range("AI12");
                    var range15 = worksheet.Range("AI13");
                    var range16 = worksheet.Range("AI14");
                    // Вставляем формулу в указанный диапазон ячеек
                    range9.FormulaA1 = "=IF(B7=\"\", \"\",AH7*$A$400)";
                    range10.FormulaA1 = "=IF(B8=\"\", \"\",AH8*$A$400)";
                    range11.FormulaA1 = "=IF(B9=\"\", \"\",AH9*$A$400)";
                    range12.FormulaA1 = "=IF(B10=\"\", \"\",AH10*$A$400)";
                    range13.FormulaA1 = "=IF(B11=\"\", \"\",AH11*$A$400)";
                    range14.FormulaA1 = "=IF(B12=\"\", \"\",AH12*$A$400)";
                    range15.FormulaA1 = "=IF(B13=\"\", \"\",AH13*$A$400)";
                    range16.FormulaA1 = "=IF(B14=\"\", \"\",AH14*$A$400)";



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
            //}
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
