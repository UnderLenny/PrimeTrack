using PrimeTrack.Models;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PrimeTrack.Views
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly Connection connection = new Connection();

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = UsernameTextBox.Text;
            string password = RegisterPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Все поля обязательны для заполнения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                connection.OpenConnection();

                byte[] salt = GenerateSalt();
                byte[] passwordHash = HashPassword(password, salt);

                string query = "INSERT INTO Пользователь (Логин, Пароль_Hash, Пароль_Salt, Дата_Создания) VALUES (@Логин, @Пароль, @Соль, @Дата_Создания)";
                using (SqlCommand command = new SqlCommand(query, connection.GetConnection()))
                {
                    command.Parameters.AddWithValue("@Логин", login);
                    command.Parameters.AddWithValue("@Пароль", passwordHash);
                    command.Parameters.AddWithValue("@Соль", salt);
                    command.Parameters.AddWithValue("@Дата_Создания", DateTime.Now);

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Регистрация прошла успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка при регистрации.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
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

        public static byte[] HashPassword(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combinedBytes = new byte[salt.Length + Encoding.UTF8.GetBytes(password).Length];
                Buffer.BlockCopy(salt, 0, combinedBytes, 0, salt.Length);
                Buffer.BlockCopy(Encoding.UTF8.GetBytes(password), 0, combinedBytes, salt.Length, Encoding.UTF8.GetBytes(password).Length);

                return sha256.ComputeHash(combinedBytes);
            }
        }

        public static byte[] GenerateSalt()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var salt = new byte[16];
                rng.GetBytes(salt);
                return salt;
            }
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
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
