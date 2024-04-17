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
    /// Логика взаимодействия для WindowOrganization.xaml
    /// </summary>
    public partial class WindowOrganization : Window
    {
        ApplicationContext db = new ApplicationContext();
        public WindowOrganization()
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
            db.Organizations.Load();
            // и устанавливаем данные в качестве контекста
            DataContext = db.Organizations.Local.ToObservableCollection();
        }

        // добавление
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            WindowNewOrganization1 WindowNewOrganization1 = new WindowNewOrganization1(new Organization());
            if (WindowNewOrganization1.ShowDialog() == true)
            {
                Organization Organization = WindowNewOrganization1.Organization;
                if (!string.IsNullOrEmpty(Organization.NameOrganization))
                {
                    db.Organizations.Add(Organization);
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
            Organization? organization = organizationList.SelectedItem as Organization;
            // если ни одного объекта не выделено, выходим
            if (organization is null) return;

            WindowNewOrganization1 WindowNewOrganization1 = new WindowNewOrganization1(new Organization
            {
                OrganizationID = organization.OrganizationID,
                NameOrganization = organization.NameOrganization,
                Managment = organization.Managment,
                City = organization.City,
                Street = organization.Street,
                House = organization.House,
                Office = organization.Office,
                Email = organization.Email,

            });

            if (WindowNewOrganization1.ShowDialog() == true)
            {
                // получаем измененный объект
                organization = db.Organizations.Find(WindowNewOrganization1.Organization.OrganizationID);
                if (organization != null)
                {
                    organization.OrganizationID = WindowNewOrganization1.Organization.OrganizationID;
                    organization.NameOrganization = WindowNewOrganization1.Organization.NameOrganization;
                    organization.Managment = WindowNewOrganization1.Organization.Managment;
                    organization.City = WindowNewOrganization1.Organization.City;
                    organization.Street = WindowNewOrganization1.Organization.Street;
                    organization.House = WindowNewOrganization1.Organization.House;
                    organization.Office = WindowNewOrganization1.Organization.Office;
                    organization.Email = WindowNewOrganization1.Organization.Email;
                    db.SaveChanges();
                    organizationList.Items.Refresh();
                }
            }
        }
        // удаление
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            Organization? organization = organizationList.SelectedItem as Organization;
            // если ни одного объекта не выделено, выходим
            if (organization is null) return;
            db.Organizations.Remove(organization);
            db.SaveChanges();
        }
    }
}
