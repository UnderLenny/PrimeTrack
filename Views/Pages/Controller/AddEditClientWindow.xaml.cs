using PrimeTrack.Models;
using System;
using System.Data.SqlClient;
using System.Windows;
using static PrimeTrack.Views.Pages.ClientsPage;

namespace PrimeTrack.Views.Pages.Controller
{
    /// <summary>
    /// Логика взаимодействия для AddEditClientWindow.xaml
    /// </summary>
    public partial class AddEditClientWindow : Window
    {
        private readonly Client _client;

        public AddEditClientWindow(Client client = null)
        {
            InitializeComponent();
            _client = client;

            if (_client != null)
            {
                LastNameTextBox.Text = _client.Фамилия;
                FirstNameTextBox.Text = _client.Имя;
                MiddleNameTextBox.Text = _client.Отчество;
                BirthDatePicker.SelectedDate = _client.Дата_Рождения;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string lastName = LastNameTextBox.Text;
            string firstName = FirstNameTextBox.Text;
            string middleName = MiddleNameTextBox.Text;
            DateTime? birthDate = BirthDatePicker.SelectedDate;

            var connection = new Connection();

            try
            {
                connection.OpenConnection();
                using (var sqlConnection = connection.GetConnection())
                {
                    SqlCommand command;

                    if (_client == null) // Добавление нового клиента
                    {
                        command = new SqlCommand(
                            "INSERT INTO [dbo].[Клиент] (Фамилия, Имя, Отчество, Дата_Рождения) VALUES (@Фамилия, @Имя, @Отчество, @Дата_Рождения)",
                            sqlConnection);
                    }
                    else // Редактирование существующего клиента
                    {
                        command = new SqlCommand(
                            "UPDATE [dbo].[Клиент] SET Фамилия = @Фамилия, Имя = @Имя, Отчество = @Отчество, Дата_Рождения = @Дата_Рождения WHERE Код_Клиента = @Код_Клиента",
                            sqlConnection);
                        command.Parameters.AddWithValue("@Код_Клиента", _client.Код_Клиента);
                    }

                    command.Parameters.AddWithValue("@Фамилия", lastName);
                    command.Parameters.AddWithValue("@Имя", firstName);
                    command.Parameters.AddWithValue("@Отчество", middleName);
                    command.Parameters.AddWithValue("@Дата_Рождения", (object)birthDate ?? DBNull.Value);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Клиент успешно сохранен.");
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении клиента: " + ex.Message);
            }
            finally
            {
                connection.CloseConnection();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public Client Client => _client;
    }
}
