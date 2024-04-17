using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tabel.Model;

namespace Tabel.View
{
    /// <summary>
    /// Логика взаимодействия для WindowEmployee.xaml
    /// </summary>
    public partial class WindowEmployee : Window
    {
        ApplicationContext db = new ApplicationContext();
        public WindowEmployee()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        // при загрузке окна
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // гарантируем, что база данных создана
            db.Database.EnsureCreated();
            // загружаем данные из БД
            db.Employees.Load();
            // и устанавливаем данные в качестве контекста
            DataContext = db.Employees.Local.ToObservableCollection();
        }

        // добавление
        // Проверка существования значения в поле DivisionID из таблицы Divisions при добавлении сотрудника
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            WindowNewEmployee1 WindowNewEmployee1 = new WindowNewEmployee1(new Employee());
            if (WindowNewEmployee1.ShowDialog() == true)
            {
                Employee Employee = WindowNewEmployee1.Employee;

                // Проверка, что все обязательные поля заполнены
                if (!string.IsNullOrEmpty(Employee.FirstName))
                {
                    // Проверка существования организации с указанным OrganizationID
                    if (!db.Organizations.Any(o => o.OrganizationID == Employee.OrganizationID))
                    {
                        // Вывод сообщения в новом окне "Такой организации не существует"
                        MessageBox.Show("Такой организации не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Проверка существования подразделения с указанным DivisionID
                    if (!db.Divisions.Any(d => d.DivisionID == Employee.DivisionID))
                    {
                        // Вывод сообщения в новом окне "Такого подразделения не существует"
                        MessageBox.Show("Такого подразделения не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    db.Employees.Add(Employee);
                    db.SaveChanges();
                }
                else
                {
                    // Вывод сообщения о том, что все поля должны быть заполнены
                    MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Редактирование сотрудника
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выделенный объект
            Employee? employee = employeeList.SelectedItem as Employee;
            // Если ни одного объекта не выделено, выходим
            if (employee is null) return;

            WindowNewEmployee1 WindowNewEmployee1 = new WindowNewEmployee1(new Employee
            {
                EmployeeID = employee.EmployeeID,
                OrganizationID = employee.OrganizationID,
                DivisionID = employee.DivisionID,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Surname = employee.Surname,
                Birthday = employee.Birthday,
                JobTitle = employee.JobTitle
            });

            if (WindowNewEmployee1.ShowDialog() == true)
            {
                // Получаем измененный объект
                employee = db.Employees.Find(WindowNewEmployee1.Employee.EmployeeID);
                if (employee != null)
                {
                    // Проверка существования организации с указанным OrganizationID
                    if (!db.Organizations.Any(o => o.OrganizationID == WindowNewEmployee1.Employee.OrganizationID))
                    {
                        // Вывод сообщения в новом окне "Такой организации не существует"
                        MessageBox.Show("Такой организации не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Проверка существования подразделения с указанным DivisionID
                    if (!db.Divisions.Any(d => d.DivisionID == WindowNewEmployee1.Employee.DivisionID))
                    {
                        // Вывод сообщения в новом окне "Такого подразделения не существует"
                        MessageBox.Show("Такого подразделения не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    employee.EmployeeID = WindowNewEmployee1.Employee.EmployeeID;
                    employee.OrganizationID = WindowNewEmployee1.Employee.OrganizationID;
                    employee.DivisionID = WindowNewEmployee1.Employee.DivisionID;
                    employee.FirstName = WindowNewEmployee1.Employee.FirstName;
                    employee.LastName = WindowNewEmployee1.Employee.LastName;
                    employee.Surname = WindowNewEmployee1.Employee.Surname;
                    employee.Birthday = WindowNewEmployee1.Employee.Birthday;
                    employee.JobTitle = WindowNewEmployee1.Employee.JobTitle;
                    db.SaveChanges();
                    employeeList.Items.Refresh();
                }
            }
        }


        // удаление
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            Employee? employee = employeeList.SelectedItem as Employee;
            // если ни одного объекта не выделено, выходим
            if (employee is null) return;
            db.Employees.Remove(employee);
            db.SaveChanges();
        }
    }
}