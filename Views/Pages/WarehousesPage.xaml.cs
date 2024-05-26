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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PrimeTrack.Views.Pages.Controller;

namespace PrimeTrack.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для WarehousesPage.xaml
    /// </summary>
    public partial class WarehousesPage : Page
    {
        public WarehousesPage()
        {
            InitializeComponent();
            LoadWarehousesData();
        }

        private void LoadWarehousesData()
        {
            var connection = new Connection();

            try
            {
                connection.OpenConnection();
                using (var sqlConnection = connection.GetConnection())
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Склад]", sqlConnection);
                    SqlDataReader reader = command.ExecuteReader();
                    var warehouses = new List<Warehouse>();

                    while (reader.Read())
                    {
                        warehouses.Add(new Warehouse
                        {
                            Код_Склада = reader.GetInt32(0),
                            Местоположение = reader.GetString(1)
                        });
                    }

                    WarehousesDataGrid.ItemsSource = warehouses;
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

        private void AddWarehouseButton_Click(object sender, RoutedEventArgs e)
        {
            var addWarehouseWindow = new AddEditWarehouseWindow();
            addWarehouseWindow.ShowDialog();
            LoadWarehousesData();
        }

        private void EditWarehouseButton_Click(object sender, RoutedEventArgs e)
        {
            if (WarehousesDataGrid.SelectedItem is Warehouse selectedWarehouse)
            {
                var editWarehouseWindow = new AddEditWarehouseWindow(selectedWarehouse);
                editWarehouseWindow.ShowDialog();
                LoadWarehousesData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите склад для редактирования.");
            }
        }

        private void DeleteWarehouseButton_Click(object sender, RoutedEventArgs e)
        {
            if (WarehousesDataGrid.SelectedItem is Warehouse selectedWarehouse)
            {
                var connection = new Connection();

                try
                {
                    connection.OpenConnection();
                    using (var sqlConnection = connection.GetConnection())
                    {
                        SqlCommand command = new SqlCommand("DELETE FROM [dbo].[Склад] WHERE Код_Склада = @Код_Склада", sqlConnection);
                        command.Parameters.AddWithValue("@Код_Склада", selectedWarehouse.Код_Склада);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Склад успешно удален.");
                        LoadWarehousesData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении склада: " + ex.Message);
                }
                finally
                {
                    connection.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите склад для удаления.");
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
                    SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Склад] WHERE Местоположение LIKE @searchTerm", sqlConnection);
                    command.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                    SqlDataReader reader = command.ExecuteReader();
                    var warehouses = new List<Warehouse>();

                    while (reader.Read())
                    {
                        warehouses.Add(new Warehouse
                        {
                            Код_Склада = reader.GetInt32(0),
                            Местоположение = reader.GetString(1)
                        });
                    }

                    WarehousesDataGrid.ItemsSource = warehouses;
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
        public class Warehouse
        {
            public int Код_Склада { get; set; }
            public string Местоположение { get; set; }
        }
    }
}