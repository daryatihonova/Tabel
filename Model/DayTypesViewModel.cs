using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Tabel.Model;
using Microsoft.Data.Sqlite;
using System.Runtime.CompilerServices;

namespace Tabel.ViewModel
{
    public class DayTypesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DayType> dayTypesList;
        public ObservableCollection<DayType> DayTypesList
        {
            get { return dayTypesList; }
            set
            {
                dayTypesList = value;
                OnPropertyChanged();
            }
        }

        private const string connectionString = "Data Source=C:\\Users\\Darya\\OneDrive\\Рабочий стол\\курсач РПМ\\Tabel\\bin\\Debug\\net6.0-windows\\Tabel.db";

        public DayTypesViewModel()
        {
            LoadDayTypesFromDatabase();
        }

        private void LoadDayTypesFromDatabase()
        {
            dayTypesList = new ObservableCollection<DayType>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM DayTypes";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DayType dayType = new DayType
                            {
                                DayTypeID = reader.GetInt32(0),
                                DayTypeName = reader.GetString(1),
                                DayTypeShortName = reader.GetString(2),
                                DayTypeHours = reader.GetInt32(3)
                            };
                            dayTypesList.Add(dayType);
                        }
                    }
                }
            }
        }

        // Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

