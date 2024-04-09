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
    /// Логика взаимодействия для WindowNewEmployee1.xaml
    /// </summary>
    public partial class WindowNewEmployee1 : Window
    {
        public Employee Employee { get; private set; }
        public WindowNewEmployee1(Employee employee)
        {
            InitializeComponent();
            Employee = employee;
            DataContext = Employee;
        }

               void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
