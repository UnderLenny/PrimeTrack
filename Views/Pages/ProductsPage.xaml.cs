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
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            LoadProductsData();
        }
        private void LoadProductsData()
        {
            var connection = new Connection();

            try
            {
                connection.OpenConnection();
                using (var sqlConnection = connection.GetConnection())
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Продукт]", sqlConnection);
                    SqlDataReader reader = command.ExecuteReader();
                    var products = new List<Product>();

                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Код_Продукта = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                            Название = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            Производитель = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Описание = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                        });
                    }

                    ProductsDataGrid.ItemsSource = products;
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

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddEditProductWindow();
            addProductWindow.ShowDialog();
            LoadProductsData();
        }

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Product selectedProduct)
            {
                var editProductWindow = new AddEditProductWindow(selectedProduct);
                editProductWindow.ShowDialog();
                LoadProductsData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите продукт для редактирования.");
            }
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Product selectedProduct)
            {
                var connection = new Connection();

                try
                {
                    connection.OpenConnection();
                    using (var sqlConnection = connection.GetConnection())
                    {
                        SqlCommand command = new SqlCommand("DELETE FROM [dbo].[Продукт] WHERE Код_Продукта = @Код_Продукта", sqlConnection);
                        command.Parameters.AddWithValue("@Код_Продукта", selectedProduct.Код_Продукта);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Продукт успешно удален.");
                        LoadProductsData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении продукта: " + ex.Message);
                }
                finally
                {
                    connection.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите продукт для удаления.");
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
                    SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Продукт] WHERE Название LIKE @searchTerm OR Производитель LIKE @searchTerm", sqlConnection);
                    command.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                    SqlDataReader reader = command.ExecuteReader();
                    var products = new List<Product>();

                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Код_Продукта = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                            Название = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            Производитель = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Описание = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                        });
                    }

                    ProductsDataGrid.ItemsSource = products;
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

        public class Product
        {
            public int Код_Продукта { get; set; }
            public string Название { get; set; }
            public string Производитель { get; set; }
            public string Описание { get; set; }
        }
    }
}