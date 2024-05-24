using PrimeTrack.Models;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace PrimeTrack.Views
{
    public partial class WaitingWindow : Window
    {
        private readonly Connection connection = new Connection();
        private readonly string login;

        public WaitingWindow(string userLogin)
        {
            InitializeComponent();
            login = userLogin;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void RecheckButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.OpenConnection();
                string query = "SELECT (SELECT TOP 1 Название_Роли FROM Роли R INNER JOIN Пользователь_Роль UR ON R.ID_Роли = UR.ID_Роли WHERE UR.ID_Пользователя = P.ID_Пользователя) AS Role FROM Пользователь P WHERE Логин = @Логин";
                using (SqlCommand command = new SqlCommand(query, connection.GetConnection()))
                {
                    command.Parameters.AddWithValue("@Логин", login);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string role = reader["Role"].ToString();

                            Window nextWindow;
                            if (role == "Администратор")
                            {
                                nextWindow = new AdminWindow();
                            }
                            else if (role == "Сотрудник")
                            {
                                nextWindow = new UserWindow();
                            }
                            else
                            {
                                MessageBox.Show("Ваша регистрация еще не подтверждена.", "Ожидание", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }
                            nextWindow.Show();
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connection.CloseConnection();
            }
        }
    }
}
