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
    /// Логика взаимодействия для WindowDayType.xaml
    /// </summary>
    public partial class WindowDayType : Window
    {
        ApplicationContext db = new ApplicationContext();
        public WindowDayType()
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
            db.DayTypes.Load();
            // и устанавливаем данные в качестве контекста
            DataContext = db.DayTypes.Local.ToObservableCollection();
        }
        // добавление
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            WindowNewDayType1 WindowNewDayType1 = new WindowNewDayType1(new DayType());
            if (WindowNewDayType1.ShowDialog() == true)
            {
                DayType DayType = WindowNewDayType1.DayType;
                if (!string.IsNullOrEmpty(DayType.DayTypeName))
                {
                    db.DayTypes.Add(DayType);
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("DayTypeName не может быть пустым.");
                }
            }
        }

        // редактирование
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            DayType? daytype = daytypeList.SelectedItem as DayType;
            // если ни одного объекта не выделено, выходим
            if (daytype is null) return;

            WindowNewDayType1 WindowNewDayType1 = new WindowNewDayType1(new DayType
            {
                DayTypeID = daytype.DayTypeID,
                DayTypeName = daytype.DayTypeName,
                DayTypeShortName = daytype.DayTypeShortName,
                DayTypeHours = daytype.DayTypeHours,

            });

            if (WindowNewDayType1.ShowDialog() == true)
            {
                // получаем измененный объект
                daytype = db.DayTypes.Find(WindowNewDayType1.DayType.DayTypeID);
                if (daytype != null)
                {
                    daytype.DayTypeID = WindowNewDayType1.DayType.DayTypeID;
                    daytype.DayTypeName = WindowNewDayType1.DayType.DayTypeName;
                    daytype.DayTypeShortName = WindowNewDayType1.DayType.DayTypeShortName;
                    daytype.DayTypeHours = WindowNewDayType1.DayType.DayTypeHours;
                    db.SaveChanges();
                    daytypeList.Items.Refresh();
                }
            }
        }

        // удаление
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            DayType? daytype = daytypeList.SelectedItem as DayType;
            // если ни одного объекта не выделено, выходим
            if (daytype is null) return;
            db.DayTypes.Remove(daytype);
            db.SaveChanges();
        }
    }
}
