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

namespace PrimeTrack.Views.Pages.Controller
{
    /// <summary>
    /// Логика взаимодействия для AddEditBatchProductWindow.xaml
    /// </summary>
    public partial class AddEditBatchProductWindow : Window
    {
            private BatchProduct _batchProduct;

            public AddEditBatchProductWindow()
            {
                InitializeComponent();
            }

            public AddEditBatchProductWindow(BatchProduct batchProduct) : this()
            {
                _batchProduct = batchProduct;
                ProductCodeTextBox.Text = batchProduct.Код_Продукта.ToString();
                BatchCodeTextBox.Text = batchProduct.Код_Партии.ToString();
                QuantityTextBox.Text = batchProduct.Количество.ToString();
            }

            private void SaveButton_Click(object sender, RoutedEventArgs e)
            {
                int productCode = int.Parse(ProductCodeTextBox.Text);
                int batchCode = int.Parse(BatchCodeTextBox.Text);
                int quantity = int.Parse(QuantityTextBox.Text);

                var connection = new Connection();

                try
                {
                    connection.OpenConnection();
                    using (var sqlConnection = connection.GetConnection())
                    {
                        SqlCommand command;
                        if (_batchProduct == null)
                        {
                            command = new SqlCommand("INSERT INTO [dbo].[Продукт_Партия] (Код_Продукта, Код_Партии, Количество) VALUES (@Код_Продукта, @Код_Партии, @Количество)", sqlConnection);
                        }
                        else
                        {
                            command = new SqlCommand("UPDATE [dbo].[Продукт_Партия] SET Количество = @Количество WHERE Код_Продукта = @Код_Продукта AND Код_Партии = @Код_Партии", sqlConnection);
                        }

                        command.Parameters.AddWithValue("@Код_Продукта", productCode);
                        command.Parameters.AddWithValue("@Код_Партии", batchCode);
                        command.Parameters.AddWithValue("@Количество", quantity);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Данные успешно сохранены.");
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении данных: " + ex.Message);
                }
                finally
                {
                    connection.CloseConnection();
                }
            }

            private void CancelButton_Click(object sender, RoutedEventArgs e)
            {
                this.Close();
            }
        }
    }