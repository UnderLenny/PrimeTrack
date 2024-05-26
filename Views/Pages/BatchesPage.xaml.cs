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
    /// Логика взаимодействия для BatchesPage.xaml
    /// </summary>
 
        public partial class BatchesPage : Page
        {
            public BatchesPage()
            {
                InitializeComponent();
                LoadBatchesData();
                LoadBatchProductsData(); // Убедитесь, что этот метод вызывается здесь
            }

            private void LoadBatchesData()
            {
                var connection = new Connection();

                try
                {
                    connection.OpenConnection();
                    using (var sqlConnection = connection.GetConnection())
                    {
                        SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Партия]", sqlConnection);
                        SqlDataReader reader = command.ExecuteReader();
                        var batches = new List<Batch>();

                        while (reader.Read())
                        {
                            batches.Add(new Batch
                            {
                                Код_Партии = reader.GetInt32(0),
                                Дата_Производства = reader.GetDateTime(1),
                                Время_Производства = reader.GetTimeSpan(2),
                                Код_Склада = reader.GetInt32(3),
                                Код_Клиента = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                                Дата_Отгрузки = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5)
                            });
                        }

                        BatchesDataGrid.ItemsSource = batches;
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

            private void LoadBatchProductsData()
            {
                var connection = new Connection();

                try
                {
                    connection.OpenConnection();
                    using (var sqlConnection = connection.GetConnection())
                    {
                        SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Продукт_Партия]", sqlConnection);
                        SqlDataReader reader = command.ExecuteReader();
                        var batchProducts = new List<BatchProduct>();

                        while (reader.Read())
                        {
                            batchProducts.Add(new BatchProduct
                            {
                                Код_Продукта = reader.GetInt32(0),
                                Код_Партии = reader.GetInt32(1),
                                Количество = reader.GetInt32(2)
                            });
                        }

                        BatchProductsDataGrid.ItemsSource = batchProducts;
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке данных продуктов партии: " + ex.Message);
                }
                finally
                {
                    connection.CloseConnection();
                }
            }

            private void AddBatchButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditBatchWindow addBatchWindow = new AddEditBatchWindow();
            addBatchWindow.ShowDialog();
            LoadBatchesData();
        }

        private void EditBatchButton_Click(object sender, RoutedEventArgs e)
        {
            if (BatchesDataGrid.SelectedItem is Batch selectedBatch)
            {
                AddEditBatchWindow editBatchWindow = new AddEditBatchWindow(selectedBatch);
                editBatchWindow.ShowDialog();
                LoadBatchesData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите партию для редактирования.");
            }
        }

        private void DeleteBatchButton_Click(object sender, RoutedEventArgs e)
        {
            if (BatchesDataGrid.SelectedItem is Batch selectedBatch)
            {
                var connection = new Connection();

                try
                {
                    connection.OpenConnection();
                    using (var sqlConnection = connection.GetConnection())
                    {
                        SqlCommand command = new SqlCommand("DELETE FROM [dbo].[Партия] WHERE Код_Партии = @Код_Партии", sqlConnection);
                        command.Parameters.AddWithValue("@Код_Партии", selectedBatch.Код_Партии);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Партия успешно удалена.");
                        LoadBatchesData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении партии: " + ex.Message);
                }
                finally
                {
                    connection.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите партию для удаления.");
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
                    SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Партия] WHERE CAST(Код_Партии AS NVARCHAR) LIKE @searchTerm OR CAST(Код_Склада AS NVARCHAR) LIKE @searchTerm", sqlConnection);
                    command.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                    SqlDataReader reader = command.ExecuteReader();
                    var batches = new List<Batch>();

                    while (reader.Read())
                    {
                        batches.Add(new Batch
                        {
                            Код_Партии = reader.GetInt32(0),
                            Дата_Производства = reader.GetDateTime(1),
                            Время_Производства = reader.GetTimeSpan(2),
                            Код_Склада = reader.GetInt32(3),
                            Код_Клиента = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                            Дата_Отгрузки = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5)
                        });
                    }

                    BatchesDataGrid.ItemsSource = batches;
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

        private void AddBatchProductButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditBatchProductWindow addBatchProductWindow = new AddEditBatchProductWindow();
            addBatchProductWindow.ShowDialog();
            LoadBatchProductsData();
        }

        private void EditBatchProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (BatchProductsDataGrid.SelectedItem is BatchProduct selectedBatchProduct)
            {
                AddEditBatchProductWindow editBatchProductWindow = new AddEditBatchProductWindow(selectedBatchProduct);
                editBatchProductWindow.ShowDialog();
                LoadBatchProductsData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите продукт партии для редактирования.");
            }
        }

        private void DeleteBatchProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (BatchProductsDataGrid.SelectedItem is BatchProduct selectedBatchProduct)
            {
                var connection = new Connection();

                try
                {
                    connection.OpenConnection();
                    using (var sqlConnection = connection.GetConnection())
                    {
                        SqlCommand command = new SqlCommand("DELETE FROM [dbo].[Продукт_Партия] WHERE Код_Продукта = @Код_Продукта AND Код_Партии = @Код_Партии", sqlConnection);
                        command.Parameters.AddWithValue("@Код_Продукта", selectedBatchProduct.Код_Продукта);
                        command.Parameters.AddWithValue("@Код_Партии", selectedBatchProduct.Код_Партии);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Продукт партии успешно удален.");
                        LoadBatchProductsData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении продукта партии: " + ex.Message);
                }
                finally
                {
                    connection.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите продукт партии для удаления.");
            }
        }
    }

    public class Batch
    {
        public int Код_Партии { get; set; }
        public DateTime Дата_Производства { get; set; }
        public TimeSpan Время_Производства { get; set; }
        public int Код_Склада { get; set; }
        public int? Код_Клиента { get; set; }
        public DateTime? Дата_Отгрузки { get; set; }
    }

    public class BatchProduct
    {
        public int Код_Продукта { get; set; }
        public int Код_Партии { get; set; }
        public int Количество { get; set; }
    }
}