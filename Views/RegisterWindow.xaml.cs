using PrimeTrack.Models;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PrimeTrack.Views
{
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
                using (SqlConnection sqlConnection = connection.GetConnection())
                {
                    sqlConnection.Open();

                    byte[] salt = GenerateSalt();
                    byte[] passwordHash = HashPassword(password, salt);

                    string insertUserQuery = "INSERT INTO Пользователь (Логин, Пароль_Hash, Пароль_Salt, Дата_Создания) OUTPUT INSERTED.ID_Пользователя VALUES (@Логин, @Пароль_Hash, @Пароль_Salt, GETDATE())";
                    SqlCommand cmd = new SqlCommand(insertUserQuery, sqlConnection);
                    cmd.Parameters.AddWithValue("@Логин", login);
                    cmd.Parameters.AddWithValue("@Пароль_Hash", passwordHash);
                    cmd.Parameters.AddWithValue("@Пароль_Salt", salt);

                    int userId = (int)cmd.ExecuteScalar();

                    string insertUserRoleQuery = "INSERT INTO Пользователь_Роль (ID_Пользователя, ID_Роли) VALUES (@ID_Пользователя, @ID_Роли)";
                    SqlCommand roleCmd = new SqlCommand(insertUserRoleQuery, sqlConnection);
                    roleCmd.Parameters.AddWithValue("@ID_Пользователя", userId);
                    roleCmd.Parameters.AddWithValue("@ID_Роли", 2); 

                    roleCmd.ExecuteNonQuery();

                    MessageBox.Show("Регистрация прошла успешно!");
                }
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
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
