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

namespace ShopApp_Potapov.Pages
{
    /// <summary>
    /// Логика взаимодействия для MyOrdersPage.xaml
    /// </summary>
    public partial class MyOrdersPage : Page
    {
        private ShopApp_PotapovEntities _context;
        private Users currentUser;

        public MyOrdersPage(ShopApp_PotapovEntities context, Users user)
        {
            InitializeComponent();
            _context = context;
            currentUser = user;

            if (currentUser.RoleId != 3)
            {
                MessageBox.Show("Эта страница доступна только покупателям", "Ошибка доступа",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                NavigationService?.GoBack();
                return;
            }

            LoadMyOrders();
        }

        private void LoadMyOrders()
        {
            var client = _context.Clients.FirstOrDefault(c => c.Id == currentUser.Id);

            if (client == null)
            {
                dgOrders.ItemsSource = null;
                return;
            }

            var orders = _context.Orders
                .Include("OrderItems")
                .Where(o => o.ClientId == client.Id)
                .OrderByDescending(o => o.DateCreated)
                .ToList();

            dgOrders.ItemsSource = orders;



        }

        private void cmbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
