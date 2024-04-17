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
        // Проверка ввода отрицательного числа в поле DayTypeHours при добавлении типа дня
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            WindowNewDayType1 WindowNewDayType1 = new WindowNewDayType1(new DayType());
            if (WindowNewDayType1.ShowDialog() == true)
            {
                DayType DayType = WindowNewDayType1.DayType;

                // Проверка, что все обязательные поля заполнены
                if (!string.IsNullOrEmpty(DayType.DayTypeName))
                {
                    // Проверка, что количество отработанных часов не является отрицательным
                    if (DayType.DayTypeHours < 0)
                    {
                        // Вывод сообщения о том, что количество отработанных часов должно быть положительным числом
                        MessageBox.Show("Количество отработанных часов должно быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    db.DayTypes.Add(DayType);
                    db.SaveChanges();
                }
                else
                {
                    // Вывод сообщения о том, что все поля должны быть заполнены
                    MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Редактирование типа дня
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выделенный объект
            DayType? daytype = daytypeList.SelectedItem as DayType;
            // Если ни одного объекта не выделено, выходим
            if (daytype is null) return;

            WindowNewDayType1 WindowNewDayType1 = new WindowNewDayType1(new DayType
            {
                DayTypeID = daytype.DayTypeID,
                DayTypeName = daytype.DayTypeName,
                DayTypeShortName = daytype.DayTypeShortName,
                DayTypeHours = daytype.DayTypeHours
            });

            if (WindowNewDayType1.ShowDialog() == true)
            {
                // Получаем измененный объект
                daytype = db.DayTypes.Find(WindowNewDayType1.DayType.DayTypeID);
                if (daytype != null)
                {
                    // Проверка, что количество отработанных часов не является отрицательным
                    if (WindowNewDayType1.DayType.DayTypeHours < 0)
                    {
                        // Вывод сообщения о том, что количество отработанных часов должно быть положительным числом
                        MessageBox.Show("Количество отработанных часов должно быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

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
