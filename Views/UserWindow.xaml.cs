using PrimeTrack.Models;
using System;
using System.Collections.Generic;
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
using PrimeTrack.Views.Pages;

namespace PrimeTrack.Views
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow(bool isAdmin)
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel(isAdmin);
        }

        private void OverviewButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OverviewPage());
        }

        private void ClientsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ClientsPage());
        }

        private void BatchesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BatchesPage());
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductsPage());
        }

        private void WarehousesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new WarehousesPage());
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsersPage());
        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReportsPage());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement logout functionality
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}