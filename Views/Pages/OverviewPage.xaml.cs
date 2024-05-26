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

namespace PrimeTrack.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для OverviewPage.xaml
    /// </summary>
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
                    int clientCount = (int)clientCountCommand.ExecuteScalar();
                    ClientCountTextBlock.Text = clientCount.ToString();

                    // Получение количества партий на складе
                    SqlCommand batchCountCommand = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Партия]", sqlConnection);
                    int batchCount = (int)batchCountCommand.ExecuteScalar();
                    BatchCountTextBlock.Text = batchCount.ToString();
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