using PrimeTrack.Models;
using PrimeTrack.Views.Pages.Controller;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace PrimeTrack.Views.Pages
{
    public partial class ClientsPage : Page
    {
        public ClientsPage()
        {
            InitializeComponent();
            LoadClientsData();
        }

        private void LoadClientsData()
        {
            var connection = new Connection();

            try
            {
                connection.OpenConnection();
                using (var sqlConnection = connection.GetConnection())
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Клиент]", sqlConnection);
                    SqlDataReader reader = command.ExecuteReader();
                    var clients = new List<Client>();

                    while (reader.Read())
                    {
                        clients.Add(new Client
                        {
                            Код_Клиента = reader.GetInt32(0),
                            Фамилия = reader.GetString(1),
                            Имя = reader.GetString(2),
                            Отчество = reader.GetString(3),
                            Дата_Рождения = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                        });
                    }

                    ClientsDataGrid.ItemsSource = clients;
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
            finally
            {
                connection.CloseConnection();
            }
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            var addClientWindow = new AddEditClientWindow();
            addClientWindow.ShowDialog();
            LoadClientsData();
        }

        private void EditClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is Client selectedClient)
            {
                var editClientWindow = new AddEditClientWindow(selectedClient);
                editClientWindow.ShowDialog();
                LoadClientsData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для редактирования.");
            }
        }

        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is Client selectedClient)
            {
                var connection = new Connection();

                try
                {
                    connection.OpenConnection();
                    using (var sqlConnection = connection.GetConnection())
                    {
                        SqlCommand command = new SqlCommand("DELETE FROM [dbo].[Клиент] WHERE Код_Клиента = @Код_Клиента", sqlConnection);
                        command.Parameters.AddWithValue("@Код_Клиента", selectedClient.Код_Клиента);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Клиент успешно удален.");
                        LoadClientsData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении клиента: " + ex.Message);
                }
                finally
                {
                    connection.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для удаления.");
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text;
            var connection = new Connection();

            try
            {
                connection.OpenConnection();
                using (var sqlConnection = connection.GetConnection())
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Клиент] WHERE Фамилия LIKE @searchTerm OR Имя LIKE @searchTerm", sqlConnection);
                    command.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                    SqlDataReader reader = command.ExecuteReader();
                    var clients = new List<Client>();

                    while (reader.Read())
                    {
                        clients.Add(new Client
                        {
                            Код_Клиента = reader.GetInt32(0),
                            Фамилия = reader.GetString(1),
                            Имя = reader.GetString(2),
                            Отчество = reader.GetString(3),
                            Дата_Рождения = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                        });
                    }

                    ClientsDataGrid.ItemsSource = clients;
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при поиске данных: " + ex.Message);
            }
            finally
            {
                connection.CloseConnection();
            }
        }

        public class Client
        {
            public int Код_Клиента { get; set; }
            public string Фамилия { get; set; }
            public string Имя { get; set; }
            public string Отчество { get; set; }
            public DateTime? Дата_Рождения { get; set; }
        }
    }
}
