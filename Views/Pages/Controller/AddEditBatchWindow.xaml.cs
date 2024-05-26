using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PrimeTrack.Models;

namespace PrimeTrack.Views.Pages.Controller
{
    /// <summary>
    /// Логика взаимодействия для AddEditBatchWindow.xaml
    /// </summary>
    public partial class AddEditBatchWindow : Window
    {
        private readonly Batch _batch;
        public AddEditBatchWindow(Batch batch = null)
        {
            InitializeComponent();
            _batch = batch;

            if (_batch != null)
            {
                BatchCodeTextBox.Text = _batch.Код_Партии.ToString();
                ProductionDatePicker.SelectedDate = _batch.Дата_Производства;
                ProductionTimeTextBox.Text = _batch.Время_Производства.ToString();
                WarehouseCodeTextBox.Text = _batch.Код_Склада.ToString();
                ClientCodeTextBox.Text = _batch.Код_Клиента?.ToString();
                ShipmentDatePicker.SelectedDate = _batch.Дата_Отгрузки;
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int warehouseCode, clientCode;
            TimeSpan productionTime;

            if (DateTime.TryParse(ProductionDatePicker.Text, out DateTime productionDate) &&
                TimeSpan.TryParse(ProductionTimeTextBox.Text, out productionTime) &&
                int.TryParse(WarehouseCodeTextBox.Text, out warehouseCode))
            {
                int? clientCodeNullable = null;
                if (int.TryParse(ClientCodeTextBox.Text, out clientCode))
                {
                    clientCodeNullable = clientCode;
                }

                DateTime? shipmentDate = null;
                if (ShipmentDatePicker.SelectedDate.HasValue)
                {
                    shipmentDate = ShipmentDatePicker.SelectedDate.Value;
                }

                var connection = new Connection();

                try
                {
                    connection.OpenConnection();
                    using (var sqlConnection = connection.GetConnection())
                    {
                        SqlCommand command;

                        if (_batch == null) // Add new batch
                        {
                            command = new SqlCommand(
                                "INSERT INTO [dbo].[Партия] (Дата_Производства, Время_Производства, Код_Склада, Код_Клиента, Дата_Отгрузки) VALUES (@Дата_Производства, @Время_Производства, @Код_Склада, @Код_Клиента, @Дата_Отгрузки)",
                                sqlConnection);
                        }
                        else // Edit existing batch
                        {
                            command = new SqlCommand(
                                "UPDATE [dbo].[Партия] SET Дата_Производства = @Дата_Производства, Время_Производства = @Время_Производства, Код_Склада = @Код_Склада, Код_Клиента = @Код_Клиента, Дата_Отгрузки = @Дата_Отгрузки WHERE Код_Партии = @Код_Партии",
                                sqlConnection);
                            command.Parameters.AddWithValue("@Код_Партии", _batch.Код_Партии);
                        }

                        command.Parameters.AddWithValue("@Дата_Производства", productionDate);
                        command.Parameters.AddWithValue("@Время_Производства", productionTime);
                        command.Parameters.AddWithValue("@Код_Склада", warehouseCode);
                        command.Parameters.AddWithValue("@Код_Клиента", (object)clientCodeNullable ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Дата_Отгрузки", (object)shipmentDate ?? DBNull.Value);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Партия успешно сохранена.");
                        Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении партии: " + ex.Message);
                }
                finally
                {
                    connection.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}