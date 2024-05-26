using PrimeTrack.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PrimeTrack.Views.Pages;

namespace PrimeTrack.Views
{
    public partial class UserWindow : Window
    {
        public UserWindow(bool isAdmin)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(isAdmin);

            // Set initial page and active button style
            MainFrame.Navigate(new OverviewPage());
            SetActiveButton(OverviewButton);
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Name)
                {
                    case "OverviewButton":
                        MainFrame.Navigate(new OverviewPage());
                        break;
                    case "ClientsButton":
                        MainFrame.Navigate(new ClientsPage());
                        break;
                    case "BatchesButton":
                        MainFrame.Navigate(new BatchesPage());
                        break;
                    case "ProductsButton":
                        MainFrame.Navigate(new ProductsPage());
                        break;
                    case "WarehousesButton":
                        MainFrame.Navigate(new WarehousesPage());
                        break;
                    case "UsersButton":
                        MainFrame.Navigate(new UsersPage());
                        break;
                    default:
                        break;
                }
                SetActiveButton(button);
            }
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductsPage());
            SetActiveButton(sender as Button);
        }

        private void WarehousesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new WarehousesPage());
            SetActiveButton(sender as Button);
        }

        private void ClientsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ClientsPage());
            SetActiveButton(sender as Button);
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsersPage());
            SetActiveButton(sender as Button);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void SetActiveButton(Button activeButton)
        {
            OverviewButton.Style = (Style)FindResource("menuButton");
            BatchesButton.Style = (Style)FindResource("menuButton");
            ProductsButton.Style = (Style)FindResource("menuButton");
            WarehousesButton.Style = (Style)FindResource("menuButton");
            ClientsButton.Style = (Style)FindResource("menuButton");
            UsersButton.Style = (Style)FindResource("menuButton");

            activeButton.Style = (Style)FindResource("activeMenuButton");
        }
    }
}
