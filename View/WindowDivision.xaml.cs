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
    /// Логика взаимодействия для WindowDivision.xaml
    /// </summary>
    public partial class WindowDivision : Window
    {
        ApplicationContext db = new ApplicationContext();
        public WindowDivision()
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
            db.Divisions.Load();
            // и устанавливаем данные в качестве контекста
            DataContext = db.Divisions.Local.ToObservableCollection();
        }

        // добавление
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            WindowNewDivision1 WindowNewDivision1 = new WindowNewDivision1(new Division());
            if (WindowNewDivision1.ShowDialog() == true)
            {
                Division Division = WindowNewDivision1.Division;
                if (!string.IsNullOrEmpty(Division.DivisionName))
                {
                    db.Divisions.Add(Division);
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // редактирование
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            Division? division = divisionList.SelectedItem as Division;
            // если ни одного объекта не выделено, выходим
            if (division is null) return;

            WindowNewDivision1 WindowNewDivision1 = new WindowNewDivision1(new Division
            {
                DivisionID = division.DivisionID,
                DivisionName = division.DivisionName,
               
            });

            if (WindowNewDivision1.ShowDialog() == true)
            {
                // получаем измененный объект
                division = db.Divisions.Find(WindowNewDivision1.Division.DivisionID);
                if (division != null)
                {
                    division.DivisionID = WindowNewDivision1.Division.DivisionID;
                    division.DivisionName = WindowNewDivision1.Division.DivisionName;
                    db.SaveChanges();
                    divisionList.Items.Refresh();
                }
            }
        }
        // удаление
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            Division? division = divisionList.SelectedItem as Division;
            // если ни одного объекта не выделено, выходим
            if (division is null) return;
            db.Divisions.Remove(division);
            db.SaveChanges();
        }
    }
}
