using PrimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using PrimeTrack.Views;
using static PrimeTrack.Views.Pages.WarehousesPage;

namespace PrimeTrack.Views.Pages.Controller
{
    /// <summary>
    /// Логика взаимодействия для AddEditWarehouseWindow.xaml
    /// </summary>
    public partial class AddEditWarehouseWindow : Window
    {
        private readonly Warehouse _warehouse;

        public AddEditWarehouseWindow(Warehouse warehouse = null)
        {
            InitializeComponent();
            _warehouse = warehouse;

            if (_warehouse != null)
            {
                LocationTextBox.Text = _warehouse.Местоположение;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string location = LocationTextBox.Text;

            var connection = new Connection();

            try
            {
                connection.OpenConnection();
                using (var sqlConnection = connection.GetConnection())
                {
                    SqlCommand command;

                    if (_warehouse == null) // Add new warehouse
                    {
                        command = new SqlCommand(
                            "INSERT INTO [dbo].[Склад] (Местоположение) VALUES (@Местоположение)",
                            sqlConnection);
                    }
                    else // Edit existing warehouse
                    {
                        command = new SqlCommand(
                            "UPDATE [dbo].[Склад] SET Местоположение = @Местоположение WHERE Код_Склада = @Код_Склада",
                            sqlConnection);
                        command.Parameters.AddWithValue("@Код_Склада", _warehouse.Код_Склада);
                    }

                    command.Parameters.AddWithValue("@Местоположение", location);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Склад успешно сохранен.");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении склада: " + ex.Message);
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
    }
}