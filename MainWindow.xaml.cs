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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tabel.View;

namespace Tabel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Employee_OnClick(object sender, RoutedEventArgs e)
        {
            WindowEmployee wEmployee = new WindowEmployee();
            wEmployee.Show();
        }
        private void Organization_OnClick(object sender, RoutedEventArgs e)
        {
            WindowOrganization wOrganization = new WindowOrganization();
            wOrganization.Show();
        }
        private void Division_OnClick(object sender, RoutedEventArgs e)
        {
            WindowDivision wDivision = new WindowDivision();
            wDivision.Show();
        }


        private void OpenTabel(object sender, RoutedEventArgs e)
        {
            WindowTabel wTabel = new WindowTabel();
            this.Visibility = Visibility.Hidden;
            wTabel.Show();
        }

        private void DayType_OnClick(object sender, RoutedEventArgs e)
        {
            WindowDayType wDayType = new WindowDayType();
            wDayType.Show();
        }
    }
}
