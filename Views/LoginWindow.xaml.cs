using PrimeTrack.Models;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PrimeTrack.Views
{
    public partial class LoginWindow : Window
    {
        private readonly Connection connection = new Connection();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Все поля обязательны для заполнения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                connection.OpenConnection();
                string query = @"
                    SELECT 
                        Пароль_Hash, 
                        Пароль_Salt, 
                        (SELECT TOP 1 Название_Роли 
                         FROM Роли R 
                         INNER JOIN Пользователь_Роль UR 
                         ON R.ID_Роли = UR.ID_Роли 
                         WHERE UR.ID_Пользователя = P.ID_Пользователя) AS Role 
                    FROM Пользователь P 
                    WHERE Логин = @Логин";

                using (SqlCommand command = new SqlCommand(query, connection.GetConnection()))
                {
                    command.Parameters.AddWithValue("@Логин", login);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            byte[] storedHash = (byte[])reader["Пароль_Hash"];
                            byte[] storedSalt = (byte[])reader["Пароль_Salt"];
                            string role = reader["Role"]?.ToString() ?? string.Empty;

                            byte[] passwordHash = HashPassword(password, storedSalt);
                            if (CompareHashes(storedHash, passwordHash))
                            {
                                MessageBox.Show("Вход выполнен успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
                                    nextWindow = new WaitingWindow(login);
                                }
                                nextWindow.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private byte[] HashPassword(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combinedBytes = new byte[salt.Length + Encoding.UTF8.GetBytes(password).Length];
                Buffer.BlockCopy(salt, 0, combinedBytes, 0, salt.Length);
                Buffer.BlockCopy(Encoding.UTF8.GetBytes(password), 0, combinedBytes, salt.Length, Encoding.UTF8.GetBytes(password).Length);

                return sha256.ComputeHash(combinedBytes);
            }
        }

        private bool CompareHashes(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
                return false;

            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                    return false;
            }
            return true;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
