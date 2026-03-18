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
using ShopApp_Potapov.Windows;

namespace ShopApp_Potapov.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {

        private ShopApp_PotapovEntities _context;
        private Users currentUser;

        public ProductsPage(ShopApp_PotapovEntities context, Users user)
        {
            InitializeComponent();
            _context = context;
            currentUser = user;

            LoadProducts();
            CheckPermissions();
        }


       

        private void LoadProducts()
        {
            var products = _context.Products
                    .Include("Categories")
                    .OrderBy(p => p.Name)
                    .ToList();

            dgProducts.ItemsSource = products;
        }

        private void CheckPermissions()
        {
            if (currentUser.RoleId == 3)
            {
                BtnAdd.Visibility = Visibility.Collapsed;
                BtnEdit.Visibility = Visibility.Collapsed;
                BtnDelete.Visibility = Visibility.Collapsed;
            }
            else
            {
                BtnAdd.Visibility = Visibility.Visible;
                BtnEdit.Visibility = Visibility.Visible;
                BtnDelete.Visibility = Visibility.Visible;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var productWindow = new ProductWindow(_context, null);
            if (productWindow.ShowDialog() == true)
            {
                LoadProducts();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem is Products product)
            {
                var productWindow = new ProductWindow(_context, product);
                if (productWindow.ShowDialog() == true)
                {
                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem is Products product)
            {
                var result = MessageBox.Show($"Удалить товар {product.Name} ?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    var orderItems = _context.OrderItems.Where(oi => oi.ProductId == product.Id).ToList();

                    _context.Products.Remove(product);
                    _context.SaveChanges();

                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

      

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
