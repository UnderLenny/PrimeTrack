using PrimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace PrimeTrack.Views.Pages
{
    public partial class OverviewPage : Page
    {
        public OverviewPage()
        {
            InitializeComponent();
            LoadOverviewData();
        }

        private void LoadOverviewData()
        {
            var connection = new Connection();

            try
            {
                connection.OpenConnection();
                using (var sqlConnection = connection.GetConnection())
                {
                    // Получение количества клиентов
                    SqlCommand clientCountCommand = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Клиент]", sqlConnection);
                    int clientCount = Convert.ToInt32(clientCountCommand.ExecuteScalar());
                    ClientCountTextBlock.Text = clientCount.ToString();

                    // Получение количества партий на складе
                    SqlCommand batchCountCommand = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Партия]", sqlConnection);
                    int batchCount = Convert.ToInt32(batchCountCommand.ExecuteScalar());
                    BatchCountTextBlock.Text = batchCount.ToString();

                    // Получение среднего размера партии
                    SqlCommand avgBatchSizeCommand = new SqlCommand("SELECT AVG(Количество) FROM [dbo].[Продукт_Партия]", sqlConnection);
                    object avgBatchSizeObj = avgBatchSizeCommand.ExecuteScalar();
                    double avgBatchSize = avgBatchSizeObj != DBNull.Value ? Convert.ToDouble(avgBatchSizeObj) : 0.0;
                    AvgBatchSizeTextBlock.Text = avgBatchSize.ToString("0.00");

                    // Получение данных для графика количества партий по складам
                    SqlCommand batchChartDataCommand = new SqlCommand("SELECT s.Город, COUNT(*) AS Количество FROM [dbo].[Партия] p INNER JOIN [dbo].[Склад] s ON p.Код_Склада = s.Код_Склада GROUP BY s.Город", sqlConnection);
                    SqlDataReader reader = batchChartDataCommand.ExecuteReader();

                    var labels = new List<string>();
                    var values = new ChartValues<int>();

                    while (reader.Read())
                    {
                        labels.Add(reader["Город"].ToString());
                        values.Add(Convert.ToInt32(reader["Количество"]));
                    }
                    reader.Close();

                    BatchChart.Series = new SeriesCollection
                    {
                        new ColumnSeries
                        {
                            Title = "Количество партий",
                            Values = values
                        }
                    };

                    BatchChart.AxisX[0].Labels = labels;
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
    }
}
