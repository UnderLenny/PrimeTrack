using PrimeTrack.Models;
using PrimeTrack.Views.Pages.Controller;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace PrimeTrack.Views.Pages
{
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            LoadUsersData();
        }

        private void LoadUsersData()
        {
            var connection = new Connection();

            try
            {
                connection.OpenConnection();
                using (var sqlConnection = connection.GetConnection())
                {
                    SqlCommand command = new SqlCommand("SELECT ID_Пользователя, Логин, Пароль_Hash, Пароль_Salt, Дата_Создания FROM [dbo].[Пользователь]", sqlConnection);
                    SqlDataReader reader = command.ExecuteReader();
                    var users = new List<User>();

                    while (reader.Read())
                    {
                        byte[] passwordHash = (byte[])reader["Пароль_Hash"];
                        byte[] passwordSalt = (byte[])reader["Пароль_Salt"];

                        users.Add(new User
                        {
                            ID_Пользователя = reader.GetInt32(0),
                            Логин = reader.GetString(1),
                            Пароль_Hash = Convert.ToBase64String(passwordHash), // Для удобства отображения
                            Пароль_Salt = Convert.ToBase64String(passwordSalt), // Для удобства отображения
                            Дата_Создания = reader.GetDateTime(4)
                        });
                    }

                    UsersDataGrid.ItemsSource = users;
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

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AddEditUserWindow();
            addUserWindow.ShowDialog();
            LoadUsersData();
        }

        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                var editUserWindow = new AddEditUserWindow(selectedUser);
                editUserWindow.ShowDialog();
                LoadUsersData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для редактирования.");
            }
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                var connection = new Connection();

                try
                {
                    connection.OpenConnection();
                    using (var sqlConnection = connection.GetConnection())
                    {
                        SqlCommand command = new SqlCommand("DELETE FROM [dbo].[Пользователь] WHERE ID_Пользователя = @ID_Пользователя", sqlConnection);
                        command.Parameters.AddWithValue("@ID_Пользователя", selectedUser.ID_Пользователя);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Пользователь успешно удален.");
                        LoadUsersData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении пользователя: " + ex.Message);
                }
                finally
                {
                    connection.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для удаления.");
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
                    SqlCommand command = new SqlCommand("SELECT ID_Пользователя, Логин, Пароль_Hash, Пароль_Salt, Дата_Создания FROM [dbo].[Пользователь] WHERE Логин LIKE @searchTerm", sqlConnection);
                    command.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                    SqlDataReader reader = command.ExecuteReader();
                    var users = new List<User>();

                    while (reader.Read())
                    {
                        byte[] passwordHash = (byte[])reader["Пароль_Hash"];
                        byte[] passwordSalt = (byte[])reader["Пароль_Salt"];

                        users.Add(new User
                        {
                            ID_Пользователя = reader.GetInt32(0),
                            Логин = reader.GetString(1),
                            Пароль_Hash = Convert.ToBase64String(passwordHash), 
                            Пароль_Salt = Convert.ToBase64String(passwordSalt), 
                            Дата_Создания = reader.GetDateTime(4)
                        });
                    }

                    UsersDataGrid.ItemsSource = users;
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

        public class User
        {
            public int ID_Пользователя { get; set; }
            public string Логин { get; set; }
            public string Пароль_Hash { get; set; }
            public string Пароль_Salt { get; set; }
            public DateTime Дата_Создания { get; set; }
            public string Роль { get; set; } 
        }
    }
}