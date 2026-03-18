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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShopApp_Potapov.Pages;

namespace ShopApp_Potapov.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private ShopApp_PotapovEntities _context;
        private Users currentUser;
        public MainPage(Users user)
        {
            InitializeComponent();
            _context = new ShopApp_PotapovEntities();

            currentUser = user;

            DisplayUserInfo();
            BuildNavigationMenu();
            

        }

        private void DisplayUserInfo()
        {
            var role = _context.Roles.FirstOrDefault(r => r.Id == currentUser.RoleId);
            string roleName = role.Name;

            txtUserInfo.Text = $"{currentUser.Login} ({roleName})";
        }


        private void BuildNavigationMenu()
        {
            NavigationMenu.Children.Clear();

            var menuTitle = new TextBlock
            {
                Text = "МЕНЮ",
                FontSize = 16,
                FontWeight = System.Windows.FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.LightGray,
                Margin = new Thickness(0, 0, 0, 15)
            };
            NavigationMenu.Children.Add(menuTitle);

            int roleId = currentUser.RoleId;

            

            if (roleId == 1 || roleId == 2) 
            {
                AddNavigationButton("Клиенты", "Clients");
                //AddNavigationButton("Заказы", "Orders");
            }

            
            if (roleId == 2)
            {
                AddNavigationButton("Товары", "Products");
            }

            if (roleId == 3) 
            {
                //AddNavigationButton("Мои заказы", "MyOrders");
                AddNavigationButton("Корзина", "Cart");
            }
        }


        private void AddNavigationButton(string text, string pageName)
        {
            var button = new Button
            {
                Content = text,
                Height = 40,
                Margin = new Thickness(0, 5, 0, 5),
                Background = System.Windows.Media.Brushes.Transparent,
                Foreground = System.Windows.Media.Brushes.White,
                BorderThickness = new Thickness(0),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(10, 0, 0, 0),
                Tag = pageName
            };

            button.Click += NavigationButton_Click;
            NavigationMenu.Children.Add(button);
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string pageName = button.Tag.ToString();
                NavigateToPage(pageName);

                foreach (var child in NavigationMenu.Children)
                {
                    if (child is Button btn)
                    {
                        btn.Background = System.Windows.Media.Brushes.Transparent;
                    }
                }
                button.Background = System.Windows.Media.Brushes.Gray;
            }
        }

        private void NavigateToPage(string pageName)
        {
            Page page;

            switch (pageName)
            {
                case "Products":
                    page = new ProductsPage(_context, currentUser);
                    break;
                case "Clients":
                    page = new ClientsPage(_context, currentUser);
                    break;
                case "Orders":
                    page = new OrdersPage(_context, currentUser);
                    break;
                case "MyOrders":
                    page = new MyOrdersPage(_context, currentUser);
                    break;
                case "Cart":
                    page = new CartPage(_context, currentUser);
                    break;
                default:
                    page = new ProductsPage(_context, currentUser);
                    break;
            }

            MasterFrame.Navigate(page);
        }

    }
}
