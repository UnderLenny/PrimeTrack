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
using static PrimeTrack.Views.Pages.ProductsPage;

namespace PrimeTrack.Views.Pages.Controller
{
    /// <summary>
    /// Логика взаимодействия для AddEditProductWindow.xaml
    /// </summary>
    public partial class AddEditProductWindow : Window
    {
        private readonly Product _product;

        public AddEditProductWindow(Product product = null)
        {
            InitializeComponent();
            _product = product;

            if (_product != null)
            {
                ProductNameTextBox.Text = _product.Название;
                ManufacturerTextBox.Text = _product.Производитель;
                DescriptionTextBox.Text = _product.Описание;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string productName = ProductNameTextBox.Text;
            string manufacturer = ManufacturerTextBox.Text;
            string description = DescriptionTextBox.Text;

            var connection = new Connection();

            try
            {
                connection.OpenConnection();
                using (var sqlConnection = connection.GetConnection())
                {
                    SqlCommand command;

                    if (_product == null) // Add new product
                    {
                        command = new SqlCommand(
                            "INSERT INTO [dbo].[Продукт] (Название, Производитель, Описание) VALUES (@Название, @Производитель, @Описание)",
                            sqlConnection);
                    }
                    else // Edit existing product
                    {
                        command = new SqlCommand(
                            "UPDATE [dbo].[Продукт] SET Название = @Название, Производитель = @Производитель, Описание = @Описание WHERE Код_Продукта = @Код_Продукта",
                            sqlConnection);
                        command.Parameters.AddWithValue("@Код_Продукта", _product.Код_Продукта);
                    }

                    command.Parameters.AddWithValue("@Название", productName);
                    command.Parameters.AddWithValue("@Производитель", manufacturer);
                    command.Parameters.AddWithValue("@Описание", description);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Продукт успешно сохранен.");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении продукта: " + ex.Message);
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