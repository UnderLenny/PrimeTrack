using PrimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
using static PrimeTrack.Views.Pages.UsersPage;

namespace PrimeTrack.Views.Pages.Controller
{
    /// <summary>
    /// Логика взаимодействия для AddEditUserWindow.xaml
    /// </summary>
    public partial class AddEditUserWindow : Window
    {
        private readonly User _user;

        public AddEditUserWindow(User user = null)
        {
            InitializeComponent();
            _user = user;
            LoadRoles(); // Load roles into the combo box

            if (_user != null)
            {
                LoginTextBox.Text = _user.Логин;
                RoleComboBox.SelectedItem = _user.Роль;
                // Оставляем поле пароля пустым
            }
        }

        private void LoadRoles()
        {
            var connection = new Connection();

            try
            {
                connection.OpenConnection();
                using (var sqlConnection = connection.GetConnection())
                {
                    SqlCommand command = new SqlCommand("SELECT Название_Роли FROM [dbo].[Роли]", sqlConnection);
                    SqlDataReader reader = command.ExecuteReader();
                    var roles = new List<string>();

                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }

                    RoleComboBox.ItemsSource = roles;
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке ролей: " + ex.Message);
            }
            finally
            {
                connection.CloseConnection();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;
            string role = RoleComboBox.SelectedItem.ToString();

            var connection = new Connection();

            try
            {
                connection.OpenConnection();
                using (var sqlConnection = connection.GetConnection())
                {
                    SqlCommand command;
                    int roleId;

                    // Get the role ID from the roles table
                    using (var roleCommand = new SqlCommand("SELECT ID_Роли FROM [dbo].[Роли] WHERE Название_Роли = @Роль", sqlConnection))
                    {
                        roleCommand.Parameters.AddWithValue("@Роль", role);
                        roleId = (int)roleCommand.ExecuteScalar();
                    }

                    if (_user == null) // Add new user
                    {
                        var salt = GenerateSalt();
                        var passwordHash = HashPassword(password, salt);

                        command = new SqlCommand(
                            "INSERT INTO [dbo].[Пользователь] (Логин, Пароль_Hash, Пароль_Salt, Дата_Создания) VALUES (@Логин, @Пароль_Hash, @Пароль_Salt, @Дата_Создания); " +
                            "DECLARE @UserID int = SCOPE_IDENTITY(); " +
                            "INSERT INTO [dbo].[Пользователь_Роль] (ID_Пользователя, ID_Роли) VALUES (@UserID, @ID_Роли)",
                            sqlConnection);
                        command.Parameters.AddWithValue("@Дата_Создания", DateTime.Now);
                        command.Parameters.AddWithValue("@Пароль_Hash", passwordHash);
                        command.Parameters.AddWithValue("@Пароль_Salt", salt);
                    }
                    else // Edit existing user
                    {
                        command = new SqlCommand(
                            "UPDATE [dbo].[Пользователь] SET Логин = @Логин WHERE ID_Пользователя = @ID_Пользователя; " +
                            "UPDATE [dbo].[Пользователь_Роль] SET ID_Роли = @ID_Роли WHERE ID_Пользователя = @ID_Пользователя",
                            sqlConnection);
                        command.Parameters.AddWithValue("@ID_Пользователя", _user.ID_Пользователя);

                        // Update the password only if it has been changed
                        if (!string.IsNullOrEmpty(password))
                        {
                            var salt = GenerateSalt();
                            var passwordHash = HashPassword(password, salt);
                            SqlCommand updatePasswordCommand = new SqlCommand(
                                "UPDATE [dbo].[Пользователь] SET Пароль_Hash = @Пароль_Hash, Пароль_Salt = @Пароль_Salt WHERE ID_Пользователя = @ID_Пользователя",
                                sqlConnection);
                            updatePasswordCommand.Parameters.AddWithValue("@Пароль_Hash", passwordHash);
                            updatePasswordCommand.Parameters.AddWithValue("@Пароль_Salt", salt);
                            updatePasswordCommand.Parameters.AddWithValue("@ID_Пользователя", _user.ID_Пользователя);
                            updatePasswordCommand.ExecuteNonQuery();
                        }
                    }

                    command.Parameters.AddWithValue("@Логин", login);
                    command.Parameters.AddWithValue("@ID_Роли", roleId);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Пользователь успешно сохранен.");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении пользователя: " + ex.Message);
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
    }
}
