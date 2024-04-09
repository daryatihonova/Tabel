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
    /// Логика взаимодействия для WindowNewDivision1.xaml
    /// </summary>
    public partial class WindowNewDivision1 : Window
    {
        public Division Division { get; private set; }
        public WindowNewDivision1(Division division)
        {
            InitializeComponent();
            Division = division;
            DataContext = Division;
        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
